using DataBase.Data;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.JsonContext;

using Shared.CustomControls.FromCode;
using Shared.Data.ServerHttpClients;
using Shared.Helper;

using System.Collections.ObjectModel;
using System.Text.Json;

namespace DriversRoutes.Data.RouteApi
{
    public interface IGetCustomersHttp
    {
        Task<ObservableCollection<CustomerRoutes>> GetCustomerRoutes(Guid routeId, SelectedDayOfWeekRoutes day, UpdateProgressBar progress, CancellationToken token = default);
    }

    public class GetCustomersHttp : IGetCustomersHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessDataBase _db;
        private readonly string _url;
        public GetCustomersHttp(IAccessDataBase db, IHttpClientFactory httpClient)
        {
            _db = db;
            _url = db.GetServerUrl();
            _httpClientFactory = httpClient;
        }


        public async Task<ObservableCollection<CustomerRoutes>> GetCustomerRoutes(Guid routeId, SelectedDayOfWeekRoutes day, UpdateProgressBar progress, CancellationToken token = default)
        {
            string url = $"{_url}/driver-routes/customer-routes/{routeId}";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            var days = day.GetDayOfWeeks();


            if (days.Length > 0)
            {
                url += "?";
            }
            for (int i = 0; i < days.Length; i++)
            {
                if (i > 0)
                {
                    url += "&";
                }
                url += $"selected_day={days[i]}";
            }
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progress.Grid);

            var response = await httpClient.DownloadAsync(url, progress.ProgressBar, token);
            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var customer = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.CustomerRoutesArray);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return [.. customer];
        }

    }
}
