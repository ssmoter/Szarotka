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
            var defoult = CreateDefoutlRoutes();
            await Task.WhenAll(routes, customer, day, address, defoult);
        }

        async Task CreateDefoutlRoutes()
        {
            var routes = new Routes[]
            {
                new()
                {
                    Id = GetGuidSed(),
                    Name = "Szyk"
                },
                new()
                {
                    Id = GetGuidSed(),
                    Name = "Pasierbiec"
                },
                new()
                {
                    Id = GetGuidSed(),
                    Name = "Słopnice"
                },
                new()
                {
                    Id = GetGuidSed(),
                    Name = "Sowliny"
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