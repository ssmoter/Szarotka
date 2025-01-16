using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewPopUp;

public partial class SingleDayPreviewPopUpV : Popup
{
    private Day _day;
    public Day Day
    {
        get => _day;
        set
        {
            if (value is not null)
            {
                _day = value;
                OnPropertyChanged(nameof(Day));
                OnPropertyChanging(nameof(Day));
            }
        }
    }
    private string _driver;
    public string Driver
    {
        get => _driver;
        set
        {
            if (value is not null)
            {
                _driver = value;
                OnPropertyChanged(nameof(Driver));
                OnPropertyChanging(nameof(Driver));
            }
        }
    }
    public SingleDayPreviewPopUpV(Day day, string driver)
    {
        InitializeComponent();
        this.CanBeDismissedByTappingOutsideOfPopup = false;
        Day = day;
        Driver = driver;
    }

    private async void Button_Clicked_Close(object sender, EventArgs e)
    {
        await this.CloseAsync();
    }

    private async void Button_Clicked_Edit(object sender, EventArgs e)
    {
        var close = this.CloseAsync();
        var navigate = Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
            new Dictionary<string, object>()
            {
                [nameof(DataBase.Model.EntitiesInventory.Day)] = Day,

            });
        var toast = Toast.Make("Wczytywanie wybranego dnia");
        await Task.WhenAll(close, navigate, toast.Show());
    }
}