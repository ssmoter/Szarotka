using Inventory.Pages.Options.CreateTable;

namespace SzarotkaNET10.Pages.Options.Main;

public partial class MainOptionsV : ContentPage
{
    public MainOptionsV(MainOptionsVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}