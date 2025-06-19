using CommunityToolkit.Mvvm.ComponentModel;

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
    }


    public class UpdateDifferencesDriverRoutes : ObservableObject
    {
        private CustomerRoutes? customerOrigin;
        public CustomerRoutes? CustomerOrigin
        {
            get => customerOrigin;
            set
            {
                if (SetProperty(ref customerOrigin, value, nameof(CustomerOrigin)))
                {
                }
            }
        }
        private CustomerRoutes? customerUpdate;
        public CustomerRoutes? CustomerUpdate
        {
            get => customerUpdate;
            set
            {
                if (SetProperty(ref customerUpdate, value, nameof(CustomerUpdate)))
                {
                }
            }
        }
    }
}
