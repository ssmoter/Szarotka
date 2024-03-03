using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Customer.CustomerSmall.DayOfWeek;

public partial class DayOfWeekV : ContentView
{
    public static readonly BindableProperty DayOfWeekProperty
    = BindableProperty.Create(nameof(DayOfWeek), typeof(SelectedDayOfWeekRoutes), typeof(DayOfWeekV), propertyChanged: (bindable, oldValu, newValue) =>
    {

    });
    public SelectedDayOfWeekRoutes DayOfWeek
    {
        get => (SelectedDayOfWeekRoutes)GetValue(DayOfWeekProperty);
        set => SetValue(DayOfWeekProperty, value);
    }



    public DayOfWeekV()
    {
        InitializeComponent();
    }


    private async void TapGestureRecognizer_Tapped_CopyDayOfWeek(object sender, TappedEventArgs e)
    {
        await CopyDayOfWeek();
    }
    private async Task CopyDayOfWeek()
    {
        await Clipboard.SetTextAsync(DayOfWeek.ToStringWithTheTime());
        await DataBase.ToastNotifications.ToastNotification.MakeToast("Skopiowano dni przyjazdu");
    }

}