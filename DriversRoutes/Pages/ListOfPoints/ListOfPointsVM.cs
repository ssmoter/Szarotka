using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.JsonContext;

using DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

using Shared.Data;
using Shared.Data.File;
using Shared.Pages.ExistingFiles;
using Shared.Service;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.ListOfPoints;

public partial class ListOfPointsVM : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(FilesPath), out object filesPath))
        {
            if (filesPath is string _filesPath)
            {
                FilesPath = _filesPath;
            }
        }
        if (query.TryGetValue(nameof(Routes), out object route))
        {
            if (route is Routes _route)
            {
                Route = _route;
            }
        }
    }
    private Routes route;
    public Routes Route
    {
        get => route;
        set
        {
            if (SetProperty(ref route, value, nameof(Route))) { }
        }
    }

    private ObservableCollection<CustomerRoutes> customerRoutes;
    public ObservableCollection<CustomerRoutes> CustomerRoutes
    {
        get => customerRoutes;
        set
        {
            if (SetProperty(ref customerRoutes, value, nameof(CustomerRoutes))) { }
        }
    }

    private CustomerRoutes locationThisCustomer;
    public CustomerRoutes LocationThisCustomer
    {
        get => locationThisCustomer;
        set
        {
            if (SetProperty(ref locationThisCustomer, value, nameof(LocationThisCustomer))) { }
        }
    }

    private bool showLocationThisCustomer;
    public bool ShowLocationThisCustomer
    {
        get => showLocationThisCustomer;
        set
        {
            if (SetProperty(ref showLocationThisCustomer, value, nameof(ShowLocationThisCustomer))) { }
        }
    }

    private bool customerListRefresh;
    public bool CustomerListRefresh
    {
        get => customerListRefresh;
        set
        {
            if (SetProperty(ref customerListRefresh, value, nameof(CustomerListRefresh))) { }
        }
    }

    private bool saveData;
    public bool SaveData
    {
        get => saveData;
        set
        {
            if (SetProperty(ref saveData, value, nameof(SaveData)))
            {
                OnPropertyChanged(nameof(SaveIsVisible));
                OnPropertyChanged(nameof(RangeIsVisible));
            }
        }
    }

    public bool SaveIsVisible
    {
        get => SaveData;
    }
    public bool RangeIsVisible
    {
        get => !SaveData;
    }

    string filesPath;
    public string FilesPath
    {
        set
        {
            if (value is not null)
            {
                filesPath = value;

                SetSaveData(true);

                var extension = Path.GetExtension(filesPath);
                if (extension == FileHelper.jsonTyp || extension == FileHelper.txtTyp)
                {
                    GetCustomerPointsFromFile();
                }
            }
        }
    }
    private SelectedDayOfWeekRoutes lastSelectedDayOfWeekRoutes;
    private void GetCustomerPointsFromFile()
    {
        try
        {
            CustomerListRefresh = true;

            var result = JsonFile.GetFileJson<CustomerRoutes[]>(filesPath, SzarotkaJsonSerializerContext.Default.CustomerRoutesArray);

            CustomerRoutes?.Clear();
            CustomerRoutes = new ObservableCollection<CustomerRoutes>(result);

            GetRouteFromPoints();
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            CustomerListRefresh = false;
        }
    }

    readonly AccessDataBase _db;
    readonly Service.ISelectRoutes _selectRoutes;
    readonly Service.ISaveRoutes _saveRoutes;

    public Action CalculateRoute;

    public ListOfPointsVM(AccessDataBase db, Service.ISelectRoutes selectRoutes, Service.ISaveRoutes saveRoutes)
    {
        _db = db;
        _selectRoutes = selectRoutes;
        CustomerRoutes ??= [];
        _saveRoutes = saveRoutes;
    }

    #region Method

    public async void GetPointsFireAndForget(Routes routes, SelectedDayOfWeekRoutes week)
    {
        try
        {
            CustomerRoutes = await GetPointsAsync(routes, week);
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }

    public ObservableCollection<CustomerRoutes> GetPoints(Routes routes, SelectedDayOfWeekRoutes week)
    {
        try
        {
            CustomerListRefresh = true;
            lastSelectedDayOfWeekRoutes = week;
            var result = _selectRoutes.GetCustomerRoutesQuery(routes, week);
            return new ObservableCollection<CustomerRoutes>(result);
        }
        catch (Exception)
        {
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
            lastSelectedDayOfWeekRoutes = week;
            var result = await _selectRoutes.GetCustomerRoutesQueryAsync(routes, week);
            return new ObservableCollection<CustomerRoutes>(result);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            CustomerListRefresh = false;
        }
    }

    void GetRouteFromPoints()
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

        var routes = _db.DataBase.Table<Routes>().ToArray();

        for (int i = 0; i < routes.Length; i++)
        {
            var customer = CustomerRoutes.FirstOrDefault();
            if (customer is null)
                break;
            if (customer.RoutesId == routes[i].Id)
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
            _db.SaveLogExtension(ex);
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
            _db.SaveLogExtension(ex);
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
                GetPointsFireAndForget(Route, day);
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }

    [RelayCommand]
    void Refresh()
    {
        if (RangeIsVisible)
        {
            if (lastSelectedDayOfWeekRoutes is not null)
            {
                // GetPointsFireAndForget(Route, lastSelectedDayOfWeekRoutes);
            }
        }
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
            await Toast.Make("Trwa zapisywanie zmian", ToastDuration.Long).Show();

            Task[] task = new Task[CustomerRoutes.Count];
            for (int i = 0; i < CustomerRoutes.Count; i++)
            {
                task[i] = _saveRoutes.SaveCustomer(CustomerRoutes[i], CustomerRoutes[i].RoutesId.ToByteArray());
            }
            await Task.WhenAll(task);
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            await Toast.Make("Zapisywanie zmiań zakończone", ToastDuration.Long).Show();
            SetSaveData(false);
        }
    }

    public void SetSaveData(bool value)
    {
        SaveData = value;
        OnPropertyChanged(nameof(SaveIsVisible));
        OnPropertyChanged(nameof(RangeIsVisible));
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
            var response = await JsonFile.SaveFileJson(allCustomers.ToArray(), SzarotkaJsonSerializerContext.Default.CustomerRoutesArray, name, FileHelper.DriversRoutes);
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = name,
                File = new ShareFile(response)
            });

        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
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
            _db.SaveLogExtension(ex);
        }
    }


    [RelayCommand]
    async Task MoveTimeOnPoints(SelectedDayOfWeekRoutes selectDayMs)
    {
        try
        {
            var result = await MoveTimeOnCustomersV.ShowPopUp(Route, selectDayMs, _selectRoutes, _saveRoutes);
            if (result)
            {
                Refresh();
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }

    [RelayCommand]
    async Task DiscardFromFile()
    {
        Refresh();
        SetSaveData(false);
        await Toast.Make("Wczytany plik usunięto").Show();
    }

    [RelayCommand]
    void NavigationToRoutes(CustomerRoutes customer)
    {
        try
        {
            if (customer is null)
            {
                return;
            }
            ShowLocationThisCustomer = true;
            LocationThisCustomer = customer;
            CalculateRoute?.Invoke();
            //if (customer is null)
            //{
            //    return;
            //}

            //await Shell.Current.GoToAsync($"{nameof(Pages.Maps.Navigate.NavigateV)}?",
            //    new Dictionary<string, object>()
            //    {
            //        [nameof(ObservableCollection<CustomerRoutes>)] = CustomerRoutes,
            //        [nameof(DataBase.Model.EntitiesRoutes.CustomerRoutes)] = customer
            //    });
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }
    #endregion

}

