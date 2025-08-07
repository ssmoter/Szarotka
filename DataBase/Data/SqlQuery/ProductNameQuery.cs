using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class ProductNameQuery
    {
        public static string SaveOrUpdate(
            Guid Id,
            int Arrangement,
            string Name,
            string Description,
            string Img,
            bool IsVisible,
            long CreatedTicks,
            long UpdatedTicks,
            Guid UserCreatedId,
            Guid UserUpdatedId,
            bool IsDelete)
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
                        @{nameof(Id)}, 
                        @{nameof(Arrangement)}, 
                        @{nameof(Name)}, 
                        @{nameof(Description)}, 
                        @{nameof(Img)}, 
                        @{nameof(IsVisible)}, 
                        @{nameof(CreatedTicks)}, 
                        @{nameof(UpdatedTicks)}, 
                        @{nameof(UserCreatedId)}, 
                        @{nameof(UserUpdatedId)},
                        @{nameof(IsDelete)}
                    )
                    ON CONFLICT({nameof(ProductName.Id)}) DO UPDATE SET
                        {nameof(ProductName.Arrangement)} = @{nameof(Arrangement)},
                        {nameof(ProductName.Name)} = @{nameof(Name)},
                        {nameof(ProductName.Description)} = @{nameof(Description)},
                        {nameof(ProductName.Img)} = @{nameof(Img)},
                        {nameof(ProductName.IsVisible)} = @{nameof(IsVisible)},
                        {nameof(ProductName.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                        {nameof(ProductName.UserUpdatedId)} = @{nameof(UserUpdatedId)},
                        {nameof(ProductName.IsDelete)} = @{nameof(IsDelete)};
                ";

            return sql;
        }


        public static string GetNameAndPrice(bool isDelete = false)
        {
            var sql = $@"
SELECT {nameof(ProductName)}.*, 
       COALESCE((
           SELECT json_group_array(json_object(
               '{nameof(ProductPrice.Id)}', {nameof(ProductPrice)}.{nameof(ProductPrice.Id)},
               '{nameof(ProductPrice.Price)}', {nameof(ProductPrice.Price)},
               '{nameof(ProductPrice.CreatedTicks)}', {nameof(ProductPrice)}.{nameof(ProductPrice.CreatedTicks)},
               '{nameof(ProductPrice.UpdatedTicks)}', {nameof(ProductPrice)}.{nameof(ProductPrice.UpdatedTicks)},
               '{nameof(ProductPrice.UserCreatedId)}', {nameof(ProductPrice)}.{nameof(ProductPrice.UserCreatedId)},
               '{nameof(ProductPrice.UserUpdatedId)}', {nameof(ProductPrice)}.{nameof(ProductPrice.UserUpdatedId)},
               '{nameof(ProductPrice.IsDelete)}', {nameof(ProductPrice)}.{nameof(ProductPrice.IsDelete)}
           )) 
           FROM {nameof(ProductPrice)}
           WHERE {nameof(ProductPrice)}.{nameof(ProductPrice.ProductNameId)} = {nameof(ProductName)}.{nameof(ProductName.Id)}
       ), '[]') AS JsonPrice
FROM {nameof(ProductName)}

{(!isDelete ? 
$@"WHERE (
{nameof(ProductName)}.{nameof(ProductName.IsDelete)} == {isDelete}
OR {nameof(ProductName)}.{nameof(ProductName.IsDelete)} IS NULL
)" : "")}
;";

            return sql;
        }

    }
}
