using CommunityToolkit.Mvvm.ComponentModel;

using DriversRoutes.Model;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.AddCustomer.ProbableAddresses
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
