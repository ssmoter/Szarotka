using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

namespace Shared.Pages.UpdateDifference.Templates;

public partial class DayUpdate : ContentView, IDisposable
{
    public static readonly BindableProperty DayServerProperty =
    BindableProperty.Create(
        nameof(DayServer),
        typeof(Day),
        typeof(DayUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {

        });
    public Day DayServer
    {
        get => (Day)GetValue(DayServerProperty);
        set => SetValue(DayServerProperty, value);
    }


    public static readonly BindableProperty DayUpdateValueProperty =
    BindableProperty.Create(
        nameof(DayUpdateValue),
        typeof(Day),
        typeof(DayUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
        });
    public Day DayUpdateValue
    {
        get => (Day)GetValue(DayUpdateValueProperty);
        set => SetValue(DayUpdateValueProperty, value);
    }

    public static readonly BindableProperty UpdateBoolProperty =
BindableProperty.Create(
    nameof(UpdateBool),
    typeof(BoolDayUpdate),
    typeof(DayUpdate),
    defaultBindingMode: BindingMode.TwoWay,
    propertyChanged: (bindable, oldValue, newValue) =>
    {
        if (newValue is null)
        {
            var view = bindable as DayUpdate;
            view.UpdateBool = new BoolDayUpdate();
        }
    });
    public BoolDayUpdate UpdateBool
    {
        get => (BoolDayUpdate)GetValue(UpdateBoolProperty);
        set => SetValue(UpdateBoolProperty, value);
    }

    public static readonly BindableProperty DayCollectionViewExpandedProperty =
        BindableProperty.Create(
        nameof(DayCollectionViewExpanded),
        typeof(DayCollectionViewExpanded),
        typeof(DayUpdate),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
        });

    public DayCollectionViewExpanded DayCollectionViewExpanded
    {
        get => (DayCollectionViewExpanded)GetValue(DayCollectionViewExpandedProperty);
        set => SetValue(DayCollectionViewExpandedProperty, value);
    }
    public static readonly BindableProperty SelectedBoolProperty =
    BindableProperty.Create(
        nameof(SelectedBool),
        typeof(bool),
        typeof(DayUpdate),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is DayUpdate view)
            {
                if (newValue is bool value)
                {
                    view.UpdateBool.BoolUpdate = value;
                    view.UpdateBool.BoolDescription = value;
                    view.UpdateBool.BoolRest = value;
                }
            }
        });
    public bool SelectedBool
    {
        get => (bool)GetValue(SelectedBoolProperty);
        set => SetValue(SelectedBoolProperty, value);
    }


    public DayUpdate()
    {
        DayServer ??= new();
        DayUpdateValue ??= new();
        UpdateBool ??= new();
        UpdateBool.PropertyChanged += UpdateBool_PropertyChanged;
        DayCollectionViewExpanded = new();

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

    private static void SetUpdateBool(BoolDayUpdate UpdateBool)
    {
        if (UpdateBool.BoolDescription || UpdateBool.BoolRest)
        {
            UpdateBool.BoolUpdate = true;
        }
        else
        {
            UpdateBool.BoolUpdate = false;
        }
    }

    public static Day ApplyUpdateBoolTo(Day server,
                                                  Day update,
                                                  object updateSelected)
    {
        var updateBool = updateSelected as BoolDayUpdate;
        ArgumentNullException.ThrowIfNull(updateBool);
        SetUpdateBool(updateBool);
        Day target = new()
        {
            Id = updateBool.BoolUpdate ? update.Id : server.Id,
            IsDelete = updateBool.BoolUpdate ? update.IsDelete : server.IsDelete,

            Created = updateBool.BoolUpdate ? update.Created : server.Created,
            UserCreatedId = updateBool.BoolUpdate ? update.UserCreatedId : server.UserCreatedId,

            Updated = updateBool.BoolUpdate ? update.Updated : server.Updated,
            UserUpdatedId = updateBool.BoolUpdate ? update.UserUpdatedId : server.UserUpdatedId,

            Description = updateBool.BoolDescription ? update.Description : server.Description,
            SelectedDateString = updateBool.BoolRest ? update.SelectedDateString : server.SelectedDateString,
            SelectedDateTicks = updateBool.BoolRest ? update.SelectedDateTicks : server.SelectedDateTicks,
            TotalPriceProducts = updateBool.BoolRest ? update.TotalPriceProducts : server.TotalPriceProducts,
            TotalPriceCake = updateBool.BoolRest ? update.TotalPriceCake : server.TotalPriceCake,
            TotalPrice = updateBool.BoolRest ? update.TotalPrice : server.TotalPrice,
            TotalPriceCorrect = updateBool.BoolRest ? update.TotalPriceCorrect : server.TotalPriceCorrect,
            TotalPriceAfterCorrect = updateBool.BoolRest ? update.TotalPriceAfterCorrect : server.TotalPriceAfterCorrect,
            TotalPriceMoney = updateBool.BoolRest ? update.TotalPriceMoney : server.TotalPriceMoney,
            TotalPriceDifference = updateBool.BoolRest ? update.TotalPriceDifference : server.TotalPriceDifference,
            Products = updateBool.BoolRest ? update.Products : server.Products,
            Cakes = updateBool.BoolRest ? update.Cakes : server.Cakes,
        };
        return target;
    }
    public static void SetUpdateBool(BoolDayUpdate updateSelected, bool? setAll = null)
    {
        var updateBool = updateSelected as BoolDayUpdate;
        ArgumentNullException.ThrowIfNull(updateBool);

        if (setAll is not null)
        {
            updateBool.BoolUpdate = setAll.Value;
            updateBool.BoolDescription = setAll.Value;
            updateBool.BoolRest = setAll.Value;
        }
        SetUpdateBool(updateBool);
    }


}


public partial class BoolDayUpdate : ObservableObject
{
    private bool _boolUpdate;
    public bool BoolUpdate
    {
        get => _boolUpdate;
        set => SetProperty(ref _boolUpdate, value, nameof(BoolUpdate));
    }

    private bool _boolDescription;
    public bool BoolDescription
    {
        get => _boolDescription;
        set => SetProperty(ref _boolDescription, value, nameof(BoolDescription));
    }

    private bool _boolRest;
    public bool BoolRest
    {
        get => _boolRest;
        set => SetProperty(ref _boolRest, value, nameof(BoolRest));
    }
}

public partial class DayCollectionViewExpanded : ObservableObject
{
    private bool _cake;
    public bool Cake
    {
        get => _cake;
        set => SetProperty(ref _cake, value, nameof(Cake));
    }
    private bool _product;
    public bool Product
    {
        get => _product;
        set => SetProperty(ref _product, value, nameof(Product));
    }
}