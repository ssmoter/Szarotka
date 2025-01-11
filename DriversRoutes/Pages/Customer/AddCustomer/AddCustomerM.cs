using CommunityToolkit.Mvvm.ComponentModel;


namespace DriversRoutes.Pages.Customer.AddCustomer
{
    public partial class AddCustomerM : ObservableObject
    {

        private bool address;
        public bool Address
        {
            get => address;
            set
            {
                if (SetProperty(ref address, value, nameof(Address))) { }
            }
        }

        private bool mapIsVisible;
        public bool MapIsVisible
        {
            get => mapIsVisible;
            set
            {
                if (SetProperty(ref mapIsVisible, value, nameof(MapIsVisible))) { }
            }
        }

        private bool mapIsVisibleHelperTime;
        public bool MapIsVisibleHelperTime
        {
            get => mapIsVisibleHelperTime;
            set
            {
                if (SetProperty(ref mapIsVisibleHelperTime, value, nameof(MapIsVisibleHelperTime))) { }
            }
        }

        public AddCustomerM()
        {

        }


    }
}
