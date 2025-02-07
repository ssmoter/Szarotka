using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;

using Shared.Data;
using Shared.Service;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Main
{
    public partial class MainVDriversRoutesVM : ObservableObject
    {
        private ObservableCollection<Routes> routes;
        public ObservableCollection<Routes> Routes
        {
            get => routes;
            set
            {
                if (SetProperty(ref routes, value, nameof(Routes))) { }
            }
        }

        readonly IAccessDataBase _db;
        readonly Service.ISelectRoutes selectRoutes;
        public MainVDriversRoutesVM(IAccessDataBase db, Service.ISelectRoutes selectRoutes)
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
                _db.SaveLogExtension(ex);
                return [];
            }
        }

        [RelayCommand]
        async static Task NavigationToMaps()
        {
            var permission = await AndroidPermissionService.LocationWhenInUse();

            if (!permission)
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
                var permission = await AndroidPermissionService.LocationWhenInUse();

                if (!permission)
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
                _db.SaveLogExtension(ex);
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
                    routes.Updated = DateTime.UtcNow;
                    await _db.DataBaseAsync.UpdateAsync(routes);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }
    }
}
