using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Shared.CustomControls.FromCode;
using Shared.Data.ServerHttpClients;

using System.Text.Json;

namespace Inventory.Data.InventoryApi
{
    public interface ISendDayHttp
    {
        Task<HttpResponseMessage> SendDay(Day day, bool forceUpdate = false, UpdateProgressBar progressBar = null, CancellationToken token = default);
        Task<HttpResponseMessage> SendDays(IList<Day> days, bool forceUpdate = false, UpdateProgressBar progressBar = null, CancellationToken token = default);
    }

    public partial class SendDayHttp(IAccessDataBaseAoT db, IHttpClientFactory httpClient) : ISendDayHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBaseAoT _db = db;


        public async Task<HttpResponseMessage> SendDay(Day day, bool forceUpdate = false, UpdateProgressBar progressBar = null, CancellationToken token = default)
        {
            string url = $"/inventory/day/update{(forceUpdate ? "?forceUpdate=true" : "")}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(day, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.Day);

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
        public async Task<HttpResponseMessage> SendDays(IList<Day> days, bool forceUpdate = false, UpdateProgressBar progressBar = null, CancellationToken token = default)
        {
            string url = $"/inventory/days/update{(forceUpdate ? "?forceUpdate=true" : "")}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(days, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListDay);

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
