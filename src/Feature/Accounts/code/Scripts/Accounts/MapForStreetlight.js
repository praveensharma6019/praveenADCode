// In the following example, markers appear when the user clicks on the map.
// The markers are stored in an array.
// The user can then click an option to hide, show or delete the markers.
let map;
let markers = [];

function initMap() {
    const initialLocation = { lat: 18.969539, lng: 72.8171403 };
    map = new google.maps.Map(document.getElementById("gmap"), {
        zoom: 12,
        center: initialLocation,
        mapTypeId: "terrain"
    });
    // This event listener will call addMarker() when the map is clicked.
    map.addListener("click", (event) => {
        // get lat/lon of click
        var clickLat = event.latLng.lat();
        var clickLon = event.latLng.lng();

        // show in input box
        document.getElementById("latCoordinate").value = clickLat.toFixed(5);
        document.getElementById("longCoordinate").value = clickLon.toFixed(5);

        deleteMarkers();
        addMarker(event.latLng);
    });
    // Adds a marker at the center of the map.
    addMarker(haightAshbury);
}

// Adds a marker to the map and push to the array.
function addMarker(location) {
    const marker = new google.maps.Marker({
        position: location,
        map: map
    });
    markers.push(marker);
}

// Sets the map on all markers in the array.
function setMapOnAll(map) {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers() {
    setMapOnAll(null);
}

// Shows any markers currently in the array.
function showMarkers() {
    setMapOnAll(map);
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers() {
    clearMarkers();
    markers = [];
}