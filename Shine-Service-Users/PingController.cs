using Microsoft.AspNetCore.Mvc;

namespace Shine_Service_Users;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Ping OKAY.");
    }
}