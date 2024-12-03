using Server.Endpoints;
using Server.Validation;

namespace Server.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceServer(this IServiceCollection services)
        {
            DataBase.Service.ServiceCollectionExtensionsDataBase.AddMyServiceDataBase(services);

            services.AddScoped<IUserValidation, UserValidation>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IRegisterUserEndpoint, RegisterUserEndpoint>();



            return services;
        }
    }
}
