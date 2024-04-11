namespace Inventory.Pages.SingleDay;

public partial class SingleDayVWindows : ContentPage
{
    public SingleDayVWindows(SingleDayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var context = BindingContext as SingleDayVM;

        if (context is not null)
        {
            await context.ShowCurrentDay();
        }
    }
}