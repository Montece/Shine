using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shine_Client_Android.Features.AuthService;

namespace Shine_Client_Android.ViewModels;

internal partial class RegisterViewModel : ObservableObject
{
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private string fullName;
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private string email;
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private string password;
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private string confirmPassword;
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private string errorMessage;
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private bool isErrorVisible;

    private readonly AuthService _authService;

    public RegisterViewModel()
    {
        _authService = new(CustomHttpClientHandler.CreateHttpClient(), new("https://10.6.0.144:44375/api/"));

        IsErrorVisible = false;
    }

    [RelayCommand]
    private async Task Register()
    {
        IsErrorVisible = false;

        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrEmpty(ConfirmPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Все поля должны быть заполнены.";
            IsErrorVisible = true;
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Пароли не совпадают.";
            IsErrorVisible = true;
            return;
        }

        try
        {
            var result = await _authService.RegisterAsync(fullName, email, password);

            if (result.success)
            {
                await Shell.Current.GoToAsync("ShoppingListPage");
            }
            else
            {
                throw new(result.message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
            IsErrorVisible = true;
        }
    }

    // Регистрация через Google
    [RelayCommand]
    private async Task GoogleRegister()
    {
        try
        {
            // TODO: Реализовать интеграцию с Google OAuth

            await Task.Delay(500); // Заглушка

            // После успешной регистрации
            await Shell.Current.GoToAsync("ShoppingListPage");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка Google регистрации: {ex.Message}";
            IsErrorVisible = true;
        }
    }
}