using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model;

using Shared.Data;

using System.Collections.ObjectModel;
using System.Reflection;

namespace SzarotkaNET10.Pages.Options.Main;

public partial class MainOptionsVM : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(TypOfOptions), out object? typOfOptions))
        {
            if (typOfOptions is TypOfOptions _typOfOptions)
            {
                Options = _typOfOptions;
            }
        }
    }


    private string serverUrl;
    public string ServerUrl
    {
        get => serverUrl;
        set
        {
            if (SetProperty(ref serverUrl, value, nameof(ServerUrl))) { }
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

    TypOfOptions options;
    public TypOfOptions Options
    {
        get => options;
        set
        {
            if (SetProperty(ref options, value, nameof(Options)))
            {
                SelectTypOfOptions(Options);
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
                    ChangeTheme(IsSelectedTheme);
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

    public IAccessDataBaseAoT _db { get; private set; }
    public MainOptionsVM(IAccessDataBaseAoT db, IHttpClientFactory httpClientFactory)
    {
        MainOptionsM ??= new();
        SelectTypOfOptions(TypOfOptions.Main);
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

        MainOptionsM.Version = CreatedDataBase.GetDataBaseVersion(_db);
        ServerUrl = httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka).BaseAddress!.ToString();
    }



    Dictionary<string, int> _appThemes;
    void ChangeTheme(string value)
    {
        var result = _appThemes[value];

        if (App.Current is not null)
            App.Current.UserAppTheme = (AppTheme)result;

        Preferences.Set("Theme", result);
    }
    private void SelectTypOfOptions(TypOfOptions options)
    {
        switch (options)
        {
            case TypOfOptions.Main:
                {
                    MainOptionsM.Main = true;
                    MainOptionsM.Inventory = false;
                    MainOptionsM.DriversRoutes = false;
                }
                break;
            case TypOfOptions.Inventory:
                {
                    MainOptionsM.Main = false;
                    MainOptionsM.Inventory = true;
                    MainOptionsM.DriversRoutes = false;
                }
                break;
            case TypOfOptions.DriversRoutes:
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

    [RelayCommand]
    void ChangeDisplayOptions(TypOfOptions options)
    {
        SelectTypOfOptions(options);
    }

    [RelayCommand]
    async Task GoToLogs()
    {
        await Shell.Current.GoToAsync(nameof(Shared.Pages.Log.LogV));
    }
}
public enum TypOfOptions
{
    Main,
    Inventory,
    DriversRoutes,
}

