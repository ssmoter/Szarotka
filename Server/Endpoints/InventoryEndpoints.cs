using Microsoft.AspNetCore.Mvc;

using Server.Requests;

namespace Server.Endpoints
{
    public static class InventoryEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            var map = app.MapGroup("/inventory");

            map.MapGet("empty-products", async (IInventoryProductsRequests iInventoryProducts, CancellationToken token = default)=>
            {
                var products = await iInventoryProducts.GetEmptyProducts(token);
                return products;
            });

            map.MapGet("day/{id}", async (string id, IInventoryDayRequests iInventoryDayRequests, CancellationToken token = default)=>
            {
                var day = await iInventoryDayRequests.GetDay(id, token);
                return day;
            }).RequireAuthorization();
            map.MapGet("day", async (string selectedDateString, string? userId, IInventoryDayRequests iInventoryDayRequests, CancellationToken token = default)=>
            {
                var day = await iInventoryDayRequests.GetDay(selectedDateString, userId, token);
                return day;
            }).RequireAuthorization();
            map.MapGet("days", async (string? from, string? to, [FromQuery] string[]? userId, IInventoryDayRequests iInventoryDayRequests, CancellationToken token = default)=>
            {
                var day = await iInventoryDayRequests.GetDays(from, to, userId, token);
                return day;
            }).RequireAuthorization();


        }

    }
}
