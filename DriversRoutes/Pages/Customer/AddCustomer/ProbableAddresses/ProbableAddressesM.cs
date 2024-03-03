using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesRoutes;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses
{
    public partial class ProbableAddressesM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<ResidentialAddress> residentialAddresses;

        public ProbableAddressesM()
        {
            ResidentialAddresses ??= new ObservableCollection<ResidentialAddress>();
        }



    }
}
