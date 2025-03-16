namespace Shine_Client_Android.Features.Auth;

public class RegisterRequest(string email, string password, string fullName)
{
    public string Email { get; } = email;
    public string Password { get; } = password;
    public string FullName { get; } = fullName;
}