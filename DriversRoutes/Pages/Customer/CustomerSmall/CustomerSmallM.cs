using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Customer.CustomerSmall
{
    public partial class CustomerSmallM : ObservableObject
    {
        [ObservableProperty]
        bool description;
        [ObservableProperty]
        bool phoneNumber;

        [ObservableProperty]
        bool address;

        [ObservableProperty]
        bool coordinates;


    }
}
