using System.Net.Http.Headers;
using System.Net.Http.Json;
using Shine_Client_Android.Features.ShoppingList;

namespace Shine_Client_Android.Features.Services;

internal class ShoppingService(HttpClient httpClient)
{
    public string CurrentShoppingListId { get; set; } = string.Empty;

    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<ShoppingList.ShoppingList>?> GetShoppingListsAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");

        var response = await _httpClient.GetAsync("http://10.6.0.144:5001/api/Shopping/get").ConfigureAwait(false);

        List<ShoppingList.ShoppingList>? addedShoppingLists = null;

        if (response.IsSuccessStatusCode)
        {
            if (await response.Content.ReadFromJsonAsync(typeof(List<ShoppingList.ShoppingList>)).ConfigureAwait(false) is List<ShoppingList.ShoppingList> shoppingListsAnswer)
            {
                addedShoppingLists = shoppingListsAnswer;
            }
        }

        return addedShoppingLists;
    }

    public async Task<(bool success, string message, ShoppingListAnswer? addedShoppingList)> AddShoppingListAsync(string token, string id, string name)
    {
        _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");

        var request = new AddShoppingListRequest(id, name);
        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5001/api/Shopping/add", request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            ShoppingListAnswer? addedShoppingList = null;

            var aa = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (await response.Content.ReadFromJsonAsync(typeof(ShoppingListAnswer)).ConfigureAwait(false) is ShoppingListAnswer shoppingListsAnswer)
            { 
                addedShoppingList = shoppingListsAnswer;
            }

            return (true, "Creation successful!", addedShoppingList);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, $"Creation failed: {error}", null);
    }

    public async Task<List<ShoppingListItem>?> GetShoppingListItemsAsync(string token, string shoppingListId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");

        var response = await _httpClient.GetAsync($"http://10.6.0.144:5001/api/Shopping/getitems?shoppingListId={shoppingListId}").ConfigureAwait(false);

        List<ShoppingListItem>? shoppingListItems = null;

        var a = response.Content.ReadAsStringAsync().Result;

        if (response.IsSuccessStatusCode)
        {
            if (await response.Content.ReadFromJsonAsync(typeof(List<ShoppingListItem>)).ConfigureAwait(false) is List<ShoppingListItem> shoppingListItemsAnswer)
            {
                shoppingListItems = shoppingListItemsAnswer;
            }
        }

        return shoppingListItems;
    }

    public async Task<(bool success, string message, ShoppingListItem? addedShoppingListItem)> AddShoppingListItemAsync(string token, string id, string shoppingListId, string name)
    {
        _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");

        var request = new AddShoppingListItemRequest(id, shoppingListId, name);
        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5001/api/Shopping/additem", request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            ShoppingListItem? addedShoppingListItem = null;
            
            if (await response.Content.ReadFromJsonAsync(typeof(ShoppingListItem)).ConfigureAwait(false) is ShoppingListItem shoppingListItemAnswer)
            { 
                addedShoppingListItem = shoppingListItemAnswer;
            }

            return (true, "Creation successful!", addedShoppingListItem);
        }

        var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return (false, $"Creation failed: {error}", null);
    }

    public async Task<(bool success, string message)> RemoveShoppingListItemAsync(string token, string id)
    {
        _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");

        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5001/api/Shopping/removeitem", id).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return (true, "Removing successful!");
        }

        var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return (false, $"Removing failed: {error}");
    }

    public async Task<(bool success, string message)> SetIsPurchasedShoppingListItemAsync(string token, string id, bool value)
    {
        _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");

        var request = new SetIsPurchasedShoppingListItemRequest(id, value);
        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5001/api/Shopping/setispurchaseditem", request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return (true, "Set is purchased successful!");
        }

        var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return (false, $"Set is purchased failed: {error}");
    }
}

internal class AddShoppingListRequest(string id, string name)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
}

internal class AddShoppingListItemRequest(string id, string shoppingListId, string name)
{
    public string Id { get; } = id;

    public string ShoppingListId { get; } = shoppingListId;

    public string Name { get; } = name;
}

internal class SetIsPurchasedShoppingListItemRequest(string id, bool value)
{
    public string Id { get; } = id;
    public bool Value { get; } = value;
}

internal class ShoppingListAnswer
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string UserId { get; set; }

    public required DateTime CreatedAt { get; set; }
}