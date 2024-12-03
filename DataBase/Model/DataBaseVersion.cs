using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model
{
    public partial class DataBaseVersion : ObservableObject
    {
        [PrimaryKey]
        public int Id { get; set; } = 0;

        [ObservableProperty]
        int dataBase;
        [ObservableProperty]
        int inventory;
        [ObservableProperty]
        int driversRoutes;
        [ObservableProperty]
        long lastBackup = DateTime.Today.Ticks;
        public DataBaseVersion()
        {
            DataBase = 1;
            Inventory = 1;
            DriversRoutes = 1;
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
