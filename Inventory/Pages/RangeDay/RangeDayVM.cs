using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data.File;
using DataBase.Model.EntitiesInventory;
using DataBase.Pages.ExistingFiles;
using DataBase.Service;

using Inventory.Data.File;
using Inventory.Model;
using Inventory.Service;

namespace Inventory.Pages.RangeDay
{
    [QueryProperty(nameof(FilesPath), nameof(FilesPath))]
    public partial class RangeDayVM : ObservableObject
    {
        [ObservableProperty]
        RangeDayM[] rangeDays;


        [ObservableProperty]
        IList<RangeDayM> sum = [];
        public IList<RangeDayM> SumDayOfWeek { get; set; } = [];
        public IList<RangeDayM> AveragesDayOfWeek { get; set; } = [];
        public IList<RangeDayM> SumPerOfWeek { get; set; } = [];
        public IList<RangeDayM> AveragesPerOfWeek { get; set; } = [];
        public IList<RangeDayM> SumPerOfMonth { get; set; } = [];
        public IList<RangeDayM> AveragesPerOfMonth { get; set; } = [];
        public IList<DataBase.Model.EntitiesInventory.Product> ProductsAll { get; set; }
        IList<DataBase.Model.EntitiesInventory.Driver> UniqueDriver = [];

        readonly Driver[] _allDrivers;

        [ObservableProperty]
        bool enableSave;

        [ObservableProperty]
        bool listIsVisible = true;
        [ObservableProperty]
        bool graphIsVisible;
        [ObservableProperty]
        bool tableIsVisible;


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
                        RangeDays = JsonFile.GetFileJson<RangeDayM[]>(filesPath);
                        Calculate(RangeDays);
                    }
                    if (extension == FileHelper.csvTyp)
                    {
                        RangeDays = CSVFile.GetFileCSV(filesPath);
                        Calculate(RangeDays);
                    }

                    EnableSave = true;
                }
            }
        }


        PopupDateModel PopupDate = new(DateTime.Today.Ticks, DateTime.Today.AddDays(1).Ticks, false, []);
        readonly DataBase.Data.AccessDataBase _db;
        readonly ISelectDayService _selectDayService;
        readonly ISaveDayService _dayService;
        public RangeDayVM(DataBase.Data.AccessDataBase db, ISelectDayService selectDay, ISaveDayService dayService)
        {
            sum = [];
            _db = db;
            _selectDayService = selectDay;


            var driver = _db.DataBase.Table<Driver>().ToArray();
            _allDrivers = driver;

            EnableSave = false;
            _dayService = dayService;
        }

        public RangeDayVM(Driver[] allDrivers)
        {
            _allDrivers = allDrivers;
        }

        #region Method


        async Task<RangeDayM[]> SelectDays(long from, long to, Guid[] selectedDriverName, bool moreData)
        {
            var (drivers, days) = await _selectDayService.GetDaysAndDrivers(from, to, selectedDriverName, moreData);

            var range = new RangeDayM[days.Length];

            for (int i = 0; i < range.Length; i++)
            {
                range[i] = new()
                {
                    Day = days[i],
                    Driver = drivers[i]
                };
            }

            return range;
        }

        public void Calculate(IList<RangeDayM> value)
        {
            if (value is not null)
            {
                Helper.RangeCalculations.GetUniqueDriver(value);
                UniqueDriver = Helper.RangeCalculations.UniqueDriver;

                Sum = Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(value);
                if (Sum.Count > 0)
                {
                    ProductsAll = Sum.MaxBy(x => x.Day.Products.Count).Day.Products;
                }

                SumDayOfWeek = Helper.RangeCalculations.SumDayOfWeek(value);
                AveragesDayOfWeek = Helper.RangeCalculations.AveragesDayOfWeek(value);

                SumPerOfWeek = Helper.RangeCalculations.SumPerOfWeek(value);
                AveragesPerOfWeek = Helper.RangeCalculations.AveragesPerOfWeek(value);


                SumPerOfMonth = Helper.RangeCalculations.SumPerOfMonth(value);
                AveragesPerOfMonth = Helper.RangeCalculations.AveragesPerOfMonth(value);

            }

            Table.RangeTable.OnSetRangeDayMs(RangeDays, Sum, SumDayOfWeek, AveragesDayOfWeek, SumPerOfWeek, AveragesPerOfWeek, SumPerOfMonth, AveragesPerOfMonth, ProductsAll, UniqueDriver);
            Graph.Graph.OnSetRangeDayMs(RangeDays, Sum, SumDayOfWeek, AveragesDayOfWeek, SumPerOfWeek, AveragesPerOfWeek, SumPerOfMonth, AveragesPerOfMonth, ProductsAll, UniqueDriver);

        }

        async static Task<string> SelectImportExport(string type)
        {
#if WINDOWS
            var result = await Shell.Current.CurrentPage.DisplayActionSheet($"Wybierz co chcesz{Environment.NewLine}wykonać z plikami {type}",
            "Anuluj", null, "Import", "Eksport");
#else
            var result = await Shell.Current.CurrentPage.DisplayActionSheet($"Wybierz co chcesz{Environment.NewLine}wykonać z plikami {type}",
                "Anuluj", null, "Import", "Eksport", "Pliki");
#endif
            return result;
        }

        string CreateFileName()
        {
            if (RangeDays.Length == 1)
            {
                return string.Join('_', "Szarotka", RangeDays[0].Day.Created.ToString("dd.MM.yyyy"));
            }
            var from = RangeDays.FirstOrDefault().Day.Created.ToString("dd.MM.yyyy");
            var to = RangeDays.LastOrDefault().Day.Created.ToString("dd.MM.yyyy");
            return string.Join('_', "Szarotka", from, to);
        }

        #endregion

        #region Command

        [RelayCommand]
        async Task OpenDetailpage(RangeDayM rangeDay)
        {
            try
            {
                rangeDay.Day = await _selectDayService.GetDayProcedure(rangeDay.Day.Id);
                rangeDay.Day.CanUpadte = true;
                for (int i = 0; i < rangeDay.Day.Products.Count; i++)
                {
                    rangeDay.Day.Products[i].CanUpadte = true;
                }

                await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Day)] = rangeDay.Day
                    });
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
                    var file = JsonFile.GetFileJson<RangeDayM[]>(response.FullPath);
                    RangeDays = file;
                    Calculate(RangeDays);
                    EnableSave = true;
                }
                if (result == "Eksport")
                {
                    for (int i = 0; i < RangeDays.Length; i++)
                    {
                        if (RangeDays[i].Day.Products.Count <= 0)
                            RangeDays[i].Day = await _selectDayService.GetDayProcedure(RangeDays[i].Day.Id);
                    }
                    var name = CreateFileName();
                    var response = await JsonFile.SaveFileJson(RangeDays, name);
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
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task GenerateCSVFile()
        {
            try
            {
#if ANDROID
                if (!await AndroidPermissionService.CheckAllPermissionsAboutStorage())
                {
                    return;
                }
#endif
                var result = await SelectImportExport("CSV");

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
                    var response = await FilePicker.PickAsync(ExistingFilesVM.FileTypCSV());
                    if (response == null)
                        return;
                    var file = CSVFile.GetFileCSV(response.FullPath);
                    RangeDays = file;
                    Calculate(RangeDays);
                    EnableSave = true;
                }
                if (result == "Eksport")
                {
                    for (int i = 0; i < RangeDays.Length; i++)
                    {
                        if (RangeDays[i].Day.Products.Count <= 0)
                            RangeDays[i].Day = await _selectDayService.GetDayProcedure(RangeDays[i].Day.Id);
                    }
                    var name = CreateFileName();
                    var response = CSVFile.SaveFileCSV(RangeDays, name);
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = name,
                        File = new ShareFile(response)
                    });
                }
                if (result == "Pliki")
                {
                    var files = FileHelper.GetFilesPaths(FileHelper.CsvFolder);
                    await Shell.Current.GoToAsync($"{nameof(ExistingFilesV)}?GetTyp={FileHelper.CsvFolder}",
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
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task SaveAnotherDriverData()
        {
            try
            {
                for (int i = 0; i < RangeDays.Length; i++)
                {
                    await Task.Delay(1);
                }
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task SelectMoreDate()
        {
            try
            {
                var popup = new PopupSelectRangeDate.PopupSelectRangeDateV(_allDrivers);
                var result = await Shell.Current.ShowPopupAsync(popup);

                if (result is PopupDateModel model)
                {
                    PopupDate = model;
                    RangeDays = await SelectDays(PopupDate.From, PopupDate.To, PopupDate.DriverId, PopupDate.MoreData);
                    // RangeDays = await SelectDays(0, DateTime.Today.Ticks, [], true);

                    Calculate(RangeDays);
                    EnableSave = false;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        static async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task GoToGraph()
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(Graph.GraphV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(RangeDayM)] = RangeDays
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        #endregion


    }
}
