using CommunityToolkit.Mvvm.ComponentModel;

namespace SzarotkaBlazor.Pages.Options.Main
{
    public partial class MainOptionsM : ObservableObject
    {
        [ObservableProperty]
        bool main;
        [ObservableProperty]
        bool inventory;
        [ObservableProperty]
        bool driversRoutes;

        [ObservableProperty]
        DataBase.Model.DataBaseVersion version;


        public MainOptionsM()
        {
            Version = new DataBase.Model.DataBaseVersion();
        }
    }
}
