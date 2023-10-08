using CommunityToolkit.Maui.Views;

namespace DriversRoutes.Pages.Popups.AddCustomer;

public partial class AddCustomerV : Popup
{
    public AddCustomerV(Model.Customer customer)
    {
        InitializeComponent();
        var vm = new AddCustomerVM(customer);
        BindingContext = vm;
    }
}