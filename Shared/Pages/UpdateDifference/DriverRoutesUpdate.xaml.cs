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

    public static readonly BindableProperty UpdateBoolProperty =
BindableProperty.Create(
    nameof(UpdateBool),
    typeof(CustomerRoutesUpdate),
    typeof(DriverRoutesUpdate),
    defaultBindingMode: BindingMode.TwoWay,
    propertyChanged: (bindable, oldValue, newValue) =>
    {
        if (newValue is null)
        {
            var view = bindable as DriverRoutesUpdate;
            view.UpdateBool = new CustomerRoutesUpdate();
        }
    });

    public CustomerRoutesUpdate UpdateBool
    {
        get => (CustomerRoutesUpdate)GetValue(UpdateBoolProperty);
        set => SetValue(UpdateBoolProperty, value);
    }

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
                    view.UpdateBool.BoolUpdate = value;
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



    public DriverRoutesUpdate()
    {
        CustomerRoutesServer ??= new();
        CustomerRoutesUpdate ??= new();
        UpdateBool = new();
        UpdateBool.PropertyChanged += UpdateBool_PropertyChanged;
        InitializeComponent();
    }

    private void UpdateBool_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        SetUpdateBool();
    }

    public void Dispose()
    {
        UpdateBool.PropertyChanged -= UpdateBool_PropertyChanged;
    }

    public static CustomerRoutes ApplyUpdateBoolTo(CustomerRoutes server,
                                                          CustomerRoutes update,
                                                          object updateSelected)
    {
        var updateBool = updateSelected as CustomerRoutesUpdate;
        ArgumentNullException.ThrowIfNull(updateBool);
        SetUpdateBool(updateBool);
        CustomerRoutes target = new()
        {
            Id = updateBool.BoolUpdate ? update.Id : server.Id,
            IsDelete = updateBool.BoolUpdate ? update.IsDelete : server.IsDelete,

            Created = updateBool.BoolUpdate ? update.Created : server.Created,
            UserCreatedId = updateBool.BoolUpdate ? update.UserCreatedId : server.UserCreatedId,

            Updated = updateBool.BoolUpdate ? update.Updated : server.Updated,
            UserUpdatedId = updateBool.BoolUpdate ? update.UserUpdatedId : server.UserUpdatedId,
            // RoutesId
            RoutesId = updateBool.BoolRoutesId ? update.RoutesId : server.RoutesId,
            // Name
            Name = updateBool.BoolName ? update.Name : server.Name,
            // Description
            Description = updateBool.BoolDescription ? update.Description : server.Description,
            // PhoneNumber
            PhoneNumber = updateBool.BoolPhoneNumber ? update.PhoneNumber : server.PhoneNumber,
            // DayOfWeek
            DayOfWeek = updateBool.BoolDayOfWeek ? update.DayOfWeek : server.DayOfWeek,
            // ResidentialAddress
            ResidentialAddress = updateBool.BoolResidentialAddress ? update.ResidentialAddress : server.ResidentialAddress,
            // Longitude
            Longitude = updateBool.BoolLongitude ? update.Longitude : server.Longitude,
            // Latitude
            Latitude = updateBool.BoolLatitude ? update.Latitude : server.Latitude
        };
        return target;
    }
    public static void SetUpdateBool(CustomerRoutesUpdate updateSelected, bool? setAll = null)
    {
        var updateBool = updateSelected as CustomerRoutesUpdate;
        ArgumentNullException.ThrowIfNull(updateBool);

        if (setAll is not null)
        {
            updateBool.BoolUpdate = setAll.Value;
            updateBool.BoolRoutesId = setAll.Value;
            updateBool.BoolName = setAll.Value;
            updateBool.BoolDescription = setAll.Value;
            updateBool.BoolPhoneNumber = setAll.Value;
            updateBool.BoolDayOfWeek = setAll.Value;
            updateBool.BoolResidentialAddress = setAll.Value;
            updateBool.BoolLongitude = setAll.Value;
            updateBool.BoolLatitude = setAll.Value;
        }
        if (updateBool.BoolRoutesId ||
            updateBool.BoolName ||
            updateBool.BoolDescription ||
            updateBool.BoolPhoneNumber ||
            updateBool.BoolDayOfWeek ||
            updateBool.BoolResidentialAddress ||
            updateBool.BoolLongitude ||
            updateBool.BoolLatitude)
        {
            updateBool.BoolUpdate = true;
        }
        else
        {
            updateBool.BoolUpdate = false;
        }
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