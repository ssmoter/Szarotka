using CommunityToolkit.Maui.Alerts;

namespace Shared.Pages.LogIn;

public partial class LogInV : ContentPage
{
    public LogInV(LogInVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}