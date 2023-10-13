namespace DriversRoutes.Service
{
    public static class ServiceCollectionExtensionsDriversRoutes
    {
        public static IServiceCollection AddMyServiceDriversRoutes(this IServiceCollection services)
        {

            services.AddTransient<Pages.Maps.MapsV>();
            services.AddTransient<Pages.Maps.MapsVM>();

            services.AddTransient<Pages.Main.MainVDriversRoutesV>();
            services.AddTransient<Pages.Main.MainVDriversRoutesVM>();

            services.AddTransient<Pages.Options.CreateTable.CreateTableRoutesV>();
            services.AddTransient<Pages.Options.CreateTable.CreateTableRoutesVM>();

            services.AddTransient<Service.ISelectRoutes,Data.SelectRoutes>();
            
            services.AddTransient<Pages.ListOfPoints.ListOfPointsV>();
            services.AddTransient<Pages.ListOfPoints.ListOfPointsVM>();


            return services;
        }
    }
}
