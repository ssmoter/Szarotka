using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace Szarotka.Pages.Options.Main
{
    public partial class MainOptionsVM : ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<string> themes;

        string isSelectedTheme;
        public string IsSelectedTheme
        {
            get => isSelectedTheme;
            set
            {
                if (SetProperty(ref isSelectedTheme, value))
                {
                    OnPropertyChanged(nameof(IsSelectedTheme));
                    ChangeThema(IsSelectedTheme);
                }
            }
        }

        public MainOptionsVM()
        {
            Themes = new ObservableCollection<string>()
                {
                    nameof(AppTheme.Unspecified)
                    ,nameof(AppTheme.Light)
                    ,nameof(AppTheme.Dark)
                };

            _appThemes = new Dictionary<string, int>()
            {
                [nameof(AppTheme.Unspecified)] = (int)AppTheme.Unspecified
                ,
                [nameof(AppTheme.Light)] = (int)AppTheme.Light
                ,
                [nameof(AppTheme.Dark)] = (int)AppTheme.Dark

            };
        }

        #region Method

        Dictionary<string, int> _appThemes;
        void ChangeThema(string value)
        {
            var result = _appThemes[value];

            App.Current.UserAppTheme = (AppTheme)result;
            Preferences.Set("Theme", result);
        }

        #endregion


        #region Command


        [RelayCommand]
        async Task GoToLogs()
        {
            await Shell.Current.GoToAsync(nameof(DataBase.Pages.Log.LogV));
        }
        #endregion

    }
}
