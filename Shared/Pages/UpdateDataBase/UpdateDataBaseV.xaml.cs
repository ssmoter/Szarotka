using CommunityToolkit.Maui.Alerts;

namespace Shared.Pages.UpdateDataBase;

public partial class UpdateDataBaseV : ContentPage
{
    public UpdateDataBaseV(UpdateDataBaseVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (BindingContext is UpdateDataBaseVM vm)
            {
                await vm.UpdateAsync();
            }
        }
        catch (Exception ex)
        {
            await Dispatcher.DispatchAsync(async () =>
             {
                 await Snackbar.Make(ex.Message, null, "Ok", TimeSpan.FromMinutes(1)).Show();
             });
        }

    }
}