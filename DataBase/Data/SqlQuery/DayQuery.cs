using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class DayQuery
    {

        public static string SaveOrUpdate(
            Guid Id,
            string Description,
            Guid DriverGuid,
            string SelectedDateString,
            long SelectedDateTicks,
            int TotalPriceProducts,
            int TotalPriceCake,
            int TotalPrice,
            int TotalPriceCorrect,
            int TotalPriceAfterCorrect,
            int TotalPriceMoney,
            int TotalPriceDifference,
            long CreatedTicks,
            long UpdatedTicks,
            bool IsDelete,
            Guid UserCreatedId,
            Guid UserUpdatedId)
        {
            return $@"
                INSERT INTO {nameof(Day)} (
                    {nameof(Day.Id)}, 
                    {nameof(Day.Description)}, 
                    {nameof(Day.DriverGuid)}, 
                    {nameof(Day.SelectedDateString)}, 
                    {nameof(Day.SelectedDateTicks)}, 
                    {nameof(Day.TotalPriceProducts)}, 
                    {nameof(Day.TotalPriceCake)}, 
                    {nameof(Day.TotalPrice)}, 
                    {nameof(Day.TotalPriceCorrect)}, 
                    {nameof(Day.TotalPriceAfterCorrect)}, 
                    {nameof(Day.TotalPriceMoney)}, 
                    {nameof(Day.TotalPriceDifference)},
                    {nameof(Day.CreatedTicks)},
                    {nameof(Day.UpdatedTicks)},
                    {nameof(Day.IsDelete)},
                    {nameof(Day.UserCreatedId)},
                    {nameof(Day.UserUpdatedId)}
                )
                VALUES (
                    @{nameof(Id)}, 
                    @{nameof(Description)}, 
                    @{nameof(DriverGuid)}, 
                    @{nameof(SelectedDateString)}, 
                    @{nameof(SelectedDateTicks)}, 
                    @{nameof(TotalPriceProducts)}, 
                    @{nameof(TotalPriceCake)}, 
                    @{nameof(TotalPrice)}, 
                    @{nameof(TotalPriceCorrect)}, 
                    @{nameof(TotalPriceAfterCorrect)}, 
                    @{nameof(TotalPriceMoney)}, 
                    @{nameof(TotalPriceDifference)},
                    @{nameof(CreatedTicks)},
                    @{nameof(UpdatedTicks)},
                    @{nameof(IsDelete)},
                    @{nameof(UserCreatedId)},
                    @{nameof(UserUpdatedId)}
                )
                ON CONFLICT({nameof(Day.Id)}) DO UPDATE SET
                    {nameof(Day.Description)} = @{nameof(Description)},
                    {nameof(Day.DriverGuid)} = {nameof(DriverGuid)},
                    {nameof(Day.SelectedDateString)} = @{nameof(SelectedDateString)},
                    {nameof(Day.SelectedDateTicks)} = @{nameof(SelectedDateTicks)},
                    {nameof(Day.TotalPriceProducts)} = @{nameof(TotalPriceProducts)},
                    {nameof(Day.TotalPriceCake)} = @{nameof(TotalPriceCake)},
                    {nameof(Day.TotalPrice)} = @{nameof(TotalPrice)},
                    {nameof(Day.TotalPriceCorrect)} = @{nameof(TotalPriceCorrect)},
                    {nameof(Day.TotalPriceAfterCorrect)} = @{nameof(TotalPriceAfterCorrect)},
                    {nameof(Day.TotalPriceMoney)} = @{nameof(TotalPriceMoney)},
                    {nameof(Day.TotalPriceDifference)} = @{nameof(TotalPriceDifference)},
                    {nameof(Day.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                    {nameof(Day.IsDelete)} = @{nameof(IsDelete)},
                    {nameof(Day.UserUpdatedId)} = @{nameof(UserUpdatedId)};
            ";
        }


        public static string GetFullDaysProcedureWithoutWhere()
        {
            string sql = @"

SELECT 
  D.SelectedDateString,
  D.SelectedDateTicks,
  D.TotalPriceProducts,
  D.TotalPriceCake,
  D.TotalPrice,
  D.TotalPriceCorrect,
  D.TotalPriceAfterCorrect,
  D.TotalPriceMoney,
  D.TotalPriceDifference,
  D.Description,
  D.DriverGuid,
  D.Id,
  D.CreatedTicks,
  D.UpdatedTicks,
  D.UserCreatedId,
  D.UserUpdatedId,
  (
    SELECT json_group_array(
      json_object(
        'Id', P.Id,
        'PriceTotal', P.PriceTotal,
        'PriceTotalCorrect', P.PriceTotalCorrect,
        'PriceTotalAfterCorrect', P.PriceTotalAfterCorrect,
        'DayId', P.DayId,
        'ProductNameId', P.ProductNameId,
        'ProductPriceId', P.ProductPriceId,
        'Description', P.Description,
        'Number', P.Number,
        'NumberEdit', P.NumberEdit,
        'NumberReturn', P.NumberReturn,
        'CreatedTicks', P.CreatedTicks,
        'UpdatedTicks', P.UpdatedTicks,
        'UserCreatedId',P.UserCreatedId,
        'UserUpdatedId',P.UserUpdatedId,
        'Name', json_object(
          'Id', PN.Id,
          'Name', PN.Name,
          'Description', PN.Description,
          'Img', PN.Img,
          'Arrangement', PN.Arrangement,
          'CreatedTicks', PN.CreatedTicks,
          'UpdatedTicks', PN.UpdatedTicks,
          'UserCreatedId', PN.UserCreatedId,
          'UserUpdatedId', PN.UserUpdatedId
        ),
        'Price', json_object(
          'Id', PP.Id,
          'ProductNameId', PP.ProductNameId,
          'Price', PP.Price,
          'CreatedTicks', PP.CreatedTicks,
          'UpdatedTicks', PP.UpdatedTicks,
          'UserUpdatedId', PP.UserCreatedId,
          'UserCreatedId', PP.UserUpdatedId
        )
      )
    )
    FROM Product P
    LEFT JOIN ProductName PN ON PN.Id = P.ProductNameId
    LEFT JOIN ProductPrice PP ON PP.Id = P.ProductPriceId
    WHERE P.DayId = D.Id
  ) AS JsonProducts,
  (
    SELECT json_group_array(
      json_object(
        'IsSell', C.IsSell,
        'Price', C.Price,
        'DayId', C.DayId,
        'Id', C.Id,
        'CreatedTicks', C.CreatedTicks,
        'UpdatedTicks', C.UpdatedTicks,
        'UserCreatedId', C.UserCreatedId,
        'UserUpdatedId', C.UserUpdatedId
      )
    )
    FROM Cake C
    WHERE C.DayId = D.Id
  ) AS JsonCakes
FROM Day D

";
            return sql;
        }
    }
}
