using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.SqlQuery
{
    public class CustomerRoutesQuery
    {
        public static string SaveOrUpdate(
            Guid Id,
            Guid RoutesId,
            string Name,
            string Description,
            string PhoneNumber,
            double Longitude,
            double Latitude,
            long CreatedTicks,
            long UpdatedTicks,
            bool IsDelete,
            Guid UserCreatedId,
            Guid UserUpdatedId)
        {
            string sql = $@"
                INSERT INTO {nameof(CustomerRoutes)} (
                    {nameof(CustomerRoutes.Id)}, 
                    {nameof(CustomerRoutes.RoutesId)}, 
                    {nameof(CustomerRoutes.Name)}, 
                    {nameof(CustomerRoutes.Description)}, 
                    {nameof(CustomerRoutes.PhoneNumber)}, 
                    {nameof(CustomerRoutes.Longitude)}, 
                    {nameof(CustomerRoutes.Latitude)}, 
                    {nameof(CustomerRoutes.CreatedTicks)}, 
                    {nameof(CustomerRoutes.UpdatedTicks)}, 
                    {nameof(CustomerRoutes.IsDelete)}, 
                    {nameof(CustomerRoutes.UserCreatedId)}, 
                    {nameof(CustomerRoutes.UserUpdatedId)}
                ) VALUES (
                    @{nameof(Id)}, 
                    @{nameof(RoutesId)}, 
                    @{nameof(Name)}, 
                    @{nameof(Description)}, 
                    @{nameof(PhoneNumber)}, 
                    @{nameof(Longitude)}, 
                    @{nameof(Latitude)}, 
                    @{nameof(CreatedTicks)}, 
                    @{nameof(UpdatedTicks)}, 
                    @{nameof(IsDelete)}, 
                    @{nameof(UserCreatedId)}, 
                    @{nameof(UserUpdatedId)}
                ) ON CONFLICT({nameof(CustomerRoutes.Id)}) DO UPDATE SET
                    {nameof(CustomerRoutes.RoutesId)} = @{nameof(RoutesId)},
                    {nameof(CustomerRoutes.Name)} = @{nameof(Name)},
                    {nameof(CustomerRoutes.Description)} = @{nameof(Description)},
                    {nameof(CustomerRoutes.PhoneNumber)} = @{nameof(PhoneNumber)},
                    {nameof(CustomerRoutes.Longitude)} = @{nameof(Longitude)},
                    {nameof(CustomerRoutes.Latitude)} = @{nameof(Latitude)},
                    {nameof(CustomerRoutes.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                    {nameof(CustomerRoutes.IsDelete)} = @{nameof(IsDelete)},
                    {nameof(CustomerRoutes.UserUpdatedId)} = @{nameof(UserUpdatedId)};
            ";
            return sql;
        }



        public static string GetFullProcedureWithoutWhere()
        {
            string sql = @"
        SELECT 
            CustomerRoutes.Id,
            CustomerRoutes.RoutesId,
            CustomerRoutes.Name,
            CustomerRoutes.Description,
            CustomerRoutes.PhoneNumber,
            CustomerRoutes.CreatedTicks,
            CustomerRoutes.UpdatedTicks,
            CustomerRoutes.Longitude,
            CustomerRoutes.Latitude,
            CustomerRoutes.IsDelete,
            CustomerRoutes.UserCreatedId,
            CustomerRoutes.UserUpdatedId,
            json_object(
                'Id', SelectedDayOfWeekRoutes.Id,
                'CustomerId', SelectedDayOfWeekRoutes.CustomerId,
                'Sunday', SelectedDayOfWeekRoutes.Sunday,
                'SundayTicks', SelectedDayOfWeekRoutes.SundayTicks,
                'Monday', SelectedDayOfWeekRoutes.Monday,
                'MondayTicks', SelectedDayOfWeekRoutes.MondayTicks,
                'Tuesday', SelectedDayOfWeekRoutes.Tuesday,
                'TuesdayTicks', SelectedDayOfWeekRoutes.TuesdayTicks,
                'Wednesday', SelectedDayOfWeekRoutes.Wednesday,
                'WednesdayTicks', SelectedDayOfWeekRoutes.WednesdayTicks,
                'Thursday', SelectedDayOfWeekRoutes.Thursday,
                'ThursdayTicks', SelectedDayOfWeekRoutes.ThursdayTicks,
                'Friday', SelectedDayOfWeekRoutes.Friday,
                'FridayTicks', SelectedDayOfWeekRoutes.FridayTicks,
                'Saturday', SelectedDayOfWeekRoutes.Saturday,
                'SaturdayTicks', SelectedDayOfWeekRoutes.SaturdayTicks,
                'Optional', SelectedDayOfWeekRoutes.Optional,
                'CreatedTicks', SelectedDayOfWeekRoutes.CreatedTicks,
                'UpdatedTicks', SelectedDayOfWeekRoutes.UpdatedTicks,
                'UserCreatedId', SelectedDayOfWeekRoutes.UserCreatedId,
                'UserUpdatedId', SelectedDayOfWeekRoutes.UserUpdatedId,
                'IsDelete', SelectedDayOfWeekRoutes.IsDelete
            ) AS JsonDayOfWeek,
            json_object(
                'Id', ResidentialAddress.Id,
                'CustomerId', ResidentialAddress.CustomerId,
                'Name', ResidentialAddress.Name,
                'Surname', ResidentialAddress.Surname,
                'Street', ResidentialAddress.Street,
                'HouseNumber', ResidentialAddress.HouseNumber,
                'ApartmentNumber', ResidentialAddress.ApartmentNumber,
                'PostalCode', ResidentialAddress.PostalCode,
                'City', ResidentialAddress.City,
                'Country', ResidentialAddress.Country,
                'CreatedTicks', ResidentialAddress.CreatedTicks,
                'UpdatedTicks', ResidentialAddress.UpdatedTicks,
                'UserCreatedId', ResidentialAddress.UserCreatedId,
                'UserUpdatedId', ResidentialAddress.UserUpdatedId,
                'IsDelete', ResidentialAddress.IsDelete
            ) AS JsonAddress
        FROM 
            CustomerRoutes
        LEFT JOIN 
            SelectedDayOfWeekRoutes ON CustomerRoutes.Id = SelectedDayOfWeekRoutes.CustomerId
        LEFT JOIN 
            ResidentialAddress ON CustomerRoutes.Id = ResidentialAddress.CustomerId
        ";
            return sql;
        }
    }
}
