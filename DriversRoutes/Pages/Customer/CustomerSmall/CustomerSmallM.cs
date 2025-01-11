using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Customer.CustomerSmall
{
    public partial class CustomerSmallM : ObservableObject
    {
        private bool description;
        public bool Description
        {
            get => description;
            set
            {
                if (SetProperty(ref description, value, nameof(Description))) { }
            }
        }
        private bool phoneNumber;
        public bool PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (SetProperty(ref phoneNumber, value, nameof(PhoneNumber))) { }
            }
        }

        private bool address;
        public bool Address
        {
            get => address;
            set
            {
                if (SetProperty(ref address, value, nameof(Address))) { }
            }
        }

        private bool coordinates;
        public bool Coordinates
        {
            get => coordinates;
            set
            {
                if (SetProperty(ref coordinates, value, nameof(Coordinates))) { }
            }
        }


    }
}
