namespace Inventory.Pages.Options.CreateTable;

public partial class CreateTableV : ContentView
{


    public CreateTableV()
    {
        InitializeComponent();
        BindingContext = new CreateTableVM(new());
    }
}