using DataBase.Data;

namespace Shared.Model
{
    public static class HelperTableExtension
    {
        public static async Task SetAsync(this HelperTable helperTable, IAccessDataBaseAoT accessDataBase)
        {
            var sql = HelperTable.SetQuery(helperTable);
            await accessDataBase.DbAsyncAoT.ExecuteAsync(sql, new
            {
                helperTable.Id,
                helperTable.Name,
                helperTable.Value,
                helperTable.CreatedTicks,
                helperTable.UpdatedTicks,
                helperTable.IsDelete,
                helperTable.UserCreatedId,
                helperTable.UserUpdatedId
            });
        }
        public static void Set(this HelperTable helperTable, IAccessDataBaseAoT accessDataBase)
        {
            var sql = HelperTable.SetQuery(helperTable);
            accessDataBase.DbSyncAoT.Execute(sql, new
            {
                helperTable.Id,
                helperTable.Name,
                helperTable.Value,
                helperTable.CreatedTicks,
                helperTable.UpdatedTicks,
                helperTable.IsDelete,
                helperTable.UserCreatedId,
                helperTable.UserUpdatedId
            });
        }

        public static HelperTable GetHelperTable(this string name, IAccessDataBaseAoT accessDataBase)
        {
            var query = HelperTable.GetQuery(name);
            var result = accessDataBase.DbSyncAoT.Query<HelperTable>(query, new {name});
            return result?.FirstOrDefault();
        }
        public static async Task<HelperTable> GetHelperTableAsync(this string name, IAccessDataBaseAoT accessDataBase)
        {
            var query = HelperTable.GetQuery(name);
            var result = await accessDataBase.DbAsyncAoT.QueryAsync<HelperTable>(query, new {name});
            return result?.FirstOrDefault();
        }

    }

}
