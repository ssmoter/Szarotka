using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Service;

namespace DriversRoutes.Pages.Main
{
    public partial class MainVDriversRoutesVM : ObservableObject
    {


        [RelayCommand]
        async static Task NavigationToMaps()
        {
            var permision = await AndroidPermissionService.LocationWhenInUse();

            if (!permision)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(Pages.Maps.MapsV));
        }



    }
}
