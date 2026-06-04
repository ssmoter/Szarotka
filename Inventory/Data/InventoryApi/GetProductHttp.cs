using DataBase.Data;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.JsonContext;

using Shared.CustomControls.FromCode;
using Shared.Data;
using Shared.Data.ServerHttpClients;
using Shared.Helper;

using System.Text.Json;

namespace Inventory.Data.InventoryApi
{
    public interface IGetProductHttp
    {
        Task<EmptyProducts> GetProducts(UpdateProgressBar progressContent = null, CancellationToken token = default);
    }

    public partial class GetProductHttp(IHttpClientFactory httpClientFactory, IAccessDataBase db) : IGetProductHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IAccessDataBase _db = db;
        private readonly string _url = db.GetServerUrl();

        public async Task<EmptyProducts> GetProducts(UpdateProgressBar progressContent = null, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();
            string url = $"{_url}/inventory/products/empty";
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.SetAuthorization();

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent?.Grid);

            var response = await httpClient.DownloadAsync(url
                , progress => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);

            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.EmptyProducts);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return result;
        }




    }
}
