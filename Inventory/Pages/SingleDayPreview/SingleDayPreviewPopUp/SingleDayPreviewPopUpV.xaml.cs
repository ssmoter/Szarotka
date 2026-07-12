using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewPopUp;

public partial class SingleDayPreviewPopUpV : Popup, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(Day), out object day))
        {
            if (day is Day _day)
            {
                Day = _day;
            }
        }
    }
    private Day _day;
    public Day Day
    {
        get => _day;
        set
        {
            if (value is not null)
            {
                OnPropertyChanging(nameof(Day));
                _day = value;
                OnPropertyChanged(nameof(Day));
            }
        }
    }
    public SingleDayPreviewPopUpV(Day day)
    {
        InitializeComponent();
        this.CanBeDismissedByTappingOutsideOfPopup = false;
        Day = day;
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