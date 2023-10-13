using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;

using DriversRoutes.Helper;
using DriversRoutes.Model;
using DriversRoutes.Pages.Maps;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.ListOfPoints
{
    [QueryProperty(nameof(Route), nameof(Model.Routes))]
    public partial class ListOfPointsVM : ObservableObject
    {
        [ObservableProperty]
        Routes route;

        [ObservableProperty]
        ObservableCollection<MapsM> customerRoutes;

        readonly AccessDataBase _db;
        readonly Service.ISelectRoutes _selectRoutes;

        public ListOfPointsVM(AccessDataBase db, Service.ISelectRoutes selectRoutes)
        {
            _db = db;
            _selectRoutes = selectRoutes;
            CustomerRoutes ??= new();
        }

        #region Method
        public ObservableCollection<MapsM> GetPoints(Routes routes, SelectedDayOfWeekRoutes week)
        {
            try
            {
                var result = _selectRoutes.GetCustomerRoutesQuery(routes, week);
                var points = new ObservableCollection<MapsM>();
                for (int i = 0; i < result.Length; i++)
                {
                    points.Add(result[i].ParseAsCustomerM());
                }
                return points;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }

        }
        public async Task<ObservableCollection<MapsM>> GetPointsAsync(Routes routes, SelectedDayOfWeekRoutes week)
        {
            var result = await _selectRoutes.GetCustomerRoutesQueryAsync(routes, week);
            var points = new ObservableCollection<MapsM>();
            for (int i = 0; i < result.Length; i++)
            {
                points.Add(result[i].ParseAsCustomerM());
            }
            return points;
        }

        #endregion

        #region Command

        [RelayCommand]
        async Task DeletePoint(MapsM point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }

                var id = point.Id.ToString();
                await _db.DataBaseAsync.ExecuteScalarAsync<bool>($"DELETE FROM Routes WHERE Id ='{id}'");

                var dayId = point.SelectedDayOfWeek.Id.ToString();
                await _db.DataBaseAsync.ExecuteScalarAsync<bool>($"DELETE FROM SelectedDayOfWeekRoutes WHERE Id ='{dayId}'");

                CustomerRoutes.Remove(point);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task EditPin(MapsM point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }

                var popup = new Popups.AddCustomer.AddCustomerV(new Model.CustomerRoutes()
                {
                    Id = new Guid(point.Id.ToByteArray()),
                    RoutesId = new Guid(point.RoutesId.ToByteArray()),
                    QueueNumber = point.Index,
                    Name = point.Name,
                    Description = point.Description,
                    PhoneNumber = point.PhoneNumber,
                    CreatedDate = point.Created,
                    DayOfWeek = point.SelectedDayOfWeek,
                    Longitude = point.Longitude,
                    Latitude = point.Latitude,
                });

                var update = await Shell.Current.ShowPopupAsync(popup);
                if (update is null)
                {
                    return;
                }
                int index = CustomerRoutes.IndexOf(point);

                if (update is CustomerRoutes customerUpdate)
                {
                    CustomerRoutes[index].Id = new Guid(customerUpdate.Id.ToByteArray());
                    CustomerRoutes[index].RoutesId = new Guid(customerUpdate.RoutesId.ToByteArray());
                    CustomerRoutes[index].Index = customerUpdate.QueueNumber;
                    CustomerRoutes[index].Name = customerUpdate.Name;
                    CustomerRoutes[index].Description = customerUpdate.Description;
                    CustomerRoutes[index].PhoneNumber = customerUpdate.PhoneNumber;
                    CustomerRoutes[index].Created = customerUpdate.CreatedDate;
                    CustomerRoutes[index].SelectedDayOfWeek = customerUpdate.DayOfWeek;
                    CustomerRoutes[index].SelectedDayOfWeek.Id = new Guid(customerUpdate.DayOfWeek.Id.ToByteArray());
                    CustomerRoutes[index].SelectedDayOfWeek.CustomerId = new Guid(customerUpdate.DayOfWeek.CustomerId.ToByteArray());
                    CustomerRoutes[index].Longitude = customerUpdate.Longitude;
                    CustomerRoutes[index].Latitude = customerUpdate.Latitude;

                    await _db.DataBaseAsync.UpdateAsync(customerUpdate);
                    await _db.DataBaseAsync.UpdateAsync(customerUpdate.DayOfWeek);
                }
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

        #endregion

    }
}
