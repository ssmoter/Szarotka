using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

public partial class MoveTimeOnCustomersV : Popup, IDisposable
{
    public MoveTimeOnCustomersV(SelectedDayOfWeekRoutes selectDayMs)
    {
        InitializeComponent();
        var vm = new MoveTimeOnCustomersVM(selectDayMs);
        vm.Close += CloseAsync;

        BindingContext = vm;
    }

    public void Dispose()
    {
        if (BindingContext is MoveTimeOnCustomersVM vm)
        {
            vm.Close -= CloseAsync;
        }
        GC.SuppressFinalize(this);
    }



    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton rb)
        {
            if (BindingContext is not MoveTimeOnCustomersVM vm)
            {
                return;
            }

            switch (rb.Content)
            {
                case "Poniedziałek":
                    vm.SetTimeFromSelectDayMs(vm.SelectDayMs.MondayTimeSpan);
                    break;
                case "Wtorek":
                    vm.SetTimeFromSelectDayMs(vm.SelectDayMs.TuesdayTimeSpan);
                    break;
                case "Środa":
                    vm.SetTimeFromSelectDayMs(vm.SelectDayMs.WednesdayTimeSpan);
                    break;
                case "Czwartek":
                    vm.SetTimeFromSelectDayMs(vm.SelectDayMs.ThursdayTimeSpan);
                    break;
                case "Piątek":
                    vm.SetTimeFromSelectDayMs(vm.SelectDayMs.FridayTimeSpan);
                    break;
                case "Sobota":
                    vm.SetTimeFromSelectDayMs(vm.SelectDayMs.SaturdayTimeSpan);
                    break;
                default:
                    vm.SetTimeFromSelectDayMs(new TimeSpan());
                    break;
            }
        }
    }





    public static async Task<bool> ShowPopUp(
        Routes route,
        SelectedDayOfWeekRoutes selectDayMs,
        Service.ISelectRoutes _selectRoutes,
        Service.ISaveRoutes _saveRoutes)
    {
        var popup = new MoveTimeOnCustomersV(selectDayMs);

        var result = await Shell.Current.ShowPopupAsync(popup);

        if (result is null)
        {
            return false;
        }
        if (result is SelectedDayOfWeekRoutes dayOf)
        {
            CommunityToolkit.Maui.Alerts.Toast toast = new()
            {
                Duration = CommunityToolkit.Maui.Core.ToastDuration.Long,
                Text = "Aktualizawanie wpisów",
            };
            await toast.Show();

            IEnumerable<SelectedDayOfWeekRoutes> dayOfs;

            var customers = await _selectRoutes.GetCustomerRoutesQueryAsync(route, dayOf);

            dayOfs = customers.Select(x => x.DayOfWeek);

            var isComplited = await _saveRoutes.UpdateCustomersTime(dayOfs, dayOf, selectDayMs);

            if (isComplited)
            {
                toast = new()
                {
                    Text = "Aktualizawanie zakończone",
                    Duration = CommunityToolkit.Maui.Core.ToastDuration.Short,
                };
                await toast.Show();
                customers = null;
                dayOfs = null;
                return true;
            }
        }
        return false;
    }

}