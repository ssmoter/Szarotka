using Server.Endpoints;
using Server.Validation;

namespace Server.Service
{
    public static class ServiceCollectionExtensionsDataBase
    {
        public static IServiceCollection AddMyServiceServer(this IServiceCollection services)
        {
            DataBase.Service.ServiceCollectionExtensionsDataBase.AddMyServiceDataBase(services);

            services.AddScoped<DataBase.Data.AccessDataBase>(options =>
            {
                var env = options.GetRequiredService<IWebHostEnvironment>();
                var path = Path.Combine(env.ContentRootPath,
                                env.EnvironmentName, DataBase.Helper.Constants.DatabaseName);
                return new DataBase.Data.AccessDataBase(path);
            });

            services.AddScoped<IUserValidation, UserValidation>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IRegisterUserEndpoint, RegisterUserEndpoint>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ILoginUserEndpoint, LoginUserEndpoint>();



            return services;
        }
    }
}
