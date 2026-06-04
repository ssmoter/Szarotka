using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

using Inventory.Model;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate;

public partial class PopupSelectRangeDateV : Popup, IDisposable
{
    public PopupSelectRangeDateV()
    {
        InitializeComponent();
        var vm = new PopupSelectRangeDateVM();
        vm.Close += OnVmClose;

        BindingContext = vm;
    }
    public PopupSelectRangeDateV(PopupDateModel lastResult)
    {
        InitializeComponent();
        var vm = new PopupSelectRangeDateVM(lastResult);
        vm.Close += OnVmClose;

        BindingContext = vm;
    }
    public void Dispose()
    {
        if (BindingContext is PopupSelectRangeDateVM vm)
        {
            vm.Close -= OnVmClose;
        }
        GC.SuppressFinalize(this);
    }

    private Task OnVmClose(object result, CancellationToken token)
    {
        return Shell.Current.ClosePopupAsync(result, token);
        //return CloseAsync(result);
    }
}