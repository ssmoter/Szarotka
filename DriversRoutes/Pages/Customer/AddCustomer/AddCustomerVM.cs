using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;

namespace DriversRoutes.Pages.Customer.AddCustomer
{
    [QueryProperty(nameof(Customer), nameof(CustomerRoutes))]
    [QueryProperty(nameof(RouteId), nameof(Routes))]
    public partial class AddCustomerVM : ObservableObject
    {
        #region Variable

        private const string NewPoint = "Dodawanie nowego punktu";

        [ObservableProperty]
        AddCustomerM addCustomer;
        public Routes RouteId { get; set; }

        [ObservableProperty]
        CustomerRoutes customer;

        [ObservableProperty]
        SelectedDayOfWeekRoutes dayOfWeekCustomerBefore;
        [ObservableProperty]
        bool dayOfWeekCustomerBeforeIsVisible;
        [ObservableProperty]
        SelectedDayOfWeekRoutes dayOfWeekCustomerAfter;
        [ObservableProperty]
        bool dayOfWeekCustomerAfterIsVisible;

        List<SelectedDayOfWeekRoutes> DayOfWeekCustomerBeforeList;
        List<SelectedDayOfWeekRoutes> DayOfWeekCustomerAfterList;

        readonly Service.ISaveRoutes _saveRoutes;
        readonly Data.GoogleApi.IAddressFromCoordinates _IAddressFromCoordinates;
        readonly DataBase.Data.AccessDataBase _db;
        ResidentialAddress[] _address = [];
        #endregion

        public AddCustomerVM(Service.ISaveRoutes saveRoutes, DataBase.Data.AccessDataBase db, Data.GoogleApi.IAddressFromCoordinates IAddressFromCoordinates)
        {
            AddCustomer ??= new();
            Customer ??= new();
            Customer.Name = NewPoint;
            _saveRoutes = saveRoutes;
            _db = db;
            _IAddressFromCoordinates = IAddressFromCoordinates;
        }


        public async void GetHelperDayOfWeek()
        {
            var before = GetDayOfWeekCustomerBefore();
            var after = GetDayOfWeekCustomerAfter();
            await Task.WhenAll(before, after);
        }

        private async Task GetDayOfWeekCustomerBefore()
        {
            try
            {
                var date = DateTime.Now;
                if (Customer.Name != NewPoint)
                {
                    date = Customer.DayOfWeek.GetTodayDatetimeFromSelectedDayOfWeekRoutes(date);
                }
                DayOfWeekCustomerBeforeList = await _db.DataBaseAsync.QueryAsync<SelectedDayOfWeekRoutes>(
                    Helper.SqlQuery.GetSelectedDayOfWeekRoutesNearestDate(date, Helper.SqlQuery._less, Helper.SqlQuery._DESC));
                var first = DayOfWeekCustomerBeforeList.FirstOrDefault();
                if (first is not null)
                {
                    DayOfWeekCustomerBefore = first;
                    DayOfWeekCustomerBeforeIsVisible = true;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        private async Task GetDayOfWeekCustomerAfter()
        {
            try
            {
                var date = DateTime.Now;
                if (Customer.Name != NewPoint)
                {
                    date = Customer.DayOfWeek.GetTodayDatetimeFromSelectedDayOfWeekRoutes(date);
                }
                DayOfWeekCustomerAfterList = await _db.DataBaseAsync.QueryAsync<SelectedDayOfWeekRoutes>(
                    Helper.SqlQuery.GetSelectedDayOfWeekRoutesNearestDate(date, Helper.SqlQuery._more, Helper.SqlQuery._ASC));
                var first = DayOfWeekCustomerAfterList.FirstOrDefault();
                if (first is not null)
                {
                    DayOfWeekCustomerAfter = first;
                    DayOfWeekCustomerAfterIsVisible = true;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        #region Command

        [RelayCommand]
        async Task SaveAndExit()
        {
            try
            {
                await _saveRoutes.SaveCustomer(Customer, RouteId.Id.ToByteArray());

                await Shell.Current.GoToAsync($"..?", new Dictionary<string, object>()
                {
                    [nameof(CustomerRoutes)] = Customer,
                    [nameof(Routes)] = RouteId
                });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task CancelAndExit()
        {
            try
            {

                // await Shell.Current.GoToAsync("..");
                await Shell.Current.GoToAsync($"..?", new Dictionary<string, object>()
                {
                    [nameof(CustomerRoutes)] = Customer,
                    [nameof(Routes)] = RouteId
                });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        [RelayCommand]
        void DisplayPinOnMap()
        {
            try
            {
                AddCustomer.MapIsVisible = !AddCustomer.MapIsVisible;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task GetAddressFromApi()
        {
            try
            {
                if (AddCustomer is null)
                {
                    return;
                }

                if (_address.Length < 1)
                {
                    var response = await _IAddressFromCoordinates.FindGoogleApiAddress(Customer.Latitude, Customer.Longitude);
                    _address = new ResidentialAddress[response.Results.Count];

                    for (int i = 0; i < response.Results.Count; i++)
                    {
                        _address[i] = response.Results[i].FromGoogleToAddress();
                    }
                }

                var popup = new DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses.ProbableAddressesV(_address);

                var result = await Shell.Current.ShowPopupAsync(popup);

                if (result is ResidentialAddress address)
                {
                    Customer.ResidentialAddress = address;
                }

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        void CopyTimeBefore(string dayOfS)
        {
            DayOfWeek dayOf = (DayOfWeek)int.Parse(dayOfS);
            switch (dayOf)
            {
                case DayOfWeek.Sunday:
                    Customer.DayOfWeek.SundayTimeSpan = DayOfWeekCustomerBefore.SundayTimeSpan;
                    Customer.DayOfWeek.Sunday = DayOfWeekCustomerBefore.Sunday;
                    break;
                case DayOfWeek.Monday:
                    Customer.DayOfWeek.MondayTimeSpan = DayOfWeekCustomerBefore.MondayTimeSpan;
                    Customer.DayOfWeek.Monday = DayOfWeekCustomerBefore.Monday;
                    break;
                case DayOfWeek.Tuesday:
                    Customer.DayOfWeek.TuesdayTimeSpan = DayOfWeekCustomerBefore.TuesdayTimeSpan;
                    Customer.DayOfWeek.Tuesday = DayOfWeekCustomerBefore.Tuesday;
                    break;
                case DayOfWeek.Wednesday:
                    Customer.DayOfWeek.WednesdayTimeSpan = DayOfWeekCustomerBefore.WednesdayTimeSpan;
                    Customer.DayOfWeek.Wednesday = DayOfWeekCustomerBefore.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                    Customer.DayOfWeek.ThursdayTimeSpan = DayOfWeekCustomerBefore.ThursdayTimeSpan;
                    Customer.DayOfWeek.Thursday = DayOfWeekCustomerBefore.Thursday;
                    break;
                case DayOfWeek.Friday:
                    Customer.DayOfWeek.FridayTimeSpan = DayOfWeekCustomerBefore.FridayTimeSpan;
                    Customer.DayOfWeek.Friday = DayOfWeekCustomerBefore.Friday;
                    break;
                case DayOfWeek.Saturday:
                    Customer.DayOfWeek.SaturdayTimeSpan = DayOfWeekCustomerBefore.SaturdayTimeSpan;
                    Customer.DayOfWeek.Saturday = DayOfWeekCustomerBefore.Saturday;
                    break;
                default:
                    break;
            }
        }
        [RelayCommand]
        void CopyTimeAfter(string dayOfS)
        {
            DayOfWeek dayOf = (DayOfWeek)int.Parse(dayOfS);
            switch (dayOf)
            {
                case DayOfWeek.Sunday:
                    Customer.DayOfWeek.SundayTimeSpan = DayOfWeekCustomerAfter.SundayTimeSpan;
                    Customer.DayOfWeek.Sunday = DayOfWeekCustomerAfter.Sunday;
                    break;
                case DayOfWeek.Monday:
                    Customer.DayOfWeek.MondayTimeSpan = DayOfWeekCustomerAfter.MondayTimeSpan;
                    Customer.DayOfWeek.Monday = DayOfWeekCustomerAfter.Monday;
                    break;
                case DayOfWeek.Tuesday:
                    Customer.DayOfWeek.TuesdayTimeSpan = DayOfWeekCustomerAfter.TuesdayTimeSpan;
                    Customer.DayOfWeek.Tuesday = DayOfWeekCustomerAfter.Tuesday;
                    break;
                case DayOfWeek.Wednesday:
                    Customer.DayOfWeek.WednesdayTimeSpan = DayOfWeekCustomerAfter.WednesdayTimeSpan;
                    Customer.DayOfWeek.Wednesday = DayOfWeekCustomerAfter.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                    Customer.DayOfWeek.ThursdayTimeSpan = DayOfWeekCustomerAfter.ThursdayTimeSpan;
                    Customer.DayOfWeek.Thursday = DayOfWeekCustomerAfter.Thursday;
                    break;
                case DayOfWeek.Friday:
                    Customer.DayOfWeek.FridayTimeSpan = DayOfWeekCustomerAfter.FridayTimeSpan;
                    Customer.DayOfWeek.Friday = DayOfWeekCustomerAfter.Friday;
                    break;
                case DayOfWeek.Saturday:
                    Customer.DayOfWeek.SaturdayTimeSpan = DayOfWeekCustomerAfter.SaturdayTimeSpan;
                    Customer.DayOfWeek.Saturday = DayOfWeekCustomerAfter.Saturday;
                    break;
                default:
                    break;
            }

        }

        [RelayCommand]
        void ChangeTimeBefore(string direction)
        {
            if (DayOfWeekCustomerBeforeList is null)
            {
                return;
            }
            if (DayOfWeekCustomerBeforeList.Count < 1)
            {
                return;
            }
            var index = DayOfWeekCustomerBeforeList.IndexOf(DayOfWeekCustomerBefore);
            if (direction == "left")
            {
                if (index == 0)
                {
                    DayOfWeekCustomerBefore = DayOfWeekCustomerBeforeList.LastOrDefault();
                    return;
                }
                index--;
                if (index < 0)
                {
                    index = DayOfWeekCustomerBeforeList.Count;
                }
                DayOfWeekCustomerBefore = DayOfWeekCustomerBeforeList[index];
            }
            else if (direction == "right")
            {
                if (index == DayOfWeekCustomerBeforeList.Count)
                {
                    DayOfWeekCustomerBefore = DayOfWeekCustomerBeforeList.FirstOrDefault();
                    return;
                }
                index++;
                if (index >= DayOfWeekCustomerBeforeList.Count)
                {
                    index = 0;
                }
                DayOfWeekCustomerBefore = DayOfWeekCustomerBeforeList[index];
            }
            else if (direction == "zero")
            {
                DayOfWeekCustomerBefore = DayOfWeekCustomerBeforeList.FirstOrDefault();
            }
        }
        [RelayCommand]
        void ChangeTimeAfter(string direction)
        {
            if (DayOfWeekCustomerAfterList is null)
            {
                return;
            }
            if (DayOfWeekCustomerAfterList.Count < 1)
            {
                return;
            }
            var index = DayOfWeekCustomerAfterList.IndexOf(DayOfWeekCustomerAfter);
            if (direction == "left")
            {
                if (index == 0)
                {
                    DayOfWeekCustomerAfter = DayOfWeekCustomerAfterList.LastOrDefault();
                    return;
                }
                index--;

                if (index < 0)
                {
                    index = DayOfWeekCustomerAfterList.Count;
                }

                DayOfWeekCustomerAfter = DayOfWeekCustomerAfterList[index];
            }
            else if (direction == "right")
            {
                if (index == DayOfWeekCustomerAfterList.Count)
                {
                    DayOfWeekCustomerAfter = DayOfWeekCustomerAfterList.FirstOrDefault();
                    return;
                }
                index++;
                if (index >= DayOfWeekCustomerAfterList.Count)
                {
                    index = 0;
                }
                DayOfWeekCustomerAfter = DayOfWeekCustomerAfterList[index];
            }
            else if (direction == "zero")
            {
                DayOfWeekCustomerAfter = DayOfWeekCustomerAfterList.FirstOrDefault();
            }
        }


        #endregion

    }
}
