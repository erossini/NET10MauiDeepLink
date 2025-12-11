using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MauiDeepLink.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var appInstance = AppInstance.GetCurrent();
            var e = appInstance.GetActivatedEventArgs();

            // If it's not a Protocol activation, just launch the app normally
            if (e.Kind != ExtendedActivationKind.Protocol ||
                e.Data is not ProtocolActivatedEventArgs protocol)
            {
                appInstance.Activated += AppInstance_Activated;

                base.OnLaunched(args);
                return;
            }

            // If it's a Protocol activation, redirect it to other instances
            var instances = AppInstance.GetInstances();
            await Task.WhenAll(instances
                .Select(async q => await q.RedirectActivationToAsync(e)));

            return;
        }

        private void AppInstance_Activated(object? sender, AppActivationArguments e)
        {
            if (e.Kind != ExtendedActivationKind.Protocol ||
                e.Data is not ProtocolActivatedEventArgs protocol)
            {
                return;
            }

            // Process your activation here
            Debug.WriteLine("URI activated: " + protocol.Uri);

            // Display message on UI thread
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Microsoft.Maui.Controls.Application.Current?.MainPage is not null)
                {
                    await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert(
                        "Deep Link Activated",
                        $"URI: {protocol.Uri}",
                        "OK");
                }
            });
        }
    }
}