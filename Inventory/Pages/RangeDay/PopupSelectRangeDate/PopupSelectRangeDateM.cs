using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateM : ObservableObject
    {
        public Driver Driver { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value, nameof(Name))) { }
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (SetProperty(ref isChecked, value, nameof(IsChecked))) { }
            }
        }

        public PopupSelectRangeDateM(Driver driver)
        {
            Driver = driver;
            name = driver.Name;
        }
    }
}
