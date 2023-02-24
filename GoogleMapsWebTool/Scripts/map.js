//Map Object
let map;

//Center coordinates (coordinates of the school.)
const StartCoordinates = {
    lat: 52.39060538838168,
    lng: 4.858027173294833
}

//Draws a red rectangle around the area around the given coordinates.
let debugBounds = false;
const bounds = {
    south: 52.37719398718864,
    west: 4.819403363480372,
    north: 52.40401271579783,
    east: 4.896650983109279,
};

//When the google api is loaded and we received a map.
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: StartCoordinates,
        zoom: 14,
    });


    DrawOnMap();
}

/// When the map is loaded this method is triggered and allows us to draw things on the map.
function DrawOnMap(){
    if (debugBounds) {
        var boundsDrawer = new google.maps.Rectangle({
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35,
            map: map,
            bounds: bounds
        });
    }
}

