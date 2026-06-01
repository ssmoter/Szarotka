using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Shared.CustomControls.FromCode;
using Shared.Data;
using Shared.Data.ServerHttpClients;
using Shared.Helper;

using System.Text.Json;

namespace Inventory.Data.InventoryApi
{
    public interface ISendProductHttp
    {
        Task<(HttpResponseMessage httpMessage, string content)> SendProduct(EmptyProduct product, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default);
        Task<(HttpResponseMessage httpMessage, string content)> SendProducts(EmptyProducts products, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default);
    }

    public partial class SendProductHttp : ISendProductHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessDataBase _db;
        private readonly string _url;
        public SendProductHttp(IAccessDataBase db, IHttpClientFactory httpClient)
        {
            _db = db;
            _url = db.GetServerUrl();
            _httpClientFactory = httpClient;
        }

        public async Task<(HttpResponseMessage httpMessage, string content)> SendProduct(EmptyProduct product, UpdateProgressBar progressBar = null, bool forceUpdate = false, CancellationToken token = default)
        {
            string url = $"{_url}/inventory/product/update{(forceUpdate ? "?forceUpdate=true" : "")}";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(product, DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.EmptyProduct);

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
            string url = $"{_url}/inventory/products/update{(forceUpdate ? "?forceUpdate=true" : "")}";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar?.Grid);

            var json = JsonSerializer.Serialize(products, DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.EmptyProducts);

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
