namespace Shine_Service_Shopping.Models;

public class ShoppingListItemModel
{
    public required string Id { get; init; }

    public required string ShoppingListId { get; init; }

    public required string Name { get; init; }
}