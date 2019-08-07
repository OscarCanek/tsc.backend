using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using tsc.backend.lib.Countries;
using tsc.backend.lib.Models;

namespace tsc.backend.Controllers.Countries
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> logger;
        private readonly TscContext tscContext;
        private readonly ICountryHandler handler;

        public CountriesController(ICountryHandler handler, TscContext tscContext, ILogger<CountriesController> logger)
        {
            this.handler = handler;
            this.logger = logger;
            this.tscContext = tscContext;
        }

        // GET api/countries
        [HttpGet]
        public async Task<ActionResult<Tuple<IEnumerable<CountryModel>, int>>> Get([FromQuery] int top, [FromQuery] string name, [FromQuery] string alfa2)
        {
            try
            {
                var result = await this.handler.ListAsync(new GetCountryDetails { Top = top, Name = name, Alfa2 = alfa2 });
                return Ok(new
                {
                    countries = result.Item1,
                    total = result.Item2
                });
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }

        // GET api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryModel>> Get([FromRoute] Guid id)
        {
            try
            {
                return Ok(await this.handler.GetDetailsAsync(new GetCountryDetails { Id = id }));
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }

        // POST api/countries
        [HttpPost]
        public async Task<ActionResult<CountryModel>> Post([FromBody] CreateCountryModel model)
        {
            try
            {
                return Ok(await this.handler.CreateAsync(model));
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }

        // PUT api/countries/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CountryModel>> Put(Guid id, [FromBody] UpdateCountryModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }

                return Ok(await this.handler.UpdateAsync(model));
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }

        // DELETE api/countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            try
            {
                return Ok(await this.handler.RemoveAsync(new RemoveCountryModel { Id = id }));
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }
    }
}
