using System.Net;
using JetBrains.Annotations;

namespace Shine_Service_Shopping.Middlewares;

public class AuthMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

        var (success, userId) = await ValidateJwtTokenAsync(token);

        if (string.IsNullOrEmpty(token) || !success)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Invalid or missing token");
            return;
        }

        context.Items["UserId"] = userId;

        await _next.Invoke(context);
    }

    public async Task<(bool success, string userId)> ValidateJwtTokenAsync(string jwtToken)
    {
        using var httpClient = new HttpClient();

        var response = await httpClient.GetAsync($"http://Shine-Service-Users:5000/api/Auth/validate?token={jwtToken}");
        var responseText = await response.Content.ReadAsStringAsync();

        return (response.IsSuccessStatusCode, responseText);
    }
}