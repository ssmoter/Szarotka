using CommunityToolkit.Maui.Views;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate;

public partial class PopupSelectRangeDateV : Popup
{
    public PopupSelectRangeDateV(PopupSelectRangeDateVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}