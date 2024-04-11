namespace Inventory.Helper
{
    public static class StoredProcedure
    {
        public static string GetProductWitchPriceAndName()
        {
            string sql = @"
SELECT 
Product.Id,
Product.PriceTotal,
Product.PriceTotalCorrect,
Product.PriceTotalAfterCorrect,
Product.DayId,
Product.ProductNameId,
Product.ProductPriceId,
Product.Description,
Product.Number,
Product.NumberEdit,
Product.NumberReturn,
Product.CreatedTicks,
Product.UpdatedTicks
,
	(SELECT json_object(
	'Id',Id
	,'Name',Name
	,'Description',Description
	,'Img',Img
	,'Arrangement',Arrangement
	,'CreatedTicks',CreatedTicks
	,'UpdatedTicks',UpdatedTicks
	)
	FROM ProductName 
	WHERE ProductName.Id == Product.ProductNameId) AS 'JsonName'
	,(SELECT json_object(
	'Id',Id
	,'ProductNameId',ProductNameId
	,'Price',Price
	,'CreatedTicks',CreatedTicks
	,'UpdatedTicks',UpdatedTicks
	)
	FROM ProductPrice 
WHERE ProductPrice.Id == Product.ProductPriceId) as 'JsonPrice'
FROM Product 
LEFT JOIN ProductName ON ProductName.Id == Product.ProductNameId
WHERE Product.DayId == ? 
ORDER BY ProductName.Arrangement";
            return sql;
        }
        public static string GetAllProductsNameAndPrice()
        {
            string sql = $@"
SELECT * 
,(SELECT json_object(
'Id',Id,
'ProductNameId',ProductNameId,
'Price',Price,
'CreatedTicks',CreatedTicks,
'UpdatedTicks',UpdatedTicks
)
FROM ProductPrice WHERE ProductPrice.ProductNameId == ProductName.Id ORDER BY CreatedTicks DESC LIMIT 1) as 'JsonPrice'

FROM ProductName 
WHERE ProductName.IsVisible == true
ORDER BY Arrangement
";

            return sql;
        }

    }
}
