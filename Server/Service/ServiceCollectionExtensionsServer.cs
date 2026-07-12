using DataBase.Data;

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
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AccessDataBase));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton<IAccessDataBase>(options =>
            {
                string dbPath;
#if DEBUG
                var env = options.GetRequiredService<IWebHostEnvironment>();
                dbPath = Path.Combine(env.ContentRootPath,
                                env.EnvironmentName, DataBase.Helper.Constants.DatabaseName);
#else
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dbPath = Path.Combine(appData, DataBase.Helper.Constants.DatabaseName);
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
#endif
                var db = new AccessDataBase(dbPath);
                db.DataBase.ExecuteScalar<string>("PRAGMA journal_mode=WAL;");
                db.DataBaseAsync.ExecuteScalarAsync<string>("PRAGMA journal_mode=WAL;").GetAwaiter().GetResult();
                return db;
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
            services.AddScoped<IDriverRoutesCustomerRoutesRequests, DriverRoutesCustomerRoutesRequests>();

            services.AddScoped<IUpdateLogRequests, UpdateLogRequests>();



            return services;
        }
    }
}
