using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class CakeQuery
    {
        public static string SaveOrUpdate(Cake cake)
        {
            string sql = $@"
            INSERT INTO {nameof(Cake)} (
                {nameof(Cake.Id)}, 
                {nameof(Cake.DayId)}, 
                {nameof(Cake.IsSell)}, 
                {nameof(Cake.Price)}, 
                {nameof(Cake.CreatedTicks)}, 
                {nameof(Cake.UpdatedTicks)},
                {nameof(Cake.IsDelete)},
                {nameof(Cake.UserCreatedId)},
                {nameof(Cake.UserUpdatedId)}
            )
            VALUES (
                '{cake.Id}', 
                '{cake.DayId}', 
                {cake.IsSell}, 
                {cake.Price}, 
                {cake.CreatedTicks}, 
                {cake.UpdatedTicks},
                {cake.IsDelete},
                '{cake.UserCreatedId}',
                '{cake.UserUpdatedId}'
            )
            ON CONFLICT({nameof(Cake.Id)}) DO UPDATE SET
                {nameof(Cake.DayId)} = '{cake.DayId}',
                {nameof(Cake.IsSell)} = {cake.IsSell},
                {nameof(Cake.Price)} = {cake.Price},
                {nameof(Cake.CreatedTicks)} = {cake.CreatedTicks},
                {nameof(Cake.UpdatedTicks)} = {cake.UpdatedTicks},
                {nameof(Cake.IsDelete)} = {cake.IsDelete},
                {nameof(Cake.UserCreatedId)} = '{cake.UserCreatedId}',
                {nameof(Cake.UserUpdatedId)} = '{cake.UserUpdatedId}';
            ";
            return sql;
        }
    }
}
