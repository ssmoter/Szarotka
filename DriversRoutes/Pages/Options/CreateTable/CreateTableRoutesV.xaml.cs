using DataBase.Data;
using DataBase.Model;

namespace DriversRoutes.Pages.Options.CreateTable;

public partial class CreateTableRoutesV : ContentView
{

    public static readonly BindableProperty DataBaseVersionProperty
    = BindableProperty.Create(nameof(DataBaseVersion), typeof(DataBaseVersion), typeof(CreateTableRoutesV), propertyChanged: (bindable, oldValue, newValue) =>
    { });
    public DataBaseVersion DataBaseVersion
    {
        get => GetValue(DataBaseVersionProperty) as DataBaseVersion;
        set => SetValue(DataBaseVersionProperty, value);
    }

    public CreateTableRoutesV(CreateTableRoutesVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    public CreateTableRoutesV()
    {
        InitializeComponent();
        BindingContext = new CreateTableRoutesVM(new AccessDataBase());

    }
}