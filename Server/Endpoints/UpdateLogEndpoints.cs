using Server.Requests;

namespace Server.Endpoints
{
    public static class UpdateLogEndpoints
    {

        public static void MapEndpoints(WebApplication app)
        {
            var map = app.MapGroup("/update-logs");

            map.MapGet("/{id}", async (string id, IUpdateLogRequests updateLog, CancellationToken token = default)=>
            {
                var logs = await updateLog.GetLogs(id, token);
                return logs;
            })
                .RequireAuthorization();



        }

    }
}
