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

    private void Entry_TextChanged_SetValueToSecendPosition(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            if (entry.Text.Length > 0 && entry.Text.Length <= 1)
            {
                entry.CursorPosition = 1;
            }
        }

    }
}