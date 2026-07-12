using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

namespace DriversRoutes.Pages.Popups.SelectDay;

public partial class SelectDayV : Popup
{
    public SelectDayV(SelectDayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

}