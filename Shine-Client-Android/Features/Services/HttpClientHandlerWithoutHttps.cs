namespace Shine_Client_Android.Features.Services;

public class HttpClientHandlerWithoutHttps : HttpClientHandler
{
    private HttpClientHandlerWithoutHttps()
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
    }

    public static HttpClient CreateHttpClient()
    {
        return new(new HttpClientHandlerWithoutHttps());
    }
}