using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using Shared.Model;
using Shared.Service;

namespace Shared.Data
{
    public class DriversRoutesTables(IAccessDataBaseAoT db) : IUpdateDataBase
    {
        readonly IAccessDataBaseAoT _db = db;
        public async Task Update(int oldVersion, int newVersion, Action<double, int> updateDriverRoutes)
        {
            double progressBar = 0;
            double updateProgressBar = newVersion - oldVersion;
            updateProgressBar /= updateProgressBar.ToString().Length * 10;


            if (oldVersion < 1)
            {
                await CreateDriversRoutesTables();
                await CreatedDefaultRoutes();
                progressBar += updateProgressBar;
                oldVersion = 1;
                updateDriverRoutes?.Invoke(progressBar, oldVersion);
            }

            if (oldVersion < 2)
            {
                progressBar += updateProgressBar;
                oldVersion = 2;
                updateDriverRoutes?.Invoke(progressBar, oldVersion);
            }
            updateDriverRoutes?.Invoke(1, oldVersion);
        }

        private async Task CreateDriversRoutesTables()
        {
            var routes = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedRoutes);
            var customer = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedCustomer);
            var day = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedSelectedDayOfWeekRoutes);
            var address = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedResidentialAddress);

            await Task.WhenAll(routes, customer, day, address);
        }


        const string SQlCreatedRoutes = $@"CREATE TABLE IF NOT EXISTS [{nameof(Routes)}] (
            [{nameof(Routes.Id)}] TEXT PRIMARY KEY,
            [{nameof(Routes.Name)}] TEXT,
            {HelperTable.AdditionalColumns})";

        const string SQlCreatedCustomer = $@"CREATE TABLE IF NOT EXISTS [{nameof(CustomerRoutes)}] (
            [{nameof(CustomerRoutes.Id)}] TEXT PRIMARY KEY,
            [{nameof(CustomerRoutes.Name)}] TEXT,
            [{nameof(CustomerRoutes.Description)}] TEXT,
            [{nameof(CustomerRoutes.PhoneNumber)}] TEXT,
            [{nameof(CustomerRoutes.RoutesId)}] TEXT,
            [{nameof(CustomerRoutes.Longitude)}] REAL,
            [{nameof(CustomerRoutes.Latitude)}] REAL,
            {HelperTable.AdditionalColumns})";

        const string SQlCreatedSelectedDayOfWeekRoutes = $@"CREATE TABLE IF NOT EXISTS [{nameof(SelectedDayOfWeekRoutes)}] (
            [{nameof(SelectedDayOfWeekRoutes.Id)}] TEXT PRIMARY KEY,
            [{nameof(SelectedDayOfWeekRoutes.CustomerId)}] TEXT,
            [{nameof(SelectedDayOfWeekRoutes.Sunday)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.SundayTicks)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.Monday)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.MondayTicks)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.Tuesday)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.TuesdayTicks)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.Wednesday)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.WednesdayTicks)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.Thursday)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.ThursdayTicks)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.Friday)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.FridayTicks)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.Saturday)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.SaturdayTicks)}] INTEGER,
            [{nameof(SelectedDayOfWeekRoutes.Optional)}] INTEGER,
            {HelperTable.AdditionalColumns})";

        const string SQlCreatedResidentialAddress = $@"CREATE TABLE IF NOT EXISTS [{nameof(ResidentialAddress)}] (
            [{nameof(ResidentialAddress.Id)}] TEXT PRIMARY KEY,
            [{nameof(ResidentialAddress.CustomerId)}] TEXT,
            [{nameof(ResidentialAddress.Name)}] TEXT,
            [{nameof(ResidentialAddress.Surname)}] TEXT,
            [{nameof(ResidentialAddress.Street)}] TEXT,
            [{nameof(ResidentialAddress.HouseNumber)}] TEXT,
            [{nameof(ResidentialAddress.ApartmentNumber)}] TEXT,
            [{nameof(ResidentialAddress.PostalCode)}] TEXT,
            [{nameof(ResidentialAddress.City)}] TEXT,
            [{nameof(ResidentialAddress.Country)}] TEXT,
            {HelperTable.AdditionalColumns})";

        async Task CreatedDefaultRoutes()
        {
            Routes[] routes = GetDefaultRoutes();

            foreach (var item in routes)
            {
                item.IsDelete = false;
                var sql = DataBase.Data.SqlQuery.RoutesQuery.SaveOrUpdate(item.Id,
                                                                            item.Name,
                                                                            item.CreatedTicks,
                                                                            item.UpdatedTicks,
                                                                            item.IsDelete,
                                                                            item.UserCreatedId,
                                                                            item.UserUpdatedId);
                await _db.DbAsyncAoT.ExecuteAsync(sql, new
                {
                    item.Id,
                    item.Name,
                    item.CreatedTicks,
                    item.UpdatedTicks,
                    item.IsDelete,
                    item.UserCreatedId,
                    item.UserUpdatedId
                });
            }
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
                    UserCreatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    UserUpdatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    IsDelete=false,
                },
                new()
                {
                    Id = new Guid("b254896a-12e5-1eef-9af7-227ef036e328"),
                    Name = "Pasierbiec",
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                    UserCreatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    UserUpdatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    IsDelete=false,
                },
                new()
                {
                    Id = new Guid("c1d474a5-ba17-69e3-c756-e60d4fa4da45"),
                    Name = "Słopnice",
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                    UserCreatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    UserUpdatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    IsDelete=false,
                },
                new()
                {
                    Id = new Guid("baf3bb5a-59f6-5524-10d6-2d4c3c84b98b"),
                    Name = "Sowliny",
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                    UserCreatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    UserUpdatedId=new Guid("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                    IsDelete=false,
                },
            ];
        }

    }
}