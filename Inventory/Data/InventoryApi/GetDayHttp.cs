using DataBase.Data;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.SourceGenerator;

using Shared.CustomControls.FromCode;
using Shared.Data.ServerHttpClients;

using System.Text.Json;

namespace Inventory.Data.InventoryApi
{
    public interface IGetDayHttp
    {
        Task<Day> GetDay(Guid id, UpdateProgressBar progressContent = null, CancellationToken token = default);
        Task<Day> GetDay(string selectedDateString, Guid userId, UpdateProgressBar progressContent = null, CancellationToken token = default);
        Task<IList<Day>> GetDays(long from, long to, Guid[] userId, UpdateProgressBar progressContent = null, CancellationToken token = default);
        Task<IList<Day>> GetDays(IList<Guid> Ids, UpdateProgressBar progressContent = null, CancellationToken token = default);
    }

    public partial class GetDayHttp(IHttpClientFactory httpClientFactory, IAccessDataBaseAoT db) : IGetDayHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<Day> GetDay(Guid id, UpdateProgressBar progressContent = null, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();
            string url = $"/inventory/day/{id}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);

            var response = await httpClient.DownloadAsync(url
                , progress => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);

            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.Day);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return result;
        }
        public async Task<Day> GetDay(string selectedDateString, Guid userId, UpdateProgressBar progressContent = null, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();
            string url = $"/inventory/day?selectedDateString={selectedDateString}&userId={userId}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);

            var response = await httpClient.DownloadAsync(url
                , progress => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);

            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.Day);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return result;
        }

        public async Task<IList<Day>> GetDays(long from, long to, Guid[] userIds, UpdateProgressBar progressContent = null, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();
            string url = $"/inventory/days?from={from}&to={to}";

            foreach (Guid id in userIds)
            {
                url += $"&userId={id}";
            }

            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);

            var response = await httpClient.DownloadAsync(url
                , progress => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);

            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.DayArray);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return result;
        }

        public async Task<IList<Day>> GetDays(IList<Guid> Ids, UpdateProgressBar progressContent = null, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();
            string url = $"/inventory/days_from_ids";

            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);
            var jsonContent = JsonSerializer.Serialize(Ids, SzarotkaJsonSerializerContext.Default.IListGuid);

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);
            var response = await httpClient.PostWithProgressAsync(url, jsonContent
                , progress => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);


            response.EnsureSuccessStatusCode();

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            progressContent?.Title = "Pobieranie";
            progressContent?.Description = "Anuluj";
            progressContent?.ProgressBar.Progress = 0;
            var contentLength = response.Content.Headers.ContentLength ?? 1;

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);
            using var download = await response.Content.ReadAsStreamAsync(token);

            var contentString = await HttpClientExtension.CheckProgress(progress => UpdateProgressBar.UpdateProgress(progressContent, progress),
                   contentLength, download, token);


            //var contentString = await response.Content.ReadAsStringAsync(token);

            var result = JsonSerializer.Deserialize(contentString, SzarotkaJsonSerializerContext.Default.DayArray);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return result;
        }


    }
}
