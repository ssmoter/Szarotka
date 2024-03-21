using DataBase.Model.EntitiesInventory;

namespace Inventory.Helper
{
    public static class StoredProcedure
    {
        public static string GetSingleDay(string date, string driverId)
        {
            var sql = $@"
SELECT *
,(SELECT json_group_array(json_object(
'Id',Product.Id,
'DayId',Product.DayId,
'ProductNameId',Product.ProductNameId,
'ProductPriceId',Product.ProductPriceId,
'Description',Product.Description,
'PriceTotal',Product.PriceTotal,
'PriceTotalCorrect',Product.PriceTotalCorrect,
'PriceTotalAfterCorrect',Product.PriceTotalAfterCorrect,
'Number',Product.Number,
'NumberEdit',Product.NumberEdit,
'NumberReturn',Product.NumberReturn

	,'Price',(SELECT json_object(
	'Id',Id,	
	'DayId',DayId,
	'ProductNameId',ProductNameId,
	'Price',Price,
	'CreatedTicks',CreatedTicks,
	'UpdatedTicks',UpdatedTicks
	)
	FROM ProductPrice WHERE ProductPrice.ProductNameId == Product.ProductNameId AND ProductPrice.Id == Product.ProductPriceId) 
	
	,'Name',(SELECT json_object(
	'Id',Id,
	'Name',Name,
	'Description',Description,
	'Img',Img,
	'Arrangement',Arrangement,
	'CreatedTicks',CreatedTicks,
	'UpdatedTicks',UpdatedTicks
	)
	FROM ProductName WHERE ProductName.Id == Product.ProductNameId)
)) 
FROM Product 
	LEFT JOIN ProductName 
	ON Product.ProductNameId == ProductName.Id
WHERE Product.DayId == Day.Id ORDER BY ProductName.Arrangement ) as 'ProductsJson'

,(SELECT json_group_array(json_object(
'Id',Id,
'DayId',DayId,
'IsSell',IsSell,
'Price',Price,
'CreatedTicks',CreatedTicks,
'UpdatedTicks',UpdatedTicks))
FROM Cake WHERE Cake.DayId == Day.Id
) as 'CakesJson'

FROM Day 
WHERE SelectedDateString == '{date}' 
AND DriverGuid == '{driverId}' ";

            return sql;
        }
        public static string GetSingleDay(DateTime date, string driverId)
        {
            var sql = $@"
SELECT *
,(SELECT json_group_array(json_object(
'Id',Product.Id,
'DayId',Product.DayId,
'ProductNameId',Product.ProductNameId,
'ProductPriceId',Product.ProductPriceId,
'Description',Product.Description,
'PriceTotal',Product.PriceTotal,
'PriceTotalCorrect',Product.PriceTotalCorrect,
'PriceTotalAfterCorrect',Product.PriceTotalAfterCorrect,
'Number',Product.Number,
'NumberEdit',Product.NumberEdit,
'NumberReturn',Product.NumberReturn,
'CreatedTicks',Product.CreatedTicks,
'UpdatedTicks',Product.UpdatedTicks

	,'Price',(SELECT json_object(
	'Id',Id,	
	'DayId',DayId,
	'ProductNameId',ProductNameId,
	'Price',Price,
	'CreatedTicks',CreatedTicks,
	'UpdatedTicks',UpdatedTicks
	)
	FROM ProductPrice WHERE ProductPrice.ProductNameId == Product.ProductNameId AND ProductPrice.Id == Product.ProductPriceId) 
	
	,'Name',(SELECT json_object(
	'Id',Id,
	'Name',Name,
	'Description',Description,
	'Img',Img,
	'Arrangement',Arrangement,
	'CreatedTicks',CreatedTicks,
	'UpdatedTicks',UpdatedTicks
	)
	FROM ProductName WHERE ProductName.Id == Product.ProductNameId)
)) 
FROM Product 
	LEFT JOIN ProductName 
	ON Product.ProductNameId == ProductName.Id
WHERE Product.DayId == Day.Id ORDER BY ProductName.Arrangement ) as 'ProductsJson'

,(SELECT json_group_array(json_object(
'Id',Id,
'DayId',DayId,
'IsSell',IsSell,
'Price',Price,
'CreatedTicks',CreatedTicks,
'UpdatedTicks',UpdatedTicks))
FROM Cake WHERE Cake.DayId == Day.Id
) as 'CakesJson'

FROM Day 
WHERE SelectedDateString == '{date}' 
AND DriverGuid == '{driverId}' ";

            return sql;
        }
        public static string GetSingleDay(Guid Id)
        {
            var sql = $@"
SELECT *
,(SELECT json_group_array(json_object(
'Id',Product.Id,
'DayId',Product.DayId,
'ProductNameId',Product.ProductNameId,
'ProductPriceId',Product.ProductPriceId,
'Description',Product.Description,
'PriceTotal',Product.PriceTotal,
'PriceTotalCorrect',Product.PriceTotalCorrect,
'PriceTotalAfterCorrect',Product.PriceTotalAfterCorrect,
'Number',Product.Number,
'NumberEdit',Product.NumberEdit,
'NumberReturn',Product.NumberReturn,
'CreatedTicks',Product.CreatedTicks,
'UpdatedTicks',Product.UpdatedTicks

	,'Price',(SELECT json_object(
	'Id',Id,	
	'DayId',DayId,
	'ProductNameId',ProductNameId,
	'Price',Price,
	'CreatedTicks',CreatedTicks,
	'UpdatedTicks',UpdatedTicks
	)
	FROM ProductPrice WHERE ProductPrice.ProductNameId == Product.ProductNameId AND ProductPrice.Id == Product.ProductPriceId) 
	
	,'Name',(SELECT json_object(
	'Id',Id,
	'Name',Name,
	'Description',Description,
	'Img',Img,
	'Arrangement',Arrangement,
	'CreatedTicks',CreatedTicks,
	'UpdatedTicks',UpdatedTicks
	)
	FROM ProductName WHERE ProductName.Id == Product.ProductNameId)
)) 
FROM Product 
	LEFT JOIN ProductName 
	ON Product.ProductNameId == ProductName.Id
WHERE Product.DayId == Day.Id ORDER BY ProductName.Arrangement ) as 'ProductsJson'

,(SELECT json_group_array(json_object(
'Id',Id,
'DayId',DayId,
'IsSell',IsSell,
'Price',Price,
'CreatedTicks',CreatedTicks,
'UpdatedTicks',UpdatedTicks))
FROM Cake WHERE Cake.DayId == Day.Id
) as 'CakesJson'

FROM Day 
WHERE Id == '{Id}' 
";

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
