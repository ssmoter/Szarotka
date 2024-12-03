using Shared.Helper;
using DataBase.Model.EntitiesInventory;
namespace Inventory.Pages.SingleDay;

public partial class SingleDayV : ContentPage, IDisposable
{
    readonly SingleDayVM _vm;

    public SingleDayV(SingleDayVM vm)
    {
        InitializeComponent();
        _vm = vm;
        vm.ProductScrollToObject += CVProducts.ScrollTo;
        vm.ProductScrollToInt += CVProducts.ScrollTo;
        BindingContext = vm;
    }
    public void Dispose()
    {
        if (BindingContext is SingleDayVM vm)
        {
            vm.ProductScrollToObject -= CVProducts.ScrollTo;
            vm.ProductScrollToInt -= CVProducts.ScrollTo;

        }
        GC.SuppressFinalize(this);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var context = BindingContext as SingleDayVM;

        //for (int i = 0; i < 15; i++)
        //{
        //    context.Day.Cakes.Add(new Cake() { Price = i * 100, Index = i, IsSell = true, });
        //}

        if (context is not null)
        {
            Task.Run(async () =>
            {
                await context.ShowCurrentDay();
            });
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (BindingContext is SingleDayVM vm)
        {
            vm.BackCommand.Execute(null);
        }
        return true;
        //return base.OnBackButtonPressed();
    }

    private void Entry_TextChanged_SetValueToSecondPositionEmptyIsZero(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (sender is Entry entry)
            {
                if (entry.Text is null)
                {
                    return;
                }
                if (entry.Text.Length > 0 && entry.Text.Length <= 1)
                {
                    entry.CursorPosition = 1;
                }

                if (!string.IsNullOrWhiteSpace(e.OldTextValue))
                {
                    if (e.OldTextValue.Contains('.'))
                    {
                        entry.Text = entry.Text.Replace('.', ',');
                        entry.CursorPosition = entry.Text.Length;
                    }
                }

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    entry.Text = "0";
                }
            }
        }
        catch (Exception)
        { }
    }
    private void Entry_TextChanged_SetValueToSecondPosition(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (sender is Entry entry)
            {
                if (entry.Text is null)
                {
                    return;
                }

                if (entry.Text.Length > 0 && entry.Text.Length <= 1)
                {
                    entry.CursorPosition = 1;
                }

                if (!string.IsNullOrWhiteSpace(e.OldTextValue))
                {
                    if (e.OldTextValue.Contains('.'))
                    {
                        entry.Text = entry.Text.Replace('.', ',');
                        entry.CursorPosition = entry.Text.Length;
                    }
                }
            }
        }
        catch (Exception)
        { }

    }
    private void Button_Clicked_AddProduct(object sender, EventArgs e)
    {
        if (sender is not ImageButton) { return; }

        _vm.AddProductCommand.Execute(null);
    }

    #region Product SwipeView RightItems 

    private async void Button_Clicked_FastMinusProductNumber(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        _vm.FastMinusProductNumberCommand.Execute(product);
    }
    private async void Button_Clicked_FastAddProductNumber(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }

        await item.BounceOnPressAsync();
        _vm.FastAddProductNumberCommand.Execute(product);
    }
    private async void Button_Clicked_FastMinusProductEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }

        await item.BounceOnPressAsync();
        _vm.FastMinusProductEditCommand.Execute(product);
    }
    private async void Button_Clicked_FastAddProductEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        _vm.FastAddProductEditCommand.Execute(product);
    }
    private async void Button_Clicked_FastMinusProductReturn(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        _vm.FastMinusProductReturnCommand.Execute(product);
    }
    private async void Button_Clicked_FastAddProductReturn(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        _vm.FastAddProductReturnCommand.Execute(product);
    }

    #region left

    private async void Button_Clicked_ChangeProductPrice(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        _vm.ChangeProductPriceCommand.Execute(product);
    }
    private async void Button_Clicked_DeleteSelectedProduct(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        _vm.DeleteSelectedProductCommand.Execute(product);
    }

    #endregion
    #endregion


    private void SwipeItem_Invoked_DeleteCake(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not Cake product) { return; }

        _vm.DeleteCakeCommand.Execute(product);
    }

}