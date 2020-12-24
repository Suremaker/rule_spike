using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace RuleSpike
{
    public class RulesCompiler
    {
        public Func<TInput, TOutput, Task> Compile<TInput, TOutput>(params RuleDefinition[] rules)
        {
            var sb = new StringBuilder();

            foreach (var rule in rules)
                sb.AppendLine($"if ({rule.Criteria}){{{rule.Action};return;}}");
            //sb.AppendLine($"if ({rule.Criteria}){{{rule.Action};System.Console.WriteLine(\"{rule.Name}\");return;}}");

            var code = sb.ToString();
            Console.WriteLine(code);
            var script = CSharpScript.Create(code,
                ScriptOptions.Default.WithReferences(typeof(TInput).Assembly, typeof(TOutput).Assembly),
                typeof(RuleModel<TInput, TOutput>));
            script.Compile();
            var scriptFn = script.CreateDelegate();
            return (input, output) => scriptFn.Invoke(new RuleModel<TInput, TOutput>(input, output));
        }
    }
}
