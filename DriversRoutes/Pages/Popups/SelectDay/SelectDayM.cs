using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Popups.SelectDay
{
    public partial class SelectDayM : ObservableObject
    {
        [ObservableProperty]
        DayOfWeek day;
        [ObservableProperty]
        string name;
    }
}
