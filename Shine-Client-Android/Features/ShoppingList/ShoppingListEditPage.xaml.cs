namespace Shine_Client_Android.Features.ShoppingList;

public partial class ShoppingListEditPage
{
	public ShoppingListEditPage()
	{
		InitializeComponent();
	}

    private void IsPurchased_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var viewModel = BindingContext as ShoppingListEditViewModel;

        if (checkBox?.BindingContext is ShoppingListItem shoppingListItem && viewModel?.IsPurchasedChangedCommand.CanExecute(null) == true)
        {
            viewModel.IsPurchasedChangedCommand.Execute(new Tuple<ShoppingListItem, bool>(shoppingListItem, e.Value));
        }

        viewModel?.IsPurchasedChangedCommand.Execute(e.Value);
    }
}