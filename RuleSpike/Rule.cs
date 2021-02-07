using System;
using System.Threading.Tasks;

namespace RuleSpike
{
    public class Rule<TModel>
    {
        private readonly RuleDefinition _definition;
        private readonly Func<TModel, Task<bool>> _fn;

        public Rule(RuleDefinition definition, Func<TModel, Task<bool>> fn)
        {
            _definition = definition;
            _fn = fn;
        }

        public bool StopOnSuccess => _definition.StopOnSuccess;
        public string Name => _definition.Name;

        public async Task<bool> Evaluate(TModel model)
        {
            try
            {
                return await _fn(model);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Rule {_definition.Name} execution failed with error: {e.Message}", e);
            }
        }
    }
}