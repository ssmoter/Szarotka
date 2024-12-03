using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model;

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
        DataBaseVersion version;


        public MainOptionsM()
        {
            Version = new DataBaseVersion();
        }
    }
}
