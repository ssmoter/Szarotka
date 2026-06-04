using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

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


    public double MaxMyWidth { get; set; }

    protected override Size ArrangeOverride(Rect bounds)
    {
        MaxMyWidth = bounds.Width;
        OnPropertyChanged(nameof(MaxMyWidth));
        OnPropertyChanging(nameof(MaxMyWidth));

        return base.ArrangeOverride(bounds);
    }

    public UserDisplaySmallV()
    {
        //User ??= new();
        InitializeComponent();
    }

    private async void TapGestureRecognizer_Tapped_UserEdit_Popup(object sender, TappedEventArgs e)
    {
        if (sender is not Label item) { return; }
        await item.BounceOnPressAsync();

        var userId = User.Id;
        var popup = new Shared.Pages.UserDisplay.PopupUser.UserDisplayVPopup(userId);
        if (Application.Current?.Windows[0].Page != null)
        {
            await Application.Current.Windows[0].Page.ShowPopupAsync(popup);
        }
    }
}