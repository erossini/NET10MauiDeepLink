using Microsoft.Extensions.DependencyInjection;

namespace MauiDeepLink
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            await Dispatcher.DispatchAsync(async () =>
            {
                await Windows[0].Page!.DisplayAlert("App link received", uri.ToString(), "OK");
            });

            Console.WriteLine("App link: " + uri.ToString());
        }
    }
}