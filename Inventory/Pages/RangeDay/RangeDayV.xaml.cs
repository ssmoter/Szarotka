namespace Inventory.Pages.RangeDay;

public partial class RangeDayV : ContentPage
{
    public RangeDayV(RangeDayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}