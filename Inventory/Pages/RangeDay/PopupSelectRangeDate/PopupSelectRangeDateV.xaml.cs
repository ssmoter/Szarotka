using CommunityToolkit.Maui.Views;

using Inventory.Model;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate;

public partial class PopupSelectRangeDateV : Popup
{
    public PopupSelectRangeDateV(Driver[] drivers)
    {
        InitializeComponent();
        var vm = new PopupSelectRangeDateVM(drivers);
        vm.Close += CloseAsync;

        BindingContext = vm;
    }
}