using DataBase.Data;
using DataBase.Data.MySqliteConnection;

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
                descriptor = null;
            }
//            services.AddSingleton<IAccessDataBase>(options =>
//            {
//                string dbPath;
//#if DEBUG
//                var env = options.GetRequiredService<IWebHostEnvironment>();
//                dbPath = Path.Combine(env.ContentRootPath,
//                                env.EnvironmentName, DataBase.Helper.Constants.DatabaseName);
//                var db = new AccessDataBase(dbPath);
//                db.DataBase.ExecuteScalar<string>("PRAGMA journal_mode=WAL;");
//                db.DataBaseAsync.ExecuteScalarAsync<string>("PRAGMA journal_mode=WAL;").GetAwaiter().GetResult();
//#else
//                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//                dbPath = Path.Combine(appData, DataBase.Helper.Constants.DatabaseName);
//                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

//                 var db = new AccessDataBase(dbPath);
//                db.DataBase.ExecuteScalar<string>("PRAGMA journal_mode=WAL;");
//                db.DataBaseAsync.ExecuteScalarAsync<string>("PRAGMA journal_mode=WAL;").GetAwaiter().GetResult();
//#endif
//                return db;
//            });

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DataBase.Data.MySqliteConnection.SqliteConnectionFactory));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            string connectionString = ConfigureDatabaseInRAM(services);
            services.AddSingleton<ISqliteConnectionFactory>(opt =>
            {
                //// Pobiera ścieżkę z systemu Google Cloud, jeśli nie istnieje - używa folderu aplikacji
                //string mountPath = Environment.GetEnvironmentVariable("PERSISTENT_DB_PATH") ?? AppContext.BaseDirectory;
                //Directory.CreateDirectory(mountPath);

                //string dbPath = Path.Combine(mountPath, "baza_produkcyjna.db");

                //string connectionString = $"Data Source={dbPath};VFS=unix-dotfile;";
                return new SqliteConnectionFactory(connectionString);
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



        private static string ConfigureDatabaseInRAM(this IServiceCollection services)
        {
            string? cloudPath = Environment.GetEnvironmentVariable("PERSISTENT_DB_PATH");

            string persistentDb;
            string runtimeDb;

            if (!string.IsNullOrEmpty(cloudPath))
            {
                // --- AKCJA W CHMURZE (Google Cloud Run) ---
                string runtimeDir = Path.GetTempPath(); // folder /tmp w RAM
                Directory.CreateDirectory(cloudPath);
                Directory.CreateDirectory(runtimeDir);

                persistentDb = Path.Combine(cloudPath, "szarotka_produkcyjna.db3");
                runtimeDb = Path.Combine(runtimeDir, "szarotka_produkcyjna.db3");
            }
            else
            {
                // --- AKCJA LOKALNIE (Twój komputer Windows) ---
                // Definiujemy Twoją dokładną, sztywną ścieżkę do pliku .db3
                // Używamy Path.Combine i AppContext, aby bezpiecznie wyjść z folderu bin/Debug do struktury projektów
                string localDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Development");

                // Jeśli wolisz pełną ścieżkę bezwzględną, możesz wpisać ją też wprost, np.:
                // string localDir = @"C:\repos\Szarotka\Server\Development\Szarotka";

                Directory.CreateDirectory(localDir); // Upewnia się, że folder Szarotka istnieje

                persistentDb = Path.Combine(localDir, "szarotka_produkcyjna.db3");

                // Lokalnie robocza baza też może być w tym samym folderze Temp, ale z nazwą .db3
                runtimeDb = Path.Combine(Path.GetTempPath(), "szarotka_produkcyjna.db3");
            }

            // 3. Kopiowanie bazy z chmury (Storage) do szybkiego RAMu (/tmp) przy starcie
            if (File.Exists(persistentDb) && !File.Exists(runtimeDb))
            {
                File.Copy(persistentDb, runtimeDb, overwrite: true);
            }

            // 4. Budujemy Connection String do bazy roboczej w pamięci RAM
            string runtimeConnectionString = $"Data Source={runtimeDb};";

            // 5. Włączenie trybu WAL w bazie roboczej
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(runtimeConnectionString))
            {
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL; PRAGMA busy_timeout = 10000;";
                cmd.ExecuteNonQuery();
            }

            // 6. Rejestracja automatycznej usługi synchronizacji w tle
            services.AddHostedService(sp =>
                new DatabaseBackupService(
                    runtimeConnectionString,
                    persistentDb,
                    sp.GetRequiredService<ILogger<DatabaseBackupService>>()
                )
            );
            return runtimeDb;
        }
    }
}
