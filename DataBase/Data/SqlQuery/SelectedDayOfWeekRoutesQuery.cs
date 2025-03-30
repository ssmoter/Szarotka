using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.SqlQuery
{
    public class SelectedDayOfWeekRoutesQuery
    {

        public static string SaveOrUpdate(
            Guid Id,
            Guid CustomerId,
            bool Sunday,
            long SundayTicks,
            bool Monday,
            long MondayTicks,
            bool Tuesday,
            long TuesdayTicks,
            bool Wednesday,
            long WednesdayTicks,
            bool Thursday,
            long ThursdayTicks,
            bool Friday,
            long FridayTicks,
            bool Saturday,
            long SaturdayTicks,
            bool Optional,
            long CreatedTicks,
            long UpdatedTicks,
            bool IsDelete,
            Guid UserCreatedId,
            Guid UserUpdatedId)
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
                    @{nameof(Id)}, 
                    @{nameof(CustomerId)}, 
                    @{nameof(Sunday)}, 
                    @{nameof(SundayTicks)}, 
                    @{nameof(Monday)}, 
                    @{nameof(MondayTicks)}, 
                    @{nameof(Tuesday)}, 
                    @{nameof(TuesdayTicks)}, 
                    @{nameof(Wednesday)}, 
                    @{nameof(WednesdayTicks)}, 
                    @{nameof(Thursday)}, 
                    @{nameof(ThursdayTicks)}, 
                    @{nameof(Friday)}, 
                    @{nameof(FridayTicks)}, 
                    @{nameof(Saturday)}, 
                    @{nameof(SaturdayTicks)}, 
                    @{nameof(Optional)}, 
                    @{nameof(CreatedTicks)}, 
                    @{nameof(UpdatedTicks)}, 
                    @{nameof(IsDelete)}, 
                    @{nameof(UserCreatedId)}, 
                    @{nameof(UserUpdatedId)}
                )
                ON CONFLICT ({nameof(SelectedDayOfWeekRoutes.Id)}) DO UPDATE SET
                    {nameof(SelectedDayOfWeekRoutes.CustomerId)} = @{nameof(CustomerId)}, 
                    {nameof(SelectedDayOfWeekRoutes.Sunday)} = @{nameof(Sunday)}, 
                    {nameof(SelectedDayOfWeekRoutes.SundayTicks)} = @{nameof(SundayTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.Monday)} = @{nameof(Monday)}, 
                    {nameof(SelectedDayOfWeekRoutes.MondayTicks)} = @{nameof(MondayTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.Tuesday)} = @{nameof(Tuesday)}, 
                    {nameof(SelectedDayOfWeekRoutes.TuesdayTicks)} = @{nameof(TuesdayTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.Wednesday)} = @{nameof(Wednesday)}, 
                    {nameof(SelectedDayOfWeekRoutes.WednesdayTicks)} = @{nameof(WednesdayTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.Thursday)} = @{nameof(Thursday)}, 
                    {nameof(SelectedDayOfWeekRoutes.ThursdayTicks)} = @{nameof(ThursdayTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.Friday)} = @{nameof(Friday)}, 
                    {nameof(SelectedDayOfWeekRoutes.FridayTicks)} = @{nameof(FridayTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.Saturday)} = @{nameof(Saturday)}, 
                    {nameof(SelectedDayOfWeekRoutes.SaturdayTicks)} = @{nameof(SaturdayTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.Optional)} = @{nameof(Optional)}, 
                    {nameof(SelectedDayOfWeekRoutes.UpdatedTicks)} = @{nameof(UpdatedTicks)}, 
                    {nameof(SelectedDayOfWeekRoutes.IsDelete)} = @{nameof(IsDelete)}, 
                    {nameof(SelectedDayOfWeekRoutes.UserUpdatedId)} = @{nameof(UserUpdatedId)};
                ";

            return sql;
        }
    }
}
