using System.Text;
using System.Text.Json;
using Shine.Web.Client.Models;

namespace Shine.Web.Client.Services;

public class UsersApiService : IUsersApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UsersApiService> _logger;

    public UsersApiService(HttpClient httpClient, ILogger<UsersApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<LoginResponse?> LoginAsync(LoginModel model)
    {
        try
        {
            var requestModel = new
            {
                Email = model.Email,
                Password = model.Password
            };

            var json = JsonSerializer.Serialize(requestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Login failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return null;
        }
    }

    public async Task<ApiResponse?> RegisterAsync(RegisterModel model)
    {
        try
        {
            var requestModel = new
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password
            };

            var json = JsonSerializer.Serialize(requestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Registration failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return null;
        }
    }

    public async Task<string?> ValidateTokenAsync(string token)
    {
        try
        {
            var response = await _httpClient.GetAsync($"auth/validate?token={Uri.EscapeDataString(token)}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent.Trim('"'); // Remove quotes if present
            }

            _logger.LogWarning("Token validation failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token validation");
            return null;
        }
    }
}
