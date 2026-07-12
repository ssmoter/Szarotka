using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;

using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.JsonContext;
using DataBase.Service;

namespace DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

public partial class MoveTimeOnCustomersV : Popup
{
    public MoveTimeOnCustomersV(SelectedDayOfWeekRoutes vm)
    {
        InitializeComponent();
        BindingContext = vm;
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
        IGetDriverRoutesAoT _get,
        ISaveDriverRoutesAoT _save,
        IUpdateLogService _update,
        IPopupService _popupService)
    {

        var queryAttributes = new Dictionary<string, object>
        {
            [nameof(SelectedDayOfWeekRoutes)] = selectDayMs
        };

        var result = await _popupService.ShowPopupAsync<MoveTimeOnCustomersVM, SelectedDayOfWeekRoutes>(Shell.Current,
                                                                                                        shellParameters: queryAttributes);

        if (result is null)
        {
            return false;
        }
        if (result.Result is SelectedDayOfWeekRoutes dayOf)
        {
            CommunityToolkit.Maui.Alerts.Toast toast = new()
            {
                Duration = CommunityToolkit.Maui.Core.ToastDuration.Long,
                Text = "Aktualizawanie wpisów",
            };
            await toast.Show();

            IEnumerable<SelectedDayOfWeekRoutes> dayOfs;

            var customers = await _get.CustomerRoutes(route.Id, dayOf.GetDayOfWeeks());

            dayOfs = customers.Select(x => x.DayOfWeek);

            var updates = await _save.UpdateCustomersTime(dayOfs, dayOf, selectDayMs);

            foreach (var item in updates)
            {
                await _update.Insert(new()
                {
                    IsServer = false,
                    JsonUpdate = System.Text.Json.JsonSerializer.Serialize(item, SzarotkaJsonSerializerContext.Default.SelectedDayOfWeekRoutes),
                    UpdateEnum = DataBase.Model.UpdateEnum.SelectedDayOfWeek,
                    UpdateId = item.CustomerId.ToString(),
                });
            }

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
        return false;
    }

}