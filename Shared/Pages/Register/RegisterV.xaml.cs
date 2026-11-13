namespace Shared.Pages.Register;

public partial class RegisterV : ContentPage
{
    public RegisterV(RegisterVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is RegisterVM vm)
        {
            vm.GetOldDrivers();
        }
    }

    private void Entry_TextChanged_Password(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is RegisterVM vm)
        {
            vm.IsPasswordEqual();
            vm.Validation();
        }
    }

    private void EntryName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is RegisterVM vm)
        {
            vm.Validation();
        }
    }

    private void EntryEmail_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is RegisterVM vm)
        {
            vm.Validation();
        }
    }
}