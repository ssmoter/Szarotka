using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Shared.Helper;
using DataBase.Model;
using System.Collections.ObjectModel;
using DataBase.Data;
using Shared.Data;

namespace Shared.Pages.Log
{
    public partial class LogVM : ObservableObject, IDisposable
    {
        [ObservableProperty]
        ObservableCollection<LogM> logs;

        readonly AccessDataBase _db;
        public LogVM(AccessDataBase db)
        {
            Logs = [];
            _db = db;
        }
        int take = 1;
        public async Task GetLogs()
        {
            try
            {

                var table = await _db.DataBaseAsync.Table<LogsModel>().OrderByDescending(x => x.Id).Take(10 * take).ToArrayAsync();
                if (Logs.Count != table.Length)
                {
                    Logs.Clear();
                    for (int i = 0; i < table.Length; i++)
                    {
                        Logs.Add(table[i].ParseAsLogM());
                    }
                }
                table = null;
                take++;
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task OpenSelectedLog(LogM log)
        {
            try
            {
                if (log is null)
                {
                    return;
                }
                await Shell.Current.GoToAsync($"{nameof(LogData.LogDataV)}?",
                    new Dictionary<string, object>
                    {
                        [nameof(LogM)] = log
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }

        }
        [RelayCommand]
        async Task DeleteLog(LogM log)
        {
            try
            {

                if (log is null)
                {
                    return;
                }
                var delete = log.ParseAsLog();
                await _db.DataBaseAsync.DeleteAsync(delete);
                Logs.Remove(log);
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }
        [RelayCommand]
        async Task GetMoreLogs()
        {
            await GetLogs();
        }
        [RelayCommand]
        async Task Back()
        {
            Dispose();
            await Shell.Current.GoToAsync("..");
        }


        public void Dispose() => Logs.Clear();
    }
}
