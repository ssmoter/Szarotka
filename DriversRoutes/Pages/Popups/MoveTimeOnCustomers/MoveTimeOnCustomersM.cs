using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

public partial class MoveTimeOnCustomersM : ObservableObject
{
    private DayOfWeek dayOfWeek;
    public DayOfWeek DayOfWeek
    {
        get => dayOfWeek;
        set
        {
            if (SetProperty(ref dayOfWeek, value, nameof(DayOfWeek))) { }
        }
    }
    private TimeSpan time;
    public TimeSpan Time
    {
        get => time;
        set
        {
            if (SetProperty(ref time, value, nameof(Time))) { }
        }

    }
}
