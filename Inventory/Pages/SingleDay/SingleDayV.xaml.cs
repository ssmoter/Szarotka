using DataBase.Helper;
using DataBase.Model.EntitiesInventory;
namespace Inventory.Pages.SingleDay;

public partial class SingleDayV : ContentPage
{
    readonly SingleDayVM _vm;

    public SingleDayV(SingleDayVM vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
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

    private void Entry_TextChanged_SetValueToSecendPositionEmptyIsZero(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
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
    private void Entry_TextChanged_SetValueToSecendPosition(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
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

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        var myElement = this.FindByName<Label>("CVGCakes");
        if (myElement != null)
        {
            // Wykonaj operacje na elemencie
            if (e.Value) // Jeśli przełącznik jest włączony
            {
                VisualStateManager.GoToState(myElement, "Visible"); // Pokaż element
            }
            else // Jeśli przełącznik jest wyłączony
            {
                VisualStateManager.GoToState(myElement, "Hidden"); // Ukryj element
            }
        }
    }
}