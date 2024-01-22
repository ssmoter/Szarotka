using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Model
{
    public partial class ResidentialAddress : ObservableObject, IDisposable
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [ObservableProperty]
        string name;
        [ObservableProperty]
        string surname;
        [ObservableProperty]
        string street;
        [ObservableProperty]
        string houseNumber;
        [ObservableProperty]
        string apartmentNumber;
        [ObservableProperty]
        string postalCode;
        [ObservableProperty]
        string city;
        [ObservableProperty]
        string country;

        public ResidentialAddress()
        {
            Country = "Polska";
            City = "Limanowa";
            postalCode = "34-600";
        }

        public void Dispose()
        {
            Name = "";
            Surname = "";
            Street = "";
            HouseNumber = "";
            ApartmentNumber = "";
            PostalCode = "";
            City = "";
            Country = "";
        }
    }
}
