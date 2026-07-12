using CommunityToolkit.Maui;

using Microsoft.Extensions.Configuration;


#if ANDROID
using DriversRoutes.Platforms.Android;
#endif

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

using SzarotkaBlazor.Service;

namespace SzarotkaBlazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(options =>
                {
#if WINDOWS
                    options.SetShouldEnableSnackbarOnWindows(true);
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
            builder.Configuration.AddConfiguration(config);



            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMyService(builder.Configuration);
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif


#if ANDROID
            builder.UseMauiMaps();
            AppContext.SetSwitch("BlazorWebView.AndroidFireAndForgetAsync", true);
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
            });
#endif
            return builder.Build();
        }

    }
}
