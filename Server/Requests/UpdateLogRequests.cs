using DataBase.Data;
using DataBase.Service;

namespace Server.Requests
{
    public interface IUpdateLogRequests
    {
        Task<IResult> GetLogs(string id, CancellationToken token = default);
    }

    public class UpdateLogRequests(IAccessDataBase db,
                             IUpdateLogService update) : IUpdateLogRequests
    {
        private readonly IAccessDataBase _db = db;
        private readonly IUpdateLogService _update = update;

        public async Task<IResult> GetLogs(string id, CancellationToken token = default)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    return Results.BadRequest("id is not a valid Guid");
                }
                token.ThrowIfCancellationRequested();

                var lastLogs = await _update.Select(idGuid);

                if (lastLogs is not null)
                {
                    return Results.Ok(lastLogs);
                }
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
        }


    }
}
