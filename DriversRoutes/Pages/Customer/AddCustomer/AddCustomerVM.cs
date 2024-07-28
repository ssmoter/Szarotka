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

        readonly Service.ISaveRoutes _saveRoutes;
        readonly Service.IDownloadAddress _downloadAddress;
        readonly DataBase.Data.AccessDataBase _db;
        ResidentialAddress[] _address = [];
        #endregion

        public AddCustomerVM(Service.ISaveRoutes saveRoutes, DataBase.Data.AccessDataBase db, Service.IDownloadAddress downloadAddress)
        {
            AddCustomer ??= new();
            Customer ??= new();
            Customer.Name = NewPoint;
            _saveRoutes = saveRoutes;
            _db = db;
            _downloadAddress = downloadAddress;
        }


        public async void GetHelperDayOfWeek()
        {
            var befor = GetDayOfWeekCustomerBefore();
            var after = GetDayOfWeekCustomerAfter();
            await Task.WhenAll(befor, after);
        }

        private async Task GetDayOfWeekCustomerBefore()
        {
            try
            {
                var date = DateTime.Now;
                if (Customer.Name != NewPoint)
                {
                    date = Customer.DayOfWeek.GetTodayDatetimeFromSelectedDayOfWeekRoutes();
                }
                var list = await _db.DataBaseAsync.QueryAsync<SelectedDayOfWeekRoutes>(
                    Helper.SqlQuery.GetSelectedDayOfWeekRoutesNearestDate(date, Helper.SqlQuery._less,Helper.SqlQuery._DESC));
                var first = list.FirstOrDefault();
                if (first is not null)
                {
                    DayOfWeekCustomerBefore = first;
                    DayOfWeekCustomerBeforeIsVisible = true;
                    list.Clear();
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
                    date = Customer.DayOfWeek.GetTodayDatetimeFromSelectedDayOfWeekRoutes();
                }
                var list = await _db.DataBaseAsync.QueryAsync<SelectedDayOfWeekRoutes>(
                    Helper.SqlQuery.GetSelectedDayOfWeekRoutesNearestDate(date, Helper.SqlQuery._more, Helper.SqlQuery._ASC));
                var first = list.FirstOrDefault();
                if (first is not null)
                {
                    DayOfWeekCustomerAfter = first;
                    DayOfWeekCustomerAfterIsVisible = true;
                    list.Clear();
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

                await Shell.Current.GoToAsync("..");
                //await Shell.Current.GoToAsync($"..?", new Dictionary<string, object>()
                //{
                //    [nameof(Routes)] = RouteId
                //});
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
                    var response = await _downloadAddress.FindAddressFromCoordinates(Customer.Latitude, Customer.Longitude);
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
        #endregion

    }
}
