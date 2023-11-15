using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;

using Inventory.Helper.Parse;
using Inventory.Model;
using Inventory.Model.MVVM;
using Inventory.Pages.Options.EditDriver;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Options.CreateTable
{
    public partial class CreateTableVM : ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<CreateTableM> tableMs;

        readonly AccessDataBase _db;

        public CreateTableVM(AccessDataBase dataBase)
        {
            TableMs = new ObservableCollection<CreateTableM>
            {
                new CreateTableM() { RealTableName = nameof(Day), TableName = "- z dniami" },
                new CreateTableM() { RealTableName = nameof(Product), TableName = "- z produktami" },
                new CreateTableM() { RealTableName = nameof(Cake), TableName = "- z ciastami" },
                new CreateTableM() { RealTableName = nameof(ProductName), TableName = "- z nazwami produktów" },
                new CreateTableM() { RealTableName = nameof(ProductPrice), TableName = "- z cenami produktów" },
                new CreateTableM() { RealTableName = nameof(Driver), TableName = "- z kierowcami" },
                new CreateTableM() { RealTableName = nameof(SelectedDriver), TableName = "- z wybranym kierowcą" }
            };

            this._db = dataBase;
            Task.Run(async () =>
            {
                try
                {
                    await CheckTables();
                }
                catch (Exception ex)
                {
                    _db.SaveLog(ex);
                }
            });
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

                await _db.DataBaseAsync.DropTableAsync<Inventory.Model.Driver>();
                await _db.DataBaseAsync.DropTableAsync<Inventory.Model.SelectedDriver>();
                await _db.DataBaseAsync.DropTableAsync<Inventory.Model.ProductName>();
                await _db.DataBaseAsync.DropTableAsync<Inventory.Model.ProductPrice>();
                await _db.DataBaseAsync.DropTableAsync<Inventory.Model.Product>();
                await _db.DataBaseAsync.DropTableAsync<Inventory.Model.Cake>();
                await _db.DataBaseAsync.DropTableAsync<Inventory.Model.Day>();

                await CheckTables();

                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Driver>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.SelectedDriver>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.ProductName>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.ProductPrice>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Product>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Cake>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Day>();


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

                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Driver>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.SelectedDriver>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.ProductName>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.ProductPrice>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Product>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Cake>();
                await _db.DataBaseAsync.CreateTableAsync<Inventory.Model.Day>();

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
                if (_db is null)
                {
                    return;
                }
                var response = await Shell.Current.DisplayPromptAsync("Dodawanie kierowcy", "Zdefiniu nazwę kierowcy");
                if (!string.IsNullOrWhiteSpace(response))
                {
                    var driver = new Driver()
                    {
                        Id = Guid.NewGuid(),
                        Name = response,
                    };
                    if (TableMs.FirstOrDefault(x => x.RealTableName == nameof(Driver)).IsExist)
                    {
                        await _db.DataBaseAsync.InsertAsync(driver);
                    }
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        [RelayCommand]
        async Task SelectDriver()
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
                        await _db.DataBaseAsync.InsertOrReplaceAsync(new Model.SelectedDriver() { Id = 1, SelectedGuid = selectedDriver.Id });
                        Service.DriverNameUpdateService.OnUpdate();
                    }

                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }

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
                        var selectedDrivers = await _db.DataBaseAsync.Table<Driver>().Where(x=>x.Name ==selected).ToArrayAsync();
                        var selectedDriver = selectedDrivers.FirstOrDefault();
                        selectedDriver.Id = new Guid(selectedDriver.Id.ToString());

                        await Shell.Current.GoToAsync($"{nameof(EditDriverV)}?",
                            new Dictionary<string, object>
                            {
                                [nameof(DriverM)]= selectedDriver.PareseAsDriverM(),
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

        async Task<bool> CheckIsExist(string table)
        {
            var tableInfo = await _db.DataBaseAsync.GetTableInfoAsync(table);
            bool exist = tableInfo.Count > 0;
            return exist;
        }


    }
}
