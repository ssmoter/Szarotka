using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;

using System.Text;
using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface ILoginHttp
    {
        Task<User> In(LoginUser register);
    }

    public class LoginHttp : ILoginHttp
    {
        private readonly HttpClient _httpClient;
        private readonly AccessDataBase _db;
        private readonly string _url;
        public LoginHttp(AccessDataBase db, HttpClient httpClient)
        {
            _db = db;
            _url = db.GetServerUrl();
            _httpClient = httpClient;
        }



        public async Task<User> In(LoginUser login)
        {
            string url = _url + "/user/login";

            ArgumentNullException.ThrowIfNull(login);

            var request = JsonSerializer.Serialize(login, SzarotkaJsonSerializerContext.Default.LoginUser);

            var content = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var user = JsonSerializer.Deserialize<User>(json, SzarotkaJsonSerializerContext.Default.User);
                return user;
            }

            throw ValidationExceptionClient.ThrowValidationException(json);
        }


    }
}
