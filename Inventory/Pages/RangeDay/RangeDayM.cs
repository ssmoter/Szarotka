using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.RangeDay
{
    public partial class RangeDayM : ObservableObject
    {
        [ObservableProperty]
        Day day;
        [ObservableProperty]
        Driver driver;

        public RangeDayM()
        {
            Day = new();
            Driver = new();
        }
    }

}
