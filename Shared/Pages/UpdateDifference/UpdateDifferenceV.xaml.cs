namespace Shared.Pages.UpdateDifference;

public partial class UpdateDifferenceV : ContentPage
{
    public UpdateDifferenceV(UpdateDifferenceVM vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }


    protected override bool OnBackButtonPressed()
    {
        Dispatcher.DispatchAsync(async () =>
        {
            if (BindingContext is UpdateDifferenceVM vm)
            {
                await vm.BackCommand.ExecuteAsync(null);
            }
        });

        //base.OnBackButtonPressed();
        //return base.OnBackButtonPressed();
        return true;
    }

}
