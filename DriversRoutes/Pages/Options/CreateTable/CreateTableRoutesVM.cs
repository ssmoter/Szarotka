using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Options.CreateTable
{
    public partial class CreateTableRoutesVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<CreateTableRoutesM> tableMs;

        readonly AccessDataBase _db;

        public CreateTableRoutesVM(AccessDataBase db)
        {
            _db = db;

            TableMs = new ObservableCollection<CreateTableRoutesM>
            {
                new CreateTableRoutesM() { RealTableName = nameof(Routes), TableName = "- z trasami" },
                new CreateTableRoutesM() { RealTableName = nameof(CustomerRoutes), TableName = "- z przystankami" },
                new CreateTableRoutesM() { RealTableName = nameof(SelectedDayOfWeekRoutes), TableName = "- z dniami przyjazdu" },
                new CreateTableRoutesM() { RealTableName = nameof(ResidentialAddress), TableName = "- z adresami" },

            };

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

                await _db.DataBaseAsync.CreateTableAsync<Routes>();
                await _db.DataBaseAsync.CreateTableAsync<CustomerRoutes>();
                await _db.DataBaseAsync.CreateTableAsync<SelectedDayOfWeekRoutes>();
                await _db.DataBaseAsync.CreateTableAsync<ResidentialAddress>();

                CreateDefoutlRoutes();
                await CheckTables();

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
            var routes = new List<Routes>
            {
                new Routes()
                {
                    Id = GetGuidSed(),
                    Name = "Szyk"
                },
                new Routes()
                {
                    Id = GetGuidSed(),
                    Name = "Pasierbiec"
                },
                new Routes()
                {
                    Id = GetGuidSed(),
                    Name = "Słopnice"
                },
                 new Routes()
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
