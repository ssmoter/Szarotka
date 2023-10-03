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

            return services;
        }
    }
}
