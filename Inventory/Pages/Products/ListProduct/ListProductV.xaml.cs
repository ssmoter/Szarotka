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

        if (DeviceInfo.Platform == DevicePlatform.Android)
            this.ListOfProductCV.ItemTemplate = (DataTemplate)Resources["Android"];
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            this.ListOfProductCV.ItemTemplate = (DataTemplate)Resources["WinUI"];

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

    private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
    {

        if (sender is not DragGestureRecognizer drag) { return; }

        if (drag.Parent is not Grid grid) { return; }

        if (grid.BindingContext is not ListProductM product) { return; }

        vm.DragCommand.Execute(product);
    }


    private void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
    {
        if (sender is not DropGestureRecognizer drop) { return; }

        if (drop.Parent is not Grid grid) { return; }

        if (grid.BindingContext is not ListProductM product) { return; }

        vm.DropCommand.Execute(product);
    }
}