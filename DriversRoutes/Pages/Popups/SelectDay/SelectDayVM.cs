using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Popups.SelectDay
{
    public partial class SelectDayVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<SelectDayM> selectDayMs;

        [ObservableProperty]
        int selectDayIndex;

        public Func<object, Task> Close;
        public Task OnClose(object result = null)
        {
            return Close?.Invoke(result);
        }
        public SelectDayVM()
        {
            SelectDayMs = new ObservableCollection<SelectDayM>()
            {
                new SelectDayM() {Day = DayOfWeek.Monday,Name="Poniedziałek"},
                new SelectDayM() {Day = DayOfWeek.Tuesday,Name="Wtorek"},
                new SelectDayM() {Day = DayOfWeek.Wednesday,Name="Środa"},
                new SelectDayM() {Day = DayOfWeek.Thursday,Name="Czwartek"},
                new SelectDayM() {Day = DayOfWeek.Friday,Name="Piątek"},
                new SelectDayM() {Day = DayOfWeek.Saturday,Name="Sobota"},
                new SelectDayM() {Day = DayOfWeek.Sunday,Name="Niedziela"},
            };
        }


        #region Command
        [RelayCommand]
        async Task SaveAndReturn()
        {
            await OnClose(SelectDayMs[SelectDayIndex].Day);
        }
        [RelayCommand]
        async Task CancelAndRetur()
        {
            await OnClose(null);
        }

        #endregion

    }
}
