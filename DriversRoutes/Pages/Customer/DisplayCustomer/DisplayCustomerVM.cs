using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Customer.DisplayCustomer
{
    [QueryProperty(nameof(Customer), nameof(CustomerRoutes))]
    [QueryProperty(nameof(LastSelectedDayOfWeek), nameof(SelectedDayOfWeekRoutes))]

    public partial class DisplayCustomerVM : ObservableObject
    {

        [ObservableProperty]
        CustomerRoutes customer;

        [ObservableProperty]
        DisplayCustomerM displayCustomerM;

        readonly AccessDataBase _db;
        readonly Service.ISaveRoutes _saveRoutes;
        public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }

        public DisplayCustomerVM(AccessDataBase db, Service.ISaveRoutes saveRoutes)
        {
            _db = db;
            DisplayCustomerM ??= new();
            _saveRoutes = saveRoutes;
        }


        #region Command

        [RelayCommand]
        async Task Delete(CustomerRoutes point)
        {
            try
            {
                if (point is null)
                    return;

                var result = await Shell.Current.DisplayAlert("Czy usunąć", $"Czy na pewno chcesz usunąć {point.QueueNumber}:{point.Name}", "Tak", "Anuluj");

                if (!result)
                    return;


                var taskDay = _db.DataBaseAsync.DeleteAsync(Customer.DayOfWeek);
                var taskAddress = _db.DataBaseAsync.DeleteAsync(Customer.ResidentialAddress);
                var taskCustomer = _db.DataBaseAsync.DeleteAsync(Customer);

                var taskReady = await Task.WhenAll(taskDay, taskAddress, taskCustomer);

                result = await Shell.Current.DisplayAlert("Usunięto", "Obiekt został usunięty. Czy chesz przywrócić", "Przywróć", "Nie");

                if (!result)
                    await Shell.Current.GoToAsync("..");
                if (!result)
                {
                    await _saveRoutes.SaveCustomer(Customer, Customer.RoutesId.ToByteArray());
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        void LocationOfPin()
        {
            try
            {
                DisplayCustomerM.ShowLocationThisCustomer = !DisplayCustomerM.ShowLocationThisCustomer;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task EditPin(CustomerRoutes point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }

                await Shell.Current.GoToAsync($"{nameof(Pages.Customer.AddCustomer.AddCustomerV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(CustomerRoutes)] = new CustomerRoutes()
                        {
                            Id = new Guid(point.Id.ToByteArray()),
                            RoutesId = new Guid(point.RoutesId.ToByteArray()),
                            QueueNumber = point.QueueNumber,
                            Name = point.Name,
                            Description = point.Description,
                            PhoneNumber = point.PhoneNumber,
                            Created = point.Created,
                            DayOfWeek = point.DayOfWeek,
                            ResidentialAddress = point.ResidentialAddress,
                            Longitude = point.Longitude,
                            Latitude = point.Latitude,
                        },
                        [nameof(Routes)] = new Routes() { Id = new Guid(point.RoutesId.ToByteArray()), }
                    });

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        #endregion


    }
}
