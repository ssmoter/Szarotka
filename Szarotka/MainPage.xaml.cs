using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

using Szarotka.Pages.Options.Main;

namespace Szarotka
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {

            count++;
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        private async void DataBase_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MainOptionsV));
        }

        private async void Inventory_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Inventory.Pages.Main.MainV));
        }
    }
}