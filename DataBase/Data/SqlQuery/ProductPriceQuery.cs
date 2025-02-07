using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class ProductPriceQuery
    {
        public static string SaveOrUpdate(ProductPrice productPrice)
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
                    '{productPrice.Id}', 
                    {productPrice.Price}, 
                    {productPrice.CreatedTicks}, 
                    {productPrice.UpdatedTicks}, 
                    '{productPrice.UserCreatedId}', 
                    '{productPrice.UserUpdatedId}',
                    '{productPrice.ProductNameId}',
                    {productPrice.IsDelete},
                    '{productPrice.ProductNameId}'
                )
                ON CONFLICT({nameof(ProductPrice.Id)}) DO UPDATE SET
                    {nameof(ProductPrice.Price)} = {productPrice.Price},
                    {nameof(ProductPrice.UpdatedTicks)} = {productPrice.UpdatedTicks},
                    {nameof(ProductPrice.UserUpdatedId)} = '{productPrice.UserUpdatedId}',
                    {nameof(ProductPrice.ProductNameId)} = '{productPrice.ProductNameId}',
                    {nameof(ProductPrice.IsDelete)} = {productPrice.IsDelete},
                    {nameof(ProductPrice.ProductNameId)} = '{productPrice.ProductNameId}';
            ";
            return sql;
        }
    }
}
