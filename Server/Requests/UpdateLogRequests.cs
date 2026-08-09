using DataBase.Data;
using DataBase.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Requests
{
    public interface IUpdateLogRequests
    {
        Task<IResult> GetLogs(string id, CancellationToken token = default);
    }

    public class UpdateLogRequests(IAccessDataBaseAoT db,
                             IUpdateLogService update,
                             ILogger<UpdateLogRequests>? logger = null) : IUpdateLogRequests
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly IUpdateLogService _update = update;
        private readonly ILogger<UpdateLogRequests> _logger = logger ?? NullLogger<UpdateLogRequests>.Instance;

        public async Task<IResult> GetLogs(string id, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetLogs started for id={Id}", id);
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    _logger.LogWarning("GetLogs: id is not a valid Guid: {Id}", id);
                    return Results.BadRequest("id is not a valid Guid");
                }
                token.ThrowIfCancellationRequested();

                var lastLogs = await _update.Select(idGuid);

                if (lastLogs is not null)
                {
                    _logger.LogInformation("GetLogs: returning {Count} logs for id={Id}", lastLogs.Count, id);
                    return Results.Ok(lastLogs);
                }
                _logger.LogInformation("GetLogs: no logs found for id={Id}", id);
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetLogs for id={Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetLogs for id={Id}", id);
                _db.SaveLog(ex);
                throw;
            }
        }


    }
}
