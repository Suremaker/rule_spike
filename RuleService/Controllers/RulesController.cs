using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuleService.Models;
using RuleSpike;

namespace RuleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RulesController : ControllerBase
    {
        private readonly RulesEngine _engine;

        public RulesController(RulesEngine engine)
        {
            _engine = engine;
        }


        [HttpPost("/{id}/evaluate")]
        public async Task<IActionResult> Evaluate(Guid id, IFormFile file)
        {
            if (!string.Equals(Path.GetExtension(file.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest();
            var ruleSet = _engine.GetRuleSet(id);
            
            var input = ReadCsv(file);
            
            var results = await EvaluateRecords(ruleSet, input);
            
            return await WriteResults(results);
        }

        private async Task<IActionResult> WriteResults(List<RuleModel<Input, Output>> results)
        {
            var mem = new MemoryStream();
            await using (var sw = new StreamWriter(mem))
            {
                var writer = new CsvWriter(sw, CultureInfo.InvariantCulture);
                await writer.WriteRecordsAsync(results);
            }

            return File(mem.ToArray(), "text/csv", "results.csv");
        }

        private async Task<List<RuleModel<Input, Output>>> EvaluateRecords(RuleSet<Input, Output> ruleSet, List<Input> input)
        {
            var results = new List<RuleModel<Input, Output>>();
            foreach (var i in input)
            {
                var o = new Output();
                await ruleSet.Evaluate(i, o);
                results.Add(new RuleModel<Input,Output>(i,o));
            }

            return results;
        }

        private static List<Input> ReadCsv(IFormFile file)
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            return reader.GetRecords<Input>().ToList();
        }
    }
}