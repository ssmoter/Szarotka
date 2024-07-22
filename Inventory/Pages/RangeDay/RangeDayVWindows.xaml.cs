namespace Inventory.Pages.RangeDay;

public partial class RangeDayVWindows : ContentPage
{
    public RangeDayVWindows(RangeDayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}