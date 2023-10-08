using CommunityToolkit.Maui;

using DataBase.Service;

using Microsoft.Extensions.Logging;


using Szarotka.Service;

namespace Szarotka
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if ANDROID
            builder.UseMauiMaps();
#endif
            builder.Services.AddMyServiceDataBase();
            builder.Services.AddMyService();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}