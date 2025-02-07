using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model;

using Shared.Helper;

using System.Collections.ObjectModel;
using System.Reflection;

namespace SzarotkaBlazor.Pages.Options.Main;

public partial class MainOptionsVM : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(ListOfEnums.TypOfOptions), out object? typOfOptions))
        {
            if (typOfOptions is ListOfEnums.TypOfOptions _typOfOptions)
            {
                TypOfOptions = _typOfOptions;
            }
        }
    }

    private ObservableCollection<string> themes = [];
    public ObservableCollection<string> Themes
    {
        get => themes;
        set
        {
            if (SetProperty(ref themes, value, nameof(Themes))) { }
        }
    }

    private MainOptionsM mainOptionsM = new();
    public MainOptionsM MainOptionsM
    {
        get => mainOptionsM;
        set
        {
            if (SetProperty(ref mainOptionsM, value, nameof(MainOptionsM))) { }
        }
    }

    ListOfEnums.TypOfOptions typOfOptions;
    public ListOfEnums.TypOfOptions TypOfOptions
    {
        get => typOfOptions;
        set
        {
            if (SetProperty(ref typOfOptions, value, nameof(TypOfOptions)))
            {
                //OnPropertyChanged(nameof(TypOfOptions));

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
            if (SetProperty(ref isSelectedTheme, value, nameof(IsSelectedTheme)))
            {
                //OnPropertyChanged(nameof(IsSelectedTheme));
                if (IsSelectedTheme is not null)
                    ChangeThema(IsSelectedTheme);
            }
        }
    }

    private string? appVersion;
    public string? AppVersion
    {
        get => appVersion;
        set
        {
            if (SetProperty(ref appVersion, value, nameof(AppVersion))) { }
        }
    }

    public IAccessDataBase _db { get; private set; }
    public MainOptionsVM(IAccessDataBase db)
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
        MainOptionsM.Version = _db.DataBase.Table<DataBaseVersion>().FirstOrDefault();
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
        await Shell.Current.GoToAsync(nameof(Shared.Pages.Log.LogV));
    }
    #endregion

}

