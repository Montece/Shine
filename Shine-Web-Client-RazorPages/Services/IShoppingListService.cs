using Shine.Web.Client.Models;

namespace Shine.Web.Client.Services;

public interface IShoppingListService
{
    Task<List<ShoppingListViewModel>?> GetShoppingListsAsync(string authToken);
    Task<ShoppingList?> CreateShoppingListAsync(string name, string authToken);
    Task<List<ShoppingListItem>?> GetShoppingListItemsAsync(string listId, string authToken);
    Task<ShoppingListItem?> AddShoppingListItemAsync(string listId, string itemName, string authToken);
    Task<bool> RemoveShoppingListItemAsync(string itemId, string authToken);
    Task<bool> ToggleItemPurchasedAsync(string itemId, bool isPurchased, string authToken);
}
