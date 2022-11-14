/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />

class BingMap {
    map: Microsoft.Maps.Map;

    constructor() {
        navigator.geolocation.getCurrentPosition(initializeCenterLocation);
        
        this.map = new Microsoft.Maps.Map(document.getElementById('myMap'),
            {
                center: centerLocation,
                mapTypeId: Microsoft.Maps.MapTypeId.aerial,
                zoom: 10,
                navigationBarMode: Microsoft.Maps.NavigationBarMode.compact,
                showLocateMeButton: true

            });
        
        Microsoft.Maps.Events.addHandler(this.map, "click", (e) => addPushPin(e));
    }
}

let clickedLocation;
let centerLocation;
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
        clickedLocation = location;
        bingMap.map.entities.push(pin);
    }
}

function initializeCenterLocation(position): void{
    centerLocation = new Microsoft.Maps.Location(position.coords.latitude, position.coords.longitude);

    if (centerLocation == null) {
        centerLocation = new Microsoft.Maps.Location(52.373275556573866, 4.899844627461851);
    }
}

function getClickedLocationCoords() {
    console.log(JSON.stringify(clickedLocation.latitude))
    return JSON.stringify(clickedLocation);
}