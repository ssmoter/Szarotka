using DataBase.Data;
using DataBase.Data.MySqliteConnection;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using DataBaseUnitTest.DataSave;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Moq;

using Server.Model;

using Shared.CustomControls.FromCode;
using Shared.Data.ServerHttpClients;
using Shared.Helper;

using System.Runtime.CompilerServices;

namespace DataBaseUnitTest
{
    [CollectionDefinition("Serwer")]
    public class ServerCollection : ICollectionFixture<CustomWebApplicationFactory>
    {
        // Ta klasa pozostaje pusta. Służy tylko jako konfiguracja dla xUnit.
    }

    // Dodajemy interfejs IAsyncLifetime, aby xUnit pozwolił nam na bezpieczną inicjalizację asynchroniczną
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public const string TestJwtSecret = "81234CFB77034ECCDDD547F5SADFAASADSFGAFGDFAEWFCVZXVB";

        public AccessDataBaseAoT? TestDatabase { get; private set; }
        public ITimeService? TimeService { get; private set; }
        public RegisterUser? User { get; private set; }
        public Server.Service.IAuthenticationService? JwtToken { get; private set; }
        public IList<CustomerRoutes> AllCustomers { get; private set; } = [];
        public IList<Day> AllDays { get; private set; } = [];
        public IList<EmptyProduct> AllProducts { get; private set; } = [];


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "HttpClients:szarotka","http://localhost/"},
                });
            });

            // !!! USUNIĘTO 'async' z deklaracji poniżej - teraz jest to w 100% synchroniczne !!!
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IAccessDataBaseAoT>();
                services.RemoveAll<IHttpClientFactory>();


                services.AddSingleton<ITimeService, CurrentUtc>();

                services.AddSingleton<ISqliteConnectionFactory>(opt =>
                {
                    string dbName = Helper.GetPath + "1TestDB.db3";
                    return new SqliteConnectionFactory(dbName);
                });

                services.AddScoped<IMyDbConnection, MyDbConnection>();
                services.AddScoped<IMyDbAsyncConnection, MyDbAsyncConnection>();

                services.AddScoped<IAccessDataBaseAoT, AccessDataBaseAoT>();


                JwtToken = new Server.Service.AuthenticationService(new JSONWebTokensSettings
                {
                    Key = TestJwtSecret,
                    Issuer = "localhost",
                    Audience = "localhostUsers",
                    DurationInAccessToken = 100,
                    DurationInRefreshTokenLong = 1
                }, TestDatabase!);

                // 3. Autoryzujemy i ustawiamy sesję (tutaj bezpiecznie robimy await)

                var mockAuthService = new Mock<IAuthService>();
                //_mockTimeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);

                services.AddSingleton<IAuthService>(opt =>
                {
                    User = SetAutorizedUser();
                    var token = JwtToken.AuthenticateAsyncAccess(User).GetAwaiter().GetResult();
                    token = JwtToken.AuthenticateAsyncRefresh(User).GetAwaiter().GetResult();
                    mockAuthService.Setup(x => x.GetAccessTokenAsync()).ReturnsAsync(token.AccessToken);
                    mockAuthService.Setup(x => x.GetRefreshTokenAsync()).ReturnsAsync(token.RefreshToken);
                    mockAuthService.Setup(x => x.SaveTokensAsync(It.IsAny<string>(), It.IsAny<RefreshToken>())).Returns(Task.CompletedTask);
                    mockAuthService.Setup(x => x.SaveTokens(It.IsAny<string>(), It.IsAny<RefreshToken>()));
                    UserAfterLogin.SetLoginUser(token).GetAwaiter().GetResult();

                    return mockAuthService.Object;
                });

                services.AddTransient<AuthHeaderHandler>();

                services.AddSingleton<IHttpClientFactory>(new LocalHttpClientFactory(this));
            });

        }

        // Ta metoda wykona się AUTOMATYCZNIE zaraz po uruchomieniu serwera, ale PRZED pierwszym testem.
        // Tutaj bezpiecznie możemy używać await!
        public async Task InitializeAsync()
        {
            // 4. Odpalamy Twoją inicjalizację bazy danych
            var db = await InitDatabase();
            TestDatabase = db;
            // 1. Inicjalizujemy użytkownika
            User = SetAutorizedUser(); // Metoda zmieniona na synchroniczną, szczegóły niżej

            // 2. Tworzymy serwis JWT (TestDatabase jest już wstrzyknięte przez DI)
            JwtToken = new Server.Service.AuthenticationService(new JSONWebTokensSettings
            {
                Key = TestJwtSecret,
                Issuer = "localhost",
                Audience = "localhostUsers",
                DurationInAccessToken = 100,
                DurationInRefreshTokenLong = 1
            }, TestDatabase!);

            // 3. Autoryzujemy i ustawiamy sesję (tutaj bezpiecznie robimy await)
            var token = await JwtToken.AuthenticateAsyncAccess(User);
            ref IAccessDataBaseAoT? privateDbField = ref SetDB(null);
            privateDbField = db;

            AllDays = await DataBaseUnitTest.DataGet.Helper.SetExampleDays(10, db);
            AllCustomers = DataBaseUnitTest.DataGet.Helper.SetExampleCustomerRoutes(10, db);

            db.Dispose();
        }

        public async Task<AccessDataBaseAoT> InitDatabase()
        {
            var db = TestDatabase;
            if (TestDatabase == null)
            {
                string dbName = Helper.GetPath + "1TestDB.db3";
                dbName = Helper.AddDBIfDontHave(dbName);
                db = DataSave.Helper.CreatedDataBaseUpdateLogForTest(dbName).GetAwaiter().GetResult();
                db.DataBase.CreateTable<RefreshToken>();
            }
            return db!;
        }

        // Zmienione na synchroniczne, ponieważ ta metoda buduje tylko czysty obiekt i konfigurację w pamięci
        private RegisterUser SetAutorizedUser()
        {
            var user = new RegisterUser
            {
                Id = Guid.NewGuid(),
                Email = "from@example.com",
                Password = "Password1!",
                Created = DateTime.MinValue,
                IsDelete = false,
                IsEmailConfirm = true,
            };

            var emailConfig = new EmailConfiguration
            {
                From = "from@example.com",
                SmtpServer = "smtp.example.com",
                Port = 587,
                UserName = "username",
                Password = "password"
            };

            var inMemorySettings = new Dictionary<string, string?>
            {
                { "EmailConfiguration:From", emailConfig.From },
                { "EmailConfiguration:SmtpServer", emailConfig.SmtpServer },
                { "EmailConfiguration:Port", emailConfig.Port.ToString() },
                { "EmailConfiguration:UserName", emailConfig.UserName },
                { "EmailConfiguration:Password", emailConfig.Password }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return user;
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return Task.CompletedTask;
        }


        [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "_db")]
        extern static ref IAccessDataBaseAoT? SetDB(
        [UnsafeAccessorType("Shared.Helper.UserAfterLogin,Shared")] object? dummy
        );
        [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "ServerUrl")]
        extern static void SetUrl(
           [UnsafeAccessorType("DataBase.Helper.Constants,DataBase")] object? dummy, string value
        );


    }

    public class LocalHttpClientFactory : IHttpClientFactory
    {
        private readonly CustomWebApplicationFactory _factory;

        public LocalHttpClientFactory(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            Shared.CustomControls.FromCode.UpdateProgressBar.MainThreadDispatcher = new FakeMainThreadDispatcher();
        }

        public HttpClient CreateClient(string name)
        {
            var authService = _factory.Services.GetRequiredService<IAuthService>();
            var serviceProvider = _factory.Services.GetRequiredService<IServiceProvider>();
            var httpclientfaktory = _factory.Services.GetRequiredService<IHttpClientFactory>();
            var auth = new AuthHeaderHandler(authService, serviceProvider, httpclientfaktory);
            return _factory.CreateDefaultClient(auth);
        }
    }

    public class FakeMainThreadDispatcher : IMainThreadDispatcher
    {
        public void BeginInvokeOnMainThread(Action action)
        {
            // Ignorujemy akcję
        }
    }
}
