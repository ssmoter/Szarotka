using CommunityToolkit.Maui.Views;

using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Data.ServerHttpClients;

namespace Shared.Pages.UserDisplay.PopupUser;

public partial class UserDisplayVPopup : Popup
{

    private bool isRefreshing = true;
    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set
        {
            if (IsRefreshing != value)
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
                OnPropertyChanging(nameof(IsRefreshing));
            }
        }
    }

    private User user;
    public User User
    {
        get { return user; }
        set
        {
            if (User != value)
            {
                user = value;
                OnPropertyChanged(nameof(User));
                OnPropertyChanging(nameof(User));
            }
        }
    }




    public UserDisplayVPopup(User user)
    {
        Init();
        User = user;
        Users.TryAdd(user.Id, user);
    }

    public UserDisplayVPopup(Guid id)
    {
        Init();
        GetUserFireAndForget(id);

    }

    private IAccessDataBase _db;
    public static Dictionary<Guid, User> Users { get; } = [];
    private ILoginHttp _loginHttp;
    private void Init()
    {
        InitializeComponent();

        IsRefreshing = true;

        this.CanBeDismissedByTappingOutsideOfPopup = false;

        _db = Shared.Service.AppServiceProvider.GetService<IAccessDataBase>();
        _loginHttp = Shared.Service.AppServiceProvider.GetService<ILoginHttp>();
    }


    private async void GetUserFireAndForget(Guid id)
    {
        try
        {
            if (Users.TryGetValue(id, out User value))
            {
                User = value;
            }
            else
            {
                User = await GetUserFromServer(id);
                Users.TryAdd(user.Id, user);
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private async Task<User> GetUserFromServer(Guid id)
    {

        var user = await _loginHttp.GetPublicUser(id);

        return user;
    }

    private async void Button_Clicked_Close(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}