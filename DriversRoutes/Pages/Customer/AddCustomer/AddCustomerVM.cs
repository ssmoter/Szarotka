using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;
using DataBase.Service;

using DriversRoutes.Helper;

using Shared.Data;
using Shared.Helper;
using Shared.Pages.UpdateDifference;

using System.Collections;
using System.Net;

namespace DriversRoutes.Pages.Customer.AddCustomer
{
    public partial class AddCustomerVM : ObservableObject, IQueryAttributable
    {

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(CustomerRoutes), out object customer))
            {
                if (customer is CustomerRoutes _customer)
                {
                    Customer = _customer;
                }
            }
            if (query.TryGetValue(nameof(Routes), out object routeId))
            {
                if (routeId is Routes _routeId)
                {
                    RouteId = _routeId;
                }
            }
        }

        private const string NewPoint = "Dodawanie nowego punktu";

        private AddCustomerM addCustomer;
        public AddCustomerM AddCustomer
        {
            get => addCustomer;
            set
            {
                if (SetProperty(ref addCustomer, value, nameof(AddCustomer))) { }
            }
        }
        private Routes routeId;
        public Routes RouteId
        {
            get => routeId;
            set
            {
                if (SetProperty(ref routeId, value, nameof(RouteId))) { }
            }
        }

        private CustomerRoutes customer;
        public CustomerRoutes Customer
        {
            get => customer;
            set
            {
                if (SetProperty(ref customer, value, nameof(Customer))) { }
            }
        }

        private CustomerRoutes customerHelperMap;
        public CustomerRoutes CustomerHelperMap
        {
            get => customerHelperMap;
            set
            {
                if (!SetProperty(ref customerHelperMap, value, nameof(CustomerHelperMap))) { }
            }
        }

        private SelectedDayOfWeekRoutes dayOfWeekCustomerBefore;
        public SelectedDayOfWeekRoutes DayOfWeekCustomerBefore
        {
            get => dayOfWeekCustomerBefore;
            set
            {
                if (SetProperty(ref dayOfWeekCustomerBefore, value, nameof(DayOfWeekCustomerBefore))) { }
            }
        }

        private bool dayOfWeekCustomerBeforeIsVisible;
        public bool DayOfWeekCustomerBeforeIsVisible
        {
            get => dayOfWeekCustomerBeforeIsVisible;
            set
            {
                if (SetProperty(ref dayOfWeekCustomerBeforeIsVisible, value, nameof(DayOfWeekCustomerBeforeIsVisible))) { }
            }
        }
        private SelectedDayOfWeekRoutes dayOfWeekCustomerAfter;
        public SelectedDayOfWeekRoutes DayOfWeekCustomerAfter
        {
            get => dayOfWeekCustomerAfter;
            set
            {
                if (SetProperty(ref dayOfWeekCustomerAfter, value, nameof(DayOfWeekCustomerAfter))) { }
            }
        }
        private bool dayOfWeekCustomerAfterIsVisible;
        public bool DayOfWeekCustomerAfterIsVisible
        {
            get => dayOfWeekCustomerAfterIsVisible;
            set
            {
                if (SetProperty(ref dayOfWeekCustomerAfterIsVisible, value, nameof(DayOfWeekCustomerAfterIsVisible))) { }
            }
        }

        private Border fullSize;
        public Border FullSize
        {
            get => fullSize;
            set
            {
                if (SetProperty(ref fullSize, value, nameof(FullSize))) { }
            }
        }
        private Border timeSize;
        public Border TimeSize
        {
            get => timeSize;
            set
            {
                if (SetProperty(ref timeSize, value, nameof(TimeSize))) { }
            }
        }

        List<SelectedDayOfWeekRoutes> DayOfWeekCustomerBeforeList;
        List<SelectedDayOfWeekRoutes> DayOfWeekCustomerAfterList;


        private readonly DataBase.Data.Save.ISaveDriverRoutesAoT _save;
        private readonly DataBase.Data.Get.IGetDriverRoutesAoT _get;
        private readonly Data.GoogleApi.IAddressFromCoordinates _IAddressFromCoordinates;
        private readonly IAccessDataBase _db;
        private readonly DataBase.Service.IUpdateLogService _update;
        private readonly Data.RouteApi.ISendCustomersHttp _sendHttp;
        internal ResidentialAddress[] Address { get; set; } = [];
        internal CustomerRoutes OriginCustomer { get; set; }


        public AddCustomerVM(IAccessDataBase db,
                             Data.GoogleApi.IAddressFromCoordinates IAddressFromCoordinates,
                             DataBase.Data.Save.ISaveDriverRoutesAoT save,
                             DataBase.Data.Get.IGetDriverRoutesAoT get,
                             DataBase.Service.IUpdateLogService update,
                             Data.RouteApi.ISendCustomersHttp sendHttp)
        {
            AddCustomer ??= new();
            Customer ??= new();
            Customer.Name = NewPoint;
            _db = db;
            _IAddressFromCoordinates = IAddressFromCoordinates;
            _save = save;
            _get = get;
            _update = update;
            _sendHttp = sendHttp;
        }


        public void GetHelperDayOfWeek()
        {
            var before = GetDayOfWeekCustomerBefore();
            var after = GetDayOfWeekCustomerAfter();
            Task.Run(async () =>
            {
                await Task.WhenAll(before, after);
            });
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
                _db.SaveLogExtension(ex);
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
                _db.SaveLogExtension(ex);
            }
        }

        private async Task Send(CancellationToken token = default)
        {
            var response = await _sendHttp.SendCustomerRoute(Customer, token: token);
            var json = await response.Content.ReadAsStringAsync(token);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var server = System.Text.Json.JsonSerializer.Deserialize(json, SzarotkaJsonSerializerContext.Default.CustomerRoutes);
                var difference = new UpdateDifference()
                {
                    Update = customer,
                    Server = server,
                };

                IList<UpdateDifference> differences = [difference];

                var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), differences },
                        {nameof(Action),(Action<IEnumerable>)(async (customer)
                        =>
                            {
                                CustomerRoutes cr = null;
                                foreach (CustomerRoutes item in customer)
                                    {
                                        cr = item;

                                        await _save.SaveCustomerRoutes(item, item.UserUpdatedId.ToByteArray(), true);
                                        await _save.SaveResidentialAddress(item.ResidentialAddress, item.ResidentialAddress.UserUpdatedId.ToByteArray(), true);
                                        await _save.SaveSelectedDayOfWeekRoutes(item.DayOfWeek, item.DayOfWeek.UserUpdatedId.ToByteArray(), true);
                                        _ = await _update.Insert(new DataBase.Model.UpdateLog()
                                        {
                                            IsServer = false,
                                        }, item);
                                    }

                                await _sendHttp.SendCustomerRoute(cr,forceUpdate:true);

                                 Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                            })
                        }
                    };
                await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
                return;
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
        }

        [RelayCommand]
        async Task SaveAndExit()
        {
            try
            {
                var user = UserAfterLogin.User;
                customer.RoutesId = RouteId.Id;
                customer.UserUpdatedId = user.Id;
                customer.DayOfWeek.UserUpdatedId = user.Id;
                customer.ResidentialAddress.UserUpdatedId = user.Id;


                await _save.SaveCustomerRoutes(customer, user.Id.ToByteArray());

                customer.ResidentialAddress.CustomerId = customer.Id;
                customer.DayOfWeek.CustomerId = customer.Id;

                await _save.SaveResidentialAddress(customer.ResidentialAddress, user.Id.ToByteArray());
                await _save.SaveSelectedDayOfWeekRoutes(customer.DayOfWeek, user.Id.ToByteArray());

                await _update.Insert(new DataBase.Model.UpdateLog()
                {
                    IsServer = false,
                }, Customer);

                await Send();

                await Shell.Current.GoToAsync($"..?", new Dictionary<string, object>()
                {
                    [nameof(CustomerRoutes)] = Customer,
                    [nameof(Routes)] = RouteId
                });
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                //HttpStatusCode.Conflict został obłużony wyżej jako zwrot danych do poprawy przy edycji
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
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
                //    [nameof(CustomerRoutes)] = originCustomer,
                //    [nameof(Routes)] = RouteId
                //});
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }
        [RelayCommand]
        void DisplayPinOnMap()
        {
            try
            {
                CustomerHelperMap = Customer;
                AddCustomer.MapIsVisible = !AddCustomer.MapIsVisible;
                if (AddCustomer.MapIsVisible)
                {
                    AddCustomer.MapIsVisibleHelperTime = false;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
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

                if (this.Address.Length < 1)
                {
                    var response = await _IAddressFromCoordinates.FindGoogleApiAddress(Customer.Latitude, Customer.Longitude);
                    this.Address = new ResidentialAddress[response.Results.Count];

                    for (int i = 0; i < response.Results.Count; i++)
                    {
                        this.Address[i] = response.Results[i].FromGoogleToAddress();
                    }
                }

                var popup = new DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses.ProbableAddressesV(this.Address);

                var result = default(object);
                if (Application.Current?.Windows[0].Page != null)
                {
                    result = await Application.Current.Windows[0].Page.ShowPopupAsync(popup);
                }

                if (result is ResidentialAddress address)
                {
                    Customer.ResidentialAddress = address;
                }

            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
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
        async Task ChangeTimeBefore(string direction)
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

            if (AddCustomer.MapIsVisibleHelperTime)
            {
                CustomerHelperMap = await _get.CustomerRoute(DayOfWeekCustomerBefore.CustomerId);
                if (CustomerHelperMap is null)
                {
                    AddCustomer.MapIsVisible = false;
                }
            }

        }
        [RelayCommand]
        async Task ChangeTimeAfter(string direction)
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
            if (AddCustomer.MapIsVisibleHelperTime)
            {
                CustomerHelperMap = await _get.CustomerRoute(DayOfWeekCustomerAfter.CustomerId);

                if (CustomerHelperMap is null)
                {
                    AddCustomer.MapIsVisible = false;
                }
            }
        }

        [RelayCommand]
        async Task DisplayMapWitchHelperTime(Guid id)
        {
            if (CustomerHelperMap is not null)
            {
                if (AddCustomer.MapIsVisibleHelperTime && CustomerHelperMap.Id != id)
                {
                    CustomerHelperMap = await _get.CustomerRoute(id);
                    if (CustomerHelperMap is not null)
                    {
                        if (AddCustomer.MapIsVisibleHelperTime)
                        {
                            AddCustomer.MapIsVisible = false;
                        }
                        return;
                    }
                }
            }

            AddCustomer.MapIsVisibleHelperTime = !AddCustomer.MapIsVisibleHelperTime;

            if (AddCustomer.MapIsVisibleHelperTime)
            {
                CustomerHelperMap = await _get.CustomerRoute(id);
            }
            if (CustomerHelperMap is null)
            {
                AddCustomer.MapIsVisibleHelperTime = false;
            }
            if (AddCustomer.MapIsVisibleHelperTime)
            {
                AddCustomer.MapIsVisible = false;
            }
        }


    }
}
