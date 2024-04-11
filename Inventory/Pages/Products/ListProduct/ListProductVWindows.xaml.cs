namespace Inventory.Pages.Products.ListProduct;

public partial class ListProductVWindows : ContentPage
{
    public ListProductVWindows(ListProductVM vm)
    {
        InitializeComponent();
        vm.ScrollTo += ListOfProductCV.ScrollTo;
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is ListProductVM vm)
        {
            await vm.SelectAllProductsAsync();
        }
    }
}