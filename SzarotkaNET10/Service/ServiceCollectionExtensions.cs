using DataBase.Service;

using DriversRoutes.Service;

using Inventory.Service;

using Microsoft.Extensions.Configuration;

using Shared.Service;

using SzarotkaNET10.Pages.Options.Main;

namespace SzarotkaNET10.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMyServiceDataBase();
            services.AddMyServiceShared();
            services.AddMyServiceInventory();
            services.AddMyServiceDriversRoutes();
            services.AddMyServiceHttpClients(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
#if ANDROID
#endif

            services.AddScoped<MainOptionsV>();
            services.AddScoped<MainOptionsVM>();
            services.AddScoped<MainPage>();

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
