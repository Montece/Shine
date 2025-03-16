using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shine_Service_Users.Database;
using Shine_Service_Users.Jwt;
using Shine_Service_Users.Models;
using Shine_Service_Users.Utils;

namespace Shine_Service_Users.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ILogger<AuthController> logger, JwtService jwtService, AppDbContext context) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly JwtService _jwtService = jwtService;
    private readonly AppDbContext _context = context;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                return BadRequest("A user with this email already exists.");
            }

            var passwordHash = PasswordUtil.HashPassword(model.Password);

            var user = new User
            {
                Email = model.Email,
                PasswordHash = passwordHash,
                FullName = model.FullName,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error register user.");

            return BadRequest("Error register user.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !PasswordUtil.VerifyPassword(model.Password, user.PasswordHash))
            {
                return Unauthorized("Incorrect email or password.");
            }

            var token = _jwtService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error login user.");

            return Unauthorized("Error login user.");
        }
    }

    [HttpGet("validate")]
    public IActionResult ValidateToken(string token)
    {
        try
        {
            var validationResult = _jwtService.IsValidJwtToken(token);

            if (validationResult)
            {
                var jwtToken = _jwtService.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
                var userId = claims["sub"];
            
                return Ok(new { UserId = userId });
            }

            return Unauthorized("Validating token failed!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token.");

            return Unauthorized("Error validating token.");
        }
    }
}