let map, directionsService, directionsRenderer;
let valueListener = [];
let valueAdvancedMarkers = [];

export async function initMap(elementId, lat, lng) {
    try {

        const { Map } = await google.maps.importLibrary("maps");
        const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

        let center = new google.maps.LatLng(lat, lng);
        let mapOptions = {
            zoom: 15,
            center: center,
            mapId: "mapId",
        };

        map = new google.maps.Map(document.getElementById(elementId), mapOptions);
        directionsService = new google.maps.DirectionsService();
        directionsRenderer = new google.maps.DirectionsRenderer();
        directionsRenderer.setMap(map);


    } catch (e) {
        console.error("Error when map was init ", e);
    }


}
export function calcRoute(start, end) {
    try {
        var request = {
            origin: start,
            destination: end,
            travelMode: 'DRIVING',
            language: "pl"
        };

        directionsService.route(request, function (result, status) {
            if (status == 'OK') {
                directionsRenderer.setDirections(result);
                console.debug("Route Direction is set");
            }
        });
    } catch (e) {
        console.error("Error when calculate route " + e);
    }

}
export function clearRoute() {
    if (directionsRenderer) {
        directionsRenderer.setDirections({ routes: [] });
    }
}


export function changeCenter(lat, lng) {
    try {
        let newCenter = new google.maps.LatLng(lat, lng);
        map.setCenter(newCenter);
        console.debug("Map set center " + lat + " " + lng);
    } catch (e) {
        console.error("Error when map set center " + e);
    }

}
export function addListener(lisinerName, methodName, assemblyName) {
    try {
        let listener = map.addListener(lisinerName, function () {
            DotNet.invokeMethodAsync(assemblyName, methodName);
            console.debug("Listener invoke " + lisinerName);
        });
        valueListener.push(listener);
        console.debug(listener);
        console.debug("Listener add to list " + lisinerName);
    } catch (e) {
        console.error("Error when listener was add to map " + e);
    }

}
export function removeListener(listenerName) {
    try {
        let index = findListener(valueListener, listenerName)
        if (index == -1) {
            console.debug("Listener not found")
            return;
        }
        google.maps.event.removeListener(valueListener[index]);
        console.debug("Listener remove  from map" + listenerName);
        removeItemAll(valueListener, index);
        console.debug("Listener remove from list " + listenerName);
    } catch (e) {
        console.error("Error when listener was remove from map" + e);
    }

}

function removeItemAll(arr, index) {
    let i = 0;
    while (i < arr.length) {
        if (i === index) {
            arr.splice(i, 1);
        } else {
            ++i;
        }
    }
    return arr;
}
function findListener(arr, value) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].Eg == value) {
            return i;
        }
    }
    return -1;
}


export function addAdvancedMarker(id, lat, lng, userIcon) {

    try {
        let latLng = new google.maps.LatLng(lat, lng);
        let parser = new DOMParser();
        let pinSvg = parser.parseFromString(
            userIcon,
            "image/svg+xml",
        ).documentElement;

        let marker = new google.maps.marker.AdvancedMarkerElement({
            position: latLng,
            map: map,
            content: pinSvg,
        });
        marker.customId = id;
        console.debug("Map add advance marker " + id);
        valueAdvancedMarkers.push(marker);
        console.debug("Map Add advanced marker to list " + id);
    }
    catch (error) {
        console.error("Error when Advanced marker was created:", error);
    }
}
export function removeAdvancedMarker(id) {
    try {
        console.debug(valueAdvancedMarkers);
        let index = findMarker(valueAdvancedMarkers, id);
        let marker = valueAdvancedMarkers[index];
        marker.setMap(null);
        removeItemAll(valueAdvancedMarkers, index);
        console.debug("Advanced marker delete " + id)
    }
    catch (e) {
        console.error("Error when remove adwanced marker " + e);
    }
}
export function setIconAdvancedMarkerSvg(id, userIcon) {
    try {
        let index = findMarker(valueAdvancedMarkers, id);
        let marker = valueAdvancedMarkers[index];

        let parser = new DOMParser();
        let pinSvg = parser.parseFromString(
            userIcon,
            "image/svg+xml",
        ).documentElement;

        marker.content = pinSvg;
        marker.draw();
        console.debug("Advanced marker set new icon " + id)
    }
    catch (e) {
        console.error("Error when Advanced marker set new icon " + e);
    }

}
export function setPositionAdvancedMarker(id, lat, lng) {
    try {
        let index = findMarker(valueAdvancedMarkers, id);
        let marker = valueAdvancedMarkers[index];
        let latLng = new google.maps.LatLng(lat, lng);

        marker.position = latLng,
            marker.draw();
        console.debug("Advanced marker set new position " + id)
    } catch (e) {
        console.error("Error when Advanced marker set new position " + e);
    }

}

function findMarker(arr, value) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].customId == value) {
            return i;
        }
    }
    return -1;
}

export function fitMapToMarkers() {
    try {
        let bounds = new google.maps.LatLngBounds();
        valueAdvancedMarkers.forEach(marker => bounds.extend(marker.position));
        map.fitBounds(bounds);
    } catch (e) {
        console.error("Error when bounds pins", e);
    }
}