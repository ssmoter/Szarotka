using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Popups.AddCustomer
{
    public partial class AddCustomerM : ObservableObject, IDisposable
    {
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        Guid routesId;

        [ObservableProperty]
        int index;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        string phoneNumber;

        [ObservableProperty]
        DateTime created;

        [ObservableProperty]
        double longitude;

        [ObservableProperty]
        double latitude;

        [ObservableProperty]
        Model.SelectedDayOfWeekRoutes selectedDayOfWeek;

        public AddCustomerM()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
            SelectedDayOfWeek ??= new Model.SelectedDayOfWeekRoutes();
        }

        public void Dispose()
        {
            Id = Guid.Empty;
            RoutesId = Guid.Empty;
            Name = "";
            Description = "";
            PhoneNumber = "";
        }
    }
}
