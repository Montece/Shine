using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Shine_Service_Users.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ILogger<AuthController> logger, AppDbContext context) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly AppDbContext _context = context;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (existingUser != null)
        {
            return BadRequest("A user with this email already exists.");
            //return BadRequest("Пользователь с таким email уже существует.");
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
        //return Ok(new { message = "Пользователь зарегистрирован!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null || !PasswordUtil.VerifyPassword(model.Password, user.PasswordHash))
        {
            return Unauthorized("Incorrect email or password.");
            //return Unauthorized("Неверный email или пароль.");
        }

        var token = GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey("shine-super-protected-giga-secret-key-15555"u8.ToArray());
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "shine",
            audience: "shine",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}