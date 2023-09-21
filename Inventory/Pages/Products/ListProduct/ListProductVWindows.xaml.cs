namespace Inventory.Pages.Products.ListProduct;

public partial class ListProductVWindows : ContentPage
{
	public ListProductVWindows(ListProductVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}