using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesRoutes
{
    public partial class ResidentialAddress : BaseEntities<Guid>, IDisposable
    {
        [ObservableProperty]
        public Guid customerId;

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
        { }

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
