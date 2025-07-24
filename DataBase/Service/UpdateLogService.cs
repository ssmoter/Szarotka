using DataBase.Data;
using DataBase.Model;
using DataBase.Model.EntitiesRoutes;

namespace DataBase.Service
{
    public interface IUpdateLogService
    {
        Task<UpdateLog> Insert(UpdateLog update);
        Task<IList<UpdateLog>> Select(Guid id);
        Task<UpdateLog> SelectFirst(bool isServer, params UpdateEnum[] updateEnum);
    }

    public class UpdateLogService : IUpdateLogService
    {
        private readonly IAccessDataBase _db;
        private readonly ITimeService _timeService;

        public UpdateLogService(IAccessDataBase db, ITimeService timeService)
        {
            _db = db;
            _timeService = timeService;
        }

        public async Task<UpdateLog> Insert(UpdateLog update)
        {
            string sql = $@"INSERT INTO {nameof(UpdateLog)} 
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
                )";

            var time = _timeService.UtcNow();

            if (update.Id == Guid.Empty)
            {
                update.Id = Guid.CreateVersion7();
                update.Created = time;
                update.Updated = time;
            }

            await _db.DataBaseAsync.ExecuteAsync(sql,
                update.Id,
                (int)update.UpdateEnum,
                update.UpdateId,
                update.CreatedTicks,
                update.UpdatedTicks,
                update.IsDelete,
                update.UserCreatedId,
                update.UserUpdatedId,
                update.JsonUpdate,
                update.IsServer
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

            var result = await _db.DataBaseAsync.QueryAsync<UpdateLog>(sql, id);

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(result.Count, 0, "Don't find a record");

            sql = $@"
                {select}
                FROM {nameof(UpdateLog)}  WHERE {nameof(UpdateLog.CreatedTicks)} <= @{nameof(UpdateLog.CreatedTicks)}";

            var results = await _db.DataBaseAsync.QueryAsync<UpdateLog>(sql, result[0].CreatedTicks);

            return results;
        }

        public async Task<UpdateLog> SelectFirst(bool isServer, params UpdateEnum[] updateEnum)
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
                FROM {nameof(UpdateLog)}  
                WHERE {nameof(UpdateLog.IsServer)} == ?
            ";

            if (updateEnum.Length > 1)
            {

                sql += " AND (";
                for (int i = 0; i < updateEnum.Length; i++)
                {
                    if (i > 0)
                    {
                        sql += " OR ";
                    }
                    sql += $@" {nameof(UpdateLog.UpdateEnum)} == ?";
                }
                sql += ")";
            }
            else
            {
                sql += $@" AND {nameof(UpdateLog.UpdateEnum)} == ?";
            }


            sql += $@"
                ORDER By {nameof(UpdateLog.CreatedTicks)} DESC
                LIMIT 1";

            var parameters = (new object[] { isServer }).Concat(updateEnum.Cast<object>()).ToArray();
            var result = await _db.DataBaseAsync.QueryAsync<UpdateLog>(sql, parameters);

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(result.Count, 0, "Don't find a record");

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

        public static async Task<UpdateLog> Insert(this IUpdateLogService service, UpdateLog update, CustomerRoutes customer)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(customer, Model.JsonContext.SzarotkaJsonSerializerContext.Default.CustomerRoutes);
            update ??= new UpdateLog();
            update.JsonUpdate = json;
            update.UserCreatedId = customer.UserUpdatedId;
            update.UserUpdatedId = customer.UserUpdatedId;
            update.UpdateEnum = UpdateEnum.CustomerRoutes;
            update.UpdateId = customer.Id.ToString();
            return await service.Insert(update);
        }
    }
}
