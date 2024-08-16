using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data
{
    public class DriversRoutesTables
    {
        readonly AccessDataBase _db;
        readonly Random _random = new(1337);

        public DriversRoutesTables(AccessDataBase db)
        {
            _db = db;
        }


        public async Task UpdateDriversRoutes(int oldVersion, int newVersion, Action<double, int> uppdateDriverRoutes)
        {
            double progressBar = 0;
            double uppdateProgressBar = newVersion - oldVersion;
            uppdateProgressBar /= uppdateProgressBar.ToString().Length * 10;


            if (oldVersion <= 1)
            {
                await CreateDriversRoutesTables();
                progressBar += uppdateProgressBar;
                oldVersion = 1;
                uppdateDriverRoutes?.Invoke(progressBar, oldVersion);
            }
            uppdateDriverRoutes?.Invoke(1, oldVersion);
        }

        private async Task CreateDriversRoutesTables()
        {
            var routes = _db.DataBaseAsync.CreateTableAsync<Routes>();
            var customer = _db.DataBaseAsync.CreateTableAsync<CustomerRoutes>();
            var day = _db.DataBaseAsync.CreateTableAsync<SelectedDayOfWeekRoutes>();
            var address = _db.DataBaseAsync.CreateTableAsync<ResidentialAddress>();
            await Task.WhenAll(routes, customer, day, address);
            await CreateDefoutlRoutes();
        }

        async Task CreateDefoutlRoutes()
        {
            var routes = new Routes[]
            {
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
            };
            
            await _db.DataBaseAsync.InsertAllAsync(routes);

        }
        Guid GetGuidSed()
        {
            byte[] guidBytes = new byte[16];
            _random.NextBytes(guidBytes);
            return new Guid(guidBytes);
        }


    }
}