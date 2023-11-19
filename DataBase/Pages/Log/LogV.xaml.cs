namespace DataBase.Pages.Log;

public partial class LogV : ContentPage
{
    readonly LogVM _vm;
    public LogV(LogVM vm)
    {
        InitializeComponent();
        this._vm = vm;
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

    private void SwipeItem_Invoked_MoreDate(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null) { return; }

        var product = item.BindingContext as LogM;
        if (product == null) { return; }

        _vm.OpenSelectedLogCommand.Execute(product);
    }

    private void SwipeItem_Invoked_Delete(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null) { return; }

        var product = item.BindingContext as LogM;
        if (product == null) { return; }

        _vm.DeleteLogCommand.Execute(product);
    }
}