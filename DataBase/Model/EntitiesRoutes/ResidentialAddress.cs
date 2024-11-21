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

        public ResidentialAddress(ResidentialAddress copy)
        {
            this.CustomerId = copy.CustomerId;
            this.Name = copy.Name;
            this.Street = copy.Street;
            this.HouseNumber = copy.HouseNumber;
            this.ApartmentNumber = copy.ApartmentNumber;
            this.PostalCode = copy.PostalCode;
            this.City = copy.City;
            this.Country = copy.Country;
            this.Updated = copy.Updated;
            this.Surname = copy.Surname;
            this.Id = copy.Id;
            this.Created = copy.Created;
        }

        public override string ToString()
        {
            string to = $"{Name} {Surname}{Environment.NewLine}" +
                $"{Street} {HouseNumber} {(string.IsNullOrWhiteSpace(ApartmentNumber) ? "" : "/")} {ApartmentNumber}{Environment.NewLine}" +
                $"{PostalCode} {City}{Environment.NewLine}" +
                $"{Country}";

            return to;
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
