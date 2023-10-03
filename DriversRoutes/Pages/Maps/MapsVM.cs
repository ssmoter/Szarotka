using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Maui.Maps;

using Map = Microsoft.Maui.Controls.Maps.Map;

namespace DriversRoutes.Pages.Maps
{
    public partial class MapsVM : ObservableObject
    {
        [ObservableProperty]
        Map map;

        public MapsVM()
        {
            var location = new Location();
            Task.Run(async () =>
            {
                location = await Geolocation.GetLocationAsync();
            });

            MapSpan mapSpan = new MapSpan(location, 0.1, 0.1);


            Map = new Map(mapSpan);
            Map.IsShowingUser = true;


        }

    }
}
