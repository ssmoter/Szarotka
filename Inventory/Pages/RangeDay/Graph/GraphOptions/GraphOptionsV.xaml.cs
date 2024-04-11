using CommunityToolkit.Maui.Views;

namespace Inventory.Pages.RangeDay.Graph.GraphOptions;

public partial class GraphOptionsV : Popup, IDisposable
{
    public GraphOptionsV(string[] productNames)
    {
        InitializeComponent();
        var vm = new GraphOptionsVM(productNames);
        vm.Close += CloseAsync;
        BindingContext = vm;

    }

    public void Dispose()
    {
        if (BindingContext is GraphOptionsVM vm)
        {
            vm.Close -= CloseAsync;
        }
    }
}