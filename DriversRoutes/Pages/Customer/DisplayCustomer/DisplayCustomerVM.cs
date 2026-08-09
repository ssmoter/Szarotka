using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;
using DataBase.Service;

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
    private readonly IAccessDataBaseAoT _db;
    private readonly DataBase.Data.Save.ISaveDriverRoutesAoT _save;
    private readonly DataBase.Service.IUpdateLogService _update;

    public DisplayCustomerVM(IAccessDataBaseAoT db, DataBase.Data.Save.ISaveDriverRoutesAoT save, DataBase.Service.IUpdateLogService update)
    {
        _db = db;
        DisplayCustomerM ??= new();
        _save = save;
        _update = update;
    }


    #region Command

    [RelayCommand]
    async Task Delete(CustomerRoutes point)
    {
        try
        {
            if (point is null)
                return;

            var result = await Shell.Current.DisplayAlertAsync("Czy usunąć", $"Czy na pewno chcesz usunąć {point.QueueNumber}:{point.Name}", "Tak", "Anuluj");

            if (!result)
                return;


            await Update(true);

            result = await Shell.Current.DisplayAlertAsync("Usunięto", "Obiekt został usunięty. Czy chcesz przywrócić", "Przywróć", "Nie");

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

            var user = UserAfterLogin.User.Id.ToByteArray();
            await _save.SaveCustomerRoutes(Customer, user);
            await _save.SaveResidentialAddress(Customer.ResidentialAddress, user);
            await _save.SaveSelectedDayOfWeekRoutes(Customer.DayOfWeek, user);

            await _update.Insert(new DataBase.Model.UpdateLog()
            {
                IsServer = false,
            }, Customer);

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
                    [nameof(CustomerRoutes)] = new CustomerRoutes(point),
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


