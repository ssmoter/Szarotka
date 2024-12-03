using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model;

namespace Shared.Pages.UpdateDataBase
{
    public partial class UpdateDataBaseM : ObservableObject
    {
        [ObservableProperty]
        DataBaseVersion fromVersion;
        [ObservableProperty]
        DataBaseVersion uppdateVersion;
        [ObservableProperty]
        DataBaseVersion toVersion;


        [ObservableProperty]
        double dataBaseProgresBar;
        [ObservableProperty]
        double inventioryProgresBar;
        [ObservableProperty]
        double driverRoutesProgresBar;

        [ObservableProperty]
        bool dataBaseIsVisible = true;
        [ObservableProperty]
        bool inventioryIsVisible = true;
        [ObservableProperty]
        bool driverRoutesIsVisible = true;

        [ObservableProperty]
        bool backIsVisible;

        public UpdateDataBaseM()
        {
            FromVersion = new DataBaseVersion() { DataBase = 0, DriversRoutes = 0, Inventory = 0 };
            ToVersion = new DataBaseVersion();
        }

    }
}
