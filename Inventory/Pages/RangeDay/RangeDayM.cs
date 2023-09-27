using CommunityToolkit.Mvvm.ComponentModel;

using Inventory.Model.MVVM;

namespace Inventory.Pages.RangeDay
{
    public partial class RangeDayM : ObservableObject
    {
        [ObservableProperty]
        DayM dayM;
        [ObservableProperty]
        DriverM driver;

        public RangeDayM()
        {
            DayM = new DayM();
            Driver = new DriverM();
        }
    }

}
