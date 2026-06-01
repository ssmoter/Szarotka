using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

namespace Server.Requests
{
    public interface IInventoryDayRequests
    {
        Task<IResult> GetDay(string id, CancellationToken token = default);
        Task<IResult> GetDay(string selectedDateString, string? userId, CancellationToken token = default);
        Task<IResult> GetDays(string? from, string? to, IList<string>? userIds, CancellationToken token = default);
        Task<IResult> SaveDay(Day day, bool forceUpdate = false, CancellationToken token = default);
        Task<IResult> SaveDays(IList<Day> days, bool forceUpdate = false, CancellationToken token = default);
    }

    public class InventoryDayRequests : IInventoryDayRequests
    {
        private readonly IAccessDataBase _db;
        private readonly IGetInventoryAoT _getInventoryAoT;
        private readonly ISaveInventoryAoT _saveInventoryAoT;
        private DataBase.Service.IUpdateLogService _UpdateLog;
        public InventoryDayRequests(IAccessDataBase db,
                                    IGetInventoryAoT getInventoryAoT,
                                    ISaveInventoryAoT saveInventoryAoT,
                                    DataBase.Service.IUpdateLogService updateLog)
        {
            _db = db;
            _getInventoryAoT = getInventoryAoT;
            _saveInventoryAoT = saveInventoryAoT;
            _UpdateLog = updateLog;
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

                if (!DateTime.TryParse(selectedDateString, DataBase.Helper.Constants.CultureInfo, out DateTime dateTime))
                {
                    return Results.BadRequest("selectedDateString is not a valid DateTime");
                }


                if (!Guid.TryParse(userId, out Guid user))
                {
                    return Results.BadRequest("userId is not a valid format Guid");
                }

                token.ThrowIfCancellationRequested();

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



        public async Task<IResult> SaveDay(Day day, bool forceUpdate = false, CancellationToken token = default)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(day, nameof(day));

                if (day.Id == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(day), "Id can't be null");
                }

                token.ThrowIfCancellationRequested();

                (bool, Day?) check = await ModelsDifferences.Check(_getInventoryAoT, day, forceUpdate);

                if (check.Item1)
                {
                    await _saveInventoryAoT.SaveDay(day, day.UserUpdatedId.ToByteArray(), true);
                    var firstLog = await _UpdateLog.Insert(new DataBase.Model.UpdateLog(), day);
                    return Results.Created(firstLog?.Id.ToString(), firstLog);
                }

                return Results.Conflict(check.Item2);
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
        public async Task<IResult> SaveDays(IList<Day> days, bool forceUpdate = false, CancellationToken token = default)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(days, nameof(days));
                token.ThrowIfCancellationRequested();

                List<UpdateDifference> differences = [];
                UpdateLog? firstLog = null;

                foreach (Day day in days)
                {
                    (bool, Day?) check = await ModelsDifferences.Check(_getInventoryAoT, day, forceUpdate);

                    if (check.Item1)
                    {
                        await _saveInventoryAoT.SaveDay(day, day.UserUpdatedId.ToByteArray(), true);
                        var update = await _UpdateLog.Insert(new DataBase.Model.UpdateLog(), day);
                        firstLog ??= update;
                    }
                    else
                    {
                        differences.Add(new UpdateDifference()
                        {
                            Server = check.Item2,
                            Update = day,
                        });
                    }
                }

                if (differences.Count > 0)
                {
                    return Results.Conflict(differences);
                }
                return Results.Created(firstLog?.Id.ToString(), firstLog);
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
