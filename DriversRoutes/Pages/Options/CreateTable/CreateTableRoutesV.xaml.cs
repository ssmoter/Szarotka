using DataBase.Model;

using Shared.Service;

namespace DriversRoutes.Pages.Options.CreateTable;

public partial class CreateTableRoutesV : ContentView
{
    public CreateTableRoutesV(CreateTableRoutesVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    public CreateTableRoutesV()
    {
        InitializeComponent();
        BindingContext = AppServiceProvider.GetService<CreateTableRoutesVM>();
    }
}