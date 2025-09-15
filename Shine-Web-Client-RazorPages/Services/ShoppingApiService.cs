using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Shine.Web.Client.Models;

namespace Shine.Web.Client.Services;

public class ShoppingApiService : IShoppingApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ShoppingApiService> _logger;

    public ShoppingApiService(HttpClient httpClient, ILogger<ShoppingApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    private void SetAuthHeader(string authToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
    }

    public async Task<List<ShoppingList>?> GetShoppingListsAsync(string authToken)
    {
        try
        {
            SetAuthHeader(authToken);
            var response = await _httpClient.GetAsync("shopping/get");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ShoppingList>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Get shopping lists failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shopping lists");
            return null;
        }
    }

    public async Task<ShoppingList?> AddShoppingListAsync(ShoppingListCreateRequest request, string authToken)
    {
        try
        {
            SetAuthHeader(authToken);
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("shopping/add", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ShoppingList>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Add shopping list failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding shopping list");
            return null;
        }
    }

    public async Task<List<ShoppingListItem>?> GetShoppingListItemsAsync(string shoppingListId, string authToken)
    {
        try
        {
            SetAuthHeader(authToken);
            var response = await _httpClient.GetAsync($"shopping/getitems?shoppingListId={Uri.EscapeDataString(shoppingListId)}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ShoppingListItem>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Get shopping list items failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shopping list items");
            return null;
        }
    }

    public async Task<ShoppingListItem?> AddShoppingListItemAsync(ShoppingListItemCreateRequest request, string authToken)
    {
        try
        {
            SetAuthHeader(authToken);
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("shopping/additem", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ShoppingListItem>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Add shopping list item failed with status code: {StatusCode}", response.StatusCode);
            return null;
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
            SetAuthHeader(authToken);
            var json = JsonSerializer.Serialize(itemId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("shopping/removeitem", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing shopping list item");
            return false;
        }
    }

    public async Task<bool> SetItemPurchasedAsync(ShoppingListItemUpdateRequest request, string authToken)
    {
        try
        {
            SetAuthHeader(authToken);
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("shopping/setispurchaseditem", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting item purchased status");
            return false;
        }
    }
}
