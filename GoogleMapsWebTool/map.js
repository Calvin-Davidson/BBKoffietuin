let map;

var AmsterdamCoordinates = {
    lat: 52.39060538838168,
    lng: 4.858027173294833
}

var bounds = {
    south : 52.37719398718864,
    west : 4.819403363480372,
    north : 52.40401271579783,
    east : 4.896650983109279,
};


function SetupMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: AmsterdamCoordinates,
        zoom: 14,
        mapTypeControl: false,
        zoomControl: false,
        streetViewControl: false,
    });

    //bounds drawer
    var boundsDrawer = new google.maps.Rectangle({
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35,
        map: map,
        bounds: bounds
    });

    //draw bounds as markers
    var northEastMarkerIcon = GeneratePointImage(50, "northEastMarkerIcon");
    var northEastMarker = new google.maps.Marker({
        position: {
            lat: bounds.north,
            lng: bounds.east
        },
        map: map,
        title: 'northEastMarker',
        icon: northEastMarkerIcon.src
    });

}

function initMap() {
    SetupMap();
}

