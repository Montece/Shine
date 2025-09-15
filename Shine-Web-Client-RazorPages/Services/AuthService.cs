using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Shine.Web.Client.Models;

namespace Shine.Web.Client.Services;

public class AuthService : IAuthService
{
    private readonly IUsersApiService _usersApiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUsersApiService usersApiService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger)
    {
        _usersApiService = usersApiService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<bool> LoginAsync(LoginModel model)
    {
        try
        {
            var loginResponse = await _usersApiService.LoginAsync(model);
            
            if (loginResponse?.Token != null)
            {
                var principal = await ValidateAndCreatePrincipalAsync(loginResponse.Token);
                
                if (principal != null)
                {
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(24)
                    };

                    await _httpContextAccessor.HttpContext!.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        authProperties);

                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return false;
        }
    }

    public async Task<bool> RegisterAsync(RegisterModel model)
    {
        try
        {
            var response = await _usersApiService.RegisterAsync(model);
            return response != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    public async Task<ClaimsPrincipal?> ValidateAndCreatePrincipalAsync(string token)
    {
        try
        {
            var userId = await _usersApiService.ValidateTokenAsync(token);
            
            if (!string.IsNullOrEmpty(userId))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, userId),
                    new("jwt_token", token)
                };

                // Add additional claims from JWT if needed
                var emailClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "email" || c.Type == ClaimTypes.Email);
                if (emailClaim != null)
                {
                    claims.Add(new Claim(ClaimTypes.Email, emailClaim.Value));
                    claims.Add(new Claim(ClaimTypes.Name, emailClaim.Value));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                return new ClaimsPrincipal(identity);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return null;
        }
    }

    public string? GetCurrentUserId(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string? GetStoredToken(ClaimsPrincipal user)
    {
        return user.FindFirst("jwt_token")?.Value;
    }
}
