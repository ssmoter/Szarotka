using DataBase.Model.EntitiesRoutes;

using Microsoft.JSInterop;

namespace DriversRoutes.Pages.Maps.Controls
{
    public partial class BlazorMap
    {
        public static Action AfterOnInitializedAsync { get; set; }
        public void Dispose()
        {
            _setCustomerAction -= _setCustomer;
            _setAdvancedMarkerAction -= _setAdvancedMarker;
            _setAdvancedMarkerIconAction -= _setAdvancedMarkerIcon;
            _removeAdvancedMarkerAction -= _removeAdvancedMarker;

            _addDirectionsAction -= _addDirections;
            _removeDirectionsAction -= _removeDirections;

            SetCenter -= SetMyLocation;
            MapDragStart -= StopLisiningLocation;

            _fitMapToAdvancedMarkersAction -= _fitMapToAdvancedMarkers;

            DataBase.Helper.MudBlazorTheme.ActionThemeChanged -= SetIconTheme;
            RemoveListener("dragstart");
        }

        public BlazorMap()
        {
            _setCustomerAction += _setCustomer;
            _setAdvancedMarkerAction += _setAdvancedMarker;
            _setAdvancedMarkerIconAction += _setAdvancedMarkerIcon;
            _removeAdvancedMarkerAction += _removeAdvancedMarker;

            _addDirectionsAction += _addDirections;
            _removeDirectionsAction += _removeDirections;

            SetCenter += SetMyLocation;
            MapDragStart += StopLisiningLocation;

            _fitMapToAdvancedMarkersAction += _fitMapToAdvancedMarkers;


            DataBase.Helper.MudBlazorTheme.ActionThemeChanged += SetIconTheme;
            DataBase.Helper.MudBlazorTheme.SetCurrentTheme(AppInfo.Current.RequestedTheme);
            DataBase.Helper.MudBlazorTheme.ActionThemeChanged?.Invoke();

        }



        private ValueTask InitMat(string mapId, double lat, double lng, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("initMap", token, mapId, lat.ToString(dot), lng.ToString(dot));
        }
        private ValueTask CalculateRoute(string start, string end, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("calcRoute", token, start, end);
        }
        private ValueTask SetMapCenter(double lat, double lng, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("changeCenter", token, lat.ToString(dot), lng.ToString(dot));
        }
        private ValueTask AddListener(string listener, string methodName, string assemblyName, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("addListener", token, listener, methodName, assemblyName);
        }
        private ValueTask RemoveListener(string listener, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("removeListener", token, listener);
        }
        private ValueTask AddAdvancedMarker(string id, double lat, double lng, string userIcon, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("addAdvancedMarker", token, id, lat.ToString(dot), lng.ToString(dot), userIcon);
        }
        private ValueTask RemoveAdvancedMarker(string id, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("removeAdvancedMarker", token, id);
        }
        private ValueTask SetIconAdvancedMarker(string id, string userIcon, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("setIconAdvancedMarkerSvg", token, id, userIcon);
        }
        private ValueTask SetPositonAdvancedMarker(string id, double lat, double lng, CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("setPositionAdvancedMarker", token, id, lat.ToString(dot), lng.ToString(dot));
        }
        private ValueTask RemoveCalculateRoute(CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("clearRoute", token);
        }
        private ValueTask FitMapToAdvancedMarkers(CancellationToken token = default)
        {
            return _module.InvokeVoidAsync("fitMapToMarkers", token);
        }


        System.Globalization.NumberFormatInfo dot = new System.Globalization.NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
        };

        #region CustomerRoutesAction

        private CustomerRoutes _customer;

        private static event Action<CustomerRoutes> _setCustomerAction;
        private void _setCustomer(CustomerRoutes customer)
        {
            _customer = customer;
        }
        public static void OnSetCustomer(CustomerRoutes customer = null)
        {
            _setCustomerAction?.Invoke(customer);
        }

        private static event Action<CustomerRoutes, string> _setAdvancedMarkerAction;
        private void _setAdvancedMarker(CustomerRoutes customer = null, string text = "")
        {
            if (customer is null)
            {
                if (_customer is null)
                {
                    return;
                }
                customer = _customer;
            }

            var lat = customer.Latitude;
            var lng = customer.Longitude;
            var img = Data.DrawIconOnMap.CreatePinSvg(text);
            AddAdvancedMarker(customer.Id.ToString(), lat, lng, img);

        }
        /// <summary>
        /// Dodaje advanced marker z dana pozycją
        /// </summary>
        /// <param name="customer">Jeżeli customer nie zostanie ustawiony zostanie dodany marker z prywatnego pola</param>
        public static void OnSetAdvancedMarker(CustomerRoutes customer = null, string text = "")
        {
            _setAdvancedMarkerAction?.Invoke(customer, text);
        }

        private static event Action<CustomerRoutes> _removeAdvancedMarkerAction;
        private void _removeAdvancedMarker(CustomerRoutes customer = null)
        {
            if (customer is null)
            {
                if (_customer is null)
                {
                    return;
                }
                customer = _customer;
            }
            RemoveAdvancedMarker(customer.Id.ToString());
        }
        /// <summary>
        /// Usuwa Advanced marker
        /// </summary>
        /// <param name="customer">Jeżeli customer nie zostanie ustawiony zostanie wybrany z prywatnego pola</param>
        public static void OnRemoveAdvancedMarker(CustomerRoutes customer = null)
        {
            _removeAdvancedMarkerAction?.Invoke(customer);
        }


        private static event Action<CustomerRoutes, string> _setAdvancedMarkerIconAction;
        private void _setAdvancedMarkerIcon(CustomerRoutes customer = null, string icon = "")
        {
            if (customer is null)
            {
                if (_customer is null)
                {
                    return;
                }
                customer = _customer;
            }
            SetIconAdvancedMarker(customer.Id.ToString(), icon);
        }
        /// <summary>
        /// Zmienia icone advance marker
        /// </summary>
        /// <param name="customer">Jeżeli customer nie zostanie ustawiony zostanie ustawiony z prywatnego pola</param>
        /// <param name="icon">icona jako svg</param>
        public static void OnSetAdvancedMarkerIcon(CustomerRoutes customer = null, string icon = "")
        {
            _setAdvancedMarkerIconAction?.Invoke(customer, icon);
        }




        private static event Func<CustomerRoutes, CancellationToken, Task> _addDirectionsAction;
        private async Task _addDirections(CustomerRoutes customer = null, CancellationToken token = default)
        {
            if (customer is null)
            {
                if (_customer is null)
                {
                    return;
                }
                customer = _customer;
            }

            var currentLocation = await Helper.CurrentLocation.Get(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));

            var latLngStart = $"{currentLocation.Center.Latitude.ToString(dot)},{currentLocation.Center.Longitude.ToString(dot)}";

            var latLngEnd = $"{customer.Latitude.ToString(dot)},{customer.Longitude.ToString(dot)}";

            await CalculateRoute(latLngStart, latLngEnd, token);

        }
        /// <summary>
        /// Dodaje trasę z twojej aktualnej pozycji do wybranego puntku
        /// </summary>
        /// <param name="customer">Jeżeli customer nie zostanie ustawiony zostanie wybrany puntk z prywatnego pola</param>
        /// <returns></returns>
        public static async Task OnAddDirections(CustomerRoutes customer = null, CancellationToken token = default)
        {
            await _addDirectionsAction?.Invoke(customer, token);
        }

        private static event Action<CancellationToken> _removeDirectionsAction;
        private void _removeDirections(CancellationToken token = default)
        {
            RemoveCalculateRoute(token);
        }
        public static void OnRemoveDrirections(CancellationToken token = default)
        {
            _removeDirectionsAction?.Invoke(token);
        }

        private static event Action<CancellationToken> _fitMapToAdvancedMarkersAction;
        private void _fitMapToAdvancedMarkers(CancellationToken token = default)
        {
            FitMapToAdvancedMarkers(token);
        }
        public static void OnFitMapToAdvancedMarkers(CancellationToken token = default)
        {
            _fitMapToAdvancedMarkersAction?.Invoke(token);
        }


        #endregion




    }
}