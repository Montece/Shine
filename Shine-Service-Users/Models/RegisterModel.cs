namespace Shine_Service_Users.Models;

public class RegisterModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FullName { get; set; }
}