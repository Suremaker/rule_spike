using System.Text.Json.Serialization;

namespace RuleSpike
{
    public class RuleDefinition
    {
        public RuleDefinition()
        {
        }
        public RuleDefinition(string name, string criteria, string action, bool stopOnSuccess = true)
        {
            Name = name;
            Criteria = criteria;
            Action = action;
            StopOnSuccess = stopOnSuccess;
        }

        public string Name { get; set; }
        public string Criteria { get; set; }
        public string Action { get; set; }
        public bool StopOnSuccess { get; set; }
    }
}