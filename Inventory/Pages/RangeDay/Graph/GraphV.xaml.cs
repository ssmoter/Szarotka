using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.Graph;

public partial class GraphV : ContentView, IDisposable
{

    public static readonly BindableProperty RangeDayMsProperty
    = BindableProperty.Create(nameof(RangeDayMs), typeof(RangeDayM[]), typeof(GraphV), propertyChanged: (bindable, oldValu, newValue) =>
    {
        if (bindable is GraphV view)
        {

            if (newValue is RangeDayM[] range)
            {
                view.Vm.RangeDayMs = range;
            }

        }

    });
    public RangeDayM[] RangeDayMs
    {
        get => GetValue(RangeDayMsProperty) as RangeDayM[];
        set => SetValue(RangeDayMsProperty, value);
    }

    public GraphV(GraphVM vm)
    {
        InitializeComponent();
        vm.ReDraw += ReDrawGraph;
        vm.OnReDraw(vm.DrawGraph);
    }

    public GraphVM Vm { get; set; } = new(new());

    public GraphV()
    {
        InitializeComponent();
        Vm.ReDraw += ReDrawGraph;
        Vm.OnReDraw(Vm.DrawGraph);
    }

    public void Dispose()
    {
        if (BindingContext is GraphVM vm)
        {
            vm.ReDraw -= ReDrawGraph;
        }
        GC.SuppressFinalize(this);
    }

    void ReDrawGraph(IDrawable drawable)
    {
        this.GVDrawGraph.Drawable ??= drawable;
        this.GVDrawGraph.Invalidate();
    }

}