using DataBase.Data;
using DataBase.Data.Save;

using Microsoft.Extensions.DependencyInjection;

namespace DataBase.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceDataBase(this IServiceCollection services)
        {
            services.AddTransient<AccessDataBase>();

            services.AddScoped<ISaveInventoryAoT, SaveInventoryAoT>();
            services.AddScoped<ISaveDriverRoutesAoT, SaveDriverRoutesAoT>();

            return services;
        }

    }
}
