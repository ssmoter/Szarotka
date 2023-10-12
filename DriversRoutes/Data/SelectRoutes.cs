using DataBase.Data;

using DriversRoutes.Model;
using DriversRoutes.Service;

namespace DriversRoutes.Data
{
    public class SelectRoutes : ISelectRoutes
    {
        readonly DataBase.Data.AccessDataBase _db;

        public SelectRoutes(AccessDataBase db)
        {
            _db = db;
        }

        public async Task<CustomerRoutes[]> GetCustomerRoutes(Routes routes, SelectedDayOfWeekRoutes dayOf)
        {
            var customers = await _db.DataBaseAsync.Table<CustomerRoutes>()
                .Where(x => x.RoutesId == routes.Id).ToArrayAsync();

            for (int i = 0; i < customers.Length; i++)
            {
                customers[i].DayOfWeek = await GetDayOfWeek(customers[i], dayOf);
            }


            var result = customers.Where(x => x.DayOfWeek is not null);

            return result.ToArray();
        }

        async Task<SelectedDayOfWeekRoutes> GetDayOfWeek(CustomerRoutes customer, SelectedDayOfWeekRoutes dayOf)
        {
            var week = await _db.DataBaseAsync.Table<SelectedDayOfWeekRoutes>()
                .FirstOrDefaultAsync(x => x.CustomerId == customer.Id && x.ValuesAsString.Contains(dayOf.ValuesAsString));

            return week;
        }

        public async Task<CustomerRoutes[]> GetCustomerRoutesQuery(Routes routes, SelectedDayOfWeekRoutes dayOf)
        {
            var queryResult = await _db.DataBaseAsync.QueryAsync<FullModelForQuery>(Helper.SqlQuery.GetQueryForSelectedRoutes(routes.Id.ToString(), dayOf));
            var customers = new CustomerRoutes[queryResult.Count];

            for (int i = 0; i < queryResult.Count; i++)
            {
                var cust = new CustomerRoutes()
                {
                    Id = queryResult[i].Id,
                    RoutesId = queryResult[i].RoutesId,
                    Index = queryResult[i].Index,
                    Name = queryResult[i].Name,
                    Description = queryResult[i].Description,
                    PhoneNumber = queryResult[i].PhoneNumber,
                    CreatedDate = queryResult[i].CreatedDate,
                    Longitude = queryResult[i].Longitude,
                    Latitude = queryResult[i].Latitude,
                    DayOfWeek = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectedDayOfWeekRoutes>(queryResult[i].JsonDayOfWeek),
                };
                customers[i] = cust;
            }

            queryResult.Clear();
            return customers.ToArray();
        }

        class FullModelForQuery : Model.CustomerRoutes
        {
            public string JsonDayOfWeek { get; set; }
        }

    }
}
