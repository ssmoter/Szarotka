namespace Inventory.Pages.Products.ListProduct;

public partial class ListProductV : ContentPage
{
    public ListProductV(ListProductVM vm)
    {
        InitializeComponent();
        vm.ScrollTo += ListOfProductCV.ScrollTo;
        BindingContext = vm;
    }

    //protected override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    base.OnNavigatedTo(args);
    //    var vm = BindingContext as ListProductVM;
    //    if (vm != null)
    //    {
    //        Task.Run(async () =>
    //        {
    //            await vm.SelectAllProductsAsync();
    //        });
    //    }
    //}


}