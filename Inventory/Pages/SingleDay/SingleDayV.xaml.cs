using Inventory.Model.MVVM;

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

    #region Product SwipeView RightItems 

    private void Button_Clicked_FastMinusProductNumber(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ProductM product) { return; }

        _vm.FastMinusProductNumberCommand.Execute(product);
    }
    private void Button_Clicked_FastAddProductNumber(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ProductM product) { return; }

        _vm.FastAddProductNumberCommand.Execute(product);
    }
    private void Button_Clicked_FastMinusProductEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ProductM product) { return; }

        _vm.FastMinusProductEditCommand.Execute(product);
    }
    private void Button_Clicked_FastAddProductEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ProductM product) { return; }

        _vm.FastAddProductEditCommand.Execute(product);
    }
    private void Button_Clicked_FastMinusProductReturn(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ProductM product) { return; }

        _vm.FastMinusProductReturnCommand.Execute(product);
    }
    private void Button_Clicked_FastAddProductReturn(object sender, EventArgs e)
    {
        if (sender is not ImageButton item) { return; }

        if (item.BindingContext is not ProductM product) { return; }

        _vm.FastAddProductReturnCommand.Execute(product);
    }


    #endregion
    private void SwipeItem_Invoked_DeleteCake(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not CakeM product) { return; }

        _vm.DeleteCakeCommand.Execute(product);
    }
}