using DataBase.Data;

namespace DataBase.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceDataBase(this IServiceCollection services)
        {
            services.AddTransient<AccessDataBase>();

            services.AddTransient<Pages.Log.LogData.LogDataVM>();
            services.AddTransient<Pages.Log.LogData.LogDataV>();

            services.AddTransient<Pages.Log.LogVM>();
            services.AddScoped<Pages.ExistingFiles.ExistingFilesV>();
            services.AddScoped<Pages.ExistingFiles.ExistingFilesVM>();
#if WINDOWS
            services.AddTransient<Pages.Log.LogVWindows>();
#else
            services.AddTransient<Pages.Log.LogV>();
#endif

            return services;
        }
    }
}
