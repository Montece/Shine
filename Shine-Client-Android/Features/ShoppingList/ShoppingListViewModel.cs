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
        var list = ServicesManager.Instance.ShoppingService.GetShoppingListsAsync(ServicesManager.Instance.AuthService.Token).Result;
        
        if (list != null)
        {
            ShoppingLists = list;
        }
    }

    [RelayCommand]
    private async Task AddList()
    {
        // TODO: Реализовать добавление нового списка.

        //ShoppingLists.Add(new() { Name = "Новый список" });
    }

    [RelayCommand]
    private async Task OpenList(ShoppingList list)
    {
        // TODO: Реализовать переход на страницу редактирования списка.

        //await Shell.Current.GoToAsync($"ShoppingListEditPage?listId={list.Id}");
    }
}