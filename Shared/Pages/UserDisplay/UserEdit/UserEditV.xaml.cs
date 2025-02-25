namespace Shared.Pages.UserDisplay.UserEdit;

public partial class UserEditV : ContentPage
{
    public UserEditV(UserEditVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (BindingContext is UserEditVM vm)
        {
            vm.MaxMyWidth = width;
        }
    }

}