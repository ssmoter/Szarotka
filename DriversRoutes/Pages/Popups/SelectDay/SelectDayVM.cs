using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DriversRoutes.Model;

namespace DriversRoutes.Pages.Popups.SelectDay
{
    public partial class SelectDayVM : ObservableObject
    {
        [ObservableProperty]
        SelectedDayOfWeekRoutes selectDayMs;

        [ObservableProperty]
        int selectDayIndex;

        public Func<object, Task> Close;
        public Task OnClose(object result = null)
        {
            return Close?.Invoke(result);
        }
        public SelectDayVM()
        {
            this.SelectDayMs ??= new();
        }


        #region Command
        [RelayCommand]
        async Task SaveAndReturn()
        {
            await OnClose(SelectDayMs);
        }
        [RelayCommand]
        async Task CancelAndRetur()
        {
            await OnClose(null);
        }

        #endregion

    }
}
