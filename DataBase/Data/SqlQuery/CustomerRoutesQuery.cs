using DataBase.Model.EntitiesRoutes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Data.SqlQuery
{
    public class CustomerRoutesQuery
    {
        public static string SaveOrUpdate(CustomerRoutes customer)
        {
            string sql = $@"
                INSERT INTO {nameof(CustomerRoutes)} (
                    {nameof(CustomerRoutes.Id)}, 
                    {nameof(CustomerRoutes.RoutesId)}, 
                    {nameof(CustomerRoutes.QueueNumber)}, 
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
                    '{customer.Id}', 
                    '{customer.RoutesId}', 
                    {customer.QueueNumber}, 
                    '{customer.Name}', 
                    '{customer.Description}', 
                    '{customer.PhoneNumber}', 
                    {customer.Longitude}, 
                    {customer.Latitude}, 
                    {customer.CreatedTicks}, 
                    {customer.UpdatedTicks}, 
                    {customer.IsDelete}, 
                    '{customer.UserCreatedId}', 
                    '{customer.UserUpdatedId}'
                ) ON CONFLICT({nameof(CustomerRoutes.Id)}) DO UPDATE SET
                    {nameof(CustomerRoutes.RoutesId)} = '{customer.RoutesId}',
                    {nameof(CustomerRoutes.QueueNumber)} = {customer.QueueNumber},
                    {nameof(CustomerRoutes.Name)} = '{customer.Name}',
                    {nameof(CustomerRoutes.Description)} = '{customer.Description}',
                    {nameof(CustomerRoutes.PhoneNumber)} = '{customer.PhoneNumber}',
                    {nameof(CustomerRoutes.Longitude)} = {customer.Longitude},
                    {nameof(CustomerRoutes.Latitude)} = {customer.Latitude},
                    {nameof(CustomerRoutes.UpdatedTicks)} = {customer.UpdatedTicks},
                    {nameof(CustomerRoutes.IsDelete)} = {customer.IsDelete},
                    {nameof(CustomerRoutes.UserUpdatedId)} = '{customer.UserUpdatedId}';
            ";
            return sql;
        }
    }
}
