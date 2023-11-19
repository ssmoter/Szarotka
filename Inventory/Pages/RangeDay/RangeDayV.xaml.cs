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
        var item = sender as SwipeItem;
        if (item is null) { return; }

        var product = item.BindingContext as RangeDayM;
        if (product == null) { return; }

        _vm.OpenDetailpageCommand.Execute(product);
    }
}