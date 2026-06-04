using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateM(DataBase.Model.EntitiesServer.User user) : ObservableObject
    {
        public DataBase.Model.EntitiesServer.User Driver { get; set; } = user;

        private string name = user.Name;
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
    }
}
