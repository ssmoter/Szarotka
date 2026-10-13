namespace Inventory.Pages.RangeDay;

public partial class RangeDayV : ContentPage
{
    readonly RangeDayVM _vm;
    public RangeDayV(RangeDayVM vm)
    {
        InitializeComponent();
        this._vm = vm;
        BindingContext = vm;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }

    private void SwipeItem_Invoked_OpenDay(object sender, EventArgs e)
    {
        if (sender is not SwipeItem item) { return; }

        if (item.BindingContext is not RangeDayM product) { return; }

        _vm.OpenDetailPageCommand.Execute(product);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Task.Run(async () =>
        {
            if (BindingContext is RangeDayVM vm)
            {
                if (string.IsNullOrWhiteSpace(vm.FilesPath))
                {
                    await vm.SetDefaultsHeaders();
                    var month = PopupSelectRangeDate.PopupSelectRangeDateVM.SelectMonth(DateTime.Today.Month);
                    await vm.SelectData(new(month.Item1, month.Item2, []));
                }
            }
        });
    }
}