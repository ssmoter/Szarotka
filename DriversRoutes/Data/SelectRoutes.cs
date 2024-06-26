﻿using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Service;

namespace DriversRoutes.Data
{
    public class SelectRoutes(AccessDataBase db) : ISelectRoutes
    {
        readonly AccessDataBase _db = db;

        public async Task<CustomerRoutes[]> GetCustomerRoutesQueryAsync(Routes routes, SelectedDayOfWeekRoutes dayOf)
        {
            var queryResult = await _db.DataBaseAsync.QueryAsync<FullModelForQuery>(Helper.SqlQuery.GetQueryForSelectedRoutes(routes.Id.ToString(), dayOf));
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
                    DayOfWeek = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectedDayOfWeekRoutes>(queryResult[i].JsonDayOfWeek),
                    ResidentialAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<ResidentialAddress>(queryResult[i].JsonAddress)
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
                    DayOfWeek = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectedDayOfWeekRoutes>(queryResult[i].JsonDayOfWeek),
                    ResidentialAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<ResidentialAddress>(queryResult[i].JsonAddress)
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
