﻿using CommunityToolkit.Mvvm.ComponentModel;


namespace DriversRoutes.Pages.Customer.AddCustomer
{
    public partial class AddCustomerM : ObservableObject
    {

        [ObservableProperty]
        bool address;

        [ObservableProperty]
        bool mapIsVisible;

        [ObservableProperty]
        bool mapIsVisibleHelperTime;

        public AddCustomerM()
        {

        }


    }
}
