using DataBase.Data;

namespace Shared.Model
{
    public partial class HelperTable : DataBase.Model.BaseEntities<Guid>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HelperTable()
        { }
        public HelperTable(string name, string value)
        {
            var now = DateTime.Now;
            Name = name;
            Value = value;
            Id = Guid.CreateVersion7();
            Created = now;
            Updated = now;
        }
        public static string GetQuery(string name)
        {
            string sql = $@"SELECT 
                         {nameof(Id)}, 
                         {nameof(Name)}, 
                         {nameof(Value)},  
                         {nameof(CreatedTicks)},  
                         {nameof(UpdatedTicks)}, 
                         {nameof(IsDelete)}  
                         FROM {nameof(HelperTable)} WHERE {nameof(Name)} = '{name}'";
            return sql;
        }
        public static string SetQuery(HelperTable helperTable)
        {
            string sql = $@"
                INSERT INTO {nameof(HelperTable)} (
                    {nameof(Id)}, 
                    {nameof(Name)}, 
                    {nameof(Value)}, 
                    {nameof(CreatedTicks)}, 
                    {nameof(UpdatedTicks)}, 
                    {nameof(IsDelete)}
                )
                VALUES (
                    '{helperTable.Id}', 
                    '{helperTable.Name}', 
                    '{helperTable.Value}', 
                    '{helperTable.CreatedTicks}', 
                    '{helperTable.UpdatedTicks}', 
                    '{helperTable.IsDelete}'
                )
                ON CONFLICT({nameof(Id)}) 
                DO UPDATE SET 
                    {nameof(Name)} = '{helperTable.Name}', 
                    {nameof(Value)} = '{helperTable.Value}', 
                    {nameof(UpdatedTicks)} = '{helperTable.UpdatedTicks}'";
            return sql;
        }
        public static async Task<HelperTable> Get(string name, IAccessDataBase accessDataBase)
        {
            List<HelperTable> result = [];
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var sql = HelperTable.GetQuery(name);
                    result = await accessDataBase.DataBaseAsync.QueryAsync<HelperTable>(sql);

                    return result.FirstOrDefault();
                }
                catch (Exception)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }

            }
            return result.FirstOrDefault();
        }
    }
    public static class HelperTableExtension
    {
        public static string Set(this HelperTable helperTable)
        {
            return HelperTable.SetQuery(helperTable);
        }
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
    }
}
