using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model;

namespace SzarotkaBlazor.Pages.Options.Main
{
    public partial class MainOptionsM : ObservableObject
    {
        private bool main;
        public bool Main
        {
            get => main;
            set
            {
                if (SetProperty(ref main, value, nameof(Main))) { }
            }
        }

        private bool inventory;
        public bool Inventory
        {
            get => inventory;
            set
            {
                if (SetProperty(ref inventory, value, nameof(Inventory))) { }
            }
        }

        private bool driversRoutes;
        public bool DriversRoutes
        {
            get => driversRoutes;
            set
            {
                if (SetProperty(ref driversRoutes, value, nameof(DriversRoutes))) { }
            }
        }

        private DataBaseVersion version = new();
        public DataBaseVersion Version
        {
            get => version;
            set
            {
                if (SetProperty(ref version, value, nameof(Version))) { }
            }
        }

        public MainOptionsM()
        {
            Version = new DataBaseVersion();
        }
    }
}
