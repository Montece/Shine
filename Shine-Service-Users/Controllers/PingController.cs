using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shine_Service_Users.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Ping OK.");
    }

    [Authorize]
    [HttpGet("auth_ping")]
    public IActionResult AuthPing()
    {
        return Ok("Auth ping OK.");
    }
}