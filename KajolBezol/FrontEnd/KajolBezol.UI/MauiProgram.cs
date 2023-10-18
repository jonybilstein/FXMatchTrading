using Firebase;
using KajolBezol.UI.Model;
using KajolBezol.UI.Pages;
using KajolBezol.UI.Services;
using KajolBezol.UI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Platforms.Android;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.CloudMessaging;
using System.Reflection;






namespace KajolBezol.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterFirebaseServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<LoginPage>();

            builder.Services.AddSingleton<MatchPageViewModel>();
            builder.Services.AddSingleton<MatchPage>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomePageViewModel>();

            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ProfilePageViewModel>();

            builder.Services.AddSingleton<TradingPage>();
            builder.Services.AddSingleton<TradingPageViewModel>();


            builder.Services.AddSingleton<KajolBezolServiceUI>();

            builder.Services.AddHttpClient();

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("KajolBezol.UI.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }


        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddAndroid(android => android.OnCreate(async (activity, bundle) =>
                {
                    Firebase.FirebaseApp.InitializeApp(activity);
                }));
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(
                 isAuthEnabled: true,
                 isCloudMessagingEnabled: true,
                 isAnalyticsEnabled: true);
        }
    }
}