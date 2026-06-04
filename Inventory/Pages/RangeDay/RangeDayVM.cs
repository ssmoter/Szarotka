using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;
using DataBase.Service;

using Inventory.Helper.Calculations;
using Inventory.Model;

using Microsoft.Maui.Platform;

using MudBlazor;

using Shared.CustomControls.FromCode;
using Shared.Data;
using Shared.Data.File;
using Shared.Data.ServerHttpClients;
using Shared.Pages.ExistingFiles;
using Shared.Pages.UpdateDifference;
using Shared.Service;

using System.Collections;
using System.Collections.ObjectModel;
using System.Net;

namespace Inventory.Pages.RangeDay;

public partial class RangeDayVM : ObservableObject, IQueryAttributable, IDisposable
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
    }

    private IList<Day> allDays = [];
    public IList<Day> AllDays
    {
        get => allDays;
        set
        {
            if (SetProperty(ref allDays, value, nameof(AllDays))) { }
        }
    }

    private ObservableCollection<DayExpanded> sortedDays;
    public ObservableCollection<DayExpanded> SortedDays
    {
        get => sortedDays;
        set
        {
            if (SetProperty(ref sortedDays, value, nameof(SortedDays))) { }
        }
    }
    private ObservableCollection<string> sortedHeaders;
    public ObservableCollection<string> SortedHeaders
    {
        get => sortedHeaders;
        set
        {
            if (SetProperty(ref sortedHeaders, value, nameof(SortedHeaders))) { }
        }
    }
    private ObservableCollection<string> sortedHeadersHide;
    public ObservableCollection<string> SortedHeadersHide
    {
        get => sortedHeadersHide;
        set
        {
            if (SetProperty(ref sortedHeadersHide, value, nameof(SortedHeadersHide))) { }
        }
    }

    private ObservableCollection<string> defaultsHeaderNames;
    public ObservableCollection<string> DefaultsHeaderNames
    {
        get => defaultsHeaderNames;
        set
        {
            if (SetProperty(ref defaultsHeaderNames, value, nameof(DefaultsHeaderNames))) { }
        }
    }
    private ObservableCollection<string> defaultsHeaderKeys;
    public ObservableCollection<string> DefaultsHeaderKeys
    {
        get => defaultsHeaderKeys;
        set
        {
            if (SetProperty(ref defaultsHeaderKeys, value, nameof(DefaultsHeaderKeys))) { }
        }
    }


    private readonly string[] _defaultsHeaders =
    [
        "*","Data","Kierowca","Zapłacono","Utarg suma","Różnica"
    ];
    private RangeDayM optionsM;
    public RangeDayM OptionsM
    {
        get => optionsM;
        set
        {
            if (SetProperty(ref optionsM, value, nameof(OptionsM))) { }
        }
    }
    private FilterTyp filterTyp;
    public FilterTyp FilterTyp
    {
        get => filterTyp;
        set
        {
            if (SetProperty(ref filterTyp, value, nameof(FilterTyp)))
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
                var extension = Path.GetExtension(filesPath);
                if (extension == FileHelper.jsonTyp)
                {
                    AllDays = [.. JsonFile.GetFileJson<Day[]>(filesPath, SzarotkaJsonSerializerContext.Default.DayArray)];
                }
                OptionsM.EnableSave = true;
            }
        }
    }


    private PopupDateModel PopupDate = null;
    private readonly IAccessDataBase _db;
    private readonly IGetInventoryAoT _get;
    private readonly ISaveInventoryAoT _save;
    private readonly Data.InventoryApi.IGetDayHttp _getDayHttp;
    private readonly Data.InventoryApi.ISendDayHttp _sendDayHttp;
    private readonly Shared.Data.ServerHttpClients.IUpdateLogsHttp _updateLogsHttp;
    private readonly DataBase.Service.IUpdateLogService _updateLogService;
    public RangeDayVM(IAccessDataBase db,
                      IGetInventoryAoT get,
                      Data.InventoryApi.IGetDayHttp getDayHttp,
                      Data.InventoryApi.ISendDayHttp sendDayHttp,
                      Shared.Data.ServerHttpClients.IUpdateLogsHttp updateLogsHttp,
                      DataBase.Service.IUpdateLogService updateLogService,
                      ISaveInventoryAoT save)
    {
        _db = db;
        _get = get;
        OptionsM ??= new();
        FilterTyp ??= new();
        OptionsM.EnableSave = false;
        DefaultsHeaderNames ??= [];
        DefaultsHeaderKeys ??= [];
        SortedHeaders ??= [];
        SortedHeadersHide ??= [.. _defaultsHeaders];
        FilterTyp.PropertyChanged += FilterTyp_PropertyChanged;
        _getDayHttp = getDayHttp;
        this._sendDayHttp = sendDayHttp;
        _updateLogsHttp = updateLogsHttp;
        _updateLogService = updateLogService;
        _save = save;
    }
    private bool _isScheduledFilterTyp = false;
    private bool _isScheduledOrderBy = false;
    private async void FilterTyp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (_isScheduledFilterTyp)
            return;
        if (e.PropertyName == nameof(FilterTyp.OrderBy))
            return;

        _isScheduledFilterTyp = true;
        OptionsM.IsRefreshing = true;
        try
        {
            await Task.Delay(FilterTyp.TimerDelay);
            FilterDaysMethod(FilterTyp);
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            _isScheduledFilterTyp = false;
            OptionsM.IsRefreshing = false;
        }
    }


    public void Dispose()
    {
        FilterTyp.PropertyChanged -= FilterTyp_PropertyChanged;
    }


    async static Task<string> SelectImportExport(string type)
    {
#if WINDOWS
            var result = await Shell.Current.CurrentPage.DisplayActionSheetAsync($"Wybierz co chcesz{Environment.NewLine}wykonać z plikami {type}",
            "Anuluj", null, "Import", "Eksport");
#else
        var result = await Shell.Current.CurrentPage.DisplayActionSheetAsync($"Wybierz co chcesz{Environment.NewLine}wykonać z plikami {type}",
            "Anuluj", null, "Import", "Eksport", "Pliki");
#endif
        return result;
    }

    internal async Task SetDefaultsHeaders()
    {
        var productNames = await _get.EmptyProductsNameAndPrices(isDelete: true);

        DefaultsHeaderNames.Clear();
        DefaultsHeaderKeys.Clear();
        DefaultsHeaderKeys.Add("Ilość");
        DefaultsHeaderKeys.Add("Edycja");
        DefaultsHeaderKeys.Add("Zwrot");
        DefaultsHeaderKeys.Add("Sprzedane");
        DefaultsHeaderKeys.Add("Po korekcie");
        DefaultsHeaderKeys.Add("Korekta");
        DefaultsHeaderKeys.Add("Utarg");
        DefaultsHeaderKeys.Add("Zapłacono");
        DefaultsHeaderKeys.Add("Utarg produkty");
        DefaultsHeaderKeys.Add("Utarg ciasto");
        DefaultsHeaderKeys.Add("Utarg suma");
        DefaultsHeaderKeys.Add("Różnica");

        DefaultsHeaderNames.Add("");
        foreach (var item in productNames)
        {
            DefaultsHeaderNames.Add(item.Item1.Name);
        }
    }


    private static string CreateFileName(IList<Day> days)
    {
        if (days.Count == 1)
        {
            return string.Join('_', "Szarotka", days[0].Created.ToString("dd.MM.yyyy"));
        }
        var from = days[0].Created.ToString("dd.MM.yyyy");
        var to = days[^1].Created.ToString("dd.MM.yyyy");
        return string.Join('_', "Szarotka", from, to);
    }

    [RelayCommand]
    async Task OpenDetailPage(Day day)
    {
        try
        {
            if (day is null)
            {
                return;
            }
            var popup = new SingleDayPreview.SingleDayPreviewPopUp.SingleDayPreviewPopUpV(day);

            if (Application.Current?.Windows != null && Application.Current.Windows.Count > 0)
            {
                await Application.Current.Windows[0].Page.ShowPopupAsync(popup);
            }
            else if (Application.Current?.Windows[0].Page != null)
            {
                await Application.Current.Windows[0].Page.ShowPopupAsync(popup);
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
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
            var result = await SelectImportExport("Json");

            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (result == "Anuluj")
            {
                return;
            }
            if (result == "Import")
            {
                var response = await FilePicker.PickAsync(ExistingFilesVM.FileTypJson());
                if (response == null)
                    return;
                var file = JsonFile.GetFileJson<Day[]>(response.FullPath, SzarotkaJsonSerializerContext.Default.DayArray);
                AllDays = [.. file];
                OptionsM.EnableSave = true;
            }
            if (result == "Eksport")
            {
                var name = CreateFileName(AllDays);
                var response = await JsonFile.SaveFileJson(AllDays, SzarotkaJsonSerializerContext.Default.DayArray, name);
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = name,
                    File = new ShareFile(response)
                });
            }
            if (result == "Pliki")
            {
                var files = FileHelper.GetFilesPaths(FileHelper.JsonFolder);
                await Shell.Current.GoToAsync($"{nameof(ExistingFilesV)}?GetTyp={FileHelper.JsonFolder}",
                    new Dictionary<string, object>
                    {
                        [nameof(ExistingFilesM)] = ExistingFilesVM.GetExistingFiles(files)
                        ,
                        ["ReturnPage"] = nameof(RangeDayV)
                    }); ;
            }

        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }

    [RelayCommand]
    async Task SaveAnotherDriverData()
    {
        try
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }

    [RelayCommand]
    async Task SelectMoreDate()
    {
        try
        {
            PopupSelectRangeDate.PopupSelectRangeDateV popup;
            if (PopupDate is null)
            {
                popup = new PopupSelectRangeDate.PopupSelectRangeDateV();
            }
            else
            {
                popup = new PopupSelectRangeDate.PopupSelectRangeDateV(PopupDate);
            }
            object result = null;
            if (Application.Current?.Windows != null && Application.Current.Windows.Count > 0)
            {
                result = await Application.Current.Windows[0].Page.ShowPopupAsync(popup);
            }
            else if (Application.Current?.MainPage != null)
            {
                result = await Application.Current.MainPage.ShowPopupAsync(popup);
            }

            if (result is PopupDateModel model)
            {
                PopupDate = model;
                var list = await _get.Days(model.From, model.To, model.DriverId);
                AllDays = [.. list.OrderByDescending(x => x.SelectedDateTicks)];
                OptionsM.EnableSave = false;

                FilterDaysMethod(FilterTyp);
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }

    private void FilterDaysMethod(FilterTyp filterTyp)
    {
        if (AllDays is null || AllDays?.Count <= 0)
        {
            return;
        }

        var key = filterTyp.SelectedKey;
        var name = filterTyp.SelectedName;

        if (filterTyp.CalculationBy != CalculationTyp.None)
        {
            if (!SortedHeadersHide.Contains("Zakres"))
            {
                SortedHeadersHide.Add("Zakres");
            }
        }
        else
        {
            SortedHeadersHide.Remove("Zakres");
        }

        var selected =
                AllDays.Where(z => filterTyp.GetSelectedDays().Contains(z.SelectedDate.DayOfWeek))
                .Select((x, index) =>
                {
                    decimal? selectedProduct = GetValueFromName(x, key, name);

                    return new DayExpanded(
                        day: x,
                        index: index + 1,
                        selectedValue: selectedProduct.ToString(),
                        selectedHeaders: MergeArrays(_defaultsHeaders, SortedHeaders)
                    );
                });

        var calculation = GetCalculationTyp(selected, filterTyp.CalculationBy);

        SortedDays = [.. calculation];
    }
    static IEnumerable<string> MergeArrays(string[] a, ObservableCollection<string> b)
    {
        foreach (var item in a)
            yield return item;

        foreach (var item in b)
            yield return item;
    }
    private string lastOrderByKey = "";
    private string lastOrderByName = "";

    [RelayCommand]
    async Task OrderBy(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }
        if (SortedDays is null || SortedDays.Count == 0)
        {
            return;
        }

        GetKeyAndNameFromHeader(value, out string key, out string name);

        if (lastOrderByKey != key || lastOrderByName != name)
        {
            _isScheduledOrderBy = false;
            FilterTyp.OrderBy = FilterTyp.OrderTyp.None;
        }

        lastOrderByKey = key;
        lastOrderByName = name;
        if (_isScheduledOrderBy)
            return;

        _isScheduledOrderBy = true;
        OptionsM.IsRefreshing = true;

        try
        {
            await Task.Delay(FilterTyp.TimerDelay);

            if (FilterTyp.OrderBy == FilterTyp.OrderTyp.Desc)
            {
                FilterTyp.OrderBy = FilterTyp.OrderTyp.Asc;
            }
            else if (FilterTyp.OrderBy == FilterTyp.OrderTyp.Asc)
            {
                FilterTyp.OrderBy = FilterTyp.OrderTyp.Desc;
            }
            else
            {
                FilterTyp.OrderBy = FilterTyp.OrderTyp.Asc;
            }

            SortedDays = FilterTyp.OrderBy switch
            {
                FilterTyp.OrderTyp.None => [.. SortedDays.OrderBy(x => x.Index)],
                FilterTyp.OrderTyp.Desc => [.. GetSortedDESC(SortedDays, name, key)],
                FilterTyp.OrderTyp.Asc => [.. GetSortedASC(SortedDays, name, key)],
                _ => [.. SortedDays.OrderBy(x => x.Index)]
            };
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            _isScheduledOrderBy = false;
            OptionsM.IsRefreshing = false;
        }
    }

    public static void GetKeyAndNameFromHeader(string value, out string key, out string name)
    {
        var split = value.Split(';');
        key = split.Length > 1 ? split[1] : split[0];
        name = split.Length > 1 ? split[0] : "";
    }

    private static IEnumerable<DayExpanded> GetSortedASC(IEnumerable<DayExpanded> source, string name, string key)
    {
        var translateKey = Controls.ContentFromList.TranslateHeader(key);

        if (string.IsNullOrWhiteSpace(name))
        {
            var sortedDay = translateKey switch
            {
                nameof(Day.TotalPriceAfterCorrectDecimal) => source.OrderBy(x => x.Day.TotalPriceAfterCorrectDecimal),
                nameof(Day.TotalPriceCakeDecimal) => source.OrderBy(x => x.Day.TotalPriceCakeDecimal),
                nameof(Day.TotalPriceCorrectDecimal) => source.OrderBy(x => x.Day.TotalPriceCorrectDecimal),
                nameof(Day.TotalPriceDecimal) => source.OrderBy(x => x.Day.TotalPriceDecimal),
                nameof(Day.TotalPriceDifferenceDecimal) => source.OrderBy(x => x.Day.TotalPriceDifferenceDecimal),
                nameof(Day.TotalPriceMoneyDecimal) => source.OrderBy(x => x.Day.TotalPriceMoneyDecimal),
                nameof(Day.TotalPriceProductsDecimal) => source.OrderBy(x => x.Day.TotalPriceProductsDecimal),
                nameof(Day.SelectedDate) => source.OrderBy(x => x.Day.SelectedDate),
                "*" => source.OrderBy(x => x.Index),
                _ => source,
            };
            return sortedDay;
        }


        var sortedProduct = translateKey switch
        {
            nameof(Product.Number) => source.OrderBy(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Number),
            nameof(Product.NumberEdit) => source.OrderBy(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).NumberEdit),
            nameof(Product.NumberReturn) => source.OrderBy(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).NumberReturn),
            nameof(Product.PriceTotalAfterCorrectDecimal) => source.OrderBy(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalAfterCorrectDecimal),
            nameof(Product.PriceTotalCorrectDecimal) => source.OrderBy(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalCorrectDecimal),
            nameof(Product.PriceTotalDecimal) => source.OrderBy(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalDecimal),
            "Sell" => source.OrderBy(x => SellReturn(x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))),
            _ => source,
        };
        return sortedProduct;
    }
    private static IEnumerable<DayExpanded> GetSortedDESC(IEnumerable<DayExpanded> source, string name, string key)
    {
        var translateKey = Controls.ContentFromList.TranslateHeader(key);
        if (string.IsNullOrWhiteSpace(name))
        {
            var sortedDay = translateKey switch
            {
                nameof(Day.TotalPriceAfterCorrectDecimal) => source.OrderByDescending(x => x.Day.TotalPriceAfterCorrectDecimal),
                nameof(Day.TotalPriceCakeDecimal) => source.OrderByDescending(x => x.Day.TotalPriceCakeDecimal),
                nameof(Day.TotalPriceCorrectDecimal) => source.OrderByDescending(x => x.Day.TotalPriceCorrectDecimal),
                nameof(Day.TotalPriceDecimal) => source.OrderByDescending(x => x.Day.TotalPriceDecimal),
                nameof(Day.TotalPriceDifferenceDecimal) => source.OrderByDescending(x => x.Day.TotalPriceDifferenceDecimal),
                nameof(Day.TotalPriceMoneyDecimal) => source.OrderByDescending(x => x.Day.TotalPriceMoneyDecimal),
                nameof(Day.TotalPriceProductsDecimal) => source.OrderByDescending(x => x.Day.TotalPriceProductsDecimal),
                nameof(Day.SelectedDate) => source.OrderByDescending(x => x.Day.SelectedDate),
                "*" => source.OrderByDescending(x => x.Index),
                _ => source,
            };
            return sortedDay;
        }


        var sortedProduct = translateKey switch
        {
            nameof(Product.Number) => source.OrderByDescending(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Number),
            nameof(Product.NumberEdit) => source.OrderByDescending(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).NumberEdit),
            nameof(Product.NumberReturn) => source.OrderByDescending(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).NumberReturn),
            nameof(Product.PriceTotalAfterCorrectDecimal) => source.OrderByDescending(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalAfterCorrectDecimal),
            nameof(Product.PriceTotalCorrectDecimal) => source.OrderByDescending(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalCorrectDecimal),
            nameof(Product.PriceTotalDecimal) => source.OrderByDescending(x => x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalDecimal),
            "Sell" => source.OrderByDescending(x => SellReturn(x.Day.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))),
            _ => source,
        };
        return sortedProduct;
    }
    private static IEnumerable<DayExpanded> GetCalculationTyp(IEnumerable<DayExpanded> source, CalculationTyp calculation)
    {
        return calculation switch
        {
            CalculationTyp.None => source,
            CalculationTyp.SumWeek => DayRangeCalculationSum.Week(source),
            CalculationTyp.SumMonth => DayRangeCalculationSum.Month(source),
            CalculationTyp.SumYear => DayRangeCalculationSum.Year(source),
            CalculationTyp.SumDayOfWeek => DayRangeCalculationSum.DayOfWeek(source),
            CalculationTyp.SumAll => DayRangeCalculationSum.All(source),
            CalculationTyp.AverageWeek => DayRangeCalculationAverage.Week(source),
            CalculationTyp.AverageMonth => DayRangeCalculationAverage.Month(source),
            CalculationTyp.AverageYear => DayRangeCalculationAverage.Year(source),
            CalculationTyp.AverageDayOfWeek => DayRangeCalculationAverage.DayOfWeek(source),
            CalculationTyp.AverageAll => DayRangeCalculationAverage.All(source),
            CalculationTyp.MedianWeek => DayRangeCalculationMedian.Week(source),
            CalculationTyp.MedianMonth => DayRangeCalculationMedian.Month(source),
            CalculationTyp.MedianYear => DayRangeCalculationMedian.Year(source),
            CalculationTyp.MedianDayOfWeek => DayRangeCalculationMedian.DayOfWeek(source),
            CalculationTyp.MedianAll => DayRangeCalculationMedian.All(source),
            _ => source,
        };
    }

    public static decimal? GetValueFromName(Day x, string key, string name)
    {
        decimal? result = 0;
        if (string.IsNullOrWhiteSpace(name))
        {
            result = key switch
            {
                nameof(Day.TotalPriceAfterCorrectDecimal) => x.TotalPriceAfterCorrectDecimal,
                nameof(Day.TotalPriceCakeDecimal) => x.TotalPriceCakeDecimal,
                nameof(Day.TotalPriceCorrectDecimal) => x.TotalPriceCorrectDecimal,
                nameof(Day.TotalPriceDecimal) => x.TotalPriceDecimal,
                nameof(Day.TotalPriceDifferenceDecimal) => x.TotalPriceDifferenceDecimal,
                nameof(Day.TotalPriceMoneyDecimal) => x.TotalPriceMoneyDecimal,
                nameof(Day.TotalPriceProductsDecimal) => x.TotalPriceProductsDecimal,
                nameof(Day.SelectedDate) => x.SelectedDateTicks,
                _ => null,
            };
            return result;
        }

        result = key switch
        {
            nameof(Product.Number) => x.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Number,
            nameof(Product.NumberEdit) => x.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).NumberEdit,
            nameof(Product.NumberReturn) => x.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).NumberReturn,
            nameof(Product.PriceTotalAfterCorrectDecimal) => x.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalAfterCorrectDecimal,
            nameof(Product.PriceTotalCorrectDecimal) => x.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalCorrectDecimal,
            nameof(Product.PriceTotalDecimal) => x.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).PriceTotalDecimal,
            "Sell" => SellReturn(x.Products.FirstOrDefault(p => p.Name.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))),
            _ => null,
        };

        return result;


    }
    private static decimal SellReturn(Product product)
    {
        return product.Number + product.NumberEdit - product.NumberReturn;
    }
    [RelayCommand]
    static async Task Back()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    void SortTableChangeVisibility()
    {
        OptionsM.SortTableIsVisible = !OptionsM.SortTableIsVisible;
    }

    [RelayCommand]
    void SetSortedName(string name)
    {
        this.FilterTyp.SelectedName = name;
    }
    [RelayCommand]
    void SetSortedKey(string key)
    {
        this.FilterTyp.SelectedKey = key;
    }
    [RelayCommand]
    void AddNewHeader()
    {
        var head = string.Join(';', FilterTyp.SelectedName, FilterTyp.SelectedKey);
        SortedHeaders.Add(head);
        SortedHeadersHide.Add(head);
        FilterTyp.SelectedName = "";
        FilterTyp.SelectedKey = "";
    }
    [RelayCommand]
    void RemoveSelectedHeader(string value)
    {
        SortedHeaders.Remove(value);
        SortedHeadersHide.Remove(value);
    }



    private async Task<HttpResponseMessage> SendData(IList<Day> days, bool forceUpdate = false, CancellationTokenSource sourceToken = default)
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
                            await Toast.Make("Anulowano wysyłani listy").Show();
                            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);

                        }
        : null);

        HttpResponseMessage resultMessage;
        if (sourceToken is not null)
        {
            resultMessage = await _sendDayHttp.SendDays(days
                 , progressBar: progress, forceUpdate: forceUpdate, token: sourceToken.Token);
        }
        else
        {
            resultMessage = await _sendDayHttp.SendDays(days
                , progressBar: progress, forceUpdate: forceUpdate);
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

    private async Task SaveAll(IEnumerable days, bool isServer = false)
    {
        foreach (Day day in days)
        {
            await SaveDay(day, isServer);
        }
    }
    private async Task SaveDay(Day day, bool isServer = false)
    {
        await _db.DataBaseAsync.RunInTransactionAsync(async c =>
        {
            await _save.SaveDay(day, day.UserUpdatedId.ToByteArray(), isServer);
        });

        foreach (Product product in day.Products)
        {
            await _save.SaveProduct(product, day.UserUpdatedId.ToByteArray(), isServer);
        }
        foreach (Cake cake in day.Cakes)
        {
            await _save.SaveCake(cake, day.UserUpdatedId.ToByteArray(), isServer);
        }
        _ = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
        {
            IsServer = isServer,
        }, day);
    }

    [RelayCommand]
    async Task Send()
    {
        using CancellationTokenSource cancellationTokenSource = new();
        try
        {
            var allDays = await _get.Days(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks, []);


            var result = await SendData(allDays, false, cancellationTokenSource);

            if (result.StatusCode == HttpStatusCode.Conflict)
            {
                await Toast.Make("Wysyłanie Zakończone. Pobierane są różnice do poprawy").Show();

                using var progressDifference = UpdateProgressBar.CreatedUpdateProgressBar(
                        title: "Pobieranie różnic"
                        , description: "Brak możliwości anulowania"
                        , icon: UpdateProgressBar.GetSyncImage()
                        , rotateIcon: true
                        , action: null);

                Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressDifference.Grid);

                using var download = await result.Content.ReadAsStreamAsync();
                var differenceJson = await HttpClientExtension.CheckProgress(
                    (progress) => UpdateProgressBar.UpdateProgress(progressDifference, progress)
                    , result.Content.Headers.ContentLength ?? 1
                    , download);

                UpdateDifference[] exists = System.Text.Json.JsonSerializer.Deserialize(
                    differenceJson, SzarotkaJsonSerializerContext.Default.UpdateDifferenceArray);

                if (exists.Length > 0)
                {
                    var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), exists },
                        {nameof(Action),(Action<IEnumerable>)(async (names)
                        =>
                            {
                                 await SaveAll(names,true);
                                 await SendData((IList<Day>)names, forceUpdate:true, sourceToken:null);
                                 Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                            })
                        }
                    };
                    await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
                }
            }

            result.EnsureSuccessStatusCode();
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
            cancellationTokenSource?.Dispose();
        }
    }
    [RelayCommand]
    async Task Download()
    {
        using CancellationTokenSource cancellationTokenSource = new();
        try
        {
            using var progress = UpdateProgressBar.CreatedUpdateProgressBar(
                title: "Anuluj"
                , description: "Pobieranie listy punktów"
                , icon: UpdateProgressBar.GetSyncImage()
                , rotateIcon: true
                , action: async () =>
                {
                    cancellationTokenSource?.Cancel();
                    await Toast.Make("Anulowano pobieranie listy punktów").Show();
                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                });
            var result = await _getDayHttp.GetDays(from: DateTime.MinValue.Ticks, to: DateTime.MaxValue.Ticks, []
                  , progress, cancellationTokenSource.Token);

            await Toast.Make("Pobieranie zakończone").Show();
            await Toast.Make("Rozpoczęto zapisywanie").Show();

            IList<UpdateDifference> exists = [];

            using var progressBar
                = UpdateProgressBar.CreatedUpdateProgressBar(title: "Zapisywanie",
                                                              icon: UpdateProgressBar.GetSyncImage(),
                                                              rotateIcon: true);
            Shell.Current.FlyoutIsPresented = true;
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar.Grid);

            int progressInt = 0;
            int count = result.Count;

            foreach (Day day in result)
            {
                if (!Shared.Pages.FlyoutHeader.FlyoutHeaderVM.IsCustomContentExist())
                {
                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressBar.Grid);
                }

                (bool canUpdate, Day isExist) = await ModelsDifferences.Check(_get, day, false);
                if (canUpdate)
                {
                    await SaveDay(day, true);
                }
                if (!canUpdate)
                {
                    exists.Add(new UpdateDifference()
                    {
                        Update = isExist!,
                        Server = day
                    });
                }
                progressInt++;
                UpdateProgressBar.UpdateProgress(progressBar, (double)(progressInt / (double)count));
            }
            await Toast.Make("Zapisywanie zakończone").Show();
            if (exists.Count > 0)
            {
                await Toast.Make("Popraw różnice").Show();
                var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), exists },
                        {nameof(Action),(Action<IEnumerable>)(async (days)
                        =>
                            {
                                 await SaveAll(days);
                                 await SendData((IList<Day>)days, forceUpdate:true, sourceToken:null);
                                 Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                            })
                        }
                    };

                await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
            }
            Shell.Current.FlyoutIsPresented = false;

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
            cancellationTokenSource?.Dispose();
        }
    }

}


