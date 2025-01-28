using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class ProductQuery
    {
        public static string SaveOrUpdate(Product product)
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
                        '{product.Id}',
                        '{product.DayId}',
                        '{product.ProductNameId}',
                        '{product.ProductPriceId}',
                        '{product.Description}',
                        {product.PriceTotal},
                        {product.PriceTotalCorrect},
                        {product.PriceTotalAfterCorrect},
                        {product.Number},
                        {product.NumberEdit},
                        {product.NumberReturn},
                        {product.CreatedTicks},
                        {product.UpdatedTicks},
                        {product.IsDelete},
                        '{product.UserCreatedId}',
                        '{product.UserUpdatedId}'
                    )
                    ON CONFLICT({nameof(Product.Id)}) DO UPDATE SET
                        {nameof(Product.DayId)} = '{product.DayId}',
                        {nameof(Product.ProductNameId)} = '{product.ProductNameId}',
                        {nameof(Product.ProductPriceId)} = '{product.ProductPriceId}',
                        {nameof(Product.Description)} = '{product.Description}',
                        {nameof(Product.PriceTotal)} = {product.PriceTotal},
                        {nameof(Product.PriceTotalCorrect)} = {product.PriceTotalCorrect},
                        {nameof(Product.PriceTotalAfterCorrect)} = {product.PriceTotalAfterCorrect},
                        {nameof(Product.Number)} = {product.Number},
                        {nameof(Product.NumberEdit)} = {product.NumberEdit},
                        {nameof(Product.NumberReturn)} = {product.NumberReturn},
                        {nameof(Product.CreatedTicks)} = {product.CreatedTicks},
                        {nameof(Product.UpdatedTicks)} = {product.UpdatedTicks},
                        {nameof(Product.IsDelete)} = {product.IsDelete},
                        {nameof(Product.UserCreatedId)} = '{product.UserCreatedId}',
                        {nameof(Product.UserUpdatedId)} = '{product.UserUpdatedId}';
                ";

            return sql;
        }

    }
}
