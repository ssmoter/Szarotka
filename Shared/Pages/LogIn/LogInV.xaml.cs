
namespace Shared.Pages.LogIn;

public partial class LogInV : ContentPage
{
    public LogInV(LogInVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        var pages = Shell.Current.Navigation;
        if (pages.NavigationStack.Count <= 2)
        {
            return true;
        }

        return base.OnBackButtonPressed();
    }
}