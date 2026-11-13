using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateM : ObservableObject
    {
        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (SetProperty(ref isChecked, value, nameof(IsChecked))) { }
            }
        }

        private Guid id;
        public Guid Id
        {
            get => id;
            set
            {
                if (SetProperty(ref id, value, nameof(Id))) { }
            }
        }

        public PopupSelectRangeDateM(Guid id) => Id = id;
    }
}
