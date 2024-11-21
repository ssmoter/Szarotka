using CommunityToolkit.Maui;

#if ANDROID
using DriversRoutes.Platforms.Android;
#endif


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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMyService();

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
