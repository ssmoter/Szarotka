using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class CakeQuery
    {
        public static string SaveOrUpdate(
            Guid Id,
            Guid DayId,
            bool IsSell,
            int Price,
            long CreatedTicks,
            long UpdatedTicks,
            bool IsDelete,
            Guid UserCreatedId,
            Guid UserUpdatedId)
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
                @{nameof(Id)}, 
                @{nameof(DayId)}, 
                @{nameof(IsSell)}, 
                @{nameof(Price)}, 
                @{nameof(CreatedTicks)}, 
                @{nameof(UpdatedTicks)},
                @{nameof(IsDelete)},
                @{nameof(UserCreatedId)},
                @{nameof(UserUpdatedId)}
            )
            ON CONFLICT({nameof(Cake.Id)}) DO UPDATE SET
                {nameof(Cake.DayId)} = @{nameof(DayId)},
                {nameof(Cake.IsSell)} = @{nameof(IsSell)},
                {nameof(Cake.Price)} = @{nameof(Price)},
                {nameof(Cake.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                {nameof(Cake.IsDelete)} = @{nameof(IsDelete)},
                {nameof(Cake.UserUpdatedId)} = @{nameof(UserUpdatedId)};
            ";
            return sql;
        }
    }
}
