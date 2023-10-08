using CommunityToolkit.Maui.Views;

namespace DriversRoutes.Pages.Popups.SelectDay;

public partial class SelectDayV : Popup
{
    public SelectDayV()
    {
        InitializeComponent();
        var vm = new SelectDayVM();
        vm.Close += CloseAsync;

        BindingContext = vm;
    }
}