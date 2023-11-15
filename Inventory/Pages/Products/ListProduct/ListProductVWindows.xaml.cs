namespace Inventory.Pages.Products.ListProduct;

public partial class ListProductVWindows : ContentPage
{
	public ListProductVWindows(ListProductVM vm)
	{
		InitializeComponent();
		vm.ScrollTo += ListOfProductCV.ScrollTo;
		BindingContext = vm;
	}
}