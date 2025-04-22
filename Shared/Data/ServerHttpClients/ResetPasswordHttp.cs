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

    public class ResetPasswordHttp : IResetPasswordHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessDataBase _db;
        private readonly string _url;
        public ResetPasswordHttp(IAccessDataBase db, IHttpClientFactory httpClient)
        {
            _db = db;
            _url = db.GetServerUrl();
            _httpClientFactory = httpClient;
        }

        public async Task SendEmail(string email)
        {
            string url = _url + "/user/reset-password-email/" + email;

            using var httpClient = _httpClientFactory.CreateClient();

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
            string url = _url + "/user/reset-password/" + code;

            using var httpClient = _httpClientFactory.CreateClient();

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
            string url = $"{_url}/user/reset-password/{code}/{password}";

            using var httpClient = _httpClientFactory.CreateClient();

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
