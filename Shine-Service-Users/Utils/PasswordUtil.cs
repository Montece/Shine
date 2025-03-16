using System.Security.Cryptography;
using System.Text;

namespace Shine_Service_Users.Utils;

internal static class PasswordUtil
{
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        var passwordHash = HashPassword(password);
        return storedHash == passwordHash;
    }
}