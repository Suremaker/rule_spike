using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using RuleSpike;

namespace RuleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleSetController : ControllerBase
    {
        private readonly RuleSetDefinitionRepository _repo;

        public RuleSetController(RuleSetDefinitionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("/")]
        public IEnumerable<RuleSetDefinition> GetAll() => _repo.GetAll();

        [HttpGet("/{id}/export")]
        public IActionResult Export(Guid id)
        {
            var definition = _repo.Get(id);
            var mem = new MemoryStream();
            using (var sw = new StreamWriter(mem))
            {
                var writer = new CsvWriter(sw, CultureInfo.InvariantCulture);
                writer.WriteRecords(definition.Rules);
            }
            return File(mem.ToArray(), "text/csv", $"{definition.Name}.csv");
        }

        [HttpGet("/{id}")]
        public RuleSetDefinition Get(Guid id) => _repo.Get(id);

        [HttpPost("/")]
        public RuleSetDefinition Save(RuleSetDefinition definition)
        {
            ApplyRuleSet(definition);
            return definition;
        }

        [HttpPost("/import")]
        public IActionResult Import(string name, IFormFile file)
        {
            if (!string.Equals(Path.GetExtension(file.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest();
            using var streamReader = new StreamReader(file.OpenReadStream());
            var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            var rules = reader.GetRecords<RuleDefinition>().ToList();
            var definition = new RuleSetDefinition { Name = name, Rules = rules };
            ApplyRuleSet(definition);
            return Ok(definition);
        }

        private void ApplyRuleSet(RuleSetDefinition definition)
        {
            _repo.Save(definition);
        }
    }
}
