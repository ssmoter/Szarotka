using DataBase.Data;
using DataBase.Model.EntitiesRoutes;
using Shared.Service;

namespace Shared.Data
{
    public class DriversRoutesTables(AccessDataBase db) : IUpdateDataBase
    {
        readonly AccessDataBase _db = db;
        readonly Random _random = new(1337);

        public async Task Update(int oldVersion, int newVersion, Action<double, int> updateDriverRoutes)
        {
            double progressBar = 0;
            double updateProgressBar = newVersion - oldVersion;
            updateProgressBar /= updateProgressBar.ToString().Length * 10;


            if (oldVersion <= 1)
            {
                await CreateDriversRoutesTables();
                progressBar += updateProgressBar;
                oldVersion = 1;
                updateDriverRoutes?.Invoke(progressBar, oldVersion);
            }
            updateDriverRoutes?.Invoke(1, oldVersion);
        }

        private async Task CreateDriversRoutesTables()
        {
            var routes = _db.DataBaseAsync.CreateTableAsync<Routes>();
            var customer = _db.DataBaseAsync.CreateTableAsync<CustomerRoutes>();
            var day = _db.DataBaseAsync.CreateTableAsync<SelectedDayOfWeekRoutes>();
            var address = _db.DataBaseAsync.CreateTableAsync<ResidentialAddress>();
            await Task.WhenAll(routes, customer, day, address);
            await CreatedDefaultRoutes();
        }

        async Task CreatedDefaultRoutes()
        {
            Routes[] routes = GetDefaultRoutes();

            await _db.DataBaseAsync.InsertAllAsync(routes);

        }

        public static Routes[] GetDefaultRoutes()
        {
            return
            [
                new()
                {
                    Id = new Guid("c7f40068-5e43-aa02-c27c-4fd927fc2227"),
                    Name = "Szyk",
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                },
                new()
                {
                    Id = new Guid("b254896a-12e5-1eef-9af7-227ef036e328"),
                    Name = "Pasierbiec",
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                },
                new()
                {
                    Id = new Guid("c1d474a5-ba17-69e3-c756-e60d4fa4da45"),
                    Name = "Słopnice",
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                },
                new()
                {
                    Id = new Guid("baf3bb5a-59f6-5524-10d6-2d4c3c84b98b"),
                    Name = "Sowliny",
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                },
            ];
        }

        Guid GetGuidSed()
        {
            byte[] guidBytes = new byte[16];
            _random.NextBytes(guidBytes);
            return new Guid(guidBytes);
        }


    }
}