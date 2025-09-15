namespace Shine_Client_Android.Features.ShoppingList;

internal class ShoppingList
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string UserId { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
}