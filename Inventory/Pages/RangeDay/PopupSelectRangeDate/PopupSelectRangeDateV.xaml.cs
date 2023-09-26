using CommunityToolkit.Maui.Views;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate;

public partial class PopupSelectRangeDateV : Popup
{
    public PopupSelectRangeDateV()
    {
        InitializeComponent();
        BindingContext = new PopupSelectRangeDateVM();
    }
}