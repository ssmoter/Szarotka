using CommunityToolkit.Maui.Views;

namespace Inventory.Pages.PopupCalculator;

public partial class PopupCalculatorV : Popup
{
	public PopupCalculatorV()
	{
		InitializeComponent();
		BindingContext= new PopupCalculatorVM();
	}
}