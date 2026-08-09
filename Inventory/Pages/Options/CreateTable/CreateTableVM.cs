using CommunityToolkit.Mvvm.ComponentModel;

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

        readonly IAccessDataBaseAoT _db;
        public CreateTableVM(IAccessDataBaseAoT dataBase)
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

            CheckTables();
            Version = CreatedDataBase.GetDataBaseVersion(_db);

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

