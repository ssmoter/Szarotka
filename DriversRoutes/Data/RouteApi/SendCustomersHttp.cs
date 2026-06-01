using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using Shared.CustomControls.FromCode;
using Shared.Data;
using Shared.Data.ServerHttpClients;
using Shared.Helper;

using System.Text.Json;

namespace DriversRoutes.Data.RouteApi
{
    public interface ISendCustomersHttp
    {
        Task<HttpResponseMessage> SendCustomerRoute(CustomerRoutes customers, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default);
        Task<HttpResponseMessage> SendCustomerRoutes(IList<CustomerRoutes> customers, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default);
    }

    public class SendCustomersHttp : ISendCustomersHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessDataBase _db;
        private readonly string _url;
        public SendCustomersHttp(IAccessDataBase db, IHttpClientFactory httpClient)
        {
            _db = db;
            _url = db.GetServerUrl();
            _httpClientFactory = httpClient;
        }


        public async Task<HttpResponseMessage> SendCustomerRoute(CustomerRoutes customers, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default)
        {
            string url = $"{_url}/driver-routes/update{(forceUpdate ? "?forceUpdate=true" : "")}";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(customers, DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.CustomerRoutes);

            var response = await httpClient.PostWithProgressAsync(url, json,
                (progress) => { UpdateProgressBar.UpdateProgress(progressBar, progress); }
                , token);

            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                var responseJson = await response.Content.ReadAsStringAsync(token);
                _db.SaveLog(new Exception(responseJson));
            }
            return response;
        }

        public async Task<HttpResponseMessage> SendCustomerRoutes(IList<CustomerRoutes> customers, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default)
        {
            string url = $"{_url}/driver-routes/updates{(forceUpdate ? "?forceUpdate=true" : "")}";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(customers, DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.IListCustomerRoutes);

            var response = await httpClient.PostWithProgressAsync(url, json,
                (progress) => { UpdateProgressBar.UpdateProgress(progressBar, progress); }
                , token);

            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                var responseJson = await response.Content.ReadAsStringAsync(token);
                _db.SaveLog(new Exception(responseJson));
            }
            return response;
        }
    }
}
