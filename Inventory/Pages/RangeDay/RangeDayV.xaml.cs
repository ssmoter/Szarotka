namespace Inventory.Pages.RangeDay;

public partial class RangeDayV : ContentPage
{
    readonly RangeDayVM _vm;
    public RangeDayV(RangeDayVM vm)
    {
        InitializeComponent();
        this._vm = vm;
        BindingContext = vm;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private void SwipeItem_Invoked_OpenDay(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not RangeDayM product) { return; }

        _vm.OpenDetailpageCommand.Execute(product);
    }
}