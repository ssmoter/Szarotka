namespace Shared.Pages.Log;

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
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not LogM product) { return; }

        _vm.OpenSelectedLogCommand.Execute(product);
    }

    private void SwipeItem_Invoked_Delete(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not LogM product) { return; }

        _vm.DeleteLogCommand.Execute(product);
    }
}