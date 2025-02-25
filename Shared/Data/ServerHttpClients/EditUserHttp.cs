using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;

using Shared.Helper;

using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface IEditUserHttp
    {
        Task<User> EditUser(User user);
    }

    public class EditUserHttp : IEditUserHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessDataBase _db;
        private readonly string _url;
        public EditUserHttp(IAccessDataBase db, IHttpClientFactory httpClient)
        {
            _db = db;
            _url = db.GetServerUrl();
            _httpClientFactory = httpClient;
        }


        public async Task<User> EditUser(User user)
        {
            string url = _url + $"/user/edit";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            var response = await httpClient.PostAsJsonAsync(url, user, SzarotkaJsonSerializerContext.Default.User);
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                response.EnsureSuccessStatusCode();
            }
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<User>(json, SzarotkaJsonSerializerContext.Default.User);
                return result;
            }

            throw ValidationExceptionClient.ThrowValidationException(json);
        }

    }
}
