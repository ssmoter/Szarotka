using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DriversRoutes.Model;

using DriversRoutes.Helper;

namespace DriversRoutes.Pages.AddCustomer
{
    [QueryProperty(nameof(Customer), nameof(Model.CustomerRoutes))]
    [QueryProperty(nameof(RouteId), nameof(Routes))]
    public partial class AddCustomerVM : ObservableObject, IDisposable
    {
        #region Variable


        [ObservableProperty]
        AddCustomerM addCustomer;
        public Routes RouteId { get; set; }

        public Model.CustomerRoutes Customer { get; set; }
        readonly Service.ISaveRoutes _saveRoutes;
        readonly Service.IDownloadAddress _downloadAddress;
        readonly DataBase.Data.AccessDataBase _db;
        DriversRoutes.Model.ResidentialAddress[] _address = Array.Empty<DriversRoutes.Model.ResidentialAddress>();
        #endregion

        public AddCustomerVM(Service.ISaveRoutes saveRoutes, DataBase.Data.AccessDataBase db, Service.IDownloadAddress downloadAddress)
        {
            AddCustomer ??= new();
            AddCustomer.Name = "Dodawanie nowego punktu";
            _saveRoutes = saveRoutes;
            _db = db;
            _downloadAddress = downloadAddress;
        }
        public void SetCustomer(Model.CustomerRoutes customer)
        {
            try
            {
                AddCustomer = new AddCustomerM()
                {
                    Id = customer.Id,
                    RoutesId = customer.RoutesId,
                    Index = customer.QueueNumber,
                    Name = customer.Name,
                    Description = customer.Description,
                    PhoneNumber = customer.PhoneNumber,
                    Created = customer.CreatedDate,
                    SelectedDayOfWeek = customer.DayOfWeek,
                    ResidentialAddress = customer.ResidentialAddress,
                    Longitude = customer.Longitude,
                    Latitude = customer.Latitude,
                };
                AddCustomer.SelectedDayOfWeek ??= new();
                AddCustomer.ResidentialAddress ??= new();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }


        }


        #region Command

        [RelayCommand]
        async Task SaveAndExit()
        {
            try
            {
                var customer = new Model.CustomerRoutes()
                {
                    Id = AddCustomer.Id,
                    RoutesId = AddCustomer.RoutesId,
                    QueueNumber = AddCustomer.Index,
                    Name = AddCustomer.Name,
                    Description = AddCustomer.Description,
                    PhoneNumber = AddCustomer.PhoneNumber,
                    CreatedDate = AddCustomer.Created,
                    DayOfWeek = AddCustomer.SelectedDayOfWeek,
                    Longitude = AddCustomer.Longitude,
                    Latitude = AddCustomer.Latitude,
                    ResidentialAddress = AddCustomer.ResidentialAddress,
                };
                customer.DayOfWeek.ValuesAsString = customer.DayOfWeek.ToString();
                await _saveRoutes.SaveCustomer(customer, RouteId.Id.ToByteArray());

                await Shell.Current.GoToAsync("..");
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
                    var response = await _downloadAddress.FindAddressFromCoordinates(AddCustomer.Latitude, AddCustomer.Longitude);
                    _address = new Model.ResidentialAddress[response.Results.Count];

                    for (int i = 0; i < response.Results.Count; i++)
                    {
                        _address[i] = response.Results[i].FromGoogleToAddress();
                    }
                }

                var popup = new DriversRoutes.Pages.AddCustomer.ProbableAddresses.ProbableAddressesV(_address);

                var result = await Shell.Current.ShowPopupAsync(popup);

                if (result is DriversRoutes.Model.ResidentialAddress address)
                {
                    AddCustomer.ResidentialAddress = address;
                }

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        #endregion



        public void Dispose()
        {
            AddCustomer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
