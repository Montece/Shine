using Shine.Web.Client.Models;

namespace Shine.Web.Client.Services;

public class ShoppingListService : IShoppingListService
{
    private readonly IShoppingApiService _shoppingApiService;
    private readonly ILogger<ShoppingListService> _logger;

    public ShoppingListService(IShoppingApiService shoppingApiService, ILogger<ShoppingListService> logger)
    {
        _shoppingApiService = shoppingApiService;
        _logger = logger;
    }

    public async Task<List<ShoppingListViewModel>?> GetShoppingListsAsync(string authToken)
    {
        try
        {
            var lists = await _shoppingApiService.GetShoppingListsAsync(authToken);
            
            if (lists == null) return null;

            var viewModels = new List<ShoppingListViewModel>();

            foreach (var list in lists)
            {
                var items = await _shoppingApiService.GetShoppingListItemsAsync(list.Id, authToken) ?? new List<ShoppingListItem>();
                
                viewModels.Add(new ShoppingListViewModel
                {
                    List = list,
                    Items = items
                });
            }

            return viewModels.OrderByDescending(vm => vm.List.CreatedAt).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shopping lists");
            return null;
        }
    }

    public async Task<ShoppingList?> CreateShoppingListAsync(string name, string authToken)
    {
        try
        {
            var request = new ShoppingListCreateRequest
            {
                Id = GenerateId(),
                Name = name
            };

            return await _shoppingApiService.AddShoppingListAsync(request, authToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating shopping list");
            return null;
        }
    }

    public async Task<List<ShoppingListItem>?> GetShoppingListItemsAsync(string listId, string authToken)
    {
        try
        {
            return await _shoppingApiService.GetShoppingListItemsAsync(listId, authToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shopping list items");
            return null;
        }
    }

    public async Task<ShoppingListItem?> AddShoppingListItemAsync(string listId, string itemName, string authToken)
    {
        try
        {
            var request = new ShoppingListItemCreateRequest
            {
                Id = GenerateId(),
                ShoppingListId = listId,
                Name = itemName
            };

            return await _shoppingApiService.AddShoppingListItemAsync(request, authToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding shopping list item");
            return null;
        }
    }

    public async Task<bool> RemoveShoppingListItemAsync(string itemId, string authToken)
    {
        try
        {
            return await _shoppingApiService.RemoveShoppingListItemAsync(itemId, authToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing shopping list item");
            return false;
        }
    }

    public async Task<bool> ToggleItemPurchasedAsync(string itemId, bool isPurchased, string authToken)
    {
        try
        {
            var request = new ShoppingListItemUpdateRequest
            {
                Id = itemId,
                Value = isPurchased
            };

            return await _shoppingApiService.SetItemPurchasedAsync(request, authToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling item purchased status");
            return false;
        }
    }

    private static string GenerateId()
    {
        return "id_" + Guid.NewGuid().ToString("N")[..16] + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("x");
    }
}
