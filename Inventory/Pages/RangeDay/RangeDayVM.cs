﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Service;

using Inventory.Data.File;
using Inventory.Helper.Parse;
using Inventory.Model;
using Inventory.Model.MVVM;
using Inventory.Service;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Inventory.Pages.RangeDay
{
    [QueryProperty(nameof(FilesPath), nameof(FilesPath))]
    public partial class RangeDayVM : ObservableObject
    {
        [ObservableProperty]
        RangeDayM[] rangeDays;

        [ObservableProperty]
        DayM totalPriceOfRange;

        readonly Driver[] _allDrivers;

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
                            RangeDays = await FileManagerCSVJson.GetFileJson<RangeDayM[]>(filesPath);
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


        PopupDateModel PopupDate = new(DateTime.Today.Ticks, DateTime.Today.AddDays(1).Ticks, false, Array.Empty<Guid>());
        readonly DataBase.Data.AccessDataBase _db;
        readonly ISelectDayService _selectDayService;
        readonly ISaveDayService _dayService;
        public RangeDayVM(DataBase.Data.AccessDataBase db, ISelectDayService selectDay, ISaveDayService dayService)
        {
            TotalPriceOfRange = new DayM();
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
            Day[] days = Array.Empty<Day>();
            if (selectedDriverName is null || selectedDriverName.Length == 0)
            {
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to).
                    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();
            }
            else
            {
                var sb = new StringBuilder();

                sb.Append("SELECT * FROM Day ");
                sb.Append("WHERE CreatedTicks >= ");
                sb.Append(from);
                sb.Append(" AND CreatedTicks <= ");
                sb.Append(to);

                if (selectedDriverName.Length > 0)
                {
                    sb.Append(" AND (");
                }

                for (int i = 0; i < selectedDriverName.Length; i++)
                {
                    if (i == 0)
                        sb.Append(" DriverGuid == '");
                    else if (i < selectedDriverName.Length)
                        sb.Append(" OR DriverGuid == '");

                    sb.Append(selectedDriverName[i]);
                    sb.Append('\'');
                }

                if (selectedDriverName.Length > 0)
                {
                    sb.Append(" )");
                }
                sb.Append("ORDER BY CreatedTicks DESC");

                var result = await _db.DataBaseAsync.QueryAsync<Day>(sb.ToString());
                days = result.ToArray();
                result.Clear();

                //days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                //    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to && x.DriverGuid == selectedDriverName).
                //    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();

            }

            RangeDayM[] dayM = new RangeDayM[days.Length];
            for (int i = 0; i < days.Length; i++)
            {
                dayM[i] = new RangeDayM();
                dayM[i].DayM = days[i].ParseAsDayM();
                var guid = days[i].DriverGuid;
                var driver = await _db.DataBaseAsync.Table<Driver>().FirstOrDefaultAsync(x => x.Id == guid);
                if (driver is not null)
                {
                    dayM[i].Driver = driver.PareseAsDriverM();
                }
            }

            if (moreData)
            {
                for (int i = 0; i < dayM.Length; i++)
                {
                    dayM[i].DayM = await _selectDayService.GetDayProcedure(dayM[i].DayM.Id);
                }
            }

            return dayM;
        }

        static DayM SumTotalPriceOfRange(RangeDayM[] range)
        {
            var day = new DayM
            {
                TotalPriceProduct = range.Sum(x => x.DayM.TotalPriceProduct),
                TotalPriceCake = range.Sum(x => x.DayM.TotalPriceCake),
                TotalPrice = range.Sum(x => x.DayM.TotalPrice),
                TotalPriceCorrect = range.Sum(x => x.DayM.TotalPriceCorrect),
                TotalPriceMoney = range.Sum(x => x.DayM.TotalPriceMoney),
                TotalPriceDifference = range.Sum(x => x.DayM.TotalPriceDifference),
                TotalPriceAfterCorrect = range.Sum(x => x.DayM.TotalPriceAfterCorrect)
            };

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
            if (RangeDays.Length == 1)
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
                    var response = await FilePicker.PickAsync(FileTypJson());
                    if (response == null)
                        return;
                    var file = await FileManagerCSVJson.GetFileJson<RangeDayM[]>(response.FullPath);
                    RangeDays = file;
                    TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                    EnableSave = true;
                }
                if (result == "Eksport")
                {
                    for (int i = 0; i < RangeDays.Length; i++)
                    {
                        if (RangeDays[i].DayM.Products.Count <= 0)
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
                    for (int i = 0; i < RangeDays.Length; i++)
                    {
                        if (RangeDays[i].DayM.Products.Count <= 0)
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
