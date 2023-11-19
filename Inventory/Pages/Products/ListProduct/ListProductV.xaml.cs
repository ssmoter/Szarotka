namespace Inventory.Pages.Products.ListProduct;

public partial class ListProductV : ContentPage
{
    readonly ListProductVM vm;
    public ListProductV(ListProductVM vm)
    {
        InitializeComponent();
        this.vm = vm;
        vm.ScrollTo += ListOfProductCV.ScrollTo;
        BindingContext = vm;
    }


    private void SwipeItem_Invoked(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null) { return; }

        var product = item.BindingContext as ListProductM;
        if (product == null) { return; }

        vm.EditProductCommand.Execute(product);
    }

    private void ImageButton_Clicked_SetDown(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ListProductM;
        if (product == null) { return; }

        vm.SetDownCommand.Execute(product);
    }

    private void ImageButton_Clicked_SetUp(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ListProductM;
        if (product == null) { return; }

        vm.SetUpCommand.Execute(product);
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