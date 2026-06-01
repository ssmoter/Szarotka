using DataBase.Model.EntitiesInventory;

using Shared.Helper;
namespace Inventory.Pages.SingleDay;

public partial class SingleDayV : ContentPage
{
    readonly SingleDayVM _vm;

    public SingleDayV(SingleDayVM vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
        if (DeviceInfo.Platform == DevicePlatform.Android)
            this.CollectionViewCakes.ItemTemplate = (DataTemplate)Resources["Android"];
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            this.CollectionViewCakes.ItemTemplate = (DataTemplate)Resources["WinUI"];
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var context = BindingContext as SingleDayVM;

        if (context is not null)
        {
            Task.Run(async () =>
            {
                await context.ShowCurrentDay();
            });
        }
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        if (BindingContext is SingleDayVM vm)
        {
            vm.RemovePropertyChangedEvent();
        }
    }

    private void Entry_TextChanged_SetValueToSecondPositionEmptyIsZero(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (sender is Entry entry)
            {
                if (string.IsNullOrWhiteSpace(entry.Text))
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
                if (string.IsNullOrWhiteSpace(entry.Text))
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


    private async void Button_Clicked_FastMinusProductNumber(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }
        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        SingleDayVM.FastChangeProductNumber(product, -1, item);
        // _vm.FastMinusProductNumberCommand.Execute(product);
    }
    private async void Button_Clicked_FastAddProductNumber(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }

        await item.BounceOnPressAsync();
        SingleDayVM.FastChangeProductNumber(product, 1, item);
        //_vm.FastAddProductNumberCommand.Execute(product);
    }
    private async void Button_Clicked_FastMinusProductEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }

        await item.BounceOnPressAsync();
        SingleDayVM.FastChangeProductEdit(product, -1, item);
        //_vm.FastMinusProductEditCommand.Execute(product);
    }
    private async void Button_Clicked_FastAddProductEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        SingleDayVM.FastChangeProductEdit(product, 1, item);
        //_vm.FastAddProductEditCommand.Execute(product);
    }
    private async void Button_Clicked_FastMinusProductReturn(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        SingleDayVM.FastChangeProductReturn(product, -1, item);
        //_vm.FastMinusProductReturnCommand.Execute(product);
    }
    private async void Button_Clicked_FastAddProductReturn(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not Product product) { return; }
        await item.BounceOnPressAsync();
        SingleDayVM.FastChangeProductReturn(product, 1, item);
        //_vm.FastAddProductReturnCommand.Execute(product);
    }


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





    private void SwipeItem_Invoked_DeleteCake(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not Cake product) { return; }

        _vm.DeleteCakeCommand.Execute(product);
    }

}