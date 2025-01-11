using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesRoutes;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses
{
    public partial class ProbableAddressesM : ObservableObject
    {
        private ObservableCollection<ResidentialAddress> residentialAddresses;
        public ObservableCollection<ResidentialAddress> ResidentialAddresses
        {
            get => residentialAddresses;
            set
            {
                if (SetProperty(ref residentialAddresses, value, nameof(ResidentialAddresses))) { }
            }
        }

        public ProbableAddressesM()
        {
            ResidentialAddresses ??= new ObservableCollection<ResidentialAddress>();
        }



    }
}
