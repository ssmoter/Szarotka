using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses;

public partial class ProbableAddressesV : Popup
{
    public ProbableAddressesV(ProbableAddressesVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }


}