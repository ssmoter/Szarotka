using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesInventory;

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