using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewPage
{
    public partial class SingleDayPreviewPageVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(Day), out object day))
            {
                if (day is Day _day)
                {
                    Day = _day;
                }
            }
        }

        private Day day;
        public Day Day
        {
            get => day;
            set
            {
                if (SetProperty(ref day, value, nameof(Day)))
                {
                }
            }
        }

        public SingleDayPreviewPageVM()
        {
            day = new();
        }

        [RelayCommand]
        async Task GoToEdit()
        {
            await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(DataBase.Model.EntitiesInventory.Day)] = Day,

                });
        }
    }
}
