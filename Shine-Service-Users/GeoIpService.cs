using System.Text.Json;

namespace Shine_Service_Users;

public class GeoIpService
{
    private readonly HttpClient _httpClient = new();

    public async Task<string?> GetCountryByIpAsync(string ip)
    {
        var response = await _httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");
        using var doc = JsonDocument.Parse(response);
        return doc.RootElement.GetProperty("country").GetString();
    }
}