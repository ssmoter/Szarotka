﻿using DataBase.Data;

using MudBlazor.Services;

namespace DataBase.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceDataBase(this IServiceCollection services)
        {
            services.AddMudServices();

            services.AddTransient<AccessDataBase>();

            services.AddScoped<ICreatedDataBase, CreatedDataBase>();

            services.AddScoped<Pages.Log.LogData.LogDataVM>();
            services.AddScoped<Pages.Log.LogData.LogDataV>();

            services.AddScoped<Pages.Log.LogVM>();
            services.AddScoped<Pages.ExistingFiles.ExistingFilesVM>();
            services.AddScoped<Pages.ExistingFiles.ExistingFilesV>();

            services.AddScoped<Pages.UpdateDataBase.UpdateDataBaseVM>();
            services.AddScoped<Pages.UpdateDataBase.UpdateDataBaseV>();

#if WINDOWS
            services.AddScoped<Pages.Log.LogVWindows>();
#else
            services.AddScoped<Pages.Log.LogV>();
#endif

            return services;
        }

    }
}
