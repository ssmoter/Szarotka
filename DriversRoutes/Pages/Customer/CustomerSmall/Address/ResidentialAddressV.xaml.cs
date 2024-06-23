using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Customer.CustomerSmall.Address;


public partial class ResidentialAddressV : ContentView
{

    public static readonly BindableProperty ResidentialAddressProperty
    = BindableProperty.Create(nameof(ResidentialAddress), typeof(ResidentialAddress), typeof(ResidentialAddressV), propertyChanged: (bindable, oldValu, newValue) =>
    {
        var control = (ResidentialAddressV)bindable;
        var address = newValue as ResidentialAddress;
        if (address is not null)
        {
            control.ResidentialAddressM.Name = !string.IsNullOrWhiteSpace(address.Name);
            control.ResidentialAddressM.Surname = !string.IsNullOrWhiteSpace(address.Surname);
            control.ResidentialAddressM.Street = !string.IsNullOrWhiteSpace(address.Street);
            control.ResidentialAddressM.HouseNumber = !string.IsNullOrWhiteSpace(address.HouseNumber);
            control.ResidentialAddressM.ApartmentNumber = !string.IsNullOrWhiteSpace(address.ApartmentNumber);
            control.ResidentialAddressM.PostalCode = !string.IsNullOrWhiteSpace(address.PostalCode);
            control.ResidentialAddressM.City = !string.IsNullOrWhiteSpace(address.City);
            control.ResidentialAddressM.Country = !string.IsNullOrWhiteSpace(address.Country);
        }

        if
        (control.ResidentialAddressM.Name
        || control.ResidentialAddressM.Surname
        || control.ResidentialAddressM.Street
        || control.ResidentialAddressM.HouseNumber
        || control.ResidentialAddressM.ApartmentNumber
        || control.ResidentialAddressM.PostalCode
        || control.ResidentialAddressM.City
        || control.ResidentialAddressM.Country)
            control.AddressIsVisible = true;
        else
            control.AddressIsVisible = false;
    });

    public ResidentialAddress ResidentialAddress
    {
        get => (ResidentialAddress)GetValue(ResidentialAddressProperty);
        set => SetValue(ResidentialAddressProperty, value);
    }

    public static readonly BindableProperty AddressIsVisibleProperty
    = BindableProperty.Create(nameof(AddressIsVisible), typeof(bool), typeof(ResidentialAddressV), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValu, newValue) =>
    {

    });
    public bool AddressIsVisible
    {
        get => (bool)GetValue(AddressIsVisibleProperty);
        set => SetValue(AddressIsVisibleProperty, value);
    }

    private ResidentialAddressM residentialAddressM;
    public ResidentialAddressM ResidentialAddressM
    {
        get => residentialAddressM;
        set
        {
            residentialAddressM = value;
            OnPropertyChanged(nameof(ResidentialAddressM));
        }
    }

    public ResidentialAddressV()
    {
        InitializeComponent();
        ResidentialAddressM ??= new();
    }



    private async void TapGestureRecognizer_Tapped_CopyAddress(object sender, TappedEventArgs e)
    {
        await CopyAddress();
    }
    private async Task CopyAddress()
    {
        await Clipboard.SetTextAsync(ResidentialAddress.ToString());

        var toast = Toast.Make("Skopiowano adres", ToastDuration.Short);
        await toast.Show();
    }
}