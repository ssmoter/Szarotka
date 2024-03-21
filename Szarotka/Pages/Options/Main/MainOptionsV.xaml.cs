using Inventory.Pages.Options.CreateTable;

namespace Szarotka.Pages.Options.Main;

public partial class MainOptionsV : ContentPage
{
    public MainOptionsV(MainOptionsVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is MainOptionsVM vm)
        {
            await CreateTableVM.OnNavigation(vm._db);
        }

    }

}