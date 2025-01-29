namespace Shine_Client_Android.Features.ShoppingList;

internal class ShoppingList
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}