using DataBase.Data;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.SourceGenerator;

using Shared.CustomControls.FromCode;
using Shared.Data.ServerHttpClients;

using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace DriversRoutes.Data.RouteApi
{
    public interface IGetCustomersHttp
    {
        Task<ObservableCollection<CustomerRoutes>> GetCustomerRoutes(Guid routeId, SelectedDayOfWeekRoutes day, UpdateProgressBar progressContent = null, CancellationToken token = default);
        Task<ObservableCollection<CustomerRoutes>> GetCustomerRoutes(Guid[] ids, UpdateProgressBar progressContent = null, CancellationToken token = default);
    }

    public class GetCustomersHttp(IAccessDataBaseAoT db, IHttpClientFactory httpClient) : IGetCustomersHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<ObservableCollection<CustomerRoutes>> GetCustomerRoutes(Guid[] ids, UpdateProgressBar progressContent = null, CancellationToken token = default)
        {
            StringBuilder url = new();
            url.Append("/driver-routes/customer-routes?");

            for (int i = 0; i < ids.Length; i++)
            {
                if (i != 0)
                {
                    url.Append('&');
                }
                url.Append("ids=");
                url.Append(ids[i]);
            }

            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);

            var response = await httpClient.DownloadAsync(url.ToString()
                , (double progress) => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);

            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var customer = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.CustomerRoutesArray);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return [.. customer];
        }
        public async Task<ObservableCollection<CustomerRoutes>> GetCustomerRoutes(Guid routeId, SelectedDayOfWeekRoutes day, UpdateProgressBar progressContent, CancellationToken token = default)
        {
            string url = $"/driver-routes/customer-routes/{routeId}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

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
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);

            var response = await httpClient.DownloadAsync(url
                , (double progress) => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);

            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var customer = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.CustomerRoutesArray);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return [.. customer];
        }

    }
}
