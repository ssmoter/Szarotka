using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesServer;

namespace DataBase.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceDataBase(this IServiceCollection services)
        {
            services.AddSingleton<IAccessDataBase, AccessDataBase>();
            services.AddSingleton<ITimeService, CurrentUtc>();

            services.AddScoped<ISaveInventoryAoT, SaveInventoryAoT>();
            services.AddScoped<ISaveDriverRoutesAoT, SaveDriverRoutesAoT>();

            services.AddScoped<IValidationException, ValidationException>();

            services.AddScoped<IGetInventoryAoT, GetInventoryAoT>();
            services.AddScoped<IGetDriverRoutesAoT, GetDriverRoutesAoT>();


            services.AddScoped<IUpdateLogService, UpdateLogService>();


            return services;
        }

    }
}
