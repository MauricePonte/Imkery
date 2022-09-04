/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
var BingMap = /** @class */ (function () {
    function BingMap() {
        this.map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
            /* No need to set credentials if already passed in URL */
            center: new Microsoft.Maps.Location(52.373275556573866, 4.899844627461851),
            mapTypeId: Microsoft.Maps.MapTypeId.aerial,
            zoom: 10,
            navigationBarMode: Microsoft.Maps.NavigationBarMode.compact,
            showLocateMeButton: true
        });
        Microsoft.Maps.Events.addHandler(this.map, "click", function (e) { return addPushPin(e); });
    }
    return BingMap;
}());
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
        bingMap.map.entities.push(pin);
    }
}
//# sourceMappingURL=InitializeBingMap.js.map