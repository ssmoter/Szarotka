using Shared.Helper;

namespace Shared.Pages.UserDisplay;

public partial class UserDisplayV : ContentPage
{
    public UserDisplayV(UserDisplayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}