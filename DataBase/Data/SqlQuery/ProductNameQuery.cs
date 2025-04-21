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


        public static string GetNameAndPrice()
        {
            var sql = @"
SELECT pn.*, 
       COALESCE((
           SELECT json_group_array(json_object(
               'Id', pp.Id,
               'Price', pp.Price,
               'CreatedTicks', pp.CreatedTicks,
               'UpdatedTicks', pp.UpdatedTicks,
               'UserCreatedId', pp.UserCreatedId,
               'UserUpdatedId', pp.UserUpdatedId
           )) 
           FROM ProductPrice pp 
           WHERE pp.ProductNameId = pn.Id
       ), '[]') AS JsonPrice
FROM ProductName pn;
";

            return sql;
        }

    }
}
