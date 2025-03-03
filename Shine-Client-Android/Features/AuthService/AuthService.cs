using System.Net.Http.Json;

namespace Shine_Client_Android.Features.AuthService;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient, Uri baseAddress)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = baseAddress;
    }

    public async Task<(bool success, string message)> RegisterAsync(string fullName, string email, string password)
    {
        var request = new RegisterRequest { FullName = fullName, Email = email, Password = password };

        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5000/api/Auth/register", request);

        if (response.IsSuccessStatusCode)
        {
            return (true, "Registration successful!");
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, $"Registration failed: {error}");
    }

    public async Task<(bool success, string message)> LoginAsync(string email, string password)
    {
        var request = new LoginRequest { Email = email, Password = password };

        var response = await _httpClient.PostAsJsonAsync("/Auth/login", request);

        if (response.IsSuccessStatusCode)
        {
            return (true, "Login successful!");
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, $"Login failed: {error}");
    }
}

public class CustomHttpClientHandler : HttpClientHandler
{
    public CustomHttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
    }

    public static HttpClient CreateHttpClient()
    {
        return new(new CustomHttpClientHandler());
    }
}