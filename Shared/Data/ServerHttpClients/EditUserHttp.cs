using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Model.SourceGenerator;

using Shared.Helper;

using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface IEditUserHttp
    {
        Task<User> EditUser(User user);
    }

    public class EditUserHttp(IAccessDataBaseAoT db, IHttpClientFactory httpClient) : IEditUserHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<User> EditUser(User user)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            string url = $"user/edit";
            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

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
