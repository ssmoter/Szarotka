using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;

namespace DriversRoutes.Pages.Customer.AddCustomer
{
    [QueryProperty(nameof(Customer), nameof(CustomerRoutes))]
    [QueryProperty(nameof(RouteId), nameof(Routes))]
    public partial class AddCustomerVM : ObservableObject
    {
        #region Variable


        [ObservableProperty]
        AddCustomerM addCustomer;
        public Routes RouteId { get; set; }

        [ObservableProperty]
        CustomerRoutes customer;

        readonly Service.ISaveRoutes _saveRoutes;
        readonly Service.IDownloadAddress _downloadAddress;
        readonly DataBase.Data.AccessDataBase _db;
        ResidentialAddress[] _address = Array.Empty<ResidentialAddress>();
        #endregion

        public AddCustomerVM(Service.ISaveRoutes saveRoutes, DataBase.Data.AccessDataBase db, Service.IDownloadAddress downloadAddress)
        {
            AddCustomer ??= new();
            Customer ??= new();
            Customer.Name = "Dodawanie nowego punktu";
            _saveRoutes = saveRoutes;
            _db = db;
            _downloadAddress = downloadAddress;
        }


        #region Command

        [RelayCommand]
        async Task SaveAndExit()
        {
            try
            {
                await _saveRoutes.SaveCustomer(Customer, RouteId.Id.ToByteArray());

                await Shell.Current.GoToAsync($"..?", new Dictionary<string, object>()
                {
                    [nameof(CustomerRoutes)] = Customer
                });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task CancelAndExit()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        [RelayCommand]
        void DisplayPinOnMap()
        {
            try
            {
                AddCustomer.MapIsVisible = !AddCustomer.MapIsVisible;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task GetAddressFromApi()
        {
            try
            {
                if (AddCustomer is null)
                {
                    return;
                }

                if (_address.Length < 1)
                {
                    var response = await _downloadAddress.FindAddressFromCoordinates(Customer.Latitude, Customer.Longitude);
                    _address = new ResidentialAddress[response.Results.Count];

                    for (int i = 0; i < response.Results.Count; i++)
                    {
                        _address[i] = response.Results[i].FromGoogleToAddress();
                    }
                }

                var popup = new DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses.ProbableAddressesV(_address);

                var result = await Shell.Current.ShowPopupAsync(popup);

                if (result is ResidentialAddress address)
                {
                    Customer.ResidentialAddress = address;
                }

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        #endregion

    }
}
