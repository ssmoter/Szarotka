namespace Inventory.Pages.Options.CreateTable;

public partial class CreateTableV : ContentView
{


    public CreateTableV()
    {
        InitializeComponent();
        BindingContext = Shared.Service.AppServiceProvider.GetService<CreateTableVM>();
    }
}