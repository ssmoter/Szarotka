using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

namespace DriversRoutes.Pages.Popups.SelectDay;

public partial class SelectDayV : Popup
{
    public SelectDayV()
    {
        InitializeComponent();
        var vm = new SelectDayVM();
        vm.Close += OnVmClose;

        BindingContext = vm;
    }

    private Task OnVmClose(object result, CancellationToken token)
    {
        return Shell.Current.ClosePopupAsync(result, token);
        //return CloseAsync(result);
    }
}