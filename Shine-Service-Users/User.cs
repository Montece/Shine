using System.ComponentModel.DataAnnotations;

namespace Shine_Service_Users;

public class User
{
    [Key]
    public int Id { get; init; }

    [Required]
    public string Email { get; init; }

    [Required]
    public string PasswordHash { get; init; }

    [Required]
    public string FullName { get; init; }

    [Required]
    public DateTime CreatedAt { get; init; }
}