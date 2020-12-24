namespace RuleSpike
{
    public class RuleModel<TInput, TOutput>
    {
        public RuleModel(TInput i, TOutput o)
        {
            I = i;
            O = o;
        }

        public TInput I { get; }
        public TOutput O { get; }
    }
}