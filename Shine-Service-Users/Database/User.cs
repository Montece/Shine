using System.ComponentModel.DataAnnotations;

namespace Shine_Service_Users.Database;

public class User
{
    [Key]
    public int Id { get; init; }

    [Required]
    public required string Email { get; init; }

    [Required]
    public required string PasswordHash { get; init; }

    [Required]
    public required string FullName { get; init; }

    [Required]
    public required DateTimeOffset CreatedAt { get; init; }
}