using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tsc.backend.Controllers.Ping
{
    [Route("api/ping")]
    [ApiController]
    [Authorize]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Ping()
        {
            return Ok();
        }
    }
}
