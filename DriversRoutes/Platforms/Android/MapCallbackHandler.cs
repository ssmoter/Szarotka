using Android.Gms.Maps;

using Microsoft.Maui.Maps.Handlers;

namespace DriversRoutes.Platforms.Android
{
    class MapCallbackHandler(IMapHandler mapHandler) : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly IMapHandler mapHandler = mapHandler;

        public void OnMapReady(GoogleMap googleMap)
        {
            mapHandler.UpdateValue(nameof(Microsoft.Maui.Maps.IMap.Pins));
        }
    }

}
