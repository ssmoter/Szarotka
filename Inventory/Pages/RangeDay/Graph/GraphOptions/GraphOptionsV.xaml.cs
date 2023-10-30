using CommunityToolkit.Maui.Views;

namespace Inventory.Pages.RangeDay.Graph.GraphOptions;

public partial class GraphOptionsV : Popup
{
    public GraphOptionsV(string[] productNames)
    {
        InitializeComponent();
        var vm = new GraphOptionsVM(productNames);
        vm.Close += CloseAsync;
        BindingContext = vm;

    }

}