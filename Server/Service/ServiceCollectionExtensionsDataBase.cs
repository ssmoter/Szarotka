namespace Server.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceServer(this IServiceCollection services)
        {
            DataBase.Service.ServiceCollectionExtensionsDataBase.AddMyServiceDataBase(services);


            return services;
        }
    }
}
