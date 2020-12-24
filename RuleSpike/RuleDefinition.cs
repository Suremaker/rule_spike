namespace RuleSpike
{
    public class RuleDefinition
    {
        public RuleDefinition(string name, string criteria, string action)
        {
            Name = name;
            Criteria = criteria;
            Action = action;
        }

        public string Name { get; }
        public string Criteria { get; }
        public string Action { get; }
    }
}