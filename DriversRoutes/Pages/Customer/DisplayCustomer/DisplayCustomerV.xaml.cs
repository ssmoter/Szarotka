namespace DriversRoutes.Pages.Customer.DisplayCustomer;

public partial class DisplayCustomerV : ContentPage
{
    public DisplayCustomerV(DisplayCustomerVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }



}