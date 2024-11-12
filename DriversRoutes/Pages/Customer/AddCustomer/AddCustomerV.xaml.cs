namespace DriversRoutes.Pages.Customer.AddCustomer;

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
            vm.GetHelperDayOfWeek();
        }
#if ANDROID || IOS
        bTimeSize.WidthRequest = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
#endif
    }
}