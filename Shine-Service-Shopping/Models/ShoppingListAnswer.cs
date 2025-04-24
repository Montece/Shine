namespace Shine_Service_Shopping.Models;

internal class ShoppingListAnswer
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string UserId { get; set; }

    public required DateTime CreatedAt { get; set; }
}