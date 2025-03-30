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

    }
}
