using DataBase.Data;

namespace Shared.Model
{
    public static class HelperTableExtension
    {
        public static async Task SetAsync(this HelperTable helperTable, IAccessDataBase accessDataBase)
        {
            var sql = HelperTable.SetQuery(helperTable);
            await accessDataBase.DataBaseAsync.ExecuteAsync(sql);
        }
        public static void Set(this HelperTable helperTable, IAccessDataBase accessDataBase)
        {
            var sql = HelperTable.SetQuery(helperTable);
            accessDataBase.DataBase.Execute(sql);
        }

        public static HelperTable GetHelperTable(this string name, IAccessDataBase accessDataBase)
        {
            var query = HelperTable.GetQuery(name);
            var result = accessDataBase.DataBase.Query<HelperTable>(query);
            return result?.FirstOrDefault();
        }
        public static async Task<HelperTable> GetHelperTableAsync(this string name, IAccessDataBase accessDataBase)
        {
            var query = HelperTable.GetQuery(name);
            var result = await accessDataBase.DataBaseAsync.QueryAsync<HelperTable>(query);
            return result?.FirstOrDefault();
        }

    }

}
