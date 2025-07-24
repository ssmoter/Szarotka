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
            string sql = $@"

SELECT 
  {nameof(Day)}.{nameof(Day.SelectedDateString)},
  {nameof(Day)}.{nameof(Day.SelectedDateTicks)},
  {nameof(Day)}.{nameof(Day.TotalPriceProducts)},
  {nameof(Day)}.{nameof(Day.TotalPriceCake)},
  {nameof(Day)}.{nameof(Day.TotalPrice)},
  {nameof(Day)}.{nameof(Day.TotalPriceCorrect)},
  {nameof(Day)}.{nameof(Day.TotalPriceAfterCorrect)},
  {nameof(Day)}.{nameof(Day.TotalPriceMoney)},
  {nameof(Day)}.{nameof(Day.TotalPriceDifference)},
  {nameof(Day)}.{nameof(Day.Description)},
  {nameof(Day)}.{nameof(Day.DriverGuid)},
  {nameof(Day)}.{nameof(Day.Id)},
  {nameof(Day)}.{nameof(Day.CreatedTicks)},
  {nameof(Day)}.{nameof(Day.UpdatedTicks)},
  {nameof(Day)}.{nameof(Day.UserCreatedId)},
  {nameof(Day)}.{nameof(Day.IsDelete)},
  {nameof(Day)}.{nameof(Day.UserUpdatedId)},
  (
    SELECT json_group_array(
      json_object(
        '{nameof(Product.Id)}', {nameof(Product)}.{nameof(Product.Id)},
        '{nameof(Product.PriceTotal)}', {nameof(Product)}.{nameof(Product.PriceTotal)},
        '{nameof(Product.PriceTotalCorrect)}', {nameof(Product)}.{nameof(Product.PriceTotalCorrect)},
        '{nameof(Product.PriceTotalAfterCorrect)}', {nameof(Product)}.{nameof(Product.PriceTotalAfterCorrect)},
        '{nameof(Product.DayId)}', {nameof(Product)}.{nameof(Product.DayId)},
        '{nameof(Product.ProductNameId)}', {nameof(Product)}.{nameof(Product.ProductNameId)},
        '{nameof(Product.ProductPriceId)}', {nameof(Product)}.{nameof(Product.ProductPriceId)},
        '{nameof(Product.Description)}', {nameof(Product)}.{nameof(Product.Description)},
        '{nameof(Product.Number)}', {nameof(Product)}.{nameof(Product.Number)},
        '{nameof(Product.NumberEdit)}', {nameof(Product)}.{nameof(Product.NumberEdit)},
        '{nameof(Product.NumberReturn)}', {nameof(Product)}.{nameof(Product.NumberReturn)},
        '{nameof(Product.CreatedTicks)}', {nameof(Product)}.{nameof(Product.CreatedTicks)},
        '{nameof(Product.UpdatedTicks)}', {nameof(Product)}.{nameof(Product.UpdatedTicks)},
        '{nameof(Product.UserCreatedId)}',{nameof(Product)}.{nameof(Product.UserCreatedId)},
        '{nameof(Product.IsDelete)}',{nameof(Product)}.{nameof(Product.IsDelete)},
        '{nameof(Product.UserUpdatedId)}',{nameof(Product)}.{nameof(Product.UserUpdatedId)},
        '{nameof(Product.Name)}', json_object(
          '{nameof(ProductName.Id)}', {nameof(ProductName)}.{nameof(ProductName.Id)},
          '{nameof(ProductName.Name)}', {nameof(ProductName)}.{nameof(ProductName.Name)},
          '{nameof(ProductName.Description)}', {nameof(ProductName)}.{nameof(ProductName.Description)},
          '{nameof(ProductName.Img)}', {nameof(ProductName)}.{nameof(ProductName.Img)},
          '{nameof(ProductName.Arrangement)}', {nameof(ProductName)}.{nameof(ProductName.Arrangement)},
          '{nameof(ProductName.CreatedTicks)}', {nameof(ProductName)}.{nameof(ProductName.CreatedTicks)},
          '{nameof(ProductName.UpdatedTicks)}', {nameof(ProductName)}.{nameof(ProductName.UpdatedTicks)},
          '{nameof(ProductName.UserCreatedId)}', {nameof(ProductName)}.{nameof(ProductName.UserCreatedId)},
          '{nameof(ProductName.IsDelete)}', {nameof(ProductName)}.{nameof(ProductName.IsDelete)},
          '{nameof(ProductName.UserUpdatedId)}', {nameof(ProductName)}.{nameof(ProductName.UserUpdatedId)}
        ),
        'Price', json_object(
          '{nameof(ProductPrice.Id)}', {nameof(ProductPrice)}.{nameof(ProductPrice.Id)},
          '{nameof(ProductPrice.ProductNameId)}', {nameof(ProductPrice)}.{nameof(ProductPrice.ProductNameId)},
          '{nameof(ProductPrice.Price)}', {nameof(ProductPrice)}.{nameof(ProductPrice.Price)},
          '{nameof(ProductPrice.CreatedTicks)}', {nameof(ProductPrice)}.{nameof(ProductPrice.CreatedTicks)},
          '{nameof(ProductPrice.UpdatedTicks)}', {nameof(ProductPrice)}.{nameof(ProductPrice.UpdatedTicks)},
          '{nameof(ProductPrice.UserCreatedId)}', {nameof(ProductPrice)}.{nameof(ProductPrice.UserCreatedId)},
          '{nameof(ProductPrice.IsDelete)}', {nameof(ProductPrice)}.{nameof(ProductPrice.IsDelete)},
          '{nameof(ProductPrice.UserUpdatedId)}', {nameof(ProductPrice)}.{nameof(ProductPrice.UserUpdatedId)}
        )
      )
    )
    FROM {nameof(Product)}
    LEFT JOIN {nameof(ProductName)} ON {nameof(ProductName)}.{nameof(ProductName.Id)} = {nameof(Product)}.{nameof(Product.ProductNameId)}
    LEFT JOIN {nameof(ProductPrice)} ON {nameof(ProductPrice)}.{nameof(ProductPrice.Id)} = {nameof(Product)}.{nameof(Product.ProductPriceId)}
    WHERE {nameof(Product)}.{nameof(Product.DayId)} = {nameof(Day)}.{nameof(Day.Id)}
  ) AS JsonProducts,
  (
    SELECT json_group_array(
      json_object(
        '{nameof(Cake.IsSell)}', {nameof(Cake)}.{nameof(Cake.IsSell)},
        '{nameof(Cake.Price)}', {nameof(Cake)}.{nameof(Cake.Price)},
        '{nameof(Cake.DayId)}', {nameof(Cake)}.{nameof(Cake.DayId)},
        '{nameof(Cake.Id)}', {nameof(Cake)}.{nameof(Cake.Id)},
        '{nameof(Cake.CreatedTicks)}', {nameof(Cake)}.{nameof(Cake.CreatedTicks)},
        '{nameof(Cake.UpdatedTicks)}', {nameof(Cake)}.{nameof(Cake.UpdatedTicks)},
        '{nameof(Cake.UserCreatedId)}', {nameof(Cake)}.{nameof(Cake.UserCreatedId)},
        '{nameof(Cake.IsDelete)}', {nameof(Cake)}.{nameof(Cake.IsDelete)},
        '{nameof(Cake.UserUpdatedId)}', {nameof(Cake)}.{nameof(Cake.UserUpdatedId)}
      )
    )
    FROM {nameof(Cake)}
    WHERE {nameof(Cake)}.{nameof(Cake.DayId)} = {nameof(Day)}.{nameof(Day.Id)}
  ) AS JsonCakes
FROM {nameof(Day)}

";
            return sql;
        }
    }
}
