using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesInventory;

using Shared.Helper;

namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewSmall;

public partial class SingleDayPreviewSmallV : ContentView, IDisposable
{

    public static readonly BindableProperty DayProperty
    = BindableProperty.Create(nameof(Day), typeof(Day), typeof(SingleDayPreviewSmallV), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
    {
        if (bindable is SingleDayPreviewSmallV view)
        {
            if (newValue is Day day)
            {
                SetProperties(view, day);
            }
        }
    });

    private static void SetProperties(SingleDayPreviewSmallV view, Day day)
    {
        if (day is not null)
        {
            view.SingleDayPreviewSmallM.Products = [.. day.Products];
            if (day.Cakes.Count > 0)
            {
                view.SingleDayPreviewSmallM.Cakes = [.. day.Cakes.OrderByDescending(x => x.IsSell).ThenBy(x => x.Index).Select(x => new CakeIsExpanded(x))];
                view.SingleDayPreviewSmallM.CountSellCakes = day.Cakes.Count(x => x.IsSell);
                view.SingleDayPreviewSmallM.CountReturnCakes = day.Cakes.Count - view.SingleDayPreviewSmallM.CountSellCakes;
            }
        }
    }

    public Day Day
    {
        get => (Day)GetValue(DayProperty);
        set => SetValue(DayProperty, value);
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

    private static Action _navigationTo;
    private void NavigationTo()
    {
        SetProperties(this, Day);
    }
    public static void OnNavigationTo() => _navigationTo?.Invoke();

    public SingleDayPreviewSmallV()
    {
        InitializeComponent();
        TapGestureRecognizer_Tapped_SelectedTypePrice(null, null);
        _navigationTo += NavigationTo;

    }
    public void Dispose()
    {
        _navigationTo -= NavigationTo;
    }
    protected override void OnParentSet()
    {
        base.OnParentSet();
        if (Parent != null)
        {
            SingleDayPreviewSmallM.Parent = this.Parent as View;
        }
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

            SingleDayPreviewSmallM.Products = [.. search];
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
            SetProperties(this, Day);
        }
        finally
        {
            SingleDayPreviewSmallM.CakesIsRefreshing = false;
        }
    }

    private async void TapGestureRecognizer_Tapped_UserEdit_Popup(object sender, TappedEventArgs e)
    {
        if (sender is not Label item) { return; }
        await item.BounceOnPressAsync();

        var userId = Day.DriverGuid;
        var popup = new Shared.Pages.UserDisplay.PopupUser.UserDisplayVPopup(userId);
        await Shell.Current.ShowPopupAsync(popup);
    }

    private Product lastProductHideElseExpanded = new();
    private void TapGestureRecognizer_Tapped_Product(object sender, TappedEventArgs e)
    {
        if (sender is not Grid grid) { return; }
        if (grid.BindingContext is not Product product) { return; }

        if (product.IsExpanded)
        {
            product.IsExpanded = false;
            return;
        }

        product.IsExpanded = true;
        if (lastProductHideElseExpanded.ProductNameId == product.ProductNameId)
        {
            return;
        }
        lastProductHideElseExpanded.IsExpanded = false;
        lastProductHideElseExpanded = product;
    }
    private CakeIsExpanded lastCakeHideElseExpanded = new();
    private void TapGestureRecognizer_Tapped_CakeIsExpanded(object sender, TappedEventArgs e)
    {
        if (sender is not Grid grid) { return; }
        if (grid.BindingContext is not CakeIsExpanded cake) { return; }

        if (cake.IsExpanded)
        {
            cake.IsExpanded = false;
            return;
        }

        cake.IsExpanded = true;
        if (lastCakeHideElseExpanded.Id == cake.Id)
        {
            return;
        }
        lastCakeHideElseExpanded.IsExpanded = false;
        lastCakeHideElseExpanded = cake;
    }


}