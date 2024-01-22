using CommunityToolkit.Mvvm.ComponentModel;


namespace DriversRoutes.Pages.AddCustomer
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
        bool address;

        [ObservableProperty]
        Model.SelectedDayOfWeekRoutes selectedDayOfWeek;
        [ObservableProperty]
        Model.ResidentialAddress residentialAddress;

        public AddCustomerM()
        {
            Created = DateTime.Now;
            SelectedDayOfWeek ??= new();
            ResidentialAddress ??= new();
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
