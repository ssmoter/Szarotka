using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.File;
using DataBase.Model.EntitiesRoutes;
using DataBase.Pages.ExistingFiles;
using DataBase.Service;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.ListOfPoints
{
    [QueryProperty(nameof(Route), nameof(Routes))]
    [QueryProperty(nameof(FilesPath), nameof(FilesPath))]
    public partial class ListOfPointsVM : ObservableObject
    {
        [ObservableProperty]
        Routes route;

        [ObservableProperty]
        ObservableCollection<CustomerRoutes> customerRoutes;

        [ObservableProperty]
        CustomerRoutes locationThisCustomer;

        [ObservableProperty]
        bool showLocationThisCustomer;

        [ObservableProperty]
        bool customerListRefresh;

        bool saveData;

        public bool SaveIsVisible
        {
            get => saveData;
        }
        public bool RangeIsVisible
        {
            get => !saveData;
        }

        string filesPath;
        public string FilesPath
        {
            set
            {
                if (value is not null)
                {
                    filesPath = value;

                    saveData = true;
                    OnPropertyChanged(nameof(SaveIsVisible));
                    OnPropertyChanged(nameof(RangeIsVisible));

                    var extension = Path.GetExtension(filesPath);
                    if (extension == FileHelper.jsonTyp)
                    {
                        Task.Run(async () => { await GetCustomerPointsFromFile(); });
                    }
                }
            }
        }

        private async Task GetCustomerPointsFromFile()
        {
            try
            {
                CustomerListRefresh = true;

                var result = await JsonFile.GetFileJson<CustomerRoutes[]>(filesPath);

                CustomerRoutes?.Clear();
                CustomerRoutes = new ObservableCollection<CustomerRoutes>(result);

                await GetRouteFromPoints();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
            finally
            {
                CustomerListRefresh = false;
            }
        }

        readonly AccessDataBase _db;
        readonly Service.ISelectRoutes _selectRoutes;
        readonly Service.ISaveRoutes _saveRoutes;
        public ListOfPointsVM(AccessDataBase db, Service.ISelectRoutes selectRoutes, Service.ISaveRoutes saveRoutes)
        {
            _db = db;
            _selectRoutes = selectRoutes;
            CustomerRoutes ??= [];
            _saveRoutes = saveRoutes;
        }

        #region Method
        public ObservableCollection<CustomerRoutes> GetPoints(Routes routes, SelectedDayOfWeekRoutes week)
        {
            try
            {
                CustomerListRefresh = true;
                var result = _selectRoutes.GetCustomerRoutesQuery(routes, week);
                return new ObservableCollection<CustomerRoutes>(result);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
            finally
            {
                CustomerListRefresh = false;
            }

        }
        public async Task<ObservableCollection<CustomerRoutes>> GetPointsAsync(Routes routes, SelectedDayOfWeekRoutes week)
        {
            try
            {
                CustomerListRefresh = true;
                var result = await _selectRoutes.GetCustomerRoutesQueryAsync(routes, week);
                return new ObservableCollection<CustomerRoutes>(result);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
            finally
            {
                CustomerListRefresh = false;
            }
        }

        async Task GetRouteFromPoints()
        {
            if (Route is not null)
            {
                return;
            }
            if (CustomerRoutes is null)
            {
                return;
            }

            if (CustomerRoutes.Count < 1)
            {
                return;
            }

            var routes = await _db.DataBaseAsync.Table<Routes>().ToArrayAsync();

            for (int i = 0; i < routes.Length; i++)
            {
                if (CustomerRoutes.FirstOrDefault().RoutesId == routes[i].Id)
                {
                    Route = routes[i];
                    break;
                }
            }
        }

        #endregion

        #region Command

        [RelayCommand]
        void LocationOfPin(CustomerRoutes point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }
                ShowLocationThisCustomer = true;
                LocationThisCustomer = point;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task DisplayPin(CustomerRoutes point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }

                await Shell.Current.GoToAsync($"{nameof(Pages.Customer.DisplayCustomer.DisplayCustomerV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(CustomerRoutes)] = new CustomerRoutes()
                        {
                            Id = new Guid(point.Id.ToByteArray()),
                            RoutesId = new Guid(point.RoutesId.ToByteArray()),
                            QueueNumber = point.QueueNumber,
                            Name = point.Name,
                            Description = point.Description,
                            PhoneNumber = point.PhoneNumber,
                            Created = point.Created,
                            DayOfWeek = point.DayOfWeek,
                            ResidentialAddress = point.ResidentialAddress,
                            Longitude = point.Longitude,
                            Latitude = point.Latitude,
                        },
                    });

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task SelectDaysOfWeek()
        {
            try
            {
                var popup = new Popups.SelectDay.SelectDayV();
                var response = await Shell.Current.ShowPopupAsync(popup);
                if (response is null)
                {
                    return;
                }
                if (response is SelectedDayOfWeekRoutes day)
                {
                    //CustomerRoutes.Clear();
                    CustomerRoutes = await GetPointsAsync(Route, day);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        void Refresh()
        {
            CustomerListRefresh = false;
        }

        [RelayCommand]
        async Task SaveFromFile()
        {
            try
            {
                var result = await Shell.Current.DisplayAlert("Zapisywanie", "Czy chcesz zapisać lub zaktualizować wczytane punkty", "Tak", "Nie");
                if (!result)
                    return;

                for (int i = 0; i < CustomerRoutes.Count; i++)
                {
                    await _saveRoutes.SaveCustomer(CustomerRoutes[i], CustomerRoutes[i].RoutesId.ToByteArray());
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task GenerateJsonFile()
        {
            try
            {
#if ANDROID
                if (!await AndroidPermissionService.CheckAllPermissionsAboutStorage())
                {
                    return;
                }
#endif
                var allCustomers = await GetPointsAsync(Route, new SelectedDayOfWeekRoutes());

                var name = $"{Route.Name}_{DateTime.Today.ToShortDateString()}";
                var response = await JsonFile.SaveFileJson(allCustomers.ToArray(), name, FileHelper.DriversRoutes);
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = name,
                    File = new ShareFile(response)
                });

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task GetFiles()
        {
            try
            {
#if ANDROID
                if (!await AndroidPermissionService.CheckAllPermissionsAboutStorage())
                {
                    return;
                }
#endif
                var files = FileHelper.GetFilesPaths(FileHelper.DriversRoutes);
                await Shell.Current.GoToAsync($"{nameof(ExistingFilesV)}?GetTyp={FileHelper.DriversRoutes}",
                    new Dictionary<string, object>
                    {
                        [nameof(ExistingFilesM)] = ExistingFilesVM.GetExistingFiles(files)
                        ,
                        ["ReturnPage"] = nameof(ListOfPointsV)
                    }); ;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        #endregion

    }
}
