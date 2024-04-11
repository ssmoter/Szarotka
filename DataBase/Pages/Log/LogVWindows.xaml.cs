namespace DataBase.Pages.Log;

public partial class LogVWindows : ContentPage
{
    public LogVWindows(LogVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is LogVM vm)
        {
            await vm.GetLogs();
        }
    }
}