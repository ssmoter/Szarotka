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
            string sql = $@"
                SELECT 
                    {nameof(CustomerRoutes.Id)},
                    {nameof(CustomerRoutes.RoutesId)},
                    {nameof(CustomerRoutes.Name)},
                    {nameof(CustomerRoutes.Description)},
                    {nameof(CustomerRoutes.PhoneNumber)},
                    {nameof(CustomerRoutes.CreatedTicks)},
                    {nameof(CustomerRoutes.UpdatedTicks)},
                    {nameof(CustomerRoutes.Longitude)},
                    {nameof(CustomerRoutes.Latitude)},
                    {nameof(CustomerRoutes.IsDelete)},
                    {nameof(CustomerRoutes.UserCreatedId)},
                    {nameof(CustomerRoutes.UserUpdatedId)},
                    json_object(
                        '{nameof(SelectedDayOfWeekRoutes.Id)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Id)},
                        '{nameof(SelectedDayOfWeekRoutes.CustomerId)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.CustomerId)},
                        '{nameof(SelectedDayOfWeekRoutes.Sunday)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Sunday)},
                        '{nameof(SelectedDayOfWeekRoutes.SundayTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.SundayTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.Monday)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Monday)},
                        '{nameof(SelectedDayOfWeekRoutes.MondayTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.MondayTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.Tuesday)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Tuesday)},
                        '{nameof(SelectedDayOfWeekRoutes.TuesdayTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.TuesdayTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.Wednesday)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Wednesday)},
                        '{nameof(SelectedDayOfWeekRoutes.WednesdayTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.WednesdayTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.Thursday)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Thursday)},
                        '{nameof(SelectedDayOfWeekRoutes.ThursdayTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.ThursdayTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.Friday)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Friday)},
                        '{nameof(SelectedDayOfWeekRoutes.FridayTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.FridayTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.Saturday)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Saturday)},
                        '{nameof(SelectedDayOfWeekRoutes.SaturdayTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.SaturdayTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.Optional)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.Optional)},
                        '{nameof(SelectedDayOfWeekRoutes.CreatedTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.CreatedTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.UpdatedTicks)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.UpdatedTicks)},
                        '{nameof(SelectedDayOfWeekRoutes.UserCreatedId)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.UserCreatedId)},
                        '{nameof(SelectedDayOfWeekRoutes.UserUpdatedId)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.UserUpdatedId)},
                        '{nameof(SelectedDayOfWeekRoutes.IsDelete)}', {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.IsDelete)}
                    ) AS JsonDayOfWeek,
                    json_object(
                        '{nameof(ResidentialAddress.Id)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.Id)},
                        '{nameof(ResidentialAddress.CustomerId)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.CustomerId)},
                        '{nameof(ResidentialAddress.Name)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.Name)},
                        '{nameof(ResidentialAddress.Surname)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.Surname)},
                        '{nameof(ResidentialAddress.Street)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.Street)},
                        '{nameof(ResidentialAddress.HouseNumber)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.HouseNumber)},
                        '{nameof(ResidentialAddress.ApartmentNumber)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.ApartmentNumber)},
                        '{nameof(ResidentialAddress.PostalCode)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.PostalCode)},
                        '{nameof(ResidentialAddress.City)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.City)},
                        '{nameof(ResidentialAddress.Country)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.Country)},
                        '{nameof(ResidentialAddress.CreatedTicks)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.CreatedTicks)},
                        '{nameof(ResidentialAddress.UpdatedTicks)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.UpdatedTicks)},
                        '{nameof(ResidentialAddress.UserCreatedId)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.UserCreatedId)},
                        '{nameof(ResidentialAddress.UserUpdatedId)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.UserUpdatedId)},
                        '{nameof(ResidentialAddress.IsDelete)}', {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.IsDelete)}
                    ) AS JsonAddress
                FROM 
                    {nameof(CustomerRoutes)}
                LEFT JOIN 
                    {nameof(SelectedDayOfWeekRoutes)} ON {nameof(CustomerRoutes)}.{nameof(CustomerRoutes.Id)} = {nameof(SelectedDayOfWeekRoutes)}.{nameof(SelectedDayOfWeekRoutes.CustomerId)}
                LEFT JOIN 
                    {nameof(ResidentialAddress)} ON {nameof(CustomerRoutes)}.{nameof(CustomerRoutes.Id)} = {nameof(ResidentialAddress)}.{nameof(ResidentialAddress.CustomerId)}
                ";
            return sql;
        }
    }
}
