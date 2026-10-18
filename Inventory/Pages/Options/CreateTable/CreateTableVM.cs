using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Helper;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

using Shared.Data;
using Shared.Helper;

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

        private ObservableCollection<Routes> routes;
        public ObservableCollection<Routes> Routes
        {
            get => routes;
            set => SetProperty(ref routes, value, nameof(Routes));
        }
        private Routes selectedRoutes;
        public Routes SelectedRoutes
        {
            get => selectedRoutes;
            set
            {
                if (SetProperty(ref selectedRoutes, value, nameof(SelectedRoutes)))
                {
                    SaveSelectedRoute(value).Forget();
                }
            }
        }
        private async Task SaveSelectedRoute(Routes route)
        {
            var helperTable = new HelperTable()
            {
                Name = RoutesName,
                Value = route.Id.ToString(),
                Created = _db.TimeService.UtcNow(),
                Updated = _db.TimeService.UtcNow(),
                Id = Guid.CreateVersion7(),
                UserCreatedId = UserAfterLogin.User.Id,
                UserUpdatedId = UserAfterLogin.User.Id,
            };
            await helperTable.SetAsync(_db);
        }

        private readonly IAccessDataBaseAoT _db;
        private readonly IGetDriverRoutesAoT _getRoutes;
        private readonly ISaveDriverRoutesAoT _saveRoutes;

        private const string RoutesName = $"{nameof(Day)}.{nameof(Day.SelectedRoute)}";


        public CreateTableVM(IAccessDataBaseAoT dataBase, IGetDriverRoutesAoT getRoutes, ISaveDriverRoutesAoT saveRoutes)
        {
            TableMs =
            [
                new() { RealTableName = nameof(Day), TableName = "- z dniami" },
                new() { RealTableName = nameof(Product), TableName = "- z produktami" },
                new() { RealTableName = nameof(Cake), TableName = "- z ciastami" },
                new() { RealTableName = nameof(ProductName), TableName = "- z nazwami produktów" },
                new() { RealTableName = nameof(ProductPrice), TableName = "- z cenami produktów" },
            ];

            this._db = dataBase;

            CheckTables();
            Version = CreatedDataBase.GetDataBaseVersion(_db);
            _getRoutes = getRoutes;
            _saveRoutes = saveRoutes;

            Routes = [];
            //SelectedRoutes = new();
        }

        [RelayCommand]
        private async Task InitializeRoutes()
        {
            var routesList = await _getRoutes.Routes();
            Routes = new ObservableCollection<Routes>(routesList);
            var helper = await RoutesName.GetHelperTableAsync(_db);
            if (helper is not null && !string.IsNullOrWhiteSpace(helper.Value))
            {
                Guid id = Guid.Parse(helper.Value);
                SelectedRoutes = Routes.FirstOrDefault(x => x.Id == id) ?? new();
            }
        }


        void CheckTables()
        {
            for (int i = 0; i < TableMs.Count; i++)
            {
                TableMs[i].IsExist = CheckIsExist(TableMs[i].RealTableName);
            }
        }
        bool CheckIsExist(string table)
        {
            var tableInfo = CreatedDataBase.GetTableInfo(_db, table);
            bool exist = tableInfo.Any();
            return exist;
        }


    }
}

