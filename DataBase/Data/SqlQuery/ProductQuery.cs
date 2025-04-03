using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class ProductQuery
    {
        public static string SaveOrUpdate(
            Guid Id,
            Guid DayId,
            Guid ProductNameId,
            Guid ProductPriceId,
            string Description,
            int PriceTotal,
            int PriceTotalCorrect,
            int PriceTotalAfterCorrect,
            int Number,
            int NumberEdit,
            int NumberReturn,
            long CreatedTicks,
            long UpdatedTicks,
            bool IsDelete,
            Guid UserCreatedId,
            Guid UserUpdatedId)
        {
            string sql = $@"
                        INSERT INTO {nameof(Product)} (
                            {nameof(Product.Id)},
                            {nameof(Product.DayId)},
                            {nameof(Product.ProductNameId)},
                            {nameof(Product.ProductPriceId)},
                            {nameof(Product.Description)},
                            {nameof(Product.PriceTotal)},
                            {nameof(Product.PriceTotalCorrect)},
                            {nameof(Product.PriceTotalAfterCorrect)},
                            {nameof(Product.Number)},
                            {nameof(Product.NumberEdit)},
                            {nameof(Product.NumberReturn)},
                            {nameof(Product.CreatedTicks)},
                            {nameof(Product.UpdatedTicks)},
                            {nameof(Product.IsDelete)},
                            {nameof(Product.UserCreatedId)},
                            {nameof(Product.UserUpdatedId)}
                        )
                        VALUES (
                            @{nameof(Id)},
                            @{nameof(DayId)},
                            @{nameof(ProductNameId)},
                            @{nameof(ProductPriceId)},
                            @{nameof(Description)},
                            @{nameof(PriceTotal)},
                            @{nameof(PriceTotalCorrect)},
                            @{nameof(PriceTotalAfterCorrect)},
                            @{nameof(Number)},
                            @{nameof(NumberEdit)},
                            @{nameof(NumberReturn)},
                            @{nameof(CreatedTicks)},
                            @{nameof(UpdatedTicks)},
                            @{nameof(IsDelete)},
                            @{nameof(UserCreatedId)},
                            @{nameof(UserUpdatedId)}
                        )
                        ON CONFLICT({nameof(Product.Id)}) DO UPDATE SET
                            {nameof(Product.DayId)} = @{nameof(DayId)},
                            {nameof(Product.ProductNameId)} = @{nameof(ProductNameId)},
                            {nameof(Product.ProductPriceId)} = @{nameof(ProductPriceId)},
                            {nameof(Product.Description)} = @{nameof(Description)},
                            {nameof(Product.PriceTotal)} = @{nameof(PriceTotal)},
                            {nameof(Product.PriceTotalCorrect)} = @{nameof(PriceTotalCorrect)},
                            {nameof(Product.PriceTotalAfterCorrect)} = @{nameof(PriceTotalAfterCorrect)},
                            {nameof(Product.Number)} = @{nameof(Number)},
                            {nameof(Product.NumberEdit)} = @{nameof(NumberEdit)},
                            {nameof(Product.NumberReturn)} = @{nameof(NumberReturn)},
                            {nameof(Product.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                            {nameof(Product.IsDelete)} = @{nameof(IsDelete)},
                            {nameof(Product.UserUpdatedId)} = @{nameof(UserUpdatedId)};
                    ";

            return sql;
        }





    }
}
