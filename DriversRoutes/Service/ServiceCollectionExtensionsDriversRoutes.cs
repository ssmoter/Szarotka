namespace DriversRoutes.Service
{
    public static class ServiceCollectionExtensionsDriversRoutes
    {
        public static IServiceCollection AddMyServiceDriversRoutes(this IServiceCollection services)
        {

            services.AddScoped<Pages.Maps.MapAndPoints.MapsV>();
            services.AddScoped<Pages.Maps.MapAndPoints.MapsVM>();

            services.AddScoped<Pages.Main.MainVDriversRoutesV>();
            services.AddScoped<Pages.Main.MainVDriversRoutesVM>();

            services.AddScoped<Pages.Options.CreateTable.CreateTableRoutesV>();
            services.AddScoped<Pages.Options.CreateTable.CreateTableRoutesVM>();

            services.AddScoped<ISelectRoutes, Data.SelectRoutes>();
            services.AddScoped<ISaveRoutes, Data.SaveRoutes>();
#if ANDROID
            services.AddScoped<IDownloadAddress, Platforms.Android.DownloadAddress>();
#endif

            services.AddScoped<Pages.ListOfPoints.ListOfPointsV>();
            services.AddScoped<Pages.ListOfPoints.ListOfPointsVM>();

            services.AddScoped<Pages.Customer.AddCustomer.AddCustomerV>();
            services.AddScoped<Pages.Customer.AddCustomer.AddCustomerVM>();

            services.AddScoped<Pages.Customer.DisplayCustomer.DisplayCustomerV>();
            services.AddScoped<Pages.Customer.DisplayCustomer.DisplayCustomerVM>();


            return services;
        }
    }
}
