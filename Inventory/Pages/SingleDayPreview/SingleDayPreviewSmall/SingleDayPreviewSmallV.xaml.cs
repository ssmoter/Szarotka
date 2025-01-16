using CommunityToolkit.Maui.Core.Platform;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewSmall;

public partial class SingleDayPreviewSmallV : ContentView
{

    public static readonly BindableProperty DayProperty
    = BindableProperty.Create(nameof(Day), typeof(Day), typeof(SingleDayPreviewSmallV), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
    {
        if (bindable is SingleDayPreviewSmallV view)
        {
            if (newValue is Day day)
            {
                view.SingleDayPreviewSmallM.Products = day.Products;
                day.Cakes = new(day.Cakes.OrderByDescending(x => x.IsSell));
                view.SingleDayPreviewSmallM.CountSellCakes = day.Cakes.Count(x => x.IsSell);
                view.SingleDayPreviewSmallM.CountReturnCakes = day.Cakes.Count - view.SingleDayPreviewSmallM.CountSellCakes;
            }
        }
    });
    public Day Day
    {
        get => (Day)GetValue(DayProperty);
        set => SetValue(DayProperty, value);
    }

    public static readonly BindableProperty DriverProperty
        = BindableProperty.Create(nameof(Driver), typeof(string), typeof(SingleDayPreviewSmallV), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is SingleDayPreviewSmallV view)
            {
            }
        });
    public string Driver
    {
        get => (string)GetValue(DriverProperty);
        set => SetValue(DriverProperty, value);
    }


    private SingleDayPreviewSmallM singleDayPreviewSmallM = new();
    public SingleDayPreviewSmallM SingleDayPreviewSmallM
    {
        get => (singleDayPreviewSmallM);
        set
        {
            if (singleDayPreviewSmallM != value)
            {
                singleDayPreviewSmallM = value;
                OnPropertyChanged(nameof(SingleDayPreviewSmallM));
                OnPropertyChanging(nameof(SingleDayPreviewSmallM));
            }
        }
    }

    public SingleDayPreviewSmallV()
    {
        InitializeComponent();
        TapGestureRecognizer_Tapped_SelectedTypePrice(null, null);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        SingleDayPreviewSmallM.Parent = this.Parent as View;
    }


    private void TapGestureRecognizer_Tapped_SelectedTypePrice(object sender, TappedEventArgs e)
    {
        SingleDayPreviewSmallM.Price = true;
        SingleDayPreviewSmallM.Product = false;
        SingleDayPreviewSmallM.Cake = false;
    }
    private void TapGestureRecognizer_Tapped_SelectedTypeProduct(object sender, TappedEventArgs e)
    {
        SingleDayPreviewSmallM.Product = true;
        SingleDayPreviewSmallM.Price = false;
        SingleDayPreviewSmallM.Cake = false;
    }
    private void TapGestureRecognizer_Tapped_SelectedTypeCake(object sender, TappedEventArgs e)
    {
        SingleDayPreviewSmallM.Cake = true;
        SingleDayPreviewSmallM.Price = false;
        SingleDayPreviewSmallM.Product = false;
    }

    private void SearchBar_TextChanged_Products(object sender, TextChangedEventArgs e)
    {
        if (Day.Products.Count == 0)
        {
            return;
        }

        if (e.NewTextValue.Length > 2)
        {
            var search = Day.Products.Where(x => x.Name.Name.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase));
            SingleDayPreviewSmallM.Products = new(search);
        }
        else
        {
            SingleDayPreviewSmallM.Products = Day.Products;
        }
    }
    private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        if (sender is SearchBar bar)
        {
#if ANDROID
            await bar.HideKeyboardAsync();
#else
            await Task.Delay(TimeSpan.FromMilliseconds(1));
#endif
        }
    }
    private void RefreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            SingleDayPreviewSmallM.CakesIsRefreshing = true;
            SingleDayPreviewSmallM.Products = Day.Products;
            Day.Cakes = new(Day.Cakes.OrderByDescending(x => x.IsSell));
            SingleDayPreviewSmallM.CountSellCakes = Day.Cakes.Count(x => x.IsSell);
            SingleDayPreviewSmallM.CountReturnCakes = Day.Cakes.Count - SingleDayPreviewSmallM.CountSellCakes;
        }
        finally
        {
            SingleDayPreviewSmallM.CakesIsRefreshing = false;
        }
    }
}