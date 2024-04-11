using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate;

public partial class PopupSelectRangeDateV : Popup, IDisposable
{
    public PopupSelectRangeDateV(Driver[] drivers)
    {
        InitializeComponent();
        var vm = new PopupSelectRangeDateVM(drivers);
        vm.Close += CloseAsync;

        BindingContext = vm;
    }

    public void Dispose()
    {
        if (BindingContext is PopupSelectRangeDateVM vm)
        {
            vm.Close -= CloseAsync;
        }
        GC.SuppressFinalize(this);
    }
}