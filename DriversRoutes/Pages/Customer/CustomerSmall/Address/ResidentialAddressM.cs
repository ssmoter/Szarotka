using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Customer.CustomerSmall.Address
{
    public partial class ResidentialAddressM : ObservableObject
    {
        private bool name;
        public bool Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value, nameof(Name))) { }
            }
        }
        private bool surname;
        public bool Surname
        {
            get => surname;
            set
            {
                if (SetProperty(ref surname, value, nameof(Surname))) { }
            }
        }
        private bool street;
        public bool Street
        {
            get => street;
            set
            {
                if (SetProperty(ref street, value, nameof(Street))) { }
            }
        }
        private bool houseNumber;
        public bool HouseNumber
        {
            get => houseNumber;
            set
            {
                if (SetProperty(ref houseNumber, value, nameof(HouseNumber))) { }
            }
        }
        private bool apartmentNumber;
        public bool ApartmentNumber
        {
            get => apartmentNumber;
            set
            {
                if (SetProperty(ref apartmentNumber, value, nameof(ApartmentNumber))) { }
            }
        }
        private bool postalCode;
        public bool PostalCode
        {
            get => postalCode;
            set
            {
                if (SetProperty(ref postalCode, value, nameof(PostalCode))) { }
            }
        }
        private bool city;
        public bool City
        {
            get => city;
            set
            {
                if (SetProperty(ref city, value, nameof(City))) { }
            }
        }
        private bool country;
        public bool Country
        {
            get => country;
            set
            {
                if (SetProperty(ref country, value, nameof(Country))) { }
            }
        }

    }
}
