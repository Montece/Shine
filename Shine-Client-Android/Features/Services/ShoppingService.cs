using System.Net.Http.Json;

namespace Shine_Client_Android.Features.Services;

internal class ShoppingService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<ShoppingList.ShoppingList>?> GetShoppingListsAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var response = await _httpClient.GetAsync("http://10.6.0.144:5001/api/Shopping/get").ConfigureAwait(false);

        List<ShoppingList.ShoppingList>? shoppingLists = null;

        if (response.IsSuccessStatusCode)
        {
            if (await response.Content.ReadFromJsonAsync(typeof(List<ShoppingList.ShoppingList>)) is List<ShoppingList.ShoppingList> shoppingListsAnswer)
            {
                shoppingLists = shoppingListsAnswer;
            }
        }

        return shoppingLists;
    }

    public async Task<List<ShoppingList.ShoppingList>?> AddShoppingListAsync(string token, string id, string name)
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var request = new AddShoppingListRequest(id, name);
        var response = await _httpClient.PostAsJsonAsync("http://10.6.0.144:5000/api/Auth/register", request).ConfigureAwait(false);

        List<ShoppingList.ShoppingList>? shoppingLists = null;

        if (response.IsSuccessStatusCode)
        {
            if (await response.Content.ReadFromJsonAsync(typeof(List<ShoppingList.ShoppingList>)) is List<ShoppingList.ShoppingList> shoppingListsAnswer)
            {
                shoppingLists = shoppingListsAnswer;
            }
        }

        return shoppingLists;
    }
}

internal class AddShoppingListRequest(string id, string name)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
}