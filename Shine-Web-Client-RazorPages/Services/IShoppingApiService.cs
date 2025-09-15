using Shine.Web.Client.Models;

namespace Shine.Web.Client.Services;

public interface IShoppingApiService
{
    Task<List<ShoppingList>?> GetShoppingListsAsync(string authToken);
    Task<ShoppingList?> AddShoppingListAsync(ShoppingListCreateRequest request, string authToken);
    Task<List<ShoppingListItem>?> GetShoppingListItemsAsync(string shoppingListId, string authToken);
    Task<ShoppingListItem?> AddShoppingListItemAsync(ShoppingListItemCreateRequest request, string authToken);
    Task<bool> RemoveShoppingListItemAsync(string itemId, string authToken);
    Task<bool> SetItemPurchasedAsync(ShoppingListItemUpdateRequest request, string authToken);
}
