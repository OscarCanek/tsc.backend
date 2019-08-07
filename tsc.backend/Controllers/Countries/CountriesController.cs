using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using tsc.backend.Models;

namespace tsc.backend.Controllers.Countries
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> logger;
        private readonly TscContext tscContext;

        public CountriesController(TscContext tscContext, ILogger<CountriesController> logger)
        {
            this.logger = logger;
            this.tscContext = tscContext;
        }

        // GET api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            return await this.tscContext.Countries.Select(x => x.CommonName).ToArrayAsync();
        }

        // GET api/countries/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/countries
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/countries/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/countries/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
