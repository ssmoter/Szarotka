namespace Inventory.Helper
{
    public static class StoredProcedure
    {
        public static string GetSingleDay(string date)
        {
            var sql = $@"SELECT *
						,(SELECT json_group_array(json_object(
								'Id',Id,
								'DayId',DayId,
								'ProductNameId',ProductNameId,
								'ProductPriceId',ProductPriceId,
								'Description',Description,
								'PriceTotal',PriceTotal,
								'PriceTotalCorrect',PriceTotalCorrect,
								'PriceTotalAfterCorrect',PriceTotalAfterCorrect,
								'Number',Number,
								'NumberEdit',NumberEdit,
								'NumberReturn',NumberReturn
	
									,'ProductName',(SELECT json_object(
										'Id',Id,
										'Name',Name,
										'Description',Description,
										'Img',Img
										)
										FROM ProductName WHERE ProductName.Id == Product.ProductNameId)
					
									,'ProductPrice',(SELECT json_object(
										'Id',Id,
										'ProductNameId',ProductNameId,
										'Price',Price,
										'Created',Created
										)
										FROM ProductPrice WHERE ProductPrice.ProductNameId == Product.ProductNameId AND ProductPrice.Id == Product.ProductPriceId)

						)) 
						FROM Product 
						WHERE Product.DayId == Day.Id ) as ""ProductsJson""

					FROM Day 

					WHERE CreatedDate == ""{date}"" ";

            return sql;
        }
        public static string GetSingleDay(Guid Id)
        {
            var sql = $@"SELECT *
						,(SELECT json_group_array(json_object(
								'Id',Id,
								'DayId',DayId,
								'ProductNameId',ProductNameId,
								'ProductPriceId',ProductPriceId,
								'Description',Description,
								'PriceTotal',PriceTotal,
								'PriceTotalCorrect',PriceTotalCorrect,
								'PriceTotalAfterCorrect',PriceTotalAfterCorrect,
								'Number',Number,
								'NumberEdit',NumberEdit,
								'NumberReturn',NumberReturn
	
									,'ProductName',(SELECT json_object(
										'Id',Id,
										'Name',Name,
										'Description',Description,
										'Img',Img
										)
										FROM ProductName WHERE ProductName.Id == Product.ProductNameId)
					
									,'ProductPrice',(SELECT json_object(
										'Id',Id,
										'ProductNameId',ProductNameId,
										'Price',Price,
										'Created',Created
										)
										FROM ProductPrice WHERE ProductPrice.ProductNameId == Product.ProductNameId AND ProductPrice.Id == Product.ProductPriceId)

						)) 
						FROM Product 
						WHERE Product.DayId == Day.Id ) as ""ProductsJson""

					FROM Day 

					WHERE Id == ""{Id}"" ";

            return sql;
        }

    }
}
