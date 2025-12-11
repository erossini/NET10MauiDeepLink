using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace MauiDeepLink
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(lifecycle =>
                {
#if ANDROID
                    lifecycle.AddAndroid(android =>
                    {
                        android.OnCreate((activity, bundle) =>
                        {
                            var action = activity.Intent?.Action;
                            var data = activity.Intent?.Data?.ToString();

                            if (action == Android.Content.Intent.ActionView && data is not null)
                            {
                                Task.Run(() => HandleAppLink(data));
                            }
                        });
                        android.OnNewIntent((activity, intent) =>
                        {
                            var action = intent?.Action;
                            var data = intent?.Data?.ToString();

                            if (action == Android.Content.Intent.ActionView && data is not null)
                            {
                                Task.Run(() => HandleAppLink(data));
                            }
                        });
                    });
#endif
#if WINDOWS
                    lifecycle.AddWindows(windows =>
                    {
                        windows.OnLaunched((app, args) =>
                        {
                            var activationArgs = args as Microsoft.UI.Xaml.LaunchActivatedEventArgs;
                            var uri = activationArgs?.Arguments;
                            if (!string.IsNullOrEmpty(uri))
                            {
                                Task.Run(() => HandleAppLink(uri));
                            }
                        });
                    });
#endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        static void HandleAppLink(string url)
        {
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
                App.Current?.SendOnAppLinkRequestReceived(uri);
        }
    }
}
