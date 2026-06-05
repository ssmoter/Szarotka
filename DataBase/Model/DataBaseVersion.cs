using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model
{
    public partial class DataBaseVersion : ObservableObject
    {
        [PrimaryKey]
        public int Id { get; set; } = 0;

        private int dataBase;
        public int DataBase
        {
            get => dataBase;
            set
            {
                if (SetProperty(ref dataBase, value, nameof(DataBase))) { }
            }
        }
        private int inventory;
        public int Inventory
        {
            get => inventory;
            set
            {
                if (SetProperty(ref inventory, value, nameof(Inventory))) { }
            }
        }
        private int driversRoutes;
        public int DriversRoutes
        {
            get => driversRoutes;
            set
            {
                if (SetProperty(ref driversRoutes, value, nameof(DriversRoutes))) { }
            }
        }
        private long lastBackup = DateTime.Today.Ticks;
        public long LastBackup
        {
            get => lastBackup;
            set
            {
                if (SetProperty(ref lastBackup, value, nameof(LastBackup))) { }
            }
        }
        public DataBaseVersion()
        {
            DataBase = 3;
            Inventory = 3;
            DriversRoutes = 2;
        }

        public override bool Equals(object? obj)
        {
            if (obj is DataBaseVersion version)
            {
                if (version.DataBase != DataBase)
                {
                    return false;
                }
                if (version.Inventory != Inventory)
                {
                    return false;
                }
                if (version.DriversRoutes != DriversRoutes)
                {
                    return false;
                }
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
