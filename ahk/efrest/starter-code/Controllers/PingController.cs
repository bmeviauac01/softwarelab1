using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    // DO NOT CHANGE ANYTHING
    // NE VALTOZTASS MEG SEMMIT
    [Route("api/ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Ping()
        {
            return Ok("pong");
        }
    }
}
