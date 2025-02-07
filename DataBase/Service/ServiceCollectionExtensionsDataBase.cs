using DataBase.Data;
using DataBase.Data.Save;

using Microsoft.Extensions.DependencyInjection;

namespace DataBase.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceDataBase(this IServiceCollection services)
        {
            services.AddTransient<IAccessDataBase, AccessDataBase>();

            services.AddScoped<ISaveInventoryAoT, SaveInventoryAoT>();
            services.AddScoped<ISaveDriverRoutesAoT, SaveDriverRoutesAoT>();
            services.AddScoped<ITimeService, CurrentUtc>();

            return services;
        }

    }
}
