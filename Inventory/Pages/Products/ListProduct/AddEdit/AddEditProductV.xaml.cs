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
        //gImg.MaximumHeightRequest = (bImg.Bounds.Height - 10) - ((vslImg.Bounds.Height + 15) );
    }

    private void BImg_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is Border)
        {
            if (e.PropertyName == nameof(Border.Bounds.Height))
            {
                gImg.MaximumHeightRequest = (bImg.Bounds.Height - 10) - ((vslImg.Bounds.Height + 15));
            }
        }
    }
}