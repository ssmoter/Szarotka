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

        mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

        mainGrid.MaximumHeightRequest = 490;
#if WINDOWS
        mainGrid.MaximumWidthRequest = 300;
#endif
        if (productNames.Length > 0)
        {
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

#if WINDOWS
            mainGrid.MaximumWidthRequest = 600;
#endif
        }

    }

    public void Dispose()
    {
        if (BindingContext is GraphOptionsVM vm)
        {
            vm.Close -= CloseAsync;
        }
        GC.SuppressFinalize(this);
    }
}