using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class ProductNameQuery
    {
        public static string SaveOrUpdate(ProductName productName)
        {
            string sql = $@"
                INSERT INTO {nameof(ProductName)} (
                    {nameof(ProductName.Id)}, 
                    {nameof(ProductName.Arrangement)}, 
                    {nameof(ProductName.Name)}, 
                    {nameof(ProductName.Description)}, 
                    {nameof(ProductName.Img)}, 
                    {nameof(ProductName.IsVisible)}, 
                    {nameof(ProductName.CreatedTicks)}, 
                    {nameof(ProductName.UpdatedTicks)}, 
                    {nameof(ProductName.UserCreatedId)}, 
                    {nameof(ProductName.UserUpdatedId)},
                    {nameof(ProductName.IsDelete)}
                )
                VALUES (
                    '{productName.Id}', 
                    {productName.Arrangement}, 
                    '{productName.Name}', 
                    '{productName.Description}', 
                    '{productName.Img}', 
                    {productName.IsVisible}, 
                    {productName.CreatedTicks}, 
                    {productName.UpdatedTicks}, 
                    '{productName.UserCreatedId}', 
                    '{productName.UserUpdatedId}',
                    {productName.IsDelete}
                )
                ON CONFLICT({nameof(ProductName.Id)}) DO UPDATE SET
                    {nameof(ProductName.Arrangement)} = {productName.Arrangement},
                    {nameof(ProductName.Name)} = '{productName.Name}',
                    {nameof(ProductName.Description)} = '{productName.Description}',
                    {nameof(ProductName.Img)} = '{productName.Img}',
                    {nameof(ProductName.IsVisible)} = {productName.IsVisible},
                    {nameof(ProductName.Updated)} = '{productName.Updated}',
                    {nameof(ProductName.UpdatedTicks)} = {productName.UpdatedTicks},
                    {nameof(ProductName.UserUpdatedId)} = '{productName.UserUpdatedId}',
                    {nameof(ProductName.IsDelete)} = {productName.IsDelete};
            ";

            return sql;
        }
    }
}
