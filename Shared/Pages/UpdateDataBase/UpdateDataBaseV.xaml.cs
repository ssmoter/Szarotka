namespace Shared.Pages.UpdateDataBase;

public partial class UpdateDataBaseV : ContentPage
{
    public UpdateDataBaseV(UpdateDataBaseVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is UpdateDataBaseVM vm)
        {
            await vm.Update();
        }
    }





}