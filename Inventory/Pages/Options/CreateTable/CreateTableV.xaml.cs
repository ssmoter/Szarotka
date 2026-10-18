namespace Inventory.Pages.Options.CreateTable;

public partial class CreateTableV : ContentView
{


    public CreateTableV()
    {
        InitializeComponent();
        BindingContext = Shared.Service.AppServiceProvider.GetService<CreateTableVM>();
        this.Loaded += CreateTableV_Loaded;
    }

    private void CreateTableV_Loaded(object sender, EventArgs e)
    {
        if (BindingContext is CreateTableVM contex)
        {
            contex.InitializeRoutesCommand.Execute(null);
        }
    }
}