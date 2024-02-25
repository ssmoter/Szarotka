using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;

using DriversRoutes.Model;

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
        public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }

        public DisplayCustomerVM(AccessDataBase db)
        {
            _db = db;
            DisplayCustomerM ??= new();
        }


        #region Command

        [RelayCommand]
        async Task Delete(CustomerRoutes point)
        {
            try
            {
                if (point is null)
                    return;

                var result = await Shell.Current.DisplayAlert("Usuń", $"Czy na pewno chcesz usunąć {point.QueueNumber}:{point.Name}", "Tak", "Anuluj");

                if (!result)
                    return;


                await Task.Delay(1000);
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
                        [nameof(Routes)] = new Model.Routes() { Id = new Guid(point.RoutesId.ToByteArray()), }
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
