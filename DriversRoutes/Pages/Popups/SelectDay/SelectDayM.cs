using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Popups.SelectDay
{
    public partial class SelectDayM : ObservableObject
    {
        private DayOfWeek day;
        public DayOfWeek Day
        {
            get => day;
            set
            {
                if (SetProperty(ref day, value, nameof(Day))) { }
            }
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value, nameof(Name))) { }
            }
        }
    }
}
