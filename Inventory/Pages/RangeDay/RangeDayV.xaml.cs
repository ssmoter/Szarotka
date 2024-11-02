using Inventory.Pages.RangeDay.Table;

using Microsoft.AspNetCore.Components.WebView.Maui;

namespace Inventory.Pages.RangeDay;

public partial class RangeDayV : ContentPage
{
    readonly RangeDayVM _vm;
    public RangeDayV(RangeDayVM vm)
    {
        InitializeComponent();
        this._vm = vm;
        BindingContext = vm;
    }
    BlazorWebView _rangeTable;
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _rangeTable = new Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView()
        {
            HostPage = "wwwroot/index.html",
        };
        _rangeTable.RootComponents.Add(new RootComponent() { ComponentType = typeof(RangeTable), Selector = "#app" });

        this.TableGrid.Add(_rangeTable);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        TableGrid.Remove(_rangeTable);
    }

    private void SwipeItem_Invoked_OpenDay(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not RangeDayM product) { return; }

        _vm.OpenDetailPageCommand.Execute(product);
    }
}