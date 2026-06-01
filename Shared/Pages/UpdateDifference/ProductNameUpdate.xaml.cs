using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

namespace Shared.Pages.UpdateDifference;

public partial class ProductNameUpdate : ContentView, IDisposable
{
    public static readonly BindableProperty PNameServerProperty =
    BindableProperty.Create(
        nameof(PNameServer),
        typeof(ProductName),
        typeof(ProductNameUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {

        });
    public ProductName PNameServer
    {
        get => (ProductName)GetValue(PNameServerProperty);
        set => SetValue(PNameServerProperty, value);
    }


    public static readonly BindableProperty PNameUpdateProperty =
    BindableProperty.Create(
        nameof(PNameUpdate),
        typeof(ProductName),
        typeof(ProductNameUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
        });
    public ProductName PNameUpdate
    {
        get => (ProductName)GetValue(PNameUpdateProperty);
        set => SetValue(PNameUpdateProperty, value);
    }

    public static readonly BindableProperty UpdateBoolProperty =
BindableProperty.Create(
    nameof(UpdateBool),
    typeof(BoolPNameUpdate),
    typeof(ProductNameUpdate),
    defaultBindingMode: BindingMode.TwoWay,
    propertyChanged: (bindable, oldValue, newValue) =>
    {
        if (newValue is null)
        {
            var view = bindable as ProductNameUpdate;
            view.UpdateBool = new BoolPNameUpdate();
        }
    });
    public BoolPNameUpdate UpdateBool
    {
        get => (BoolPNameUpdate)GetValue(UpdateBoolProperty);
        set => SetValue(UpdateBoolProperty, value);
    }

    public static readonly BindableProperty SelectedBoolProperty =
    BindableProperty.Create(
        nameof(SelectedBool),
        typeof(bool),
        typeof(ProductNameUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is ProductNameUpdate view)
            {
                if (newValue is bool value)
                {
                    view.UpdateBool.BoolUpdate = value;
                    view.UpdateBool.BoolArrangement = value;
                    view.UpdateBool.BoolName = value;
                    view.UpdateBool.BoolDescription = value;
                    view.UpdateBool.BoolImg = value;
                    view.UpdateBool.BoolIsVisible = value;
                }
            }
        });
    public bool SelectedBool
    {
        get => (bool)GetValue(SelectedBoolProperty);
        set => SetValue(SelectedBoolProperty, value);
    }


    public ProductNameUpdate()
    {
        PNameServer ??= new();
        PNameUpdate ??= new();
        UpdateBool ??= new();
        UpdateBool.PropertyChanged += UpdateBool_PropertyChanged;


        InitializeComponent();
    }

    private void UpdateBool_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        SetUpdateBool(UpdateBool);
    }

    public void Dispose()
    {
        UpdateBool.PropertyChanged -= UpdateBool_PropertyChanged;
    }

    private static void SetUpdateBool(BoolPNameUpdate UpdateBool)
    {
        if (UpdateBool.BoolName ||
            UpdateBool.BoolDescription ||
            UpdateBool.BoolArrangement ||
            UpdateBool.BoolImg ||
            UpdateBool.BoolIsVisible)
        {
            UpdateBool.BoolUpdate = true;
        }
        else
        {
            UpdateBool.BoolUpdate = false;
        }
    }

    public static ProductName ApplyUpdateBoolTo(ProductName server,
                                                      ProductName update,
                                                      object updateSelected)
    {
        var updateBool = updateSelected as BoolPNameUpdate;
        ArgumentNullException.ThrowIfNull(updateBool);
        SetUpdateBool(updateBool);
        ProductName target = new()
        {
            Id = updateBool.BoolUpdate ? update.Id : server.Id,
            IsDelete = updateBool.BoolUpdate ? update.IsDelete : server.IsDelete,

            Created = updateBool.BoolUpdate ? update.Created : server.Created,
            UserCreatedId = updateBool.BoolUpdate ? update.UserCreatedId : server.UserCreatedId,

            Updated = updateBool.BoolUpdate ? update.Updated : server.Updated,
            UserUpdatedId = updateBool.BoolUpdate ? update.UserUpdatedId : server.UserUpdatedId,

            Arrangement = updateBool.BoolArrangement ? update.Arrangement : server.Arrangement,
            IsVisible = updateBool.BoolIsVisible ? update.IsVisible : server.IsVisible,
            Img = updateBool.BoolImg ? update.Img : server.Img,
            Name = updateBool.BoolName ? update.Name : server.Name,
            Description = updateBool.BoolDescription ? update.Description : server.Description,
        };
        return target;
    }
    public static void SetUpdateBool(BoolPNameUpdate updateSelected, bool? setAll = null)
    {
        var updateBool = updateSelected as BoolPNameUpdate;
        ArgumentNullException.ThrowIfNull(updateBool);

        if (setAll is not null)
        {
            updateBool.BoolUpdate = setAll.Value;
            updateBool.BoolName = setAll.Value;
            updateBool.BoolDescription = setAll.Value;
            updateBool.BoolArrangement = setAll.Value;
            updateBool.BoolIsVisible = setAll.Value;
            updateBool.BoolImg = setAll.Value;
        }
        SetUpdateBool(updateBool);
    }


}


public partial class BoolPNameUpdate : ObservableObject
{
    private bool _boolUpdate;
    public bool BoolUpdate
    {
        get => _boolUpdate;
        set => SetProperty(ref _boolUpdate, value, nameof(BoolUpdate));
    }

    private bool _BoolArrangement;
    public bool BoolArrangement
    {
        get => _BoolArrangement;
        set => SetProperty(ref _BoolArrangement, value, nameof(BoolArrangement));
    }
    private bool _BoolName;
    public bool BoolName
    {
        get => _BoolName;
        set => SetProperty(ref _BoolName, value, nameof(BoolName));
    }
    private bool _BoolDescription;
    public bool BoolDescription
    {
        get => _BoolDescription;
        set => SetProperty(ref _BoolDescription, value, nameof(BoolDescription));
    }
    private bool _BoolImg;
    public bool BoolImg
    {
        get => _BoolImg;
        set => SetProperty(ref _BoolImg, value, nameof(BoolImg));
    }
    private bool _BoolIsVisible;
    public bool BoolIsVisible
    {
        get => _BoolIsVisible;
        set => SetProperty(ref _BoolIsVisible, value, nameof(BoolIsVisible));
    }
}