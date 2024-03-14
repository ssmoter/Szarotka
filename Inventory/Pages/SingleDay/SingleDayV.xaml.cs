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
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ProductM;
        if (product == null) { return; }

        _vm.FastMinusProductNumberCommand.Execute(product);
    }
    private void Button_Clicked_FastAddProductNumber(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ProductM;
        if (product == null) { return; }

        _vm.FastAddProductNumberCommand.Execute(product);
    }
    private void Button_Clicked_FastMinusProductEdit(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ProductM;
        if (product == null) { return; }

        _vm.FastMinusProductEditCommand.Execute(product);
    }
    private void Button_Clicked_FastAddProductEdit(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ProductM;
        if (product == null) { return; }

        _vm.FastAddProductEditCommand.Execute(product);
    }
    private void Button_Clicked_FastMinusProductReturn(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ProductM;
        if (product == null) { return; }

        _vm.FastMinusProductReturnCommand.Execute(product);
    }
    private void Button_Clicked_FastAddProductReturn(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        if (item is null) { return; }

        var product = item.BindingContext as ProductM;
        if (product == null) { return; }

        _vm.FastAddProductReturnCommand.Execute(product);
    }


    #endregion
    IVisualTreeElement[] entrys;
    //int lastIndex = 0;
    private void SwipeItem_Invoked_DeleteCake(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null) { return; }

        var product = item.BindingContext as CakeM;
        if (product == null) { return; }

        _vm.DeleteCakeCommand.Execute(product);
    }
    //private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    //{
    //    entrys = ((CollectionView)sender).GetVisualTreeDescendants().Where(x => x is Entry).ToArray();

    //    int index = -1;
    //    for (int i = 0; i < entrys?.Length; i++)
    //    {
    //        if (((Entry)entrys[i]).IsFocused)
    //        {
    //            if (lastIndex >= i)
    //                index = i + 1;
    //            else
    //                index = i - 1;

    //            lastIndex = index;
    //            break;
    //        }
    //    }
    //    if (index >= 0 && index < entrys.Length)
    //    {
    //        ((Entry)entrys[index])?.Focus();
    //    }
    //}
}