namespace DriversRoutes.Pages.Options.CreateTable;

public partial class CreateTableRoutesV : ContentView
{

    public static readonly BindableProperty DataBaseVersionProperty
    = BindableProperty.Create(nameof(DataBaseVersion), typeof(DataBase.Model.DataBaseVersion), typeof(CreateTableRoutesV), propertyChanged: (bindable, oldValue, newValue) =>
    { });
    public DataBase.Model.DataBaseVersion DataBaseVersion
    {
        get => GetValue(DataBaseVersionProperty) as DataBase.Model.DataBaseVersion;
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
        BindingContext = new CreateTableRoutesVM(new DataBase.Data.AccessDataBase());

    }
}