using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Shared.CustomControls.FromCode;
using Shared.Data.ServerHttpClients;

using System.Text.Json;

namespace Inventory.Data.InventoryApi
{
    public interface ISendProductHttp
    {
        Task<(HttpResponseMessage httpMessage, string content)> SendProduct(EmptyProduct product, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default);
        Task<(HttpResponseMessage httpMessage, string content)> SendProducts(EmptyProducts products, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default);
    }

    public partial class SendProductHttp(IAccessDataBaseAoT db, IHttpClientFactory httpClient) : ISendProductHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<(HttpResponseMessage httpMessage, string content)> SendProduct(EmptyProduct product, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default)
        {
            string url = $"/inventory/product/update{(forceUpdate ? "?forceUpdate=true" : "")}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(product, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.EmptyProduct);

            var response = await httpClient.PostWithProgressAsync(url, json,
                (progress) => { UpdateProgressBar.UpdateProgress(progressBar, progress); }
                , token);

            var responseJson = await response.Content.ReadAsStringAsync(token);

            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                _db.SaveLog(new Exception(json));
            }

            return (response, responseJson);
        }

        public async Task<(HttpResponseMessage httpMessage, string content)> SendProducts(EmptyProducts products, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default)
        {
            string url = $"/inventory/products/update{(forceUpdate ? "?forceUpdate=true" : "")}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(products, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.EmptyProducts);

            var response = await httpClient.PostWithProgressAsync(url, json,
                (progress) => { UpdateProgressBar.UpdateProgress(progressBar, progress); }
                , token);

            var responseJson = await response.Content.ReadAsStringAsync(token);

            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                _db.SaveLog(new Exception(json));
            }

            return (response, responseJson);
        }

    }
}
