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

        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default(CancellationToken))
        {
            return Close?.Invoke(result, token);
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
