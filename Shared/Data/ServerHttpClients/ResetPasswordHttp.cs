using DataBase.Data;
using DataBase.Model.EntitiesServer;

namespace Shared.Data.ServerHttpClients
{
    public interface IResetPasswordHttp
    {
        Task ResetPassword(int code, string password);
        Task SendCode(int code);
        Task SendEmail(string email);
    }

    public class ResetPasswordHttp(IAccessDataBase db, IHttpClientFactory httpClient) : IResetPasswordHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBase _db = db;

        public async Task SendEmail(string email)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            string url =  "user/reset-password-email/" + email;

            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            var response = await httpClient.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                response.EnsureSuccessStatusCode();
                return;
            }

            throw ValidationExceptionClient.ThrowValidationException(json);
        }
        public async Task SendCode(int code)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            string url = "user/reset-password/" + code;

            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            var response = await httpClient.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                response.EnsureSuccessStatusCode();
                return;
            }
            throw ValidationExceptionClient.ThrowValidationException(json);
        }
        public async Task ResetPassword(int code, string password)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            string url = $"user/reset-password/{code}/{password}";

            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            var response = await httpClient.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                response.EnsureSuccessStatusCode();
                return;
            }
            throw ValidationExceptionClient.ThrowValidationException(json);
        }

    }
}
