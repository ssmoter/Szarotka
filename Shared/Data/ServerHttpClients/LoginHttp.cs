using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;

using Shared.Helper;

using System.Text;
using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface ILoginHttp
    {
        Task<User> GetPublicUser(Guid id);
        Task<User> In(LoginUser register);
    }

    public class LoginHttp : ILoginHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessDataBase _db;
        private readonly string _url;
        public LoginHttp(IAccessDataBase db, IHttpClientFactory httpClient)
        {
            _db = db;
            _url = db.GetServerUrl();
            _httpClientFactory = httpClient;
        }


        public async Task<User> In(LoginUser login)
        {
            string url = _url + "/user/login";

            ArgumentNullException.ThrowIfNull(login);

            var request = JsonSerializer.Serialize(login, SzarotkaJsonSerializerContext.Default.LoginUser);

            var content = new StringContent(request, Encoding.UTF8, "application/json");

            using var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync(url, content);

            var json = await response.Content.ReadAsStringAsync();

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
        public async Task<User> GetPublicUser(Guid id)
        {
            string url = _url + $"/user/{id}";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            var response = await httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
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

    }
}
