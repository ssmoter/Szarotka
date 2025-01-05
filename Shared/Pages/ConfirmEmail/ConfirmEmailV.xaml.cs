namespace Shared.Pages.ConfirmEmail;

public partial class ConfirmEmailV : ContentPage
{
    public ConfirmEmailV(ConfirmEmailVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ConfirmEmailVM vm)
        {
            vm.ConfirmEmailM.Code = null;
            vm.ConfirmEmailM.Error = "";
        }
    }
}