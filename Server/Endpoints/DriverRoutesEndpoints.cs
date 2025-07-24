using DataBase.Model.EntitiesRoutes;

using Microsoft.AspNetCore.Mvc;

using Server.Requests;

namespace Server.Endpoints
{
    public static class DriverRoutesEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            var map = app.MapGroup("/driver-routes");

            map.MapGet("customer-route/{id}", async (string id, IDriverRoutesCustomerRoutesRequests getDriverRoutesAoT, CancellationToken token = default) =>
            {
                var customers = await getDriverRoutesAoT.GetCustomer(id, token);
                return customers;
            })
                .RequireAuthorization();
            map.MapGet("customer-routes/{routeId}/{isDelete?}", async (string routeId, [FromQuery] DayOfWeek[] selected_day, IDriverRoutesCustomerRoutesRequests getDriverRoutesAoT, bool isDelete = false, CancellationToken token = default) =>
            {
                var customers = await getDriverRoutesAoT.GetCustomers(routeId, selected_day,isDelete, token);
                return customers;
            })
                .RequireAuthorization();
            map.MapGet("customer-routes", async (string[] ids, IDriverRoutesCustomerRoutesRequests getDriverRoutesAoT, CancellationToken token = default) =>
            {
                var customers = await getDriverRoutesAoT.GetCustomers(ids, token);
                return customers;
            })
                .RequireAuthorization();

            map.MapPost("update", async (CustomerRoutes customer, IDriverRoutesCustomerRoutesRequests _driverRoutesCustomerRoutesRequests, bool forceUpdate = false, CancellationToken token = default) =>
            {
                var result = await _driverRoutesCustomerRoutesRequests.UpdateCustomer(customer, forceUpdate, token);

                return result;
            })
                .RequireAuthorization();
            map.MapPost("updates", async (IList<CustomerRoutes> customer, IDriverRoutesCustomerRoutesRequests _driverRoutesCustomerRoutesRequests, bool forceUpdate = false, CancellationToken token = default) =>
            {
                var result = await _driverRoutesCustomerRoutesRequests.UpdateCustomers(customer, forceUpdate, token);
                return result;
            })
                .RequireAuthorization();



        }
    }
}
