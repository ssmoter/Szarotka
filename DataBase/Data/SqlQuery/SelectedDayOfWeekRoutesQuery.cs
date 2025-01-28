using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.SqlQuery
{
    public class SelectedDayOfWeekRoutesQuery
    {

        public static string SaveOrUpdate(SelectedDayOfWeekRoutes selectedDay)
        {
            string sql = $@"
        INSERT INTO {nameof(SelectedDayOfWeekRoutes)} (
            {nameof(SelectedDayOfWeekRoutes.Id)}, 
            {nameof(SelectedDayOfWeekRoutes.CustomerId)}, 
            {nameof(SelectedDayOfWeekRoutes.Sunday)}, 
            {nameof(SelectedDayOfWeekRoutes.SundayTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.Monday)}, 
            {nameof(SelectedDayOfWeekRoutes.MondayTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.Tuesday)}, 
            {nameof(SelectedDayOfWeekRoutes.TuesdayTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.Wednesday)}, 
            {nameof(SelectedDayOfWeekRoutes.WednesdayTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.Thursday)}, 
            {nameof(SelectedDayOfWeekRoutes.ThursdayTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.Friday)}, 
            {nameof(SelectedDayOfWeekRoutes.FridayTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.Saturday)}, 
            {nameof(SelectedDayOfWeekRoutes.SaturdayTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.Optional)}, 
            {nameof(SelectedDayOfWeekRoutes.CreatedTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.UpdatedTicks)}, 
            {nameof(SelectedDayOfWeekRoutes.IsDelete)}, 
            {nameof(SelectedDayOfWeekRoutes.UserCreatedId)}, 
            {nameof(SelectedDayOfWeekRoutes.UserUpdatedId)}
        ) VALUES (
            '{selectedDay.Id}', 
            '{selectedDay.CustomerId}', 
            {selectedDay.Sunday}, 
            {selectedDay.SundayTicks}, 
            {selectedDay.Monday}, 
            {selectedDay.MondayTicks}, 
            {selectedDay.Tuesday}, 
            {selectedDay.TuesdayTicks}, 
            {selectedDay.Wednesday}, 
            {selectedDay.WednesdayTicks}, 
            {selectedDay.Thursday}, 
            {selectedDay.ThursdayTicks}, 
            {selectedDay.Friday}, 
            {selectedDay.FridayTicks}, 
            {selectedDay.Saturday}, 
            {selectedDay.SaturdayTicks}, 
            {selectedDay.Optional}, 
            {selectedDay.CreatedTicks}, 
            {selectedDay.UpdatedTicks}, 
            {selectedDay.IsDelete}, 
            '{selectedDay.UserCreatedId}', 
            '{selectedDay.UserUpdatedId}'
        )
        ON CONFLICT ({nameof(SelectedDayOfWeekRoutes.Id)}) DO UPDATE SET
            {nameof(SelectedDayOfWeekRoutes.CustomerId)} = {selectedDay.CustomerId}, 
            {nameof(SelectedDayOfWeekRoutes.Sunday)} = {selectedDay.Sunday}, 
            {nameof(SelectedDayOfWeekRoutes.SundayTicks)} = {selectedDay.SundayTicks}, 
            {nameof(SelectedDayOfWeekRoutes.Monday)} = {selectedDay.Monday}, 
            {nameof(SelectedDayOfWeekRoutes.MondayTicks)} = {selectedDay.MondayTicks}, 
            {nameof(SelectedDayOfWeekRoutes.Tuesday)} = {selectedDay.Tuesday}, 
            {nameof(SelectedDayOfWeekRoutes.TuesdayTicks)} = {selectedDay.TuesdayTicks}, 
            {nameof(SelectedDayOfWeekRoutes.Wednesday)} = {selectedDay.Wednesday}, 
            {nameof(SelectedDayOfWeekRoutes.WednesdayTicks)} = {selectedDay.WednesdayTicks}, 
            {nameof(SelectedDayOfWeekRoutes.Thursday)} = {selectedDay.Thursday}, 
            {nameof(SelectedDayOfWeekRoutes.ThursdayTicks)} = {selectedDay.ThursdayTicks}, 
            {nameof(SelectedDayOfWeekRoutes.Friday)} = {selectedDay.Friday}, 
            {nameof(SelectedDayOfWeekRoutes.FridayTicks)} = {selectedDay.FridayTicks}, 
            {nameof(SelectedDayOfWeekRoutes.Saturday)} = {selectedDay.Saturday}, 
            {nameof(SelectedDayOfWeekRoutes.SaturdayTicks)} = {selectedDay.SaturdayTicks}, 
            {nameof(SelectedDayOfWeekRoutes.Optional)} = {selectedDay.Optional}, 
            {nameof(SelectedDayOfWeekRoutes.UpdatedTicks)} = {selectedDay.UpdatedTicks}, 
            {nameof(SelectedDayOfWeekRoutes.IsDelete)} = {selectedDay.IsDelete}, 
            {nameof(SelectedDayOfWeekRoutes.UserUpdatedId)} = '{selectedDay.UserUpdatedId}';
    ";

            return sql;
        }
    }
}
