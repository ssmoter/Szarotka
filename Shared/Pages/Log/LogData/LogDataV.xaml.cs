namespace Shared.Pages.Log.LogData;

public partial class LogDataV : ContentPage
{
    public LogDataV(LogDataVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}