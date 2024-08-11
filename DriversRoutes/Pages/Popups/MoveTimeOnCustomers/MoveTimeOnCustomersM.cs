using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Popups.MoveTimeOnCustomers
{
    public partial class MoveTimeOnCustomersM : ObservableObject
    {
        [ObservableProperty]
        DayOfWeek dayOfWeek;
        [ObservableProperty]
        TimeSpan time;

    }
}
