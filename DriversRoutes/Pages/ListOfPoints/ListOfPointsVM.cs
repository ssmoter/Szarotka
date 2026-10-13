using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Model;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;
using DataBase.Model.SourceGenerator;

using DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

using Shared.CustomControls.FromCode;
using Shared.Data;
using Shared.Data.File;
using Shared.Data.ServerHttpClients;
using Shared.Helper;
using Shared.Pages.ExistingFiles;
using Shared.Pages.UpdateDifference;
using Shared.Service;

using System.Collections;
using System.Collections.ObjectModel;
using System.Net;

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
            }
        }
    }

    string filesPath;
    public string FilesPath
    {
        set
        {
            if (value is not null)
            {
                filesPath = value;

                SaveData = true;

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

    readonly IAccessDataBaseAoT _db;

    private readonly DataBase.Data.Get.IGetDriverRoutesAoT _get;
    private readonly DataBase.Data.Save.ISaveDriverRoutesAoT _save;
    private readonly DataBase.Service.IUpdateLogService _updateLogService;
    private readonly Data.RouteApi.IGetCustomersHttp _getHttp;
    private readonly Data.RouteApi.ISendCustomersHttp _sendHttp;
    private readonly Shared.Data.ServerHttpClients.IUpdateLogsHttp _updateLogsHttp;
    private readonly IPopupService _popupService;

    public Action CalculateRoute;

    public ListOfPointsVM(IAccessDataBaseAoT db,
                          DataBase.Data.Get.IGetDriverRoutesAoT get,
                          DataBase.Data.Save.ISaveDriverRoutesAoT save,
                          DataBase.Service.IUpdateLogService update,
                          Data.RouteApi.IGetCustomersHttp getCustomersHttp,
                          Data.RouteApi.ISendCustomersHttp sendHttp,
                          Shared.Data.ServerHttpClients.IUpdateLogsHttp updateLogsHttp,
                          IPopupService popupService)
    {
        _db = db;

        CustomerRoutes ??= [];
        _get = get;
        _save = save;
        _updateLogService = update;
        this._getHttp = getCustomersHttp;
        _sendHttp = sendHttp;
        _updateLogsHttp = updateLogsHttp;
        _popupService = popupService;
    }


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

    private async Task<ObservableCollection<CustomerRoutes>> GetPointsAsync(Routes routes, SelectedDayOfWeekRoutes week)
    {
        try
        {
            CustomerListRefresh = true;
            lastSelectedDayOfWeekRoutes = week;
            var result = await _get.CustomerRoutes(routes.Id, week.GetDayOfWeeks());
            result = [.. result.SortByDays(week.GetDayOfWeeks())];

            int number = 0;
            foreach (var item in result)
            {
                number++;
                item.QueueNumber = number;
            }

            return [.. result];
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

    async void GetRouteFromPoints()
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

        var routes = await _get.Routes();

        for (int i = 0; i < routes.Count; i++)
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
                    [nameof(CustomerRoutes)] = point
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

            IPopupResult<SelectedDayOfWeekRoutes> response = await _popupService.ShowPopupAsync<Popups.SelectDay.SelectDayVM, SelectedDayOfWeekRoutes>(Shell.Current);

            if (response is null)
            {
                return;
            }

            if (response.Result is SelectedDayOfWeekRoutes day)
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
        if (saveData)
        {
            if (lastSelectedDayOfWeekRoutes is not null)
            {
                GetPointsFireAndForget(Route, lastSelectedDayOfWeekRoutes);
            }
        }
        CustomerListRefresh = false;
    }

    [RelayCommand]
    async Task SaveFromFile()
    {
        try
        {
            var result = await Shell.Current.DisplayAlertAsync("Zapisywanie", "Czy chcesz zapisać lub zaktualizować wczytane punkty", "Tak", "Nie");
            if (!result)
                return;
            await Toast.Make("Trwa zapisywanie zmian", ToastDuration.Long).Show();

            var user = UserAfterLogin.User.Id.ToByteArray();

            await SaveCustomers(CustomerRoutes, false);
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            await Toast.Make("Zapisywanie zmiań zakończone", ToastDuration.Long).Show();
            SaveData = false;
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

            var result = await MoveTimeOnCustomersV.ShowPopUp(Route, selectDayMs, _get, _save, _updateLogService, _popupService);
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
        SaveData = false;
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


    async Task<HttpResponseMessage> SendData(IList<CustomerRoutes> customers, bool forceUpdate, CancellationTokenSource sourceToken = default)
    {
        await Toast.Make("Wysyłanie listy").Show();

        bool onConflict = sourceToken is not null;

        using var progress = UpdateProgressBar.CreatedUpdateProgressBar(
                        title: "Wysyłanie"
                        , description: onConflict ? "Anuluj wysyłanie" : ""
                        , icon: UpdateProgressBar.GetSyncImage()
                        , rotateIcon: true
                        , action:
                        onConflict ?
                        async () =>
                        {
                            sourceToken?.Cancel();
                            await Toast.Make("Anulowano wysyłani listy punktów").Show();
                            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);

                        }
        : null);

        HttpResponseMessage resultMessage;
        if (sourceToken is not null)
        {
            resultMessage = await _sendHttp.SendCustomerRoutes(customers
                 , progress, forceUpdate: forceUpdate, token: sourceToken.Token);
        }
        else
        {
            resultMessage = await _sendHttp.SendCustomerRoutes(customers
                  , progress, forceUpdate: forceUpdate);
        }

        if (resultMessage.StatusCode == System.Net.HttpStatusCode.Created)
        {
            var updateJson = await resultMessage.Content.ReadAsStringAsync();
            var update = System.Text.Json.JsonSerializer.Deserialize(
                updateJson, SzarotkaJsonSerializerContext.Default.UpdateLog);

            await _updateLogService.Insert(update);
            await Toast.Make("Wysyłanie Zakończone bez komplikacji").Show();
        }

        return resultMessage;
    }

    private async Task SendOnConflict(HttpResponseMessage resultMessage)
    {
        if (resultMessage.StatusCode == HttpStatusCode.Conflict)
        {
            await Toast.Make("Wysyłanie Zakończone. Pobierane są różnice do poprawy").Show();

            using var progressDifference = UpdateProgressBar.CreatedUpdateProgressBar(
                    title: "Pobieranie różnic"
                    , description: "Brak możliwości anulowania"
                    , icon: UpdateProgressBar.GetSyncImage()
                    , rotateIcon: true
                    , action: null);

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressDifference.Grid);

            using var download = await resultMessage.Content.ReadAsStreamAsync();
            var differenceJson = await HttpClientExtension.CheckProgress(
                (progress) => UpdateProgressBar.UpdateProgress(progressDifference, progress)
                , resultMessage.Content.Headers.ContentLength ?? 1
                , download);

            var exists = System.Text.Json.JsonSerializer.Deserialize(
                differenceJson, SzarotkaJsonSerializerContext.Default.UpdateDifferenceArray);

            if (exists.Length > 0)
            {
                var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), exists },
                        {nameof(Action),(Action<IEnumerable>)(async (customer)
                        =>
                            {
                                await SaveCustomers(customer,true);

                                await SendData((IList<CustomerRoutes>)customer, forceUpdate:true, sourceToken:null);
                                Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                            })
                        }
                    };
                await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
            }
        }
    }
    private async Task SaveCustomersFromDowloand(IList<CustomerRoutes> result)
    {
        IList<UpdateDifference> exists = [];
        IList<CustomerRoutes> customersToSave = [];
        foreach (CustomerRoutes customer in result)
        {
            (bool canUpdate, CustomerRoutes isExist) = await ModelsDifferences.Check(_get, customer, false);
            if (canUpdate)
            {
                customersToSave.Add(customer);
            }
            if (!canUpdate)
            {
                exists.Add(new UpdateDifference()
                {
                    Update = isExist!,
                    Server = customer
                });
            }
        }
        await SaveCustomers(customersToSave, true);
        if (exists.Count > 0)
        {
            await Toast.Make("Popraw różnice").Show();
            var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), exists },
                        {nameof(Action),(Action<IEnumerable>)(async (customer)
                        =>
                            {
                                 await SaveCustomers(customer, true);

                                 await SendData((IList<CustomerRoutes>)customer, forceUpdate:true, sourceToken:null);
                                 Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                            })
                        }
                    };

            await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
        }
    }

    private async Task SaveCustomers(IEnumerable customers, bool isServer = false)
    {
        await _save.SaveCustomerRoutesTransaction([.. customers.Cast<CustomerRoutes>()], isServer);
    }


    [RelayCommand]
    async Task SyncData()
    {
        using CancellationTokenSource cancellationTokenSource = new();
        try
        {
            await Task.Run(async () =>
            {

                UpdateLog firstLocalLogToSend = await _updateLogService.SelectFirst(isServer: true,
                                                                                  DataBase.Model.UpdateEnum.CustomerRoutes);
                IList<UpdateLog> localLogToSend = await _updateLogService.Select(firstLocalLogToSend.Id);

                IList<CustomerRoutes> routesToSend = await _getHttp.GetCustomerRoutes([.. localLogToSend.Select(x => new Guid(x.UpdateId))]);

                using var logsGet = UpdateProgressBar.CreatedUpdateProgressBar(
                                title: "Anuluj"
                                , description: "Pobieranie list logów"
                                , icon: UpdateProgressBar.GetSyncImage()
                                , rotateIcon: true
                                , action: async () =>
                                {
                                    cancellationTokenSource?.Cancel();
                                    await Toast.Make("Anulowano pobieranie").Show();
                                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                                });

                IList<UpdateLog> logToDowloand = await _updateLogsHttp.GetLogs(firstLocalLogToSend.Id, logsGet, cancellationTokenSource.Token);
                if (routesToSend.Any())
                {
                    var sendResponse = await SendData(routesToSend, forceUpdate: false, cancellationTokenSource);
                    await SendOnConflict(sendResponse);
                }

                using var progressDownload = UpdateProgressBar.CreatedUpdateProgressBar(
                                title: "Anuluj"
                                , description: "Pobieranie list logów"
                                , icon: UpdateProgressBar.GetSyncImage()
                                , rotateIcon: true
                                , action: async () =>
                                {
                                    cancellationTokenSource?.Cancel();
                                    await Toast.Make("Anulowano pobieranie").Show();
                                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                                });
                if (logToDowloand.Any())
                {
                    await _updateLogService.Insert(logToDowloand.LastOrDefault());
                    IList<CustomerRoutes> dowloandCustomers = await _getHttp.GetCustomerRoutes([.. logToDowloand.Select(x => new Guid(x.UpdateId))], progressDownload, cancellationTokenSource.Token);
                    await SaveCustomersFromDowloand(dowloandCustomers);
                }
                await Toast.Make("Synchronizacja zakończona").Show();
                GetPointsFireAndForget(Route, lastSelectedDayOfWeekRoutes);

            }, cancellationTokenSource.Token);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            //HttpStatusCode.Conflict został obłużony wyżej jako zwrot danych do poprawy przy edycji
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            Shell.Current.FlyoutIsPresented = false;
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            cancellationTokenSource?.Dispose();
        }
    }




    [RelayCommand]
    async Task SendData()
    {
        if (SaveData)
        {
            return;
        }

        using var sourceToken = new CancellationTokenSource();
        try
        {
            await Toast.Make("Wczytywane wszystkich danych z danej trasy").Show();

            await Task.Run(async () =>
            {
                var send = await GetPointsAsync(Route, new());
                var resultMessage = await SendData(send, false, sourceToken);

                await SendOnConflict(resultMessage);
                resultMessage.EnsureSuccessStatusCode();

                GetPointsFireAndForget(Route, lastSelectedDayOfWeekRoutes);
            }, sourceToken.Token);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            //HttpStatusCode.Conflict został obłużony wyżej jako zwrot danych do poprawy przy edycji
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            Shell.Current.FlyoutIsPresented = false;
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            sourceToken?.Dispose();
        }
    }


    [RelayCommand]
    async Task DownloadData()
    {
        if (SaveData)
        {
            return;
        }
        await Toast.Make("Rozpoczęto Pobieranie").Show();
        using var sourceToken = new CancellationTokenSource();
        try
        {
            await Task.Run(async () =>
            {

                using var progress = UpdateProgressBar.CreatedUpdateProgressBar(
                                title: "Anuluj"
                                , description: "Pobieranie listy punktów"
                                , icon: UpdateProgressBar.GetSyncImage()
                                , rotateIcon: true
                                , action: async () =>
                                {
                                    sourceToken.Cancel();
                                    await Toast.Make("Anulowano pobieranie listy punktów").Show();
                                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                                });
                var result = await _getHttp.GetCustomerRoutes(Route.Id, new()
                      , progress, sourceToken.Token);

                await SaveCustomersFromDowloand(result);
                GetPointsFireAndForget(Route, lastSelectedDayOfWeekRoutes);

            }, sourceToken.Token);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            //HttpStatusCode.Conflict został obłużony wyżej jako zwrot danych do poprawy przy edycji
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            Shell.Current.FlyoutIsPresented = false;
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
            sourceToken?.Dispose();
        }
    }


}


