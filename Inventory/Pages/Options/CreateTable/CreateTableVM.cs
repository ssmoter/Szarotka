using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;

using Inventory.Helper.Parse;
using Inventory.Model.MVVM;
using Inventory.Pages.Options.EditDriver;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Options.CreateTable
{
    public partial class CreateTableVM : ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<CreateTableM> tableMs;

        [ObservableProperty]
        DataBaseVersion version;

        readonly AccessDataBase _db;
        public CreateTableVM(AccessDataBase dataBase)
        {
            TableMs =
            [
                new() { RealTableName = nameof(Day), TableName = "- z dniami" },
                new() { RealTableName = nameof(Product), TableName = "- z produktami" },
                new() { RealTableName = nameof(Cake), TableName = "- z ciastami" },
                new() { RealTableName = nameof(ProductName), TableName = "- z nazwami produktów" },
                new() { RealTableName = nameof(ProductPrice), TableName = "- z cenami produktów" },
                new() { RealTableName = nameof(Driver), TableName = "- z kierowcami" },
                new() { RealTableName = nameof(SelectedDriver), TableName = "- z wybranym kierowcą" }
            ];

            this._db = dataBase;
            Task.Run(async () =>
            {
                await CheckTables(); Version = await _db.DataBaseAsync.Table<DataBaseVersion>().FirstOrDefaultAsync();
            });
        }

        public static async Task OnNavigation(AccessDataBase db)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Inventory.Helper.SelectedDriver.Id))
                {
                    await CreatedNewDriverMethod(db);
                }
            }
            catch (Exception ex)
            {
                db.SaveLog(ex);
            }
        }

        #region Command


        [RelayCommand]
        async Task CreateTables()
        {
            try
            {

                if (_db is null)
                {
                    return;
                }

                var response = await Shell.Current.DisplayAlert("Generowanie tabeli", "Przy generowaniu tabeli poprzednie tabele zostają usunięte", "Tak", "Nie");
                if (!response)
                {
                    return;
                }

                await _db.DataBaseAsync.DropTableAsync<Driver>();
                await _db.DataBaseAsync.DropTableAsync<SelectedDriver>();
                await _db.DataBaseAsync.DropTableAsync<ProductName>();
                await _db.DataBaseAsync.DropTableAsync<ProductPrice>();
                await _db.DataBaseAsync.DropTableAsync<Product>();
                await _db.DataBaseAsync.DropTableAsync<Cake>();
                await _db.DataBaseAsync.DropTableAsync<Day>();

                await CheckTables();

                await _db.DataBaseAsync.CreateTableAsync<Driver>();
                await _db.DataBaseAsync.CreateTableAsync<SelectedDriver>();
                await _db.DataBaseAsync.CreateTableAsync<ProductName>();
                await _db.DataBaseAsync.CreateTableAsync<ProductPrice>();
                await _db.DataBaseAsync.CreateTableAsync<Product>();
                await _db.DataBaseAsync.CreateTableAsync<Cake>();
                await _db.DataBaseAsync.CreateTableAsync<Day>();


                await CheckTables();

                Helper.SelectedDriver.Id = "";
                Helper.SelectedDriver.Name = "";
                Helper.SelectedDriver.Description = "";
                Service.DriverNameUpdateService.OnUpdate();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }

        }
        [RelayCommand]
        async Task CreateNewTables()
        {
            try
            {

                if (_db is null)
                {
                    return;
                }
                await CheckTables();

                await _db.DataBaseAsync.CreateTableAsync<Driver>();
                await _db.DataBaseAsync.CreateTableAsync<SelectedDriver>();
                await _db.DataBaseAsync.CreateTableAsync<ProductName>();
                await _db.DataBaseAsync.CreateTableAsync<ProductPrice>();
                await _db.DataBaseAsync.CreateTableAsync<Product>();
                await _db.DataBaseAsync.CreateTableAsync<Cake>();
                await _db.DataBaseAsync.CreateTableAsync<Day>();

                await CheckTables();

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }

        }


        [RelayCommand]
        async Task CreateDriver()
        {
            try
            {
                await CreatedNewDriverMethod(_db);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        [RelayCommand]
        async Task SelectDriver()
        {
            await SelectDriverMethod(_db);

        }


        [RelayCommand]
        async Task EditDriver()
        {
            try
            {

                if (_db is null)
                {
                    return;
                }
                if (TableMs.FirstOrDefault(x => x.RealTableName == nameof(Driver)).IsExist)
                {
                    var driver = await _db.DataBaseAsync.Table<Driver>().ToArrayAsync();
                    if (driver.Length == 0)
                    {
                        await Shell.Current.DisplayAlert("Kierowcy", "Nie dodano żadnego kierowcy", "Ok");
                        return;
                    }

                    var selected = await Shell.Current.DisplayActionSheet("Edytuj kierowce", "Anuluj", null, driver.Select(x => x.Name).ToArray());
                    if (selected == "Anuluj")
                    {
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(selected))
                    {
                        var selectedDrivers = await _db.DataBaseAsync.Table<Driver>().Where(x => x.Name == selected).ToArrayAsync();
                        var selectedDriver = selectedDrivers.FirstOrDefault();
                        selectedDriver.Id = new Guid(selectedDriver.Id.ToString());

                        await Shell.Current.GoToAsync($"{nameof(EditDriverV)}?",
                            new Dictionary<string, object>
                            {
                                [nameof(DriverM)] = selectedDriver.PareseAsDriverM(),
                            });
                    }

                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        #endregion

        async Task CheckTables()
        {
            for (int i = 0; i < TableMs.Count; i++)
            {
                TableMs[i].IsExist = await CheckIsExist(TableMs[i].RealTableName);
            }
        }

        private static async Task SelectDriverMethod(AccessDataBase db)
        {
            try
            {

                if (db is null)
                {
                    return;
                }
                var tableInfo = await db.DataBaseAsync.GetTableInfoAsync(nameof(Driver));
                bool exist = tableInfo.Count > 0;
                if (exist)
                {
                    var driver = await db.DataBaseAsync.Table<Driver>().ToArrayAsync();

                    if (driver.Length == 0)
                    {
                        await Shell.Current.DisplayAlert("Kierowcy", "Nie dodano żadnego kierowcy", "Ok");
                        return;
                    }

                    var selected = await Shell.Current.DisplayActionSheet("Dostępni kierowcy", "Anuluj", null, driver.Select(x => x.Name).ToArray());
                    if (selected == "Anuluj")
                    {
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(selected))
                    {
                        var selectedDriver = driver.FirstOrDefault(x => x.Name == selected);
                        Helper.SelectedDriver.Id = selectedDriver.Id.ToString();
                        Helper.SelectedDriver.Name = selectedDriver.Name;
                        Helper.SelectedDriver.Description = selectedDriver.Description;
                        await db.DataBaseAsync.InsertOrReplaceAsync(new SelectedDriver() { Id = 1, SelectedGuid = selectedDriver.Id });
                        Service.DriverNameUpdateService.OnUpdate();
                    }

                }
            }
            catch (Exception ex)
            {
                db.SaveLog(ex);
            }
        }
        async Task<bool> CheckIsExist(string table)
        {
            var tableInfo = await _db.DataBaseAsync.GetTableInfoAsync(table);
            bool exist = tableInfo.Count > 0;
            return exist;
        }
        private static async Task CreatedNewDriverMethod(AccessDataBase db)
        {
            if (db is null)
            {
                return;
            }            
            var response = await Shell.Current.DisplayPromptAsync("Dodawanie kierowcy", "Ustaw nazwę kierowcy");
            if (!string.IsNullOrWhiteSpace(response))
            {
                var driver = new Driver()
                {
                    Id = Guid.NewGuid(),
                    Name = response,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                };
                var tableInfo = await db.DataBaseAsync.GetTableInfoAsync(nameof(Driver));
                bool exist = tableInfo.Count > 0;
                if (exist)
                {
                    await db.DataBaseAsync.InsertAsync(driver);
                    if (string.IsNullOrWhiteSpace(Inventory.Helper.SelectedDriver.Id))
                    {
                        await SelectDriverMethod(db);
                    }
                }
            }
        }


    }
}
