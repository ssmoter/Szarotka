using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using Shared.Data;
using Shared.Helper;

namespace DriversRoutes.Pages.Customer.DisplayCustomer;
public partial class DisplayCustomerVM : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(CustomerRoutes), out object customer))
        {
            if (customer is CustomerRoutes _customer)
            {
                Customer = _customer;
            }
        }
        if (query.TryGetValue(nameof(LastSelectedDayOfWeek), out object lastSelectedDayOfWeek))
        {
            if (lastSelectedDayOfWeek is SelectedDayOfWeekRoutes _lastSelectedDayOfWeek)
            {
                LastSelectedDayOfWeek = _lastSelectedDayOfWeek;
            }
        }
    }

    private CustomerRoutes customer;
    public CustomerRoutes Customer
    {
        get => customer;
        set
        {
            if (SetProperty(ref customer, value, nameof(Customer))) { }
        }
    }

    private DisplayCustomerM displayCustomerM;
    public DisplayCustomerM DisplayCustomerM
    {
        get => displayCustomerM;
        set
        {
            if (SetProperty(ref displayCustomerM, value, nameof(DisplayCustomerM))) { }
        }
    }
    private readonly IAccessDataBase _db;
    private readonly DataBase.Data.Save.ISaveDriverRoutesAoT _save;
    public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }

    public DisplayCustomerVM(IAccessDataBase db, DataBase.Data.Save.ISaveDriverRoutesAoT save)
    {
        _db = db;
        DisplayCustomerM ??= new();
        _save = save;
    }


    #region Command

    [RelayCommand]
    async Task Delete(CustomerRoutes point)
    {
        try
        {
            if (point is null)
                return;

            var result = await Shell.Current.DisplayAlert("Czy usunąć", $"Czy na pewno chcesz usunąć {point.QueueNumber}:{point.Name}", "Tak", "Anuluj");

            if (!result)
                return;


            await Update(true);

            result = await Shell.Current.DisplayAlert("Usunięto", "Obiekt został usunięty. Czy chcesz przywrócić", "Przywróć", "Nie");

            if (!result)
                await Shell.Current.GoToAsync("..");
            if (result)
            {
                await Update(false);
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }

        async Task Update(bool delete)
        {
            customer.IsDelete = delete;
            customer.ResidentialAddress.IsDelete = delete;
            customer.DayOfWeek.IsDelete = delete;

            var user = UserAfterLogin.User;
            await _save.SaveCustomerRoutes(Customer, user.Id.ToByteArray());
            await _save.SaveResidentialAddress(Customer.ResidentialAddress, user.Id.ToByteArray());
            await _save.SaveSelectedDayOfWeekRoutes(Customer.DayOfWeek, user.Id.ToByteArray());
        }
    }

    [RelayCommand]
    void LocationOfPin()
    {
        try
        {
            DisplayCustomerM.ShowLocationThisCustomer = !DisplayCustomerM.ShowLocationThisCustomer;
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }

    [RelayCommand]
    async Task EditPin(CustomerRoutes point)
    {
        try
        {
            if (point is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(Pages.Customer.AddCustomer.AddCustomerV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(CustomerRoutes)] = new CustomerRoutes()
                    {
                        Id = new Guid(point.Id.ToByteArray()),
                        RoutesId = new Guid(point.RoutesId.ToByteArray()),
                        QueueNumber = point.QueueNumber,
                        Name = point.Name,
                        Description = point.Description,
                        PhoneNumber = point.PhoneNumber,
                        Created = point.Created,
                        DayOfWeek = point.DayOfWeek,
                        ResidentialAddress = point.ResidentialAddress,
                        Longitude = point.Longitude,
                        Latitude = point.Latitude,
                    },
                    [nameof(Routes)] = new Routes() { Id = new Guid(point.RoutesId.ToByteArray()), }
                });

        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }


    #endregion


}

