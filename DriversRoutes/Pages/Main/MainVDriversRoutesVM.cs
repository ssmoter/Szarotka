using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;
using DataBase.Service;

using DriversRoutes.Helper;

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
        }


        public async Task<ObservableCollection<Routes>> GetRoutes()
        {
            try
            {
                var result = await _db.DataBaseAsync.Table<Routes>().ToArrayAsync();
                return new(result);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                return [];
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

            await Shell.Current.GoToAsync(nameof(Pages.Maps.MapAndPoints.MapsV));
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


                await Shell.Current.GoToAsync($"{nameof(Pages.Maps.MapAndPoints.MapsV)}?",
                    new Dictionary<string, object>
                    {
                        // [nameof(MapsM)] = points,
                        [nameof(Routes)] = routes,
                        [nameof(SelectedDayOfWeekRoutes)] = week,
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

        [RelayCommand]
        async Task ChangeName(Routes routes)
        {
            try
            {
                var result = await Shell.Current.DisplayPromptAsync(routes.Name, "Zmień nazwę", "Tak", "Nie", routes.Name, initialValue: routes.Name);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    routes.Name = result;
                    await _db.DataBaseAsync.UpdateAsync(routes);
                    Routes = await GetRoutes();
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
    }
}
