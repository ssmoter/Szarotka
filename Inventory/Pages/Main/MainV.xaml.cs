namespace Inventory.Pages.Main;

public partial class MainV : ContentPage
{
    public MainV(MainVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}