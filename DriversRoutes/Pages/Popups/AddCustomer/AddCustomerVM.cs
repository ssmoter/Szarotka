using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DriversRoutes.Pages.Popups.AddCustomer
{
    public partial class AddCustomerVM : ObservableObject, IDisposable
    {
        #region Variable


        [ObservableProperty]
        AddCustomerM addCustomer;


        public Func<object, Task> Close;

        #endregion

        public AddCustomerVM(Model.CustomerRoutes customer)
        {
            AddCustomer = new AddCustomerM()
            {
                Id = customer.Id,
                RoutesId = customer.RoutesId,
                Index = customer.QueueNumber,
                Name = customer.Name,
                Description = customer.Description,
                PhoneNumber = customer.PhoneNumber,
                Created = customer.CreatedDate,
                SelectedDayOfWeek = customer.DayOfWeek,
                Longitude = customer.Longitude,
                Latitude = customer.Latitude,
            };
            addCustomer.SelectedDayOfWeek ??= new();
        }


        #region Command

        [RelayCommand]
        async Task SaveAndExit()
        {
            var customer = new Model.CustomerRoutes()
            {
                Id = AddCustomer.Id,
                RoutesId = AddCustomer.RoutesId,
                QueueNumber = AddCustomer.Index,
                Name = AddCustomer.Name,
                Description = AddCustomer.Description,
                PhoneNumber = AddCustomer.PhoneNumber,
                CreatedDate = AddCustomer.Created,
                DayOfWeek = AddCustomer.SelectedDayOfWeek,
                Longitude = AddCustomer.Longitude,
                Latitude = AddCustomer.Latitude,
            };
            customer.DayOfWeek.ValuesAsString = customer.DayOfWeek.ToString();

            await OnClose(customer);
        }
        [RelayCommand]
        async Task CancelAndExit()
        {
            await OnClose(null);
        }

        #endregion

        public Task OnClose(object result = null)
        {
            try
            {
                return Close?.Invoke(result);
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            AddCustomer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
