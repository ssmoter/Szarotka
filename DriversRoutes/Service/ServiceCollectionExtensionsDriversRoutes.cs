namespace DriversRoutes.Service
{
    public static class ServiceCollectionExtensionsDriversRoutes
    {
        public static IServiceCollection AddMyServiceDriversRoutes(this IServiceCollection services)
        {

            services.AddScoped<Pages.Maps.MapsV>();
            services.AddScoped<Pages.Maps.MapsVM>();

            services.AddScoped<Pages.Main.MainVDriversRoutesV>();
            services.AddScoped<Pages.Main.MainVDriversRoutesVM>();

            services.AddScoped<Pages.Options.CreateTable.CreateTableRoutesV>();
            services.AddScoped<Pages.Options.CreateTable.CreateTableRoutesVM>();

            services.AddScoped<ISelectRoutes, Data.SelectRoutes>();
            services.AddScoped<ISaveRoutes, Data.SaveRoutes>();
#if ANDROID
            services.AddScoped<IDownloadAddress,Platforms.Android.DownloadAddress>();
#endif

            services.AddScoped<Pages.ListOfPoints.ListOfPointsV>();
            services.AddScoped<Pages.ListOfPoints.ListOfPointsVM>();

            services.AddScoped<Pages.AddCustomer.AddCustomerV>();
            services.AddScoped<Pages.AddCustomer.AddCustomerVM>();

            return services;
        }
    }
}
