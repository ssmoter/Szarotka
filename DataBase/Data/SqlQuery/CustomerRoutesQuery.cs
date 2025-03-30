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
    }
}
