using Microsoft.AspNetCore.Mvc;

namespace Shine_Service_Shopping.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Ping OK.");
    }
}