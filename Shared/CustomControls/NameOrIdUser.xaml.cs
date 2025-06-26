using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

namespace Shared.CustomControls;

public partial class NameOrIdUser : ContentView, IDisposable
{
    public static readonly BindableProperty UserIdProperty =
    BindableProperty.Create(
        nameof(UserId),
        typeof(Guid),
        typeof(NameOrIdUser),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NameOrIdUser view)
            {
                if (newValue is Guid id)
                {
                    if (Shared.Pages.UserDisplay.PopupUser.UserDisplayVPopup.Users.TryGetValue(id, out User value))
                    {
                        view.User = value.Name;
                    }
                    else
                    {
                        view.User = id.ToString();
                    }
                }
            }
        });

    public Guid UserId
    {
        get => (Guid)GetValue(UserIdProperty);
        set => SetValue(UserIdProperty, value);
    }

    public static readonly BindableProperty UserProperty =
    BindableProperty.Create(
        nameof(User),
        typeof(string),
        typeof(NameOrIdUser),
        propertyChanged: (bindable, oldValue, newValue) =>
        {

        });

    public string User
    {
        get => (string)GetValue(UserProperty);
        set => SetValue(UserProperty, value);
    }

    private static Action _ActionUser;
    private void SetName()
    {
        if (Shared.Pages.UserDisplay.PopupUser.UserDisplayVPopup.Users.TryGetValue(UserId, out User value))
        {
            User = value.Name;
        }
        else
        {
            User = UserId.ToString();
        }
    }


    public NameOrIdUser()
    {
        _ActionUser += SetName;
        InitializeComponent();
    }

    public void Dispose()
    {
        _ActionUser -= SetName;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label item) { return; }
        await item.BounceOnPressAsync();

        var popup = new Shared.Pages.UserDisplay.PopupUser.UserDisplayVPopup(UserId);
        await Shell.Current.ShowPopupAsync(popup);

        NameOrIdUser._ActionUser?.Invoke();
    }

}