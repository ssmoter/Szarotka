using DataBase.Data;

using Microsoft.Extensions.DependencyInjection;

namespace DataBase.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceDataBase(this IServiceCollection services)
        {
            services.AddTransient<AccessDataBase>();

            return services;
        }

    }
}
