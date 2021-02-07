namespace RuleSpike
{
    public class RuleDefinition
    {
        public RuleDefinition(string name, string criteria, string action, bool stopOnSuccess = true)
        {
            Name = name;
            Criteria = criteria;
            Action = action;
            StopOnSuccess = stopOnSuccess;
        }

        public string Name { get; }
        public string Criteria { get; }
        public string Action { get; }
        public bool StopOnSuccess { get; }
    }
}