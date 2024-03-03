using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model.EntitiesRoutes
{
    public partial class ResidentialAddress : ObservableObject, IDisposable
    {
        [PrimaryKey]
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
        {   }

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
