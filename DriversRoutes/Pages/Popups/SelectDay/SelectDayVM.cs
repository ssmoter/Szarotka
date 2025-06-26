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


        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default)
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
        async Task CancelAndReturn()
        {
            await OnClose(null);
        }

        #endregion

    }
}
