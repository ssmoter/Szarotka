using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses;

using Shared.Helper;

using System.Xml.Linq;

namespace DriversRoutes.Pages.Popups.MoveTimeOnCustomers
{
    public partial class MoveTimeOnCustomersVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(SelectedDayOfWeekRoutes), out object selectDayMs))
            {
                if (selectDayMs is SelectedDayOfWeekRoutes _selectDayMs)
                {
                    SelectDayMs = _selectDayMs;
                }
            }
        }
        private const string after = "po godzinie";
        private const string befor = "przed godziną";


        private SelectedDayOfWeekRoutes selectDayMs;
        public SelectedDayOfWeekRoutes SelectDayMs
        {
            get => selectDayMs;
            set
            {
                if (SetProperty(ref selectDayMs, value, nameof(SelectDayMs))) { }
            }
        }

        private TimeSpan selectedTime;
        public TimeSpan SelectedTime
        {
            get => selectedTime;
            set
            {
                if (SetProperty(ref selectedTime, value, nameof(SelectedTime))) { }
            }
        }

        private TimeSpan addTime;
        public TimeSpan AddTime
        {
            get => addTime;
            set
            {
                if (SetProperty(ref addTime, value, nameof(AddTime))) { }
            }
        }

        private char sign;
        public char Sign
        {
            get => sign;
            set
            {
                if (SetProperty(ref sign, value, nameof(Sign))) { }
            }
        }
        private readonly IPopupService _popupService;

        public MoveTimeOnCustomersVM(IPopupService popupService)
        {
            _popupService = popupService;
            SelectDayMs = new()
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

        public MoveTimeOnCustomersVM(SelectedDayOfWeekRoutes selectDayMs, IPopupService popupService)
        {
            SelectDayMs = new(selectDayMs);
            _popupService = popupService;
        }


        public void SetTimeFromSelectDayMs(TimeSpan time)
        {
            SelectedTime = new TimeSpan(time.Ticks);
        }



        [RelayCommand]
        async Task SaveAndReturn()
        {
            if (!Helper.HelperDayOfWeek.IfAnyIsTrue(SelectDayMs))
            {
                await Shell.Current.DisplayAlertAsync("Nie wybrana dnia tygodnia", "Aby zapisać zmiany zaznacz dzień tygonia", "Ok");
                return;
            }

            var dayOfWeek = Helper.HelperDayOfWeek.GetFirstDayOfWeek(SelectDayMs);

            string hours = after;

            if (Sign == ' ')
            {
                hours = befor;
            }

            if (!await Shell.Current.DisplayAlertAsync("Zmiany", $"Przesunąć czas punktów w dniu {dayOfWeek.TranslateSelectedDay()} {hours} {SelectedTime.Hours}:{SelectedTime.Minutes} {Sign}{AddTime.TotalMinutes} minut", "Tak", "Nie"))
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

            await _popupService.ClosePopupAsync(page: Shell.Current, result: SelectDayMs);
        }
        [RelayCommand]
        async Task CancelAndReturn()
        {
            await _popupService.ClosePopupAsync(page: Shell.Current);
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

    }
}

