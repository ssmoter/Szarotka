﻿using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;


/* Unmerged change from project 'Szarotka (net8.0-android34.0)'
Before:
using DataBase.Service;
using Szarotka.Service;
After:
using DataBase.Service;

using Szarotka.Service;
*/
using Szarotka.Service;

#if ANDROID
using DriversRoutes.Platforms.Android;
#endif

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
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
            });
#endif
            builder.Services.AddMyService();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
