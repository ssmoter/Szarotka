using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateM : ObservableObject
    {
        public DataBase.Model.EntitiesServer.User Driver { get; set; }

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

        public PopupSelectRangeDateM(DataBase.Model.EntitiesServer.User user)
        {
            Driver = user;
            name = user.Name;
        }
    }
}
