namespace Inventory.Pages.Products.ListProduct.AddEdit;

public partial class AddEditProductV : ContentPage
{
    public AddEditProductV(AddEditProductVM vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = BindingContext as AddEditProductVM;
        if (vm != null)
        {
            Task.Run(async () =>
            {
                await vm.GetPrices(vm.Product.Name.Id);
            });
            if (vm.Product.Name.Id != Guid.Empty)
            {
                vm.AddEdit.AddP = false;
                vm.AddEdit.UpdateP = true;
            }
            else
            {
                vm.Product.Name.Img = Shared.Helper.Img.ImgPath.Logo;
                vm.AddEdit.AddP = true;
                vm.AddEdit.UpdateP = false;
            }
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (gImg.MaximumHeightRequest == double.PositiveInfinity || gImg.MaximumHeightRequest == 0)
        {
            gImg.MaximumHeightRequest = gImg.Bounds.Y;
        }
    }
}