using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Shared.Helper;
using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Popups.MoveTimeOnCustomers
{
    public partial class MoveTimeOnCustomersVM : ObservableObject
    {
        private const string after = "po godzinie";
        private const string befor = "przed godziną";
        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default)
        {
            return Close?.Invoke(result, token);
        }


        [ObservableProperty]
        SelectedDayOfWeekRoutes selectDayMs;

        [ObservableProperty]
        TimeSpan selectedTime;

        [ObservableProperty]
        TimeSpan addTime;

        [ObservableProperty]
        char sign;


        public MoveTimeOnCustomersVM(SelectedDayOfWeekRoutes selectDayMs)
        {
            SelectDayMs = new(selectDayMs)
            {
                Sunday = false,
                Monday = false,
                Tuesday = false,
                Wednesday = false,
                Thursday = false,
                Friday = false,
                Saturday = false,
            };
        }


        public void SetTimeFromSelectDayMs(TimeSpan time)
        {
            SelectedTime = new TimeSpan(time.Ticks);
        }


        #region Command
        [RelayCommand]
        async Task SaveAndReturn()
        {
            if (!Helper.HelperDayOfWeek.IfAnyIsTrue(SelectDayMs))
            {
                await Shell.Current.DisplayAlert("Nie wybrana dnia tygodnia", "Aby zapisać zmiany zaznacz dzień tygonia", "Ok");
                return;
            }

            var dayOfWeek = Helper.HelperDayOfWeek.GetFirstDayOfWeek(SelectDayMs);

            string hours = after;

            if (Sign == ' ')
            {
                hours = befor;
            }

            if (!await Shell.Current.DisplayAlert("Zmiany", $"Przesunąć czas punktów w dniu {dayOfWeek.TranslateSelectedDay()} {hours} {SelectedTime.Hours}:{SelectedTime.Minutes} {Sign}{AddTime.TotalMinutes} minut", "Tak", "Nie"))
            {
                return;
            }

            SelectDayMs.FridayTimeSpan = new TimeSpan(AddTime.Ticks);
            SelectDayMs.MondayTimeSpan = new TimeSpan(AddTime.Ticks);
            SelectDayMs.SaturdayTimeSpan = new TimeSpan(AddTime.Ticks);
            SelectDayMs.SundayTimeSpan = new TimeSpan(AddTime.Ticks);
            SelectDayMs.ThursdayTimeSpan = new TimeSpan(AddTime.Ticks);
            SelectDayMs.TuesdayTimeSpan = new TimeSpan(AddTime.Ticks);
            SelectDayMs.WednesdayTimeSpan = new TimeSpan(AddTime.Ticks);


            await OnClose(SelectDayMs);
        }
        [RelayCommand]
        async Task CancelAndRetur()
        {
            await OnClose(null);
        }
        [RelayCommand]
        void Add(string time)
        {
            var number = int.Parse(time);
            TimeSpan span = new(0, number + (int)AddTime.TotalMinutes, 0);
            AddTime = span;
            if (number == 0)
            {
                AddTime = new TimeSpan();
            }
            if (AddTime.TotalMinutes <= 0)
            {
                Sign = ' ';
            }
            else
            {
                Sign = '+';
            }

        }

        #endregion
    }
}
