namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewPage;

public partial class SingleDayPreviewPageV : ContentPage
{
    public SingleDayPreviewPageV(SingleDayPreviewPageVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}