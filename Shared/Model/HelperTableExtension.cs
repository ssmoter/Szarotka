using DataBase.Data;

namespace Shared.Model
{
    public static class HelperTableExtension
    {
        public static async Task SetAsync(this HelperTable helperTable, IAccessDataBaseAoT accessDataBase)
        {
            var sql = HelperTable.SetQuery(helperTable);
            await accessDataBase.DbAsyncAoT.ExecuteAsync(sql, new()
            {
                [nameof(helperTable.Id)] = helperTable.Id,
                [nameof(helperTable.Name)] = helperTable.Name,
                [nameof(helperTable.Value)] = helperTable.Value,
                [nameof(helperTable.CreatedTicks)] = helperTable.CreatedTicks,
                [nameof(helperTable.UpdatedTicks)] = helperTable.UpdatedTicks,
                [nameof(helperTable.IsDelete)] = helperTable.IsDelete,
                [nameof(helperTable.UserCreatedId)] = helperTable.UserCreatedId,
                [nameof(helperTable.UserUpdatedId)] = helperTable.UserUpdatedId
            });
        }
        public static void Set(this HelperTable helperTable, IAccessDataBaseAoT accessDataBase)
        {
            var sql = HelperTable.SetQuery(helperTable);
            accessDataBase.DbSyncAoT.Execute(sql, new()
            {
                [nameof(helperTable.Id)] = helperTable.Id,
                [nameof(helperTable.Name)] = helperTable.Name,
                [nameof(helperTable.Value)] = helperTable.Value,
                [nameof(helperTable.CreatedTicks)] = helperTable.CreatedTicks,
                [nameof(helperTable.UpdatedTicks)] = helperTable.UpdatedTicks,
                [nameof(helperTable.IsDelete)] = helperTable.IsDelete,
                [nameof(helperTable.UserCreatedId)] = helperTable.UserCreatedId,
                [nameof(helperTable.UserUpdatedId)] = helperTable.UserUpdatedId
            });
        }

        public static HelperTable GetHelperTable(this string name, IAccessDataBaseAoT accessDataBase)
        {
            var query = HelperTable.GetQuery(name);
            var result = accessDataBase.DbSyncAoT.Query<HelperTable>(query, new() { [nameof(name)] = name });
            return result?.FirstOrDefault();
        }
        public static async Task<HelperTable> GetHelperTableAsync(this string name, IAccessDataBaseAoT accessDataBase)
        {
            var query = HelperTable.GetQuery(name);
            var result = await accessDataBase.DbAsyncAoT.QueryAsync<HelperTable>(query, new() { [nameof(name)] = name });
            return result?.FirstOrDefault();
        }

    }

}
