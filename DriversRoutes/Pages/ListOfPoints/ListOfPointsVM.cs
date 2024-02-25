using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;

using DriversRoutes.Model;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.ListOfPoints
{
    [QueryProperty(nameof(Route), nameof(Model.Routes))]
    public partial class ListOfPointsVM : ObservableObject
    {
        [ObservableProperty]
        Routes route;

        [ObservableProperty]
        ObservableCollection<CustomerRoutes> customerRoutes;

        [ObservableProperty]
        CustomerRoutes locationThisCustomer;

        [ObservableProperty]
        bool showLocationThisCustomer;

        [ObservableProperty]
        bool customerListRefresh;

        readonly AccessDataBase _db;
        readonly Service.ISelectRoutes _selectRoutes;

        public ListOfPointsVM(AccessDataBase db, Service.ISelectRoutes selectRoutes)
        {
            _db = db;
            _selectRoutes = selectRoutes;
            CustomerRoutes ??= [];
        }

        #region Method
        public ObservableCollection<CustomerRoutes> GetPoints(Routes routes, SelectedDayOfWeekRoutes week)
        {
            try
            {
                CustomerListRefresh = true;
                var result = _selectRoutes.GetCustomerRoutesQuery(routes, week);
                return new ObservableCollection<CustomerRoutes>(result);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
            finally
            {
                CustomerListRefresh = false;
            }

        }
        public async Task<ObservableCollection<CustomerRoutes>> GetPointsAsync(Routes routes, SelectedDayOfWeekRoutes week)
        {
            try
            {
                CustomerListRefresh = true;
                var result = await _selectRoutes.GetCustomerRoutesQueryAsync(routes, week);
                return new ObservableCollection<CustomerRoutes>(result);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
            finally
            {
                CustomerListRefresh = false;
            }
        }

        #endregion

        #region Command

        [RelayCommand]
        void LocationOfPin(CustomerRoutes point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }
                ShowLocationThisCustomer = true;
                LocationThisCustomer = point;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task DisplayPin(CustomerRoutes point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }

                await Shell.Current.GoToAsync($"{nameof(Pages.Customer.DisplayCustomer.DisplayCustomerV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Model.CustomerRoutes)] = new Model.CustomerRoutes()
                        {
                            Id = new Guid(point.Id.ToByteArray()),
                            RoutesId = new Guid(point.RoutesId.ToByteArray()),
                            QueueNumber = point.QueueNumber,
                            Name = point.Name,
                            Description = point.Description,
                            PhoneNumber = point.PhoneNumber,
                            CreatedDate = point.CreatedDate,
                            DayOfWeek = point.DayOfWeek,
                            ResidentialAddress = point.ResidentialAddress,
                            Longitude = point.Longitude,
                            Latitude = point.Latitude,
                        },
                    });

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task SelectDaysOfWeek()
        {
            var popup = new Popups.SelectDay.SelectDayV();
            var response = await Shell.Current.ShowPopupAsync(popup);
            if (response is null)
            {
                return;
            }
            if (response is SelectedDayOfWeekRoutes day)
            {
                CustomerRoutes.Clear();
                CustomerRoutes = await GetPointsAsync(Route, day);
            }
        }

        [RelayCommand]
        void Refresh()
        {
            CustomerListRefresh = false;
        }

        #endregion

    }
}
