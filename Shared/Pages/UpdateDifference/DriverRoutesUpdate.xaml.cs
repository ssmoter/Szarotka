using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesRoutes;

namespace Shared.Pages.UpdateDifference;

public partial class DriverRoutesUpdate : ContentView, IDisposable
{
    public static readonly BindableProperty CustomerRoutesServerProperty =
        BindableProperty.Create(
            nameof(CustomerRoutesServer),
            typeof(CustomerRoutes),
            typeof(DriverRoutesUpdate),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
            });

    public CustomerRoutes CustomerRoutesServer
    {
        get => (CustomerRoutes)GetValue(CustomerRoutesServerProperty);
        set => SetValue(CustomerRoutesServerProperty, value);
    }


    public static readonly BindableProperty CustomerRoutesUpdateProperty =
    BindableProperty.Create(
        nameof(CustomerRoutesUpdate),
        typeof(CustomerRoutes),
        typeof(DriverRoutesUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
        });

    public CustomerRoutes CustomerRoutesUpdate
    {
        get => (CustomerRoutes)GetValue(CustomerRoutesUpdateProperty);
        set => SetValue(CustomerRoutesUpdateProperty, value);
    }

    public CustomerRoutesUpdate UpdateBool { get; set; }



    public static readonly BindableProperty SelectedBoolProperty =
    BindableProperty.Create(
        nameof(SelectedBool),
        typeof(bool),
        typeof(DriverRoutesUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is DriverRoutesUpdate view)
            {
                if (newValue is bool value)
                {
                    view.UpdateBool.BoolRoutesId = value;
                    view.UpdateBool.BoolName = value;
                    view.UpdateBool.BoolDescription = value;
                    view.UpdateBool.BoolPhoneNumber = value;
                    view.UpdateBool.BoolDayOfWeek = value;
                    view.UpdateBool.BoolResidentialAddress = value;
                    view.UpdateBool.BoolLongitude = value;
                    view.UpdateBool.BoolLatitude = value;
                }
            }
        });

    public bool SelectedBool
    {
        get => (bool)GetValue(SelectedBoolProperty);
        set => SetValue(SelectedBoolProperty, value);
    }
    private static Action<bool> ActionSelectedChange;
    private void _SelectedChange(bool value)
    {
        SelectedBool = value;
    }
    public static void OnSelectedChange(bool value)
    {
        ActionSelectedChange?.Invoke(value);
    }

    private static Func<CustomerRoutes> ReturnCustomerRoutes;
    public static CustomerRoutes OnReturnCustomerRoute()
    {
        return ReturnCustomerRoutes?.Invoke();
    }
    private CustomerRoutes _ReturnCustomerRoutes()
    {
        CustomerRoutes customer = new();

        ApplyUpdateBoolToCustomerRoutes(customer);

        return customer;
    }
    private static List<Func<CustomerRoutes>> ReturnCustomerRoutesList = new();

    public static void Register(Func<CustomerRoutes> func)
    {
        ReturnCustomerRoutesList.Add(func);
    }
    public static void Remove(Func<CustomerRoutes> func)
    {
        ReturnCustomerRoutesList.Remove(func);
    }
    public static List<CustomerRoutes> OnReturnCustomerRoutes()
    {
        return ReturnCustomerRoutesList.Select(f => f()).ToList();
    }


    public DriverRoutesUpdate()
    {
        CustomerRoutesServer ??= new();
        CustomerRoutesUpdate ??= new();
        UpdateBool ??= new();
        UpdateBool.PropertyChanged += UpdateBool_PropertyChanged;
        ActionSelectedChange += _SelectedChange;
        ReturnCustomerRoutes += _ReturnCustomerRoutes;
        Register(ReturnCustomerRoutes);
        InitializeComponent();
    }

    private void UpdateBool_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        SetUpdateBool();
    }

    public void Dispose()
    {
        ActionSelectedChange -= _SelectedChange;
        ReturnCustomerRoutes -= _ReturnCustomerRoutes;
        UpdateBool.PropertyChanged -= UpdateBool_PropertyChanged;
        Remove(ReturnCustomerRoutes);
    }
    public void ApplyUpdateBoolToCustomerRoutes(CustomerRoutes target)
    {
        if (target == null) return;

        SetUpdateBool();
        target.Created = CustomerRoutesServer.Created;
        target.UserCreatedId = CustomerRoutesServer.UserCreatedId;

        target.Updated = UpdateBool.BoolUpdate ? CustomerRoutesUpdate.Updated : CustomerRoutesServer.Updated;
        target.UserUpdatedId = UpdateBool.BoolUpdate ? CustomerRoutesUpdate.UserUpdatedId : CustomerRoutesServer.UserUpdatedId;
        // RoutesId
        target.RoutesId = UpdateBool.BoolRoutesId ? CustomerRoutesUpdate.RoutesId : CustomerRoutesServer.RoutesId;
        // Name
        target.Name = UpdateBool.BoolName ? CustomerRoutesUpdate.Name : CustomerRoutesServer.Name;
        // Description
        target.Description = UpdateBool.BoolDescription ? CustomerRoutesUpdate.Description : CustomerRoutesServer.Description;
        // PhoneNumber
        target.PhoneNumber = UpdateBool.BoolPhoneNumber ? CustomerRoutesUpdate.PhoneNumber : CustomerRoutesServer.PhoneNumber;
        // DayOfWeek
        target.DayOfWeek = UpdateBool.BoolDayOfWeek
            ? (CustomerRoutesUpdate.DayOfWeek != null ? new SelectedDayOfWeekRoutes(CustomerRoutesUpdate.DayOfWeek) : null)
            : (CustomerRoutesServer.DayOfWeek != null ? new SelectedDayOfWeekRoutes(CustomerRoutesServer.DayOfWeek) : null);
        // ResidentialAddress
        target.ResidentialAddress = UpdateBool.BoolResidentialAddress
            ? (CustomerRoutesUpdate.ResidentialAddress != null ? new ResidentialAddress(CustomerRoutesUpdate.ResidentialAddress) : null)
            : (CustomerRoutesServer.ResidentialAddress != null ? new ResidentialAddress(CustomerRoutesServer.ResidentialAddress) : null);
        // Longitude
        target.Longitude = UpdateBool.BoolLongitude ? CustomerRoutesUpdate.Longitude : CustomerRoutesServer.Longitude;
        // Latitude
        target.Latitude = UpdateBool.BoolLatitude ? CustomerRoutesUpdate.Latitude : CustomerRoutesServer.Latitude;
    }

    private void SetUpdateBool()
    {
        if (UpdateBool.BoolRoutesId ||
            UpdateBool.BoolName ||
            UpdateBool.BoolDescription ||
            UpdateBool.BoolPhoneNumber ||
            UpdateBool.BoolDayOfWeek ||
            UpdateBool.BoolResidentialAddress ||
            UpdateBool.BoolLongitude ||
            UpdateBool.BoolLatitude)
        {
            UpdateBool.BoolUpdate = true;
        }
        else
        {
            UpdateBool.BoolUpdate = false;
        }
    }
}
public partial class CustomerRoutesUpdate : ObservableObject
{

    private bool _boolUpdate;
    public bool BoolUpdate
    {
        get => _boolUpdate;
        set => SetProperty(ref _boolUpdate, value, nameof(BoolUpdate));
    }
    private bool _boolRoutesId;
    public bool BoolRoutesId
    {
        get => _boolRoutesId;
        set => SetProperty(ref _boolRoutesId, value, nameof(BoolRoutesId));
    }

    private bool _boolName;
    public bool BoolName
    {
        get => _boolName;
        set => SetProperty(ref _boolName, value, nameof(BoolName));
    }

    private bool _boolDescription;
    public bool BoolDescription
    {
        get => _boolDescription;
        set => SetProperty(ref _boolDescription, value, nameof(BoolDescription));
    }

    private bool _boolPhoneNumber;
    public bool BoolPhoneNumber
    {
        get => _boolPhoneNumber;
        set => SetProperty(ref _boolPhoneNumber, value, nameof(BoolPhoneNumber));
    }

    private bool _boolDayOfWeek;
    public bool BoolDayOfWeek
    {
        get => _boolDayOfWeek;
        set => SetProperty(ref _boolDayOfWeek, value, nameof(BoolDayOfWeek));
    }

    private bool _boolResidentialAddress;
    public bool BoolResidentialAddress
    {
        get => _boolResidentialAddress;
        set => SetProperty(ref _boolResidentialAddress, value, nameof(BoolResidentialAddress));
    }

    private bool _boolLongitude;
    public bool BoolLongitude
    {
        get => _boolLongitude;
        set => SetProperty(ref _boolLongitude, value, nameof(BoolLongitude));
    }

    private bool _boolLatitude;
    public bool BoolLatitude
    {
        get => _boolLatitude;
        set => SetProperty(ref _boolLatitude, value, nameof(BoolLatitude));
    }
}