using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model;

namespace Shared.Pages.UpdateDataBase
{
    public partial class UpdateDataBaseM : ObservableObject
    {
        private DataBaseVersion fromVersion;
        public DataBaseVersion FromVersion
        {
            get => fromVersion;
            set
            {
                if (SetProperty(ref fromVersion, value))
                {
                    OnPropertyChanged(nameof(FromVersion));
                }
            }
        }

        private DataBaseVersion updateVersion;
        public DataBaseVersion UpdateVersion
        {
            get => updateVersion;
            set
            {
                if (SetProperty(ref updateVersion, value))
                {
                    OnPropertyChanged(nameof(UpdateVersion));
                }
            }
        }
        private DataBaseVersion toVersion;
        public DataBaseVersion ToVersion
        {
            get => toVersion;
            set
            {
                if (SetProperty(ref toVersion, value))
                {
                    OnPropertyChanged(nameof(toVersion));
                }
            }
        }


        private double dataBaseProgressBar;
        public double DataBaseProgressBar
        {
            get => dataBaseProgressBar;
            set
            {
                if (SetProperty(ref dataBaseProgressBar, value))
                {
                    OnPropertyChanged(nameof(DataBaseProgressBar));
                }
            }
        }
        private double inventoryProgressBar;
        public double InventoryProgressBar
        {
            get => inventoryProgressBar;
            set
            {
                if (SetProperty(ref inventoryProgressBar, value))
                {
                    OnPropertyChanged(nameof(InventoryProgressBar));
                }
            }
        }

        private double driverRoutesProgressBar;
        public double DriverRoutesProgressBar
        {
            get => driverRoutesProgressBar;
            set
            {
                if (SetProperty(ref driverRoutesProgressBar, value))
                {
                    OnPropertyChanged(nameof(DriverRoutesProgressBar));
                }
            }
        }

        private bool dataBaseIsVisible = true;
        public bool DataBaseIsVisible
        {
            get => dataBaseIsVisible;
            set
            {
                if (SetProperty(ref dataBaseIsVisible, value))
                {
                    OnPropertyChanged(nameof(DataBaseIsVisible));
                }
            }
        }

        private bool inventoryIsVisible = true;
        public bool InventoryIsVisible
        {
            get => inventoryIsVisible;
            set
            {
                if (SetProperty(ref inventoryIsVisible, value))
                {
                    OnPropertyChanged(nameof(InventoryIsVisible));
                }
            }
        }

        private bool driverRoutesIsVisible = true;
        public bool DriverRoutesIsVisible
        {
            get => driverRoutesIsVisible;
            set
            {
                if (SetProperty(ref driverRoutesIsVisible, value))
                {
                    OnPropertyChanged(nameof(DriverRoutesIsVisible));
                }
            }
        }

        private bool backIsVisible;
        public bool BackIsVisible
        {
            get => backIsVisible;
            set
            {
                if (SetProperty(ref backIsVisible, value))
                {
                    OnPropertyChanged(nameof(BackIsVisible));
                }
            }
        }

        public UpdateDataBaseM()
        {
            FromVersion = new DataBaseVersion() { DataBase = 0, DriversRoutes = 0, Inventory = 0 };
            ToVersion = new DataBaseVersion();
        }

    }
}
