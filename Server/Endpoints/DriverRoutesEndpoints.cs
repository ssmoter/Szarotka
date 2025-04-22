using Microsoft.AspNetCore.Mvc;

using Server.Requests;

namespace Server.Endpoints
{
    public static class DriverRoutesEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            var map = app.MapGroup("/driver-routes");

            map.MapGet("customer-route/{id}", async (string id, IDriverRoutesCustomerRoutesRequests getDriverRoutesAoT, CancellationToken token = default)
                =>
            {
                var customers = await getDriverRoutesAoT.GetCustomer(id, token);
                return customers;
            }).RequireAuthorization();

            map.MapGet("customer-routes/{routeId}", async (string routeId, [FromQuery] DayOfWeek[] selected_day, IDriverRoutesCustomerRoutesRequests getDriverRoutesAoT, CancellationToken token = default)
                =>
            {
                var customers = await getDriverRoutesAoT.GetCustomers(routeId, selected_day, token);
                return customers;
            }).RequireAuthorization();

        }
    }
}
