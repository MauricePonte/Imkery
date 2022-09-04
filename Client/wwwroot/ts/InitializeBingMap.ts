/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />

class BingMap {
    map: Microsoft.Maps.Map;

    constructor() {
        this.map = new Microsoft.Maps.Map(document.getElementById('myMap'),
            {
                /* No need to set credentials if already passed in URL */
                center: new Microsoft.Maps.Location(52.373275556573866, 4.899844627461851),
                mapTypeId: Microsoft.Maps.MapTypeId.aerial,
                zoom: 10,
                navigationBarMode: Microsoft.Maps.NavigationBarMode.compact,
                showLocateMeButton: true

            });
        
        Microsoft.Maps.Events.addHandler(this.map, "click", (e) => addPushPin(e));
    }
}

let bingMap: BingMap;

function loadMap(): void {
    bingMap = new BingMap();
}

function addPushPin(e): void {
    if (e.targetType == "map") {
        bingMap.map.entities.pop();
        var clickedPoint = new Microsoft.Maps.Point(e.getX(), e.getY());
        var locTemp = e.target.tryPixelToLocation(clickedPoint);
        var location = new Microsoft.Maps.Location(locTemp.latitude, locTemp.longitude)
        var pin = new Microsoft.Maps.Pushpin(location, { 'draggable': false });
        bingMap.map.entities.push(pin);
    }
}