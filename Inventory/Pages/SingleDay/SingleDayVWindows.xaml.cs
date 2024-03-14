namespace Inventory.Pages.SingleDay;

public partial class SingleDayVWindows : ContentPage
{
    public SingleDayVWindows(SingleDayVM vm)
    {
        InitializeComponent();
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
    IVisualTreeElement[] entrys;
    int lastIndex = 1;
    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        entrys ??= ((CollectionView)sender).GetVisualTreeDescendants().Where(x => x is Entry).ToArray();

        int index = -1;
        for (int i = 0; i < entrys?.Length; i++)
        {
            if (((Entry)entrys[i]).IsFocused)
            {
                if (lastIndex > i)
                    index = i + 1;
                else
                    index = i - 1;

                lastIndex = index;
                break;
            }
        }
        if (index >= 0 && index <= entrys.Length)
        {
            ((Entry)entrys[index])?.Focus();
        }
    }
}