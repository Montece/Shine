using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shine_Client_Android.Features.Services;
using Shine_Client_Android.Features.ShoppingList;

namespace Shine_Client_Android.Features.Login;

internal partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private string email;
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private string password;
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private string errorMessage;
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    private bool isErrorVisible;

    public LoginViewModel()
    {
        IsErrorVisible = false;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Пожалуйста, заполните все поля.";
            IsErrorVisible = true;
            return;
        }

        try
        {
            var result = await ServicesManager.Instance.AuthService.LoginAsync(email, password);

            if (result.success)
            {
                await Shell.Current.GoToAsync(nameof(ShoppingListPage));
            }
            else
            {
                throw new(result.message);
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Неверный email или пароль.";
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        // TODO: Реализовать регистрацию.

        await Shell.Current.GoToAsync("RegisterPage");
    }
}