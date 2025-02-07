using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using Shared.Data;

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
    readonly IAccessDataBase _db;
    readonly Service.ISaveRoutes _saveRoutes;
    public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }

    public DisplayCustomerVM(IAccessDataBase db, Service.ISaveRoutes saveRoutes)
    {
        _db = db;
        DisplayCustomerM ??= new();
        _saveRoutes = saveRoutes;
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


            var taskDay = _db.DataBaseAsync.DeleteAsync(Customer.DayOfWeek);
            var taskAddress = _db.DataBaseAsync.DeleteAsync(Customer.ResidentialAddress);
            var taskCustomer = _db.DataBaseAsync.DeleteAsync(Customer);

            var taskReady = await Task.WhenAll(taskDay, taskAddress, taskCustomer);

            result = await Shell.Current.DisplayAlert("Usunięto", "Obiekt został usunięty. Czy chcesz przywrócić", "Przywróć", "Nie");

            if (!result)
                await Shell.Current.GoToAsync("..");
            if (result)
            {
                await _saveRoutes.SaveCustomer(Customer, Customer.RoutesId.ToByteArray());
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
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

