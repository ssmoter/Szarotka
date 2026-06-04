using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model;
using DataBase.Model.EntitiesRoutes;

using Shared.Data;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Options.CreateTable
{
    public partial class CreateTableRoutesVM : ObservableObject
    {
        private ObservableCollection<CreateTableRoutesM> tableMs;
        public ObservableCollection<CreateTableRoutesM> TableMs
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

        public CreateTableRoutesVM(IAccessDataBase db)
        {
            _db = db;

            TableMs =
            [
                new() { RealTableName = nameof(Routes), TableName = "- z trasami" },
                new() { RealTableName = nameof(CustomerRoutes), TableName = "- z przystankami" },
                new() { RealTableName = nameof(SelectedDayOfWeekRoutes), TableName = "- z dniami przyjazdu" },
                new() { RealTableName = nameof(ResidentialAddress), TableName = "- z adresami" },

            ];

            Task.Run(async () =>
            {
                try
                {
                    await CheckTables();
                    Version = await _db.DataBaseAsync.Table<DataBaseVersion>().FirstOrDefaultAsync();

                }
                catch (Exception ex)
                {
                    _db.SaveLogExtension(ex);
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

                var response = await Shell.Current.DisplayAlertAsync("Generowanie tabeli", "Przy generowaniu tabeli poprzednie tabele zostają usunięte", "Tak", "Nie");
                if (!response)
                {
                    return;
                }

                await _db.DataBaseAsync.DropTableAsync<Routes>();
                await _db.DataBaseAsync.DropTableAsync<CustomerRoutes>();
                await _db.DataBaseAsync.DropTableAsync<SelectedDayOfWeekRoutes>();
                await _db.DataBaseAsync.DropTableAsync<ResidentialAddress>();


                await CheckTables();

                await _db.DataBaseAsync.CreateTableAsync<Routes>();
                await _db.DataBaseAsync.CreateTableAsync<CustomerRoutes>();
                await _db.DataBaseAsync.CreateTableAsync<SelectedDayOfWeekRoutes>();
                await _db.DataBaseAsync.CreateTableAsync<ResidentialAddress>();

                CreateDefoutlRoutes();

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

                await _db.DataBaseAsync.CreateTableAsync<Routes>();
                await _db.DataBaseAsync.CreateTableAsync<CustomerRoutes>();
                await _db.DataBaseAsync.CreateTableAsync<SelectedDayOfWeekRoutes>();
                await _db.DataBaseAsync.CreateTableAsync<ResidentialAddress>();

                CreateDefoutlRoutes();
                await CheckTables();

            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }

        }


        #endregion

        #region Method

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

        readonly Random random = new(1337);
        void CreateDefoutlRoutes()
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

            var check = _db.DataBase.Table<Routes>().Count();
            if (check > 0)
            {
                return;
            }
            _db.DataBase.InsertAll(routes);
        }
        Guid GetGuidSed()
        {
            byte[] guidBytes = new byte[16];
            random.NextBytes(guidBytes);
            return new Guid(guidBytes);
        }
        #endregion
    }
}

