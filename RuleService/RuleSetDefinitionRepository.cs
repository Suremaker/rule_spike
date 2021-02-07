using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RuleSpike;

namespace RuleService
{
    public class RuleSetDefinitionRepository
    {
        private readonly ConcurrentDictionary<Guid, RuleSetDefinition> _data = new ConcurrentDictionary<Guid, RuleSetDefinition>();

        public void Save(RuleSetDefinition definition)
        {
            _data.AddOrUpdate(definition.Id, definition, (_, __) => definition);
        }

        public IEnumerable<RuleSetDefinition> GetAll() => _data.Values;
        public RuleSetDefinition Get(Guid id) => _data[id];
    }
}