using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RuleSpike
{
    public class RuleSetDefinition
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<RuleDefinition> Rules { get; set; } = new List<RuleDefinition>();
    }
}