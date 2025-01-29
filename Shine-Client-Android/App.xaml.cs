using Plugin.Firebase.CloudMessaging;

namespace Shine_Client_Android;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        CrossFirebaseCloudMessaging.Current.TokenChanged += (s, e) =>
        {
            Console.WriteLine($"FCM Token: {e.Token}");
        };

        CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("news");

        CrossFirebaseCloudMessaging.Current.NotificationReceived += (s, e) =>
        {
            Console.WriteLine($"Уведомление: {e.Notification.Title} - {e.Notification.Body}");
        };
    }
}