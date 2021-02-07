using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace RuleSpike
{
    public class RulesCompiler
    {
        public RuleSet<TInput, TOutput> Compile<TInput, TOutput>(params RuleDefinition[] definitions) =>
            Compile<TInput, TOutput>((IReadOnlyList<RuleDefinition>)definitions);

        public RuleSet<TInput, TOutput> Compile<TInput, TOutput>(IReadOnlyList<RuleDefinition> definitions)
        {
            var script = CSharpScript.Create<bool>("",
                ScriptOptions.Default.WithReferences(typeof(TInput).Assembly, typeof(TOutput).Assembly),
                typeof(RuleModel<TInput, TOutput>));

            var rules = definitions.Select(def => CompileRule<TInput, TOutput>(def, script)).ToArray();
            return new RuleSet<TInput, TOutput>(rules);
        }

        private Rule<RuleModel<TInput, TOutput>> CompileRule<TInput, TOutput>(RuleDefinition def, Script<bool> baseScript)
        {
            try
            {
                var ruleScript =
                    baseScript.ContinueWith<bool>($"if ({def.Criteria}){{{def.Action};return true;}}return false;");
                ruleScript.Compile();
                var fn = ruleScript.CreateDelegate();
                return new Rule<RuleModel<TInput, TOutput>>(def, m => fn.Invoke(m));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to compile rule \"{def.Name}\": {ex.Message}", ex);
            }
        }
    }
}
