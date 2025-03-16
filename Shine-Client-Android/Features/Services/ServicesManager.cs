namespace Shine_Client_Android.Features.Services;

internal class ServicesManager
{
    public static ServicesManager Instance => _instance ??= new();
    private static ServicesManager? _instance;

    private ServicesManager()
    {
    }

    public AuthService AuthService => _authService ??= new(HttpClientHandlerWithoutHttps.CreateHttpClient());
    private static AuthService? _authService;

    public ShoppingService ShoppingService => _shoppingService ??= new(HttpClientHandlerWithoutHttps.CreateHttpClient());
    private static ShoppingService? _shoppingService;
}