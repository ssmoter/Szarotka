using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery
{
    public class DayQuery
    {

        public static string SaveOrUpdate(Day day)
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
                    '{day.Id}', 
                    '{day.Description}', 
                    '{day.DriverGuid}', 
                    '{day.SelectedDateString}', 
                    {day.SelectedDateTicks}, 
                    {day.TotalPriceProducts}, 
                    {day.TotalPriceCake}, 
                    {day.TotalPrice}, 
                    {day.TotalPriceCorrect}, 
                    {day.TotalPriceAfterCorrect}, 
                    {day.TotalPriceMoney}, 
                    {day.TotalPriceDifference},
                    {day.CreatedTicks},
                    {day.UpdatedTicks},
                    {day.IsDelete},
                    '{day.UserCreatedId}',
                    '{day.UserUpdatedId}'
                )
                ON CONFLICT({nameof(Day.Id)}) DO UPDATE SET
                    {nameof(Day.Description)} = '{day.Description}',
                    {nameof(Day.DriverGuid)} = '{day.DriverGuid}',
                    {nameof(Day.SelectedDateString)} = '{day.SelectedDateString}',
                    {nameof(Day.SelectedDateTicks)} = {day.SelectedDateTicks},
                    {nameof(Day.TotalPriceProducts)} = {day.TotalPriceProducts},
                    {nameof(Day.TotalPriceCake)} = {day.TotalPriceCake},
                    {nameof(Day.TotalPrice)} = {day.TotalPrice},
                    {nameof(Day.TotalPriceCorrect)} = {day.TotalPriceCorrect},
                    {nameof(Day.TotalPriceAfterCorrect)} = {day.TotalPriceAfterCorrect},
                    {nameof(Day.TotalPriceMoney)} = {day.TotalPriceMoney},
                    {nameof(Day.TotalPriceDifference)} = {day.TotalPriceDifference},
                    {nameof(Day.UpdatedTicks)} = {day.UpdatedTicks},
                    {nameof(Day.IsDelete)} = {day.IsDelete},
                    {nameof(Day.UserCreatedId)} = '{day.UserCreatedId}',
                    {nameof(Day.UserUpdatedId)} = '{day.UserUpdatedId}';
            ";
        }

    }
}
