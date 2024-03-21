using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Helper;

using System.Collections.ObjectModel;
using System.Reflection;

namespace Szarotka.Pages.Options.Main
{
    [QueryProperty(nameof(TypOfOptions), nameof(ListOfEnums.TypOfOptions))]
    public partial class MainOptionsVM : ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<string> themes;

        [ObservableProperty]
        MainOptionsM mainOptionsM;

        ListOfEnums.TypOfOptions typOfOptions;
        public ListOfEnums.TypOfOptions TypOfOptions
        {
            get => typOfOptions;
            set
            {
                if (SetProperty(ref typOfOptions, value))
                {
                    OnPropertyChanged(nameof(TypOfOptions));

                    SelectTypOfOptions(TypOfOptions);
                }
            }
        }

        string? isSelectedTheme;
        public string? IsSelectedTheme
        {
            get => isSelectedTheme;
            set
            {
                if (SetProperty(ref isSelectedTheme, value))
                {
                    OnPropertyChanged(nameof(IsSelectedTheme));
                    if (IsSelectedTheme is not null)
                        ChangeThema(IsSelectedTheme);
                }
            }
        }

        [ObservableProperty]
        string? appVersion;

        public DataBase.Data.AccessDataBase _db { get; private set; }
        public MainOptionsVM(DataBase.Data.AccessDataBase db)
        {
            MainOptionsM ??= new();
            SelectTypOfOptions(ListOfEnums.TypOfOptions.Main);
            var version = Assembly.GetExecutingAssembly()
                                .GetName().Version;
            if (version is not null)
                AppVersion = version.ToString();

            Themes =
                [
                    nameof(AppTheme.Unspecified)
                    ,
                    nameof(AppTheme.Light)
                    ,
                    nameof(AppTheme.Dark)
                ];

            _appThemes = new Dictionary<string, int>()
            {
                [nameof(AppTheme.Unspecified)] = (int)AppTheme.Unspecified,
                [nameof(AppTheme.Light)] = (int)AppTheme.Light,
                [nameof(AppTheme.Dark)] = (int)AppTheme.Dark
            };
            _db = db;
            MainOptionsM.Version = _db.DataBase.Table<DataBase.Model.DataBaseVersion>().FirstOrDefault();
        }

        #region Method

        Dictionary<string, int> _appThemes;
        void ChangeThema(string value)
        {
            var result = _appThemes[value];

            if (App.Current is not null)
                App.Current.UserAppTheme = (AppTheme)result;

            Preferences.Set("Theme", result);
        }
        private void SelectTypOfOptions(ListOfEnums.TypOfOptions options)
        {
            switch (options)
            {
                case ListOfEnums.TypOfOptions.Main:
                    {
                        MainOptionsM.Main = true;
                        MainOptionsM.Inventory = false;
                        MainOptionsM.DriversRoutes = false;
                    }
                    break;
                case ListOfEnums.TypOfOptions.Inventory:
                    {
                        MainOptionsM.Main = false;
                        MainOptionsM.Inventory = true;
                        MainOptionsM.DriversRoutes = false;
                    }
                    break;
                case ListOfEnums.TypOfOptions.DriversRoutes:
                    {
                        MainOptionsM.Main = false;
                        MainOptionsM.Inventory = false;
                        MainOptionsM.DriversRoutes = true;
                    }
                    break;
                default:
                    {
                        MainOptionsM.Main = true;
                        MainOptionsM.Inventory = false;
                        MainOptionsM.DriversRoutes = false;
                    }
                    break;
            }
        }

        #endregion


        #region Command

        [RelayCommand]
        void ChangeDisplayOptions(ListOfEnums.TypOfOptions options)
        {
            SelectTypOfOptions(options);
        }

        [RelayCommand]
        async Task GoToLogs()
        {
            await Shell.Current.GoToAsync(nameof(DataBase.Pages.Log.LogV));
        }
        #endregion

    }
}
