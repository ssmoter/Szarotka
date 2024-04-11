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

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await vm.SelectAllProductsAsync();
    }

    private void SwipeItem_Invoked(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not ListProductM product) { return; }

        vm.EditProductCommand.Execute(product);
    }

    private void ImageButton_Clicked_SetDown(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ListProductM product) { return; }

        vm.SetDownCommand.Execute(product);
    }

    private void ImageButton_Clicked_SetUp(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ListProductM product) { return; }

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