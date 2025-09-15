using Shine.Web.Client.Models;

namespace Shine.Web.Client.Services;

public interface IUsersApiService
{
    Task<LoginResponse?> LoginAsync(LoginModel model);
    Task<ApiResponse?> RegisterAsync(RegisterModel model);
    Task<string?> ValidateTokenAsync(string token);
}
