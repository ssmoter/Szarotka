using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Maps.MapSmall
{
    public partial class MapSmallM : ObservableObject
    {
        [ObservableProperty]
        bool changeLocation = true;
        [ObservableProperty]
        bool saveLocation = false;



        public MapSmallM()
        {
        }

        public double OldLongitude { get; set; }
        public double OldLatitude { get; set; }

    }
}
