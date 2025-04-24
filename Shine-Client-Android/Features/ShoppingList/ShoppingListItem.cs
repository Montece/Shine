namespace Shine_Client_Android.Features.ShoppingList;

internal class ShoppingListItem
{
    public string Id { get; init; }

    public required string ShoppingListId { get; init; }

    public required string Name { get; init; }
    
    public required bool IsPurchased { get; init; }
}