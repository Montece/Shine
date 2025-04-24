using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Shine_Service_Shopping.Services;

public class FirebaseMessagingService
{
    private readonly FirebaseApp _firebaseApp;

    public FirebaseMessagingService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Secrets", "firebase.json");

            _firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(path)
            });
        }
        else
        {
            _firebaseApp = FirebaseApp.DefaultInstance;
        }
    }

    public async Task<string> SendMessageAsync(string deviceToken, string title, string body)
    {
        var message = new Message
        {
            Token = deviceToken,
            Notification = new()
            {
                Title = title,
                Body = body
            }
        };

        var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

        return response;
    }
}