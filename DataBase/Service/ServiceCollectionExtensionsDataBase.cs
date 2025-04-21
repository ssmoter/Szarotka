using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesServer;

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
            services.AddScoped<IValidationException, ValidationException>();

            services.AddScoped<IGetInventoryAoT, GetInventoryAoT>();


            return services;
        }

    }
}
