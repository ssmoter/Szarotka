using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Service;

using DriversRoutes.Helper;
using DriversRoutes.Model;
using DriversRoutes.Pages.Maps;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Main
{
    public partial class MainVDriversRoutesVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Routes> routes;

        readonly DataBase.Data.AccessDataBase _db;
        readonly Service.ISelectRoutes selectRoutes;
        public MainVDriversRoutesVM(AccessDataBase db, Service.ISelectRoutes selectRoutes)
        {
            _db = db;
            this.selectRoutes = selectRoutes;
            try
            {
                var result = _db.DataBase.Table<Routes>().ToArray();
                Routes = new(result);
            }
            catch (Exception ex)
            {
                Routes ??= new();
                _db.SaveLog(ex);
            }
        }


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

        [RelayCommand]
        async Task NavigationToMapsSelected(Routes routes)
        {
            try
            {
                var permision = await AndroidPermissionService.LocationWhenInUse();

                if (!permision)
                {
                    return;
                }

                var week = new SelectedDayOfWeekRoutes();
                week.SetTodayDayOfWeek(DateTime.Today);

                var result = await selectRoutes.GetCustomerRoutesQueryAsync(routes, week);
                var points = new ObservableCollection<MapsM>();
                for (int i = 0; i < result.Length; i++)
                {
                    points.Add(result[i].ParseAsCustomerM());
                }

                await Shell.Current.GoToAsync($"{nameof(Pages.Maps.MapsV)}?",
                    new Dictionary<string, object>
                    {
                        [nameof(MapsM)] = points,
                        [nameof(Model.Routes)] = routes,
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async static Task NavigationToListOfPoints(Routes routes)
        {
           // await Shell.Current.GoToAsync(nameof(Pages.ListOfPoints.ListOfPointsV));
            await Shell.Current.GoToAsync($"{nameof(Pages.ListOfPoints.ListOfPointsV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(Routes)] = routes,
                });
        }

    }
}
