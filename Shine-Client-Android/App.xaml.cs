using Plugin.Firebase.CloudMessaging;

namespace Shine_Client_Android;

public partial class App
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        /*CrossFirebaseCloudMessaging.Current.TokenChanged += (_, e) =>
        {
            Console.WriteLine($"FCM Token: {e.Token}");
        };

        CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("news");

        CrossFirebaseCloudMessaging.Current.NotificationReceived += (_, e) =>
        {
            Console.WriteLine($"Уведомление: {e.Notification.Title} - {e.Notification.Body}");
        };*/
    }
}