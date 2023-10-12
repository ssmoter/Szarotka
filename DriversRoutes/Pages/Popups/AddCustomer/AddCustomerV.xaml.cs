using CommunityToolkit.Maui.Views;

namespace DriversRoutes.Pages.Popups.AddCustomer;

public partial class AddCustomerV : Popup
{
    public AddCustomerV(Model.CustomerRoutes customer)
    {
        InitializeComponent();
        var vm = new AddCustomerVM(customer);
        vm.Close += CloseAsync;
        BindingContext = vm;
    }
}