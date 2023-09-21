using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.SingleDay
{
    public partial class SingleDayM : ObservableObject
    {
        [ObservableProperty]
        bool productIsVisible=true;
        [ObservableProperty]
        bool cakeIsVisible = false;
    }
}
