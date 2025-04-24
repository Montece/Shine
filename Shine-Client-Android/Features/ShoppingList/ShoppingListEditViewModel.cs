using System.Collections.ObjectModel;
using System.Windows.Input;
using Android.Widget;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shine_Client_Android.Features.Services;

namespace Shine_Client_Android.Features.ShoppingList;

internal partial class ShoppingListEditViewModel : ObservableObject
{
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private string newItemName;
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private ObservableCollection<ShoppingListItem> shoppingListItems;
    public ICommand IsPurchasedChangedCommand { get; }

    public ShoppingListEditViewModel()
    {
        IsPurchasedChangedCommand = new Command<Tuple<ShoppingListItem, bool>>(IsPurchasedChanged);

        InitializeList();
    }

    private void InitializeList()
    {
        ShoppingListItems = new ObservableCollection<ShoppingListItem>();

        var list = ServicesManager.Instance.ShoppingService.GetShoppingListItemsAsync(ServicesManager.Instance.AuthService.Token, ServicesManager.Instance.ShoppingService.CurrentShoppingListId).Result;
        
        if (list != null)
        {
            foreach (var shoppingListItem in list)
            {
                ShoppingListItems.Add(shoppingListItem);
            }
        }
    }

    [RelayCommand]
    private async void AddItem()
    {
        if (string.IsNullOrEmpty(NewItemName) || string.IsNullOrWhiteSpace(NewItemName))
        {
            Toast.MakeText(MainActivity.Context, "Неверное имя позиции!", ToastLength.Short)?.Show();
            return;
        }

        var result = await ServicesManager.Instance.ShoppingService.AddShoppingListItemAsync(ServicesManager.Instance.AuthService.Token, Guid.NewGuid().ToString(), ServicesManager.Instance.ShoppingService.CurrentShoppingListId, NewItemName).ConfigureAwait(false);
        
        if (!result.success)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(MainActivity.Context, "Не удалось создать позицию!", ToastLength.Short)?.Show();
            });
            
            return;
        }

        ShoppingListItems.Add(result.addedShoppingListItem);

        NewItemName = string.Empty;
    }

    [RelayCommand]
    private async void RemoveItem(ShoppingListItem shoppingListItem)
    {
        var result = await ServicesManager.Instance.ShoppingService.RemoveShoppingListItemAsync(ServicesManager.Instance.AuthService.Token, shoppingListItem.Id).ConfigureAwait(false);

        if (!result.success)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(MainActivity.Context, "Не удалось удалить позицию!", ToastLength.Short)?.Show();
            });

            return;
        }

        ShoppingListItems.Remove(shoppingListItem);
    }

    private async void IsPurchasedChanged(Tuple<ShoppingListItem, bool> values)
    {
        if (values == null || values.Item1 == null)
        {
            Toast.MakeText(MainActivity.Context, "Неверная позиция!", ToastLength.Short)?.Show();
            return;
        }

        var result = await ServicesManager.Instance.ShoppingService.SetIsPurchasedShoppingListItemAsync(ServicesManager.Instance.AuthService.Token, values.Item1.Id, values.Item2).ConfigureAwait(false);
        
        if (!result.success)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(MainActivity.Context, "Не удалось отметить позицию!", ToastLength.Short)?.Show();
            });
            
            return;
        }
    }
}