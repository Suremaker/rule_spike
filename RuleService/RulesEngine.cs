using System;
using System.Collections.Concurrent;
using RuleService.Models;
using RuleSpike;

namespace RuleService
{
    public class RulesEngine
    {
        private readonly RuleSetDefinitionRepository _repo;
        private readonly ConcurrentDictionary<Guid, RuleSet<Input, Output>> _ruleSets = new ConcurrentDictionary<Guid, RuleSet<Input, Output>>();

        public RulesEngine(RuleSetDefinitionRepository repo)
        {
            _repo = repo;
        }


        public RuleSet<Input, Output> GetRuleSet(Guid id) => _ruleSets.GetOrAdd(id, CreateRuleSet);

        private RuleSet<Input, Output> CreateRuleSet(Guid id)
        {
            var definition = _repo.Get(id);
            return new RulesCompiler().Compile<Input, Output>(definition.Rules);
        }
    }
}