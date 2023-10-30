using CommunityToolkit.Mvvm.ComponentModel;

using Inventory.Model;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateM : ObservableObject
    {
        public Driver Driver { get; set; }

        [ObservableProperty]
        string name;

        [ObservableProperty]
        bool isChecked;

        public PopupSelectRangeDateM(Driver driver)
        {
            Driver = driver;
            name = driver.Name;
        }
    }
}
