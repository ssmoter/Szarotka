using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Maui.Controls.Maps;

using System.Collections.ObjectModel;

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
