namespace Inventory.Pages.RangeDay.Graph;

public partial class GraphV : ContentPage
{
    public GraphV(GraphVM vm)
    {
        InitializeComponent();
        vm.ReDraw += ReDrawGraph;
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is GraphVM vm)
        {
            if (vm.RangeDayMs is not null)
            {
                vm.RangeDayMs = vm.RangeDayMs.Reverse().ToArray();
            }
            vm.OnReDraw(vm.DrawGraph);
        }
    }

    void ReDrawGraph(IDrawable drawable)
    {
        this.GVDrawGraph.Drawable ??= drawable;
        this.GVDrawGraph.Invalidate();
    }

}