using System.ComponentModel.DataAnnotations;

namespace Shine_Service_Shopping;

public class ShoppingList
{
    [Key]
    public int Id { get; init; }

    [Required]
    public string Name { get; init; }

    [Required]
    public int UserId { get; set; }
}