using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface IRegisterHttp
    {
        Task<User> ConfirmEmail(string code);
        Task<bool> PostNewAccount(RegisterUser user);
    }

    public class RegisterHttp : IRegisterHttp
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessDataBase _db;
        private string _url;

        public RegisterHttp(HttpClient httpClient, IAccessDataBase db)
        {
            _httpClient = httpClient;
            _db = db;
            _url = _db.GetServerUrl();
        }

        public async Task<bool> PostNewAccount(RegisterUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var request = JsonSerializer.Serialize(user, SzarotkaJsonSerializerContext.Default.RegisterUser);

            var content = new StringContent(request, Encoding.UTF8, "application/json");

            var url = _url + "/user" + "/register";

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }
            var json = await response.Content.ReadAsStringAsync();

            throw ValidationExceptionClient.ThrowValidationException(json);
        }

        public async Task<User> ConfirmEmail(string code)
        {
            ArgumentNullException.ThrowIfNull(code);

            var ulr = _url + "/user" + "/confirm_email/" + code;

            var response = await _httpClient.GetAsync(ulr);

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
