namespace Shine_Client_Android.Features.Auth;

public class LoginRequest(string email, string password)
{
    public string Email { get; } = email;
    public string Password { get; } = password;
}