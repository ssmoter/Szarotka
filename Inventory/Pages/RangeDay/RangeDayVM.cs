using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Data.File;
using Inventory.Helper.Parse;
using Inventory.Model;
using Inventory.Model.MVVM;
using Inventory.Service;

using System.Collections.ObjectModel;

using Szarotka.Service;

namespace Inventory.Pages.RangeDay
{
    [QueryProperty(nameof(FilesPath), nameof(FilesPath))]
    public partial class RangeDayVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<RangeDayM> rangeDays;

        [ObservableProperty]
        DayM totalPriceOfRange;

        [ObservableProperty]
        ObservableCollection<DriverM> drivers;

        [ObservableProperty]
        bool enableSave;

        string filesPath;
        public string FilesPath
        {
            set
            {
                if (value is not null)
                {
                    filesPath = value;
                    var extension = Path.GetExtension(filesPath);
                    if (extension == FileManagerCSVJson.jsonTyp)
                    {
                        Task.Run(async () =>
                        {
                            RangeDays = await FileManagerCSVJson.GetFileJson<ObservableCollection<RangeDayM>>(filesPath);
                            TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                        });
                    }
                    if (extension == FileManagerCSVJson.csvTyp)
                    {
                        RangeDays = FileManagerCSVJson.GetFileCSV(filesPath);
                        TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                    }

                    EnableSave = true;
                }
            }
        }

        DriverM selectedDriverName;
        public DriverM SelectedDriverName
        {
            get => selectedDriverName;
            set
            {
                if (SetProperty(ref selectedDriverName, value))
                {
                    OnPropertyChanged(nameof(SelectedDriverName));
                    Task.Run(async () =>
                    {
                        try
                        {
                            RangeDays = await SelectDays(PopupDate.From, PopupDate.To, SelectedDriverName.Id);
                        }
                        catch (Exception ex)
                        {
                            _db.SaveLog(ex);
                        }
                    });
                }
            }
        }

        PopupDateModel PopupDate = new(DateTime.Today.Ticks, DateTime.Today.AddDays(1).Ticks);
        readonly DataBase.Data.AccessDataBase _db;
        readonly ISelectDayService _selectDayService;
        readonly ISaveDayService _dayService;
        public RangeDayVM(DataBase.Data.AccessDataBase db, ISelectDayService selectDay, ISaveDayService dayService)
        {
            RangeDays = new ObservableCollection<RangeDayM>();
            TotalPriceOfRange = new DayM();
            _db = db;
            _selectDayService = selectDay;


            var driver = _db.DataBase.Table<Driver>().ToArray();
            Drivers = new ObservableCollection<DriverM>
            {
                new DriverM()
                {
                    Name = "Kierowcy",
                    Id = Guid.Empty,
                }
            };
            for (int i = 0; i < driver.Length; i++)
            {
                Drivers.Add(driver[i].PareseAsDriverM());
            }
            SelectedDriverName = new DriverM() { Id = Guid.Empty, Name = "Kierowcy" };
            EnableSave = false;
            _dayService = dayService;
        }

        #region Method


        async Task<ObservableCollection<RangeDayM>> SelectDays(long from, long to, Guid selectedDriverName)
        {
            Day[] days = Array.Empty<Day>();
            if (selectedDriverName == Guid.Empty)
            {
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to).
                    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();
            }
            else
            {
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to && x.DriverGuid == selectedDriverName).
                    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();
            }

            var dayM = new ObservableCollection<RangeDayM>();
            for (int i = 0; i < days.Length; i++)
            {
                dayM.Add(new RangeDayM());
                dayM[i].DayM = days[i].ParseAsDayM();
                var guid = days[i].DriverGuid;
                var driver = await _db.DataBaseAsync.Table<Driver>().FirstOrDefaultAsync(x => x.Id == guid);
                if (driver is not null)
                {
                    dayM[i].Driver = driver.PareseAsDriverM();
                }
            }

            return dayM;
        }

        static DayM SumTotalPriceOfRange(ObservableCollection<RangeDayM> range)
        {
            var day = new DayM();
            var lastValue = Service.ProductUpdatePriceService.EnableUpdate;
            Service.ProductUpdatePriceService.EnableUpdate = false;

            day.TotalPriceProduct = range.Sum(x => x.DayM.TotalPriceProduct);
            day.TotalPriceCake = range.Sum(x => x.DayM.TotalPriceCake);
            day.TotalPrice = range.Sum(x => x.DayM.TotalPrice);
            day.TotalPriceCorrect = range.Sum(x => x.DayM.TotalPriceCorrect);
            day.TotalPriceMoney = range.Sum(x => x.DayM.TotalPriceMoney);
            day.TotalPriceDifference = range.Sum(x => x.DayM.TotalPriceDifference);
            day.TotalPriceAfterCorrect = range.Sum(x => x.DayM.TotalPriceAfterCorrect);

            Service.ProductUpdatePriceService.EnableUpdate = lastValue;
            return day;
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

        static PickOptions FileTypCSV()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { "csv" } },
                { DevicePlatform.Android, new[] { "csv" } }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }
        static PickOptions FileTypJson()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { "json", "txt" } },
                { DevicePlatform.Android, new[] { "json", "txt" } }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }

        string CreateFileName()
        {
            if (RangeDays.Count == 1)
            {
                return string.Join('_', "Szarotka", RangeDays[0].DayM.Created.ToString("dd.MM.yyyy"));
            }
            var from = RangeDays.FirstOrDefault().DayM.Created.ToString("dd.MM.yyyy");
            var to = RangeDays.LastOrDefault().DayM.Created.ToString("dd.MM.yyyy");
            return string.Join('_', "Szarotka", from, to);
        }

        static ObservableCollection<ExistingFiles.ExistingFilesM> GetExistingFiles(IList<string> values)
        {
            var response = new ObservableCollection<ExistingFiles.ExistingFilesM>();
            for (int i = 0; i < values.Count; i++)
            {
                response.Add(new ExistingFiles.ExistingFilesM()
                {
                    Path = values[i],
                    Name = Path.GetFileName(values[i])
                });

            }
            return response;
        }

        #endregion

        #region Command

        [RelayCommand]
        async Task OpenDetailpage(RangeDayM rangeDay)
        {
            try
            {
                rangeDay.DayM = await _selectDayService.GetDay(rangeDay.DayM.Id);

                await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Model.MVVM.DayM)] = rangeDay.DayM
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
                if (!await AndroidPermissionService.CheckAllPermissions())
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
                    var response = await FilePicker.PickAsync(FileTypJson());
                    if (response == null)
                        return;
                    var file = await FileManagerCSVJson.GetFileJson<ObservableCollection<RangeDayM>>(response.FullPath);
                    RangeDays = file;
                    TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                    EnableSave = true;
                }
                if (result == "Eksport")
                {
                    for (int i = 0; i < RangeDays.Count; i++)
                    {
                        RangeDays[i].DayM = await _selectDayService.GetDay(RangeDays[i].DayM.Id);
                    }
                    var name = CreateFileName();
                    var response = await FileManagerCSVJson.SaveFileJson(RangeDays, name);
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = name,
                        File = new ShareFile(response)
                    });
                }
                if (result == "Pliki")
                {
                    var files = FileManagerCSVJson.GetFilesPaths(FileManagerCSVJson.JsonFolder);
                    await Shell.Current.GoToAsync($"{nameof(ExistingFiles.ExistingFilesV)}?GetTyp={FileManagerCSVJson.JsonFolder}",
                        new Dictionary<string, object>
                        {
                            [nameof(ExistingFiles.ExistingFilesM)] = RangeDayVM.GetExistingFiles(files)
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
                if (!await AndroidPermissionService.CheckAllPermissions())
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
                    var response = await FilePicker.PickAsync(FileTypCSV());
                    if (response == null)
                        return;
                    var file = FileManagerCSVJson.GetFileCSV(response.FullPath);
                    RangeDays = file;
                    TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                    EnableSave = true;
                }
                if (result == "Eksport")
                {
                    for (int i = 0; i < RangeDays.Count; i++)
                    {
                        RangeDays[i].DayM = await _selectDayService.GetDay(RangeDays[i].DayM.Id);
                    }
                    var name = CreateFileName();
                    var response = FileManagerCSVJson.SaveFileCSV(RangeDays, name);
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = name,
                        File = new ShareFile(response)
                    });
                }
                if (result == "Pliki")
                {
                    var files = FileManagerCSVJson.GetFilesPaths(FileManagerCSVJson.CsvFolder);
                    await Shell.Current.GoToAsync($"{nameof(ExistingFiles.ExistingFilesV)}?GetTyp={FileManagerCSVJson.CsvFolder}",
                        new Dictionary<string, object>
                        {
                            [nameof(ExistingFiles.ExistingFilesM)] = RangeDayVM.GetExistingFiles(files)
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
                for (int i = 0; i < RangeDays.Count; i++)
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
                var popup = new PopupSelectRangeDate.PopupSelectRangeDateV();
                var result = await Shell.Current.ShowPopupAsync(popup);

                if (result is PopupDateModel model)
                {
                    PopupDate = model;
                    RangeDays = await SelectDays(PopupDate.From, PopupDate.To, SelectedDriverName.Id);

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

        #endregion


    }
}
