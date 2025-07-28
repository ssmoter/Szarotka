using DataBase.Data;

namespace Shared.Model
{
    public partial class HelperTable : DataBase.Model.BaseEntities<Guid>
    {
        [SQLite.Unique]
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
                ON CONFLICT({nameof(Name)}) 
                DO UPDATE SET 
                    {nameof(Name)} = '{helperTable.Name}', 
                    {nameof(Value)} = '{helperTable.Value}', 
                    {nameof(IsDelete)} = '{helperTable.IsDelete}', 
                    {nameof(UpdatedTicks)} = '{helperTable.UpdatedTicks}'";
            return sql;
        }
    }
}
