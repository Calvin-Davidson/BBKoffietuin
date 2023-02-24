//settings
var start_width = 512
var start_height = 512;

//references
let input_width = document.getElementById('input_width');
let input_height = document.getElementById('input_height');
let input_zoom = document.getElementById('input_zoom');
let mapHtmlReference = document.getElementById('map');

//Setup the default values 
setup();

function setup() {
    input_width.value = start_width;
    input_height.value = start_height;
    mapHtmlReference.style.width = start_width + "px";
    mapHtmlReference.style.height = start_height + "px";
}

//methods 
function applyChangeWidth(num) {
    input_width.value = num;
    mapHtmlReference.style.width = num + "px";

    //reload the map
    initMap();
}

function applyChangeHeight(num) {
    input_height.value = num;
    mapHtmlReference.style.height = num + "px";

    //reload the map
    initMap();
}

// checks if the input is a power of two if its not we add 1 so it is.
function makePowerOfTwo(input) {
    var number = input.value;
    var valid = number % 2 == 0;
    if (!valid) number++;
    return number;
}

//Handle zoom of the map (input field and scroll) and makes sure the values are synced
function onZoomInputChange(input) {
    map.setZoom(parseInt(input.value));
}

google.maps.event.addListener(map, 'zoom_changed', function () {
    input_zoom.value = map.getZoom();
});

