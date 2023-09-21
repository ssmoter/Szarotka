using DataBase.Service;

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

            services.AddSingleton<MainOptionsV>();
            services.AddSingleton<MainOptionsVM>();

            return services;
        }
    }
}
