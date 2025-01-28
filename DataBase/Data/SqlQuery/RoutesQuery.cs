using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.SqlQuery
{
    public class RoutesQuery
    {
        public static string SaveOrUpdate(Routes routes)
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
                        '{routes.Id}', 
                        '{routes.Name}', 
                        {routes.CreatedTicks}, 
                        {routes.UpdatedTicks}, 
                        {routes.IsDelete}, 
                        '{routes.UserCreatedId}', 
                        '{routes.UserUpdatedId}'
                    ) ON CONFLICT({nameof(Routes.Id)}) DO UPDATE SET
                        {nameof(Routes.Name)} = '{routes.Name}',
                        {nameof(Routes.UpdatedTicks)} = {routes.UpdatedTicks},
                        {nameof(Routes.IsDelete)} = {routes.IsDelete},
                        {nameof(Routes.UserUpdatedId)} = '{routes.UserUpdatedId}';
                ";
            return sql;
        }
    }
}
