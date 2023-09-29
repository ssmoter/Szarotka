using DataBase.Data;

namespace DataBase.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceDataBase(this IServiceCollection services)
        {
            services.AddTransient<AccessDataBase>();

            services.AddSingleton<Pages.Log.LogData.LogDataVM>();
            services.AddSingleton<Pages.Log.LogData.LogDataV>();

            services.AddSingleton<Pages.Log.LogVM>();
#if WINDOWS
            services.AddSingleton<Pages.Log.LogVWindows>();
#else
            services.AddSingleton<Pages.Log.LogV>();
#endif

            return services;
        }
    }
}
