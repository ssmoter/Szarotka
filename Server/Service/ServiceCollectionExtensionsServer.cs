using DataBase.Data;
using DataBase.Data.Get;

using MailKit.Net.Smtp;

using Server.Model;
using Server.Requests;
using Server.Validation;

namespace Server.Service
{
    public static class ServiceCollectionExtensionsServer
    {
        public static IServiceCollection AddMyServiceServer(this IServiceCollection services
                                                            , IConfiguration configuration)
        {
            DataBase.Service.ServiceCollectionExtensionsDataBase.AddMyServiceDataBase(services);
            services.AddScoped<IAccessDataBase>(options =>
            {
                var env = options.GetRequiredService<IWebHostEnvironment>();
                var path = Path.Combine(env.ContentRootPath,
                                env.EnvironmentName, DataBase.Helper.Constants.DatabaseName);
                return new AccessDataBase(path);
            });

            services.AddScoped<IUserValidation, UserValidation>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IRegisterUserRequests, RegisterUserRequests>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ILoginUserRequests, LoginUserRequests>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailConfirmService, EmailConfirmService>();
            services.AddScoped<JSONWebTokensSettings>(options =>
            {
                //services.Configure<JSONWebTokensSettings>
                // (configuration.GetSection("JSONWebTokensSettings"));
                return new JSONWebTokensSettings(
                    configuration["JSONWebTokensSettings:Key"],
                    configuration["JSONWebTokensSettings:Issuer"],
                    configuration["JSONWebTokensSettings:Audience"],
                    configuration["JSONWebTokensSettings:DurationInMinutes"],
                    configuration["JSONWebTokensSettings:DurationInDays"]
                    );
            });
            services.AddScoped<IEditUserService, EditUserService>();
            services.AddScoped<IEditUserRequests, EditUserRequests>();
            services.AddScoped<ISmtpClient, SmtpClient>();

            services.AddScoped<IResetPasswordRequests, ResetPasswordRequests>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();

            services.AddScoped<IInventoryProductsRequests, InventoryProductsRequests>();
            services.AddScoped<IInventoryDayRequests, InventoryDayRequests>();
            services.AddScoped<IGetDriverRoutesAoT, GetDriverRoutesAoT>();
            services.AddScoped<IDriverRoutesCustomerRoutesRequests, DriverRoutesCustomerRoutesRequests>();



            return services;
        }
    }
}
