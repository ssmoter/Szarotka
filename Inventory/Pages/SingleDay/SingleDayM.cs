using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.SingleDay
{
    public partial class SingleDayM : ObservableObject
    {
        [ObservableProperty]
        bool productIsVisible = true;
        [ObservableProperty]
        bool cakeIsVisible = false;
        [ObservableProperty]
        bool productIsRefreshing;

        [ObservableProperty]
        int cakeSortPriceRotateX;
        [ObservableProperty]
        int cakeSortDateRotateX;

        [ObservableProperty]
        bool cakeAllIsVisible;
    }
}
