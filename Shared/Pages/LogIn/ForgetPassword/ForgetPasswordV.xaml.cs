namespace Shared.Pages.LogIn.ForgetPassword;

public partial class ForgetPasswordV : ContentPage
{

    public ForgetPasswordV(ForgetPasswordVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is not ForgetPasswordVM vm) { return; }
        vm.Model.IsSendCodeEnable = true;
        vm.Model.IsEmailEnable = true;
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ForgetPasswordVM vm)
        {
            vm.IsPasswordEqualsCommand.Execute(null);
        }
    }
}