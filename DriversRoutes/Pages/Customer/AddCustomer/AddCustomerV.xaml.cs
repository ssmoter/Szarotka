namespace DriversRoutes.Pages.Customer.AddCustomer;

public partial class AddCustomerV : ContentPage
{

    public AddCustomerV(AddCustomerVM vm)
    {
        InitializeComponent();
        vm.FullSize = bFullSize;
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is AddCustomerVM vm)
        {
            vm.originCustomer?.Dispose();
            vm.originCustomer = new DataBase.Model.EntitiesRoutes.CustomerRoutes(vm.Customer);
            vm._address = [];
            vm.GetHelperDayOfWeek();
        }
    }
}