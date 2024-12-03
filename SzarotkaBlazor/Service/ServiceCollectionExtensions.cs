using Shared.Service;

using DriversRoutes.Service;

using Inventory.Service;

using SzarotkaBlazor.Pages.Options.Main;
using DataBase.Service;

namespace SzarotkaBlazor.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyService(this IServiceCollection services)
        {
            services.AddMyServiceDataBase();
            services.AddMyServiceShared();
            services.AddMyServiceInventory();
            services.AddMyServiceDriversRoutes();
#if ANDROID
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
