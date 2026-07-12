using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;

using System.Text;
using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface IRegisterHttp
    {
        Task<User> ConfirmEmail(string code);
        Task<bool> PostNewAccount(RegisterUser user);
    }

    public class RegisterHttp(IHttpClientFactory httpClient, IAccessDataBase db) : IRegisterHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBase db = db;

        public async Task<bool> PostNewAccount(RegisterUser user)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            ArgumentNullException.ThrowIfNull(user);

            var request = JsonSerializer.Serialize(user, SzarotkaJsonSerializerContext.Default.RegisterUser);

            var content = new StringContent(request, Encoding.UTF8, "application/json");

            var url = "user/register";

            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                response.EnsureSuccessStatusCode();
            }
            throw ValidationExceptionClient.ThrowValidationException(json);
        }

        public async Task<User> ConfirmEmail(string code)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            ArgumentNullException.ThrowIfNull(code);

            var ulr =  "user/confirm-email/" + code;
            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            var response = await httpClient.GetAsync(ulr);
            response.EnsureSuccessStatusCode();

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
