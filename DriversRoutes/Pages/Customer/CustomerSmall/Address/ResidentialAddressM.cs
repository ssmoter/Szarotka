using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Customer.CustomerSmall.Address
{
    public partial class ResidentialAddressM : ObservableObject
    {
        [ObservableProperty]
        bool name;
        [ObservableProperty]
        bool surname;
        [ObservableProperty]
        bool street;
        [ObservableProperty]
        bool houseNumber;
        [ObservableProperty]
        bool apartmentNumber;
        [ObservableProperty]
        bool postalCode;
        [ObservableProperty]
        bool city;
        [ObservableProperty]
        bool country;

    }
}
