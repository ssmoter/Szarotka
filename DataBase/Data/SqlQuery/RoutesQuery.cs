using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.SqlQuery
{
    public class RoutesQuery
    {
        public static string SaveOrUpdate(Guid Id,
                                          string Name,
                                          long CreatedTicks,
                                          long UpdatedTicks,
                                          bool IsDelete,
                                          Guid UserCreatedId,
                                          Guid UserUpdatedId)
        {
            string sql = $@"
                    INSERT INTO {nameof(Routes)} ( 
                        {nameof(Routes.Id)}, 
                        {nameof(Routes.Name)}, 
                        {nameof(Routes.CreatedTicks)}, 
                        {nameof(Routes.UpdatedTicks)}, 
                        {nameof(Routes.IsDelete)}, 
                        {nameof(Routes.UserCreatedId)}, 
                        {nameof(Routes.UserUpdatedId)}
                    ) VALUES (
                        @{nameof(Id)}, 
                        @{nameof(Name)}, 
                        @{nameof(CreatedTicks)}, 
                        @{nameof(UpdatedTicks)}, 
                        @{nameof(IsDelete)}, 
                        @{nameof(UserCreatedId)}, 
                        @{nameof(UserUpdatedId)}
                    ) ON CONFLICT({nameof(Routes.Id)}) DO UPDATE SET
                        {nameof(Routes.Name)} = @{nameof(Name)},
                        {nameof(Routes.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                        {nameof(Routes.IsDelete)} = @{nameof(IsDelete)},
                        {nameof(Routes.UserUpdatedId)} = @{nameof(UserUpdatedId)};
                ";
            return sql;
        }
    }
}
