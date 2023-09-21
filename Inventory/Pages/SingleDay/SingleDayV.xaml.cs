namespace Inventory.Pages.SingleDay;

public partial class SingleDayV : ContentPage
{
    public SingleDayV(SingleDayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var context = BindingContext as SingleDayVM;

        if (context is not null)
        {
            Task.Run(async () =>
            {
                await context.ShowCurrentDay();
            });
        }
    }
}