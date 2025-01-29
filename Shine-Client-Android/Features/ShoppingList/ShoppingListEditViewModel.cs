using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Shine_Client_Android.Features.ShoppingList;

internal partial class ShoppingListEditViewModel : ObservableObject
{
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private string newItemName;
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private ObservableCollection<ShoppingItem.ShoppingItem> items;

    public ShoppingListEditViewModel()
    {
        Items =
        [
            new() { Name = "Молоко", IsPurchased = false },
            new() { Name = "Хлеб", IsPurchased = true }
        ];
    }

    [RelayCommand]
    private void AddItem()
    {
        if (string.IsNullOrEmpty(newItemName) || string.IsNullOrWhiteSpace(NewItemName))
        {
            return;
        }

        Items.Add(new() { Name = NewItemName, IsPurchased = false });

        NewItemName = string.Empty;
    }

    [RelayCommand]
    private void RemoveItem(ShoppingItem.ShoppingItem item)
    {
        Items.Remove(item);
    }
}