using Inventory.Pages.SingleDayPreview.SingleDayPreviewSmall;

namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewPage;

public partial class SingleDayPreviewPageV : ContentPage
{
    public SingleDayPreviewPageV(SingleDayPreviewPageVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        SingleDayPreviewSmallV.OnNavigationTo();
    }

}