namespace DataBase.Pages.Log;

public partial class LogVWindows : ContentPage
{
    public LogVWindows(LogVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is LogVM vm)
        {
            Task.Run(async () =>
            {
                await vm.GetLogs();
            });
        }
    }
}