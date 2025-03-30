using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class ProductPriceQuery
    {
        public static string SaveOrUpdate(
            Guid Id,
            int Price,
            long CreatedTicks,
            long UpdatedTicks,
            Guid UserCreatedId,
            Guid UserUpdatedId,
            Guid ProductNameId,
            bool IsDelete)
        {
            string sql = $@"
                INSERT INTO {nameof(ProductPrice)} (
                    {nameof(ProductPrice.Id)}, 
                    {nameof(ProductPrice.Price)}, 
                    {nameof(ProductPrice.CreatedTicks)}, 
                    {nameof(ProductPrice.UpdatedTicks)}, 
                    {nameof(ProductPrice.UserCreatedId)}, 
                    {nameof(ProductPrice.UserUpdatedId)},
                    {nameof(ProductPrice.ProductNameId)},
                    {nameof(ProductPrice.IsDelete)},
                    {nameof(ProductPrice.ProductNameId)}
                )
                VALUES (
                    @{nameof(Id)}, 
                    @{nameof(Price)}, 
                    @{nameof(CreatedTicks)}, 
                    @{nameof(UpdatedTicks)}, 
                    @{nameof(UserCreatedId)}, 
                    @{nameof(UserUpdatedId)},
                    @{nameof(ProductNameId)},
                    @{nameof(IsDelete)},
                    @{nameof(ProductNameId)}
                )
                ON CONFLICT({nameof(ProductPrice.Id)}) DO UPDATE SET
                    {nameof(ProductPrice.Price)} = @{nameof(Price)},
                    {nameof(ProductPrice.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                    {nameof(ProductPrice.UserUpdatedId)} = @{nameof(UserUpdatedId)},
                    {nameof(ProductPrice.ProductNameId)} = @{nameof(ProductNameId)},
                    {nameof(ProductPrice.IsDelete)} = @{nameof(IsDelete)},
                    {nameof(ProductPrice.ProductNameId)} = @{nameof(ProductNameId)};
            ";
            return sql;
        }
    }
}
