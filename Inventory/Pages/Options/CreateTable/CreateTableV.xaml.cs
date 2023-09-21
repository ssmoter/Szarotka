using DataBase.Data;

namespace Inventory.Pages.Options.CreateTable;

public partial class CreateTableV : ContentView
{
    public CreateTableV(CreateTableVM contex)
    {
        InitializeComponent();
        BindingContext = contex;
    }
    public CreateTableV()
    {
        InitializeComponent();
        BindingContext = new CreateTableVM(new AccessDataBase());
    }
}