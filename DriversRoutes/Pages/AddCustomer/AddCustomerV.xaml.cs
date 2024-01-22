namespace DriversRoutes.Pages.AddCustomer;

public partial class AddCustomerV : ContentPage
{

    public AddCustomerV(AddCustomerVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is AddCustomerVM vm)
        {
            if (vm.Customer is not null)
            {
                vm.SetCustomer(vm.Customer);
            }
        }
    }
}