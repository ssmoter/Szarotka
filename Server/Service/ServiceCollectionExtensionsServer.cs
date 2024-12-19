using DataBase.Data;

using Server.Endpoints;
using Server.Model;
using Server.Validation;

namespace Server.Service
{
    public static class ServiceCollectionExtensionsServer
    {
        public static IServiceCollection AddMyServiceServer(this IServiceCollection services
                                                            , IConfiguration configuration)
        {
            DataBase.Service.ServiceCollectionExtensionsDataBase.AddMyServiceDataBase(services);

            services.AddScoped<AccessDataBase>(options =>
            {
                var env = options.GetRequiredService<IWebHostEnvironment>();
                var path = Path.Combine(env.ContentRootPath,
                                env.EnvironmentName, DataBase.Helper.Constants.DatabaseName);
                return new AccessDataBase(path);
            });

            services.AddScoped<IUserValidation, UserValidation>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IRegisterUserEndpoint, RegisterUserEndpoint>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ILoginUserEndpoint, LoginUserEndpoint>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailConfirmService, EmailConfirmService>();
            services.AddScoped<JSONWebTokensSettings>(options =>
            {
                services.Configure<JSONWebTokensSettings>
                 (configuration.GetSection("JSONWebTokensSettings"));
                return new JSONWebTokensSettings(
                    configuration["JSONWebTokensSettings:Key"],
                    configuration["JSONWebTokensSettings:Issuer"],
                    configuration["JSONWebTokensSettings:Audience"],
                    configuration["JSONWebTokensSettings:DurationInMinutes"]
                    );
            });


            return services;
        }
    }
}
