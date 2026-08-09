using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Model.SourceGenerator;

using Shared.Helper;

using System.Text;
using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface ILoginHttp
    {
        Task<User> GetPublicUser(Guid id, CancellationToken token = default);
        Task<User> In(LoginUser register, CancellationToken token = default);
    }

    public partial class LoginHttp(IAccessDataBaseAoT db, IHttpClientFactory httpClient) : ILoginHttp, IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<User> In(LoginUser login, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            string url =  "user/login";

            ArgumentNullException.ThrowIfNull(login);

            var request = JsonSerializer.Serialize(login, SzarotkaJsonSerializerContext.Default.LoginUser);

            var content = new StringContent(request, Encoding.UTF8, "application/json");

            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            var response = await httpClient.PostAsync(url, content, token);

            var json = await response.Content.ReadAsStringAsync(token);

            if (string.IsNullOrWhiteSpace(json))
            {
                response.EnsureSuccessStatusCode();
            }

            if (response.IsSuccessStatusCode)
            {
                var user = JsonSerializer.Deserialize<User>(json, SzarotkaJsonSerializerContext.Default.User);
                return user;
            }

            throw ValidationExceptionClient.ThrowValidationException(json);
        }
        public async Task<User> GetPublicUser(Guid id, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            string url = $"user/{id}";
            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            httpClient.SetAuthorization();

            var response = await httpClient.GetAsync(url, token);
            var json = await response.Content.ReadAsStringAsync(token);
            if (string.IsNullOrWhiteSpace(json))
            {
                response.EnsureSuccessStatusCode();
            }
            if (response.IsSuccessStatusCode)
            {
                var user = JsonSerializer.Deserialize<User>(json, SzarotkaJsonSerializerContext.Default.User);
                return user;
            }
            throw ValidationExceptionClient.ThrowValidationException(json);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
