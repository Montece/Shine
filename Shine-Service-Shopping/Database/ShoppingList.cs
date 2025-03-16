using System.ComponentModel.DataAnnotations;

namespace Shine_Service_Shopping.Database;

public class ShoppingList
{
    [Key]
    public int Id { get; init; }

    [Required]
    public required string Name { get; init; }

    [Required]
    public required string UserId { get; init; }

    [Required]
    public required DateTime CreatedAt { get; init; }
}