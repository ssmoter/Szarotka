using DataBase.Service;

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

            return services;
        }
    }
}
