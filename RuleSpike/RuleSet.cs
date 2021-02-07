using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleSpike
{
    public class RuleSet<TInput, TOutput>
    {
        private readonly IReadOnlyList<Rule<RuleModel<TInput, TOutput>>> _rules;

        public RuleSet(IReadOnlyList<Rule<RuleModel<TInput, TOutput>>> rules)
        {
            _rules = rules;
        }

        public async Task<bool> Evaluate(TInput input, TOutput output, bool verbose = false)
        {
            var model = new RuleModel<TInput, TOutput>(input, output);

            var success = false;
            foreach (var r in _rules)
            {
                if (!await r.Evaluate(model))
                    continue;

                success = true;

                if (verbose)
                    Console.WriteLine($"{r.Name}");

                if (r.StopOnSuccess)
                    return true;
            }

            return success;
        }
    }
}