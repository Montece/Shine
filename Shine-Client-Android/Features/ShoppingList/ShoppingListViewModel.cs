using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shine_Client_Android.Features.Services;

namespace Shine_Client_Android.Features.ShoppingList;

internal partial class ShoppingListViewModel : ObservableObject
{
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private ObservableCollection<ShoppingList> shoppingLists;

    public ShoppingListViewModel()
    {
        InitializeList();
    }

    private void InitializeList()
    {
        ShoppingLists = new ObservableCollection<ShoppingList>();

        var list = ServicesManager.Instance.ShoppingService.GetShoppingListsAsync(ServicesManager.Instance.AuthService.Token).Result;
        
        if (list != null)
        {
            foreach (var shoppingList in list)
            {
                ShoppingLists.Add(shoppingList);
            }
        }
    }

    [RelayCommand]
    private async Task AddList()
    {
        var id = Guid.NewGuid().ToString();
        var name = $"Список {DateTime.Now.ToShortDateString()}";
        
        var result = await ServicesManager.Instance.ShoppingService.AddShoppingListAsync(ServicesManager.Instance.AuthService.Token, id, name).ConfigureAwait(false);

        if (result.success)
        {
            if (result.addedShoppingList != null)
            {
                ShoppingLists.Add(new ShoppingList
                {
                    Id = result.addedShoppingList.Id,
                    Name = result.addedShoppingList.Name,
                    UserId = result.addedShoppingList.UserId,
                    CreatedAt = result.addedShoppingList.CreatedAt
                });
            }
        }
    }

    [RelayCommand]
    private async Task OpenList(ShoppingList shoppingList)
    {
        ServicesManager.Instance.ShoppingService.CurrentShoppingListId = shoppingList.Id;

        await Shell.Current.GoToAsync(nameof(ShoppingListEditPage)).ConfigureAwait(false);
    }
}