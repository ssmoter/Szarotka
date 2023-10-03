using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DriversRoutes.Pages.Main
{
    public partial class MainVDriversRoutesVM : ObservableObject
    {


        [RelayCommand]
        async static Task NavigationToMaps()
        {


            await Shell.Current.GoToAsync(nameof(Pages.Maps.MapsV));
        }



    }
}
