using System.ComponentModel.DataAnnotations;

namespace Shine_Service_Shopping.Database;

public class ShoppingListItem
{
    [Key]
    public string Id { get; init; }

    [Required]
    public required string ShoppingListId { get; init; }

    [Required]
    public required string Name { get; init; }

    [Required]
    public required bool IsPurchased { get; set; }

    [Required]
    public required DateTime CreatedAt { get; init; }
}