using CommunityToolkit.Mvvm.ComponentModel;

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

        readonly IAccessDataBaseAoT _db;

        public CreateTableRoutesVM(IAccessDataBaseAoT db)
        {
            _db = db;

            TableMs =
            [
                new() { RealTableName = nameof(Routes), TableName = "- z trasami" },
                new() { RealTableName = nameof(CustomerRoutes), TableName = "- z przystankami" },
                new() { RealTableName = nameof(SelectedDayOfWeekRoutes), TableName = "- z dniami przyjazdu" },
                new() { RealTableName = nameof(ResidentialAddress), TableName = "- z adresami" },

            ];
            Version = CreatedDataBase.GetDataBaseVersion(db);
            CheckTables();
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

