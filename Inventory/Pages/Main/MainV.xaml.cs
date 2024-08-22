namespace Inventory.Pages.Main;

public partial class MainV : ContentPage
{
    public MainV(MainVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is MainVM vm)
        {
            await vm.LookingForSelectedDriver();
        }

    }
}