﻿using DataBase.Service;

using DriversRoutes.Service;

using Inventory.Service;

using Szarotka.Pages.Options.Main;

namespace Szarotka.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyService(this IServiceCollection services)
        {
            services.AddMyServiceDataBase();

            services.AddMyServiceInventory();

#if ANDROID
            services.AddMyServiceDriversRoutes();
#endif

            services.AddSingleton<MainOptionsV>();
            services.AddSingleton<MainOptionsVM>();

#if WINDOWS
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("NoLabel", (handler, View) =>
            {
                handler.PlatformView.OnContent = null;
                handler.PlatformView.OffContent = null;

                // Add this to remove the padding around the switch as well
                handler.PlatformView.MinWidth = 0;
            });
#endif


            return services;
        }
    }
}
