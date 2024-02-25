using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Customer.DisplayCustomer
{
    public partial class DisplayCustomerM : ObservableObject
    {
        bool showLocationThisCustomer;
        public bool ShowLocationThisCustomer
        {
            get => showLocationThisCustomer;
            set
            {
                if (SetProperty(ref showLocationThisCustomer, value))
                {
                    OnPropertyChanged(nameof(ShowLocationThisCustomer));

                    if (ShowLocationThisCustomerInt == 0)
                        ShowLocationThisCustomerInt = 1;
                    else
                        ShowLocationThisCustomerInt = 0;
                }
            }
        }

        [ObservableProperty]
        int showLocationThisCustomerInt;
    }
}
