/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
var BingMap = /** @class */ (function () {
    function BingMap() {
        navigator.geolocation.getCurrentPosition(initializeCenterLocation);
        this.map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
            center: centerLocation,
            mapTypeId: Microsoft.Maps.MapTypeId.aerial,
            zoom: 10,
            navigationBarMode: Microsoft.Maps.NavigationBarMode.compact,
            showLocateMeButton: true
        });
        Microsoft.Maps.Events.addHandler(this.map, "click", function (e) { return addPushPin(e); });
    }
    return BingMap;
}());
var clickedLocation;
var centerLocation;
var bingMap;
function loadMap() {
    bingMap = new BingMap();
}
function addPushPin(e) {
    if (e.targetType == "map") {
        bingMap.map.entities.pop();
        var clickedPoint = new Microsoft.Maps.Point(e.getX(), e.getY());
        var locTemp = e.target.tryPixelToLocation(clickedPoint);
        var location = new Microsoft.Maps.Location(locTemp.latitude, locTemp.longitude);
        var pin = new Microsoft.Maps.Pushpin(location, { 'draggable': false });
        clickedLocation = location;
        bingMap.map.entities.push(pin);
    }
}
function initializeCenterLocation(position) {
    centerLocation = new Microsoft.Maps.Location(position.coords.latitude, position.coords.longitude);
    if (centerLocation == null) {
        centerLocation = new Microsoft.Maps.Location(52.373275556573866, 4.899844627461851);
    }
}
function getClickedLocationCoords() {
    console.log(JSON.stringify(clickedLocation.latitude));
    return JSON.stringify(clickedLocation);
}
//# sourceMappingURL=InitializeBingMap.js.map