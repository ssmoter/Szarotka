using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model;

using Shared.Data;
using Shared.Helper;

using System.Collections.ObjectModel;

namespace Shared.Pages.Log
{
    public partial class LogVM : ObservableObject, IDisposable
    {
        private ObservableCollection<LogM> logs;
        public ObservableCollection<LogM> Logs
        {
            get => logs;
            set
            {
                if (SetProperty(ref logs, value))
                {
                    OnPropertyChanged(nameof(Logs));
                }
            }
        }

        readonly IAccessDataBaseAoT _db;
        public LogVM(IAccessDataBaseAoT db)
        {
            Logs = [];
            _db = db;
        }
        int take = 1;
        public async Task GetLogs()
        {
            try
            {
                string sql = $"SELECT * FROM LogsModel ORDER BY Id DESC LIMIT {10 * take}";

                var itable = await _db.DbAsyncAoT.QueryAsync<LogsModel>(sql);
                var table = itable.ToArray();
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
