using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;
using RuleSpike;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var compiler = new RulesCompiler();
            var rules = compiler.Compile<Order, Discounts>(
                new RuleDefinition("Red is cheap", "I.Color==\"red\"", "O.Discount=30"),
                new RuleDefinition("Too much blue", "I.Color==\"blue\" && I.Quantity > 10", "O.Discount=50"),
                new RuleDefinition("Special model", "I.Model==\"retro\"", "O.Discount=10"),
                new RuleDefinition("Nothing special", "true", "O.Discount=0")
                );

            var orders = new[]
            {
                new Order {Color = "blue", Model = "normal", Quantity = 5},
                new Order {Color = "blue", Model = "retro", Quantity = 20},
                new Order {Color = "red", Model = "normal", Quantity = 1},
                new Order {Color = "blue", Model = "retro", Quantity = 2},
                new Order {Color = "yellow", Model = "normal", Quantity = 1}
            };

            foreach (var order in orders)
            {
                Console.WriteLine($"Running: {order}");
                var discounts = new Discounts();

                await rules.Invoke(order, discounts);
                Console.WriteLine($"Discount is: {discounts.Discount}");
                Console.WriteLine();
            }

            Console.ReadLine();

            await MeasureTime(orders, rules);
        }

        private static async Task MeasureTime(Order[] orders, Func<Order, Discounts, Task> rules)
        {
            var sw = Stopwatch.StartNew();
            var total = Enumerable.Range(0, 10000).SelectMany(x => orders).ToArray();
            foreach (var o in total)
                await rules.Invoke(o, new Discounts());
            sw.Stop();

            Console.WriteLine($"Total time for {total.Length}: {sw.Elapsed}; Single run: {sw.Elapsed / total.Length}");
        }
    }
}
