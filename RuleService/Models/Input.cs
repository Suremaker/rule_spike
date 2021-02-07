namespace RuleService.Models
{
    public class Input
    {
        public string Color { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public override string ToString() => $"Model={Model}, Color={Color}, Quantity={Quantity}";
    }
}