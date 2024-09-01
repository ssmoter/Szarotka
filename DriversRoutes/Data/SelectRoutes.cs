using DataBase.Data;
using DataBase.Helper;
using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Service;

namespace DriversRoutes.Data
{
    public class SelectRoutes(AccessDataBase db) : ISelectRoutes
    {
        readonly AccessDataBase _db = db;
        private Routes _routesLast;

        public async Task<CustomerRoutes[]> GetCustomerRoutesQueryAsync(Routes routes, SelectedDayOfWeekRoutes dayOf)
        {
            routes ??= _routesLast;
            _routesLast = routes;

            var queryResult = await _db.DataBaseAsync.QueryAsync<FullModelForQuery>(Helper.SqlQuery.GetQueryForSelectedRoutes(routes.Id.ToString(), dayOf));
            var customers = new CustomerRoutes[queryResult.Count];


            for (int i = 0; i < queryResult.Count; i++)
            {
                var cust = new CustomerRoutes
                {
                    Id = queryResult[i].Id,
                    RoutesId = queryResult[i].RoutesId,
                    QueueNumber = i + 1,
                    Name = queryResult[i].Name,
                    Description = queryResult[i].Description,
                    PhoneNumber = queryResult[i].PhoneNumber,
                    Created = queryResult[i].Created,
                    Longitude = queryResult[i].Longitude,
                    Latitude = queryResult[i].Latitude,
                    DayOfWeek = System.Text.Json.JsonSerializer.Deserialize<SelectedDayOfWeekRoutes>(queryResult[i].JsonDayOfWeek
                    , JsonOptions.JsonSerializeOptions),
                    ResidentialAddress = System.Text.Json.JsonSerializer.Deserialize<ResidentialAddress>(queryResult[i].JsonAddress
                    , JsonOptions.JsonSerializeOptions)
                };
                customers[i] = cust;
            }

            queryResult.Clear();
            return customers;
        }

        public CustomerRoutes[] GetCustomerRoutesQuery(Routes routes, SelectedDayOfWeekRoutes dayOf)
        {
            var queryResult = _db.DataBase.Query<FullModelForQuery>(Helper.SqlQuery.GetQueryForSelectedRoutes(routes.Id.ToString(), dayOf));
            var customers = new CustomerRoutes[queryResult.Count];

            for (int i = 0; i < queryResult.Count; i++)
            {
                var cust = new CustomerRoutes()
                {
                    Id = queryResult[i].Id,
                    RoutesId = queryResult[i].RoutesId,
                    QueueNumber = i + 1,
                    Name = queryResult[i].Name,
                    Description = queryResult[i].Description,
                    PhoneNumber = queryResult[i].PhoneNumber,
                    Created = queryResult[i].Created,
                    Longitude = queryResult[i].Longitude,
                    Latitude = queryResult[i].Latitude,
                    DayOfWeek = System.Text.Json.JsonSerializer.Deserialize<SelectedDayOfWeekRoutes>(queryResult[i].JsonDayOfWeek
                    , JsonOptions.JsonSerializeOptions),
                    ResidentialAddress = System.Text.Json.JsonSerializer.Deserialize<ResidentialAddress>(queryResult[i].JsonAddress
                    , JsonOptions.JsonSerializeOptions)
                };
                customers[i] = cust;
            }

            queryResult.Clear();
            return customers;
        }

        class FullModelForQuery : CustomerRoutes
        {
            public string JsonDayOfWeek { get; set; }
            public string JsonAddress { get; set; }

        }

    }
}
