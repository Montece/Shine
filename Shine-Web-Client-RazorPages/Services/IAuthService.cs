using Shine.Web.Client.Models;
using System.Security.Claims;

namespace Shine.Web.Client.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginModel model);
    Task<bool> RegisterAsync(RegisterModel model);
    Task LogoutAsync();
    Task<ClaimsPrincipal?> ValidateAndCreatePrincipalAsync(string token);
    string? GetCurrentUserId(ClaimsPrincipal user);
    string? GetStoredToken(ClaimsPrincipal user);
}
