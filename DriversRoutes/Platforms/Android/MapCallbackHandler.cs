using Android.Gms.Maps;

using Microsoft.Maui.Maps.Handlers;

namespace DriversRoutes.Platforms.Android
{
    class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly IMapHandler mapHandler;

        public MapCallbackHandler(IMapHandler mapHandler)
        {
            this.mapHandler = mapHandler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mapHandler.UpdateValue(nameof(Microsoft.Maui.Maps.IMap.Pins));
        }
    }

}
