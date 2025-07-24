using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using Shared.CustomControls.FromCode;
using Shared.Data.ServerHttpClients;
using Shared.Helper;

using System.Collections;
using System.Text.Json;

namespace DriversRoutes.Data.RouteApi
{
    public interface ISendCustomersHttp
    {
        Task<HttpResponseMessage> SendCustomerRoutes(IList<CustomerRoutes> customers, UpdateProgressBar progressBar, bool forceUpdate = false, CancellationToken token = default);
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

        public async Task<HttpResponseMessage> SendCustomerRoutes(IList<CustomerRoutes> customers, UpdateProgressBar progressBar, bool forceUpdate = false, CancellationToken token = default)
        {
            string url = $"{_url}/driver-routes/updates{(forceUpdate ? "?forceUpdate=true" : "")}";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar.Grid);

            var json = JsonSerializer.Serialize(customers, DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.IListCustomerRoutes);

            var response = await httpClient.PostWithProgressAsync(url, json,
                (progress) => { UpdateProgressBar.UpdateProgress(progressBar, progress); }
                , token);

            //Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return response;
        }
    }
}
