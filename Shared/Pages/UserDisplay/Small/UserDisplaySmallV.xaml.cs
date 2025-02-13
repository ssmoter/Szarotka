using DataBase.Model.EntitiesServer;

namespace Shared.Pages.UserDisplay.Small;


public partial class UserDisplaySmallV : ContentView
{

    public static readonly BindableProperty UserProperty
    = BindableProperty.Create(nameof(User), typeof(User), typeof(UserDisplaySmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public User User
    {
        get => (User)GetValue(UserProperty);
        set => SetValue(UserProperty, value);
    }


    public UserDisplaySmallV()
    {
        //User ??= new();
        InitializeComponent();
    }
}