using DataBase.Data;
using DataBase.Model;
using DataBase.Model.SourceGenerator;

using Shared.CustomControls.FromCode;
using Shared.Helper;

using System.Text.Json;

namespace Shared.Data.ServerHttpClients
{
    public interface IUpdateLogsHttp
    {
        Task<IList<UpdateLog>> GetLogs(Guid logId, UpdateProgressBar progressContent, CancellationToken token = default);
    }

    public partial class UpdateLogsHttp(IAccessDataBaseAoT db, IHttpClientFactory httpClient) : IUpdateLogsHttp
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClient;
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<IList<UpdateLog>> GetLogs(Guid logId, UpdateProgressBar progressContent, CancellationToken token = default)
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            string url = $"update-logs/{logId}";
            var httpClient = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka);


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressContent.Grid);

            var response = await httpClient.DownloadAsync(url
                , (progress) => UpdateProgressBar.UpdateProgress(progressContent, progress)
                , token);

            response.HttpResponseMessage.EnsureSuccessStatusCode();

            var logs = JsonSerializer.Deserialize(response.content, SzarotkaJsonSerializerContext.Default.IListUpdateLog);
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            return logs;
        }


    }
}
