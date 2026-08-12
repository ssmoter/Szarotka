using DataBase.Data;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

using System.Text.Json;

namespace DataBase.Service
{
    public interface IUpdateLogService
    {
        Task<UpdateLog> Insert(UpdateLog update);
        Task<IList<UpdateLog>> Select(Guid id);
        Task<UpdateLog> SelectFirst(bool isServer, params UpdateEnum[] updateEnum);
    }

    public class UpdateLogService(IAccessDataBaseAoT db, ITimeService timeService) : IUpdateLogService
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly ITimeService _timeService = timeService;

        public async Task<UpdateLog> Insert(UpdateLog update)
        {
            var time = _timeService.UtcNow();
            var halfHour = TimeSpan.FromMinutes(30).Ticks;

            string sqlSelect = $@"
SELECT *
FROM {nameof(UpdateLog)}
WHERE {nameof(UpdateLog)}.{nameof(UpdateLog.UpdateId)} == @{nameof(update.UpdateId)}
AND ABS({nameof(UpdateLog)}.{nameof(UpdateLog.UpdatedTicks)} - @{nameof(time.Ticks)} ) <= @{nameof(halfHour)}
";

            var Iexists = await _db.DbAsyncAoT.QueryAsync<UpdateLog>(sqlSelect,
                                                                    new()
                                                                    {
                                                                        [nameof(update.UpdateId)] = update.UpdateId,
                                                                        [nameof(time.Ticks)] = time.Ticks,
                                                                        [nameof(halfHour)] = halfHour
                                                                    });
            var exists = Iexists.ToList();

            string sqlInsert = $@"INSERT INTO {nameof(UpdateLog)} 
                (
                    {nameof(UpdateLog.Id)},
                    {nameof(UpdateLog.UpdateEnum)}, 
                    {nameof(UpdateLog.UpdateId)}, 
                    {nameof(UpdateLog.CreatedTicks)}, 
                    {nameof(UpdateLog.UpdatedTicks)}, 
                    {nameof(UpdateLog.IsDelete)}, 
                    {nameof(UpdateLog.UserCreatedId)}, 
                    {nameof(UpdateLog.UserUpdatedId)},
                    {nameof(UpdateLog.JsonUpdate)},
                    {nameof(UpdateLog.IsServer)}                    
                ) 
                VALUES 
                (
                    @{nameof(UpdateLog.Id)},
                    @{nameof(UpdateLog.UpdateEnum)}, 
                    @{nameof(UpdateLog.UpdateId)}, 
                    @{nameof(UpdateLog.CreatedTicks)}, 
                    @{nameof(UpdateLog.UpdatedTicks)}, 
                    @{nameof(UpdateLog.IsDelete)}, 
                    @{nameof(UpdateLog.UserCreatedId)}, 
                    @{nameof(UpdateLog.UserUpdatedId)},
                    @{nameof(UpdateLog.JsonUpdate)},
                    @{nameof(UpdateLog.IsServer)}
                )
            ON CONFLICT({nameof(UpdateLog.Id)}) DO UPDATE SET
                {nameof(UpdateLog.UpdateEnum)} = @{nameof(UpdateLog.UpdateEnum)},
                {nameof(UpdateLog.UpdateId)} = @{nameof(UpdateLog.UpdateId)},
                {nameof(UpdateLog.UpdatedTicks)} = @{nameof(UpdateLog.UpdatedTicks)},
                {nameof(UpdateLog.IsDelete)} = @{nameof(UpdateLog.IsDelete)},
                {nameof(UpdateLog.UserUpdatedId)} = @{nameof(UpdateLog.UserUpdatedId)},
                {nameof(UpdateLog.JsonUpdate)} = @{nameof(UpdateLog.JsonUpdate)},
                {nameof(UpdateLog.IsServer)} = @{nameof(UpdateLog.IsServer)}
";

            if (exists?.Count > 0)
            {
                update.Id = exists[0].Id;
                update.UserCreatedId = exists[0].UserCreatedId;
                update.CreatedTicks = exists[0].CreatedTicks;
                update.Created = time;
            }
            if (update.Id == Guid.Empty)
            {
                update.Id = Guid.CreateVersion7();
                update.Created = time;
                update.Updated = time;
            }

            await _db.DbAsyncAoT.ExecuteAsync(sqlInsert,
      new()
      {         
          [nameof(UpdateLog.Id)] = update.Id,
          [nameof(UpdateLog.UpdateEnum)] = update.UpdateEnum,
          [nameof(UpdateLog.UpdateId)] = update.UpdateId,
          [nameof(UpdateLog.CreatedTicks)] = update.CreatedTicks,
          [nameof(UpdateLog.UpdatedTicks)] = update.UpdatedTicks,
          [nameof(UpdateLog.IsDelete)] = update.IsDelete,
          [nameof(UpdateLog.UserCreatedId)] = update.UserCreatedId,
          [nameof(UpdateLog.UserUpdatedId)] = update.UserUpdatedId,
          [nameof(UpdateLog.JsonUpdate)] = update.JsonUpdate,
          [nameof(UpdateLog.IsServer)] = update.IsServer
      }
            );
            return update;
        }


        public async Task<IList<UpdateLog>> Select(Guid id)
        {
            string select = $@"SELECT 
                {nameof(UpdateLog.Id)}, 
                {nameof(UpdateLog.UpdateEnum)}, 
                {nameof(UpdateLog.UpdateId)}, 
                {nameof(UpdateLog.CreatedTicks)}, 
                {nameof(UpdateLog.UpdatedTicks)}, 
                {nameof(UpdateLog.IsDelete)}, 
                {nameof(UpdateLog.UserCreatedId)}, 
                {nameof(UpdateLog.UserUpdatedId)},
                {nameof(UpdateLog.JsonUpdate)},
                {nameof(UpdateLog.IsServer)}
                ";

            string sql = $@"
                {select}
                FROM {nameof(UpdateLog)}  WHERE {nameof(UpdateLog.Id)} == @id";

            UpdateLog[] result = [.. await _db.DbAsyncAoT.QueryAsync<UpdateLog>(sql, new() { [nameof(id)] = id })];

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(result.Length, 0, "Don't find a record");

            sql = $@"
                {select}
                FROM {nameof(UpdateLog)}  WHERE {nameof(UpdateLog.CreatedTicks)} <= @{nameof(UpdateLog.CreatedTicks)}";

            var results = await _db.DbAsyncAoT.QueryAsync<UpdateLog>(sql, new() { [nameof(UpdateLog.CreatedTicks)] = result[0].CreatedTicks });

            return [.. results];
        }

        public async Task<UpdateLog> SelectFirst(bool isServer, params UpdateEnum[] updateEnum)
        {
            string enums = JsonSerializer.Serialize(updateEnum);

            string select = $@"SELECT 
                {nameof(UpdateLog.Id)}, 
                {nameof(UpdateLog.UpdateEnum)}, 
                {nameof(UpdateLog.UpdateId)}, 
                {nameof(UpdateLog.CreatedTicks)}, 
                {nameof(UpdateLog.UpdatedTicks)}, 
                {nameof(UpdateLog.IsDelete)}, 
                {nameof(UpdateLog.UserCreatedId)}, 
                {nameof(UpdateLog.UserUpdatedId)},
                {nameof(UpdateLog.JsonUpdate)},
                {nameof(UpdateLog.IsServer)}
                FROM {nameof(UpdateLog)}  
                WHERE {nameof(UpdateLog.IsServer)} == @{nameof(isServer)} AND
                {nameof(UpdateLog.UpdateEnum)}
                IN (SELECT value FROM json_each(@{nameof(enums)}))
                ORDER By {nameof(UpdateLog.CreatedTicks)} DESC
                LIMIT 1
                ";
            UpdateLog[] result = [.. await _db.DbAsyncAoT.QueryAsync<UpdateLog>(select, new() { [nameof(isServer)] = isServer, [nameof(enums)] = enums })];

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(result.Length, 0, "Don't find a record");

            return result[0];
        }
    }

    public static class UpdateLogServiceExtensions
    {
        public static async Task<UpdateLog> SelectFirstFromDriversRoutes(this IUpdateLogService service, bool isServer)
        {
            UpdateLog result = await service.SelectFirst(isServer
                , UpdateEnum.CustomerRoutes
                , UpdateEnum.SelectedDayOfWeek
                , UpdateEnum.ResidentialAddress);

            return result;
        }




        public static async Task<UpdateLog> Insert(this IUpdateLogService service, UpdateLog update, ProductName name)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(name, Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.ProductName);
            update ??= new UpdateLog();
            update.JsonUpdate = json;
            update.UserCreatedId = name.UserUpdatedId;
            update.UserUpdatedId = name.UserUpdatedId;
            update.UpdateEnum = UpdateEnum.ProductName;
            update.UpdateId = name.Id.ToString();
            return await service.Insert(update);
        }
        public static async Task<UpdateLog> Insert(this IUpdateLogService service, UpdateLog update, ProductPrice price)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(price, Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.ProductPrice);
            update ??= new UpdateLog();
            update.JsonUpdate = json;
            update.UserCreatedId = price.UserUpdatedId;
            update.UserUpdatedId = price.UserUpdatedId;
            update.UpdateEnum = UpdateEnum.ProductPrice;
            update.UpdateId = price.Id.ToString();
            return await service.Insert(update);
        }
        public static async Task<UpdateLog> Insert(this IUpdateLogService service, UpdateLog update, CustomerRoutes customer)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(customer, Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.CustomerRoutes);
            update ??= new UpdateLog();
            update.JsonUpdate = json;
            update.UserCreatedId = customer.UserUpdatedId;
            update.UserUpdatedId = customer.UserUpdatedId;
            update.UpdateEnum = UpdateEnum.CustomerRoutes;
            update.UpdateId = customer.Id.ToString();
            return await service.Insert(update);
        }
        public static async Task<UpdateLog> Insert(this IUpdateLogService service, UpdateLog update, Day day)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(day, Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.Day);
            update ??= new UpdateLog();
            update.JsonUpdate = json;
            update.UserCreatedId = day.UserUpdatedId;
            update.UserUpdatedId = day.UserUpdatedId;
            update.UpdateEnum = UpdateEnum.CustomerRoutes;
            update.UpdateId = day.Id.ToString();
            return await service.Insert(update);
        }
    }
}
