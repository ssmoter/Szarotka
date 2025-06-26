using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

namespace DataBase.Model.EntitiesServer
{
    public class UpdateDifferences : ObservableObject
    {
        private IList<UpdateDifferencesDriverRoutes>? updateDifferencesDriverRoutes;
        public IList<UpdateDifferencesDriverRoutes>? UpdateDifferencesDriverRoutes
        {
            get => updateDifferencesDriverRoutes;
            set
            {
                if (SetProperty(ref updateDifferencesDriverRoutes, value, nameof(UpdateDifferencesDriverRoutes)))
                {
                }
            }
        }

        private IList<UpdateDifferencesInventory>? updateDifferencesInventory;
        public IList<UpdateDifferencesInventory>? UpdateDifferencesInventory
        {
            get => updateDifferencesInventory;
            set
            {
                if (SetProperty(ref updateDifferencesInventory, value, nameof(UpdateDifferencesInventory)))
                {
                }
            }
        }
    }


    public class UpdateDifferencesDriverRoutes : ObservableObject
    {
        private CustomerRoutes server = new();
        public CustomerRoutes Server
        {
            get => server;
            set
            {
                if (SetProperty(ref server, value, nameof(Server)))
                {
                }
            }
        }
        private CustomerRoutes update = new();
        public CustomerRoutes Update
        {
            get => update;
            set
            {
                if (SetProperty(ref update, value, nameof(Update)))
                {
                }
            }
        }

    }
    public class UpdateDifferencesInventory : ObservableObject
    {
        private Day server = new();
        public Day Server
        {
            get => server;
            set
            {
                if (SetProperty(ref server, value, nameof(Server)))
                {
                }
            }
        }
        private Day update = new();
        public Day Update
        {
            get => update;
            set
            {
                if (SetProperty(ref update, value, nameof(Update)))
                {
                }
            }
        }
    }
}
