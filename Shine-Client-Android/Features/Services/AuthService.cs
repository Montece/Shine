using Shine_Client_Android.Features.Auth;
using System.Net.Http.Json;

namespace Shine_Client_Android.Features.Services;

public class AuthService(HttpClient httpClient)
{
    public string Token { get; private set; } = string.Empty;

    private readonly HttpClient _httpClient = httpClient;

    public async Task<(bool success, string message)> RegisterAsync(string fullName, string email, string password)
    {
        var request = new RegisterRequest(email, password, fullName);

        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5000/api/Auth/register", request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return (true, "Registration successful!");
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, $"Registration failed: {error}");
    }

    public async Task<(bool success, string message)> LoginAsync(string email, string password)
    {
        var request = new LoginRequest(email, password);

        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5000/api/Auth/login", request).ConfigureAwait(false);
        var responseText = string.Empty;

        if (await response.Content.ReadFromJsonAsync(typeof(LoginAnswer)) is LoginAnswer loginAnswer)
        { 
            responseText = loginAnswer.Token;
        }

        Token = responseText;

        if (response.IsSuccessStatusCode)
        {
            return (true, "Login successful!");
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, $"Login failed: {error}");
    }
}