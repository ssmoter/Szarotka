using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Popups.SelectDay
{
    public partial class SelectDayVM : ObservableObject
    {
        private SelectedDayOfWeekRoutes selectDayMs;
        public SelectedDayOfWeekRoutes SelectDayMs
        {
            get => selectDayMs;
            set
            {
                if (SetProperty(ref selectDayMs, value, nameof(SelectDayMs))) { }
            }
        }

        private int selectDayIndex;
        public int SelectDayIndex
        {
            get => selectDayIndex;
            set
            {
                if (SetProperty(ref selectDayIndex, value, nameof(SelectDayIndex))) { }
            }
        }

        private readonly IPopupService _popupService;

        public SelectDayVM(IPopupService popupService)
        {
            this.SelectDayMs ??= new();
            _popupService = popupService;
        }


        [RelayCommand]
        async Task SaveAndReturn()
        {
            await _popupService.ClosePopupAsync<SelectedDayOfWeekRoutes>(page: Shell.Current, result: SelectDayMs);
        }
        [RelayCommand]
        async Task CancelAndReturn()
        {
            await _popupService.ClosePopupAsync(page: Shell.Current);
        }


    }
}
