using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;
using DataBase.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

    public class InventoryDayRequests(IAccessDataBase db,
                                IGetInventoryAoT getInventoryAoT,
                                ISaveInventoryAoT saveInventoryAoT,
                                DataBase.Service.IUpdateLogService updateLog,
                                ILogger<InventoryDayRequests>? logger = null) : IInventoryDayRequests
    {
        private readonly IAccessDataBase _db = db;
        private readonly IGetInventoryAoT _getInventoryAoT = getInventoryAoT;
        private readonly ISaveInventoryAoT _saveInventoryAoT = saveInventoryAoT;
        private DataBase.Service.IUpdateLogService _UpdateLog = updateLog;
        private readonly ILogger<InventoryDayRequests> _logger = logger ?? NullLogger<InventoryDayRequests>.Instance;

        public async Task<IResult> GetDay(string id, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetDay(id) started for id={Id}", id);
                if (!Guid.TryParse(id, out var guid))
                {
                    _logger.LogWarning("GetDay(id): id is not a valid Guid: {Id}", id);
                    return Results.BadRequest("id is not a valid Guid");
                }
                token.ThrowIfCancellationRequested();
                var day = await _getInventoryAoT.Day(guid);

                if (day is not null)
                {
                    _logger.LogInformation("GetDay(id): found day for id={Id}", id);
                    return Results.Ok(day);
                }
                _logger.LogInformation("GetDay(id): day not found for id={Id}", id);
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetDay(id) for id={Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetDay(id) for id={Id}", id);
                _db.SaveLog(ex);
                throw;
            }
        }
        public async Task<IResult> GetDay(string selectedDateString, string? userId, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetDay(selectedDateString) started for date={Date} userId={UserId}", selectedDateString, userId);
                if (string.IsNullOrWhiteSpace(selectedDateString))
                {
                    _logger.LogWarning("GetDay(selectedDateString): selectedDateString is empty");
                    return Results.BadRequest("selectedDateString is empty");
                }

                if (!DateTime.TryParse(selectedDateString, DataBase.Helper.Constants.CultureInfo, out DateTime dateTime))
                {
                    _logger.LogWarning("GetDay(selectedDateString): invalid DateTime string={Date}", selectedDateString);
                    return Results.BadRequest("selectedDateString is not a valid DateTime");
                }


                if (!Guid.TryParse(userId, out Guid user))
                {
                    _logger.LogWarning("GetDay(selectedDateString): userId is not a valid Guid: {UserId}", userId);
                    return Results.BadRequest("userId is not a valid format Guid");
                }

                token.ThrowIfCancellationRequested();

                var day = await _getInventoryAoT.DaySelectedDateString(selectedDateString, user);

                if (day is not null)
                {
                    _logger.LogInformation("GetDay(selectedDateString): found day for date={Date} userId={UserId}", selectedDateString, userId);
                    return Results.Ok(day);
                }
                _logger.LogInformation("GetDay(selectedDateString): not found for date={Date} userId={UserId}", selectedDateString, userId);
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetDay(selectedDateString) for date={Date} userId={UserId}", selectedDateString, userId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetDay(selectedDateString) for date={Date} userId={UserId}", selectedDateString, userId);
                _db.SaveLog(ex);
                throw;
            }
        }
        public async Task<IResult> GetDays(string? from, string? to, IList<string>? userIds, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetDays started from={From} to={To} userIdsCount={Count}", from, to, userIds?.Count ?? 0);
                token.ThrowIfCancellationRequested();

                if (!long.TryParse(from, out long fromLong))
                {
                    _logger.LogDebug("GetDays: unable to parse 'from'={From}", from);
                }
                if (!long.TryParse(to, out long toLong))
                {
                    _logger.LogDebug("GetDays: unable to parse 'to'={To}", to);
                }
                List<Guid> userIdGuids = [];
                for (int i = 0; i < userIds?.Count; i++)
                {
                    if (Guid.TryParse(userIds[i], out Guid result))
                    {
                        userIdGuids.Add(result);
                    }
                    else
                    {
                        _logger.LogWarning("GetDays: skipping invalid userId at index {Index}: {UserId}", i, userIds[i]);
                    }
                }
                token.ThrowIfCancellationRequested();

                var days = await _getInventoryAoT.Days(fromLong, toLong, userIdGuids);

                if (days is not null)
                {
                    _logger.LogInformation("GetDays: returning {Count} days", days.Count);
                    return Results.Ok(days);
                }
                _logger.LogInformation("GetDays: no days found");
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetDays");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetDays");
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
                    _logger.LogWarning("SaveDay: day.Id is empty");
                    throw new ArgumentNullException(nameof(day), "Id can't be null");
                }

                token.ThrowIfCancellationRequested();

                (bool, Day?) check = await ModelsDifferences.Check(_getInventoryAoT, day, forceUpdate);

                if (check.Item1)
                {
                    await _saveInventoryAoT.SaveDay(day, day.UserUpdatedId.ToByteArray(), true);
                    var firstLog = await _UpdateLog.Insert(new DataBase.Model.UpdateLog(), day);
                    _logger.LogInformation("SaveDay: saved dayId={DayId} and created logId={LogId}", day.Id, firstLog?.Id);
                    return Results.Created(firstLog?.Id.ToString(), firstLog);
                }

                return Results.Conflict(check.Item2);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in SaveDay for dayId={DayId}", day?.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SaveDay for dayId={DayId}", day?.Id);
                _db.SaveLog(ex);
                throw;
            }
        }
        public async Task<IResult> SaveDays(IList<Day> days, bool forceUpdate = false, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("SaveDays started for {Count} days", days?.Count ?? 0);
                ArgumentNullException.ThrowIfNull(days, nameof(days));
                token.ThrowIfCancellationRequested();

                List<UpdateDifference> differences = [];
                UpdateLog? firstLog = null;

                foreach (Day day in days)
                {
                    _logger.LogDebug("SaveDays: processing dayId={DayId}", day?.Id);
                    (bool, Day?) check = await ModelsDifferences.Check(_getInventoryAoT, day, forceUpdate);

                    if (check.Item1)
                    {
                        await _saveInventoryAoT.SaveDay(day, day.UserUpdatedId.ToByteArray(), true);
                        var update = await _UpdateLog.Insert(new DataBase.Model.UpdateLog(), day);
                        firstLog ??= update;
                        _logger.LogInformation("SaveDays: saved dayId={DayId} with logId={LogId}", day.Id, update?.Id);
                    }
                    else
                    {
                        differences.Add(new UpdateDifference()
                        {
                            Server = check.Item2,
                            Update = day,
                        });
                        _logger.LogInformation("SaveDays: conflict for dayId={DayId}", day.Id);
                    }
                }

                if (differences.Count > 0)
                {
                    _logger.LogInformation("SaveDays: {Count} conflicts found", differences.Count);
                    return Results.Conflict(differences);
                }
                _logger.LogInformation("SaveDays: all days saved, returning logId={LogId}", firstLog?.Id);
                return Results.Created(firstLog?.Id.ToString(), firstLog);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in SaveDays");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SaveDays");
                _db.SaveLog(ex);
                throw;
            }
        }

    }
}
