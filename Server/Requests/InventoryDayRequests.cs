using DataBase.Data;
using DataBase.Data.Get;

namespace Server.Requests
{
    public interface IInventoryDayRequests
    {
        Task<IResult> GetDay(string id, CancellationToken token = default);
        Task<IResult> GetDay(string selectedDateString, string? userId, CancellationToken token = default);
        Task<IResult> GetDays(string? from, string? to, IList<string>? userIds, CancellationToken token = default);
    }

    public class InventoryDayRequests : IInventoryDayRequests
    {
        private readonly IAccessDataBase _db;
        private readonly IGetInventoryAoT _getInventoryAoT;
        public InventoryDayRequests(IAccessDataBase db, IGetInventoryAoT getInventoryAoT)
        {
            _db = db;
            _getInventoryAoT = getInventoryAoT;
        }

        public async Task<IResult> GetDay(string id, CancellationToken token = default)
        {
            try
            {
                if (!Guid.TryParse(id, out var guid))
                {
                    return Results.BadRequest("id is not a valid Guid");
                }
                token.ThrowIfCancellationRequested();
                var day = await _getInventoryAoT.Day(guid);

                if (day is not null)
                {
                    return Results.Ok(day);
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
        public async Task<IResult> GetDay(string selectedDateString, string? userId, CancellationToken token = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(selectedDateString))
                {
                    return Results.BadRequest("selectedDateString is empty");
                }

                if (!DateTime.TryParse(selectedDateString, out DateTime dateTime))
                {
                    return Results.BadRequest("selectedDateString is not a valid DateTime");
                }

                token.ThrowIfCancellationRequested();

                _ = Guid.TryParse(userId, out Guid user);

                var day = await _getInventoryAoT.DaySelectedDateString(selectedDateString, user);

                if (day is not null)
                {
                    return Results.Ok(day);
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
        public async Task<IResult> GetDays(string? from, string? to, IList<string>? userIds, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                if (!long.TryParse(from, out long fromLong))
                { }
                if (!long.TryParse(to, out long toLong))
                { }
                List<Guid> userIdGuids = [];
                for (int i = 0; i < userIds?.Count; i++)
                {
                    if (Guid.TryParse(userIds[i], out Guid result))
                    {
                        userIdGuids.Add(result);
                    }
                }

                token.ThrowIfCancellationRequested();

                var days = await _getInventoryAoT.Days(fromLong, toLong, userIdGuids);

                if (days is not null)
                {
                    return Results.Ok(days);
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
