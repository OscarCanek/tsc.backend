using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tsc.backend.lib.Subdivisions;
using tsc.backend.lib.Models;

namespace tsc.backend.Controllers.Subdivisions
{
    [Route("api/subdivisions")]
    [ApiController]
    public class SubdivisionsController : ControllerBase
    {
        private readonly ILogger<SubdivisionsController> logger;
        private readonly ISubdivisionHandler handler;

        public SubdivisionsController(ISubdivisionHandler handler, ILogger<SubdivisionsController> logger)
        {
            this.handler = handler;
            this.logger = logger;
        }

        // GET api/subdivisions
        [HttpGet]
        public async Task<ActionResult<Tuple<IEnumerable<SubdivisionModel>, int>>> Get([FromQuery] int top, [FromQuery] Guid countryId)
        {
            try
            {
                var result = await this.handler.ListAsync(new GetSubdivisionDetails { Top = top, CountryId = countryId });
                return Ok(new
                {
                    subdivisions = result.Item1,
                    total = result.Item2
                });
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }

        // GET api/subdivisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubdivisionModel>> Get([FromRoute] Guid id, [FromQuery] Guid countryId)
        {
            try
            {
                return Ok(await this.handler.GetDetailsAsync(new GetSubdivisionDetails { Id = id, CountryId = countryId }));
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }

        // POST api/subdivisions
        [HttpPost]
        public async Task<ActionResult<SubdivisionModel>> Post([FromBody] CreateSubdivisionModel model)
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

        // PUT api/subdivisions/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SubdivisionModel>> Put(Guid id, [FromBody] UpdateSubdivisionModel model)
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

        // DELETE api/subdivisions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            try
            {
                return Ok(await this.handler.RemoveAsync(new RemoveSubdivisionModel { Id = id }));
            }
            catch (Exception)
            {
                // here we should capture the custom exception to return the correct response
                throw;
            }
        }
    }
}
