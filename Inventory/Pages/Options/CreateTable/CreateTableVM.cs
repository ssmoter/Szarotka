using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;


using Shared.Data;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Options.CreateTable
{
    public partial class CreateTableVM : ObservableObject
    {

        private ObservableCollection<CreateTableM> tableMs;
        public ObservableCollection<CreateTableM> TableMs
        {
            get => tableMs;
            set
            {
                if (SetProperty(ref tableMs, value, nameof(TableMs))) { }
            }
        }

        private DataBaseVersion version;
        public DataBaseVersion Version
        {
            get => version;
            set
            {
                if (SetProperty(ref version, value, nameof(Version))) { }
            }
        }

        readonly IAccessDataBase _db;
        public CreateTableVM(IAccessDataBase dataBase)
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
                await CheckTables(); 
                Version = await _db.DataBaseAsync.Table<DataBaseVersion>().FirstOrDefaultAsync();
            });
        }


        [RelayCommand]
        async Task CreateTables()
        {
            try
            {
                if (_db is null)
                {
                    return;
                }

                var response = await Shell.Current.DisplayAlertAsync("Generowanie tabeli", "Przy generowaniu tabeli poprzednie tabele zostają usunięte", "Tak", "Nie");
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

            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
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
                _db.SaveLogExtension(ex);
            }

        }


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

