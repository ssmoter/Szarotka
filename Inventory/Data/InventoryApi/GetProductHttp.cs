using DataBase.Data;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.SourceGenerator;

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

    public partial class GetProductHttp(IHttpClientFactory httpClientFactory, IAccessDataBaseAoT db) : IGetProductHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<EmptyProducts> GetProducts(UpdateProgressBar progressContent = null, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();
            string url = $"/inventory/products/empty";
            using var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);

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
