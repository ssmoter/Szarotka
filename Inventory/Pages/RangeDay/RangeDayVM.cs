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
        RangeDayM[] totalPriceOfRange;

        readonly Driver[] _allDrivers;

        [ObservableProperty]
        bool enableSave;

        [ObservableProperty]
        bool listIsVisible=true;
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
                        Task.Run(async () =>
                        {
                            RangeDays = await JsonFile.GetFileJson<RangeDayM[]>(filesPath);
                            TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                        });
                    }
                    if (extension == FileHelper.csvTyp)
                    {
                        RangeDays = CSVFile.GetFileCSV(filesPath);
                        TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                    }

                    EnableSave = true;
                }
            }
        }


        PopupDateModel PopupDate = new(DateTime.Today.Ticks, DateTime.Today.AddDays(1).Ticks, false, Array.Empty<Guid>());
        readonly DataBase.Data.AccessDataBase _db;
        readonly ISelectDayService _selectDayService;
        readonly ISaveDayService _dayService;
        public RangeDayVM(DataBase.Data.AccessDataBase db, ISelectDayService selectDay, ISaveDayService dayService)
        {
            TotalPriceOfRange = [];
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

        static RangeDayM[] SumTotalPriceOfRange(RangeDayM[] range)
        {
            //List<Driver> drivers = [];
            //for (int i = 0; i < range.Length; i++)
            //{
            //    if (i == 0)
            //    {
            //        drivers.Add(range[i].Driver);
            //        continue;
            //    }

            //    if (drivers.Any(x => x.Id != range[i].Driver.Id))
            //    {
            //        drivers.Add(range[i].Driver);
            //    }
            //}

            //RangeDayM[] SumRange = new RangeDayM[drivers.Count];
            //for (int i = 0; i < drivers.Count; i++)
            //{
            //    var driver = drivers[i];

            //    var product = range.Where(z => z.Driver.Id == driver.Id);
            //    var cake = range.Where(z => z.Driver.Id == driver.Id);
            //    var price = range.Where(z => z.Driver.Id == driver.Id);
            //    var correct = range.Where(z => z.Driver.Id == driver.Id);
            //    var money = range.Where(z => z.Driver.Id == driver.Id);
            //    var difference = range.Where(z => z.Driver.Id == driver.Id);
            //    var after = range.Where(z => z.Driver.Id == driver.Id);

            //    var day = new Day
            //    {
            //        TotalPriceProducts = product.Sum(x => x.Day.TotalPriceProducts),
            //        TotalPriceCake = cake.Sum(x => x.Day.TotalPriceCake),
            //        TotalPrice = price.Sum(x => x.Day.TotalPrice),
            //        TotalPriceCorrect = correct.Sum(x => x.Day.TotalPriceCorrect),
            //        TotalPriceMoney = money.Sum(x => x.Day.TotalPriceMoney),
            //        TotalPriceDifference = difference.Sum(x => x.Day.TotalPriceDifference),
            //        TotalPriceAfterCorrect = after.Sum(x => x.Day.TotalPriceAfterCorrect)
            //    };
            //    SumRange[i] = new()
            //    {
            //        Driver = drivers[i],
            //        Day = day,
            //    };
            //}

            //return SumRange;

            return Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(range).ToArray();
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
                    var file = await JsonFile.GetFileJson<RangeDayM[]>(response.FullPath);
                    RangeDays = file;
                    TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
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
                    TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
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
                    
                    Table.RangeTable.OnSetRangeDayMs(RangeDays);

                    TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
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
