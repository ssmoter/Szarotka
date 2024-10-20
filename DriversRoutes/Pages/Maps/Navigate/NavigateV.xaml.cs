namespace DriversRoutes.Pages.Maps.Navigate;

public partial class NavigateV : ContentPage
{
    public NavigateV(NavigateVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is NavigateVM vm)
        {
            if (sender is RadioButton radio)
            {
                var value = radio.Value;
                vm.ChangeTypeOfMapCommand.Execute(value);
            }
        }
    }
}