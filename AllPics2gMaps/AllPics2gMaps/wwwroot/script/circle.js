/*global ko*/

(function (ns) {
    "use strict";
    ns.CircleViewModel = function () {
        var self = this,
            map = ns.map;

        self.radius = ko.observable(603502);
        self.lat = ko.observable(45.23781111);
        self.lng = ko.observable(19.82721111);
        self.createCircle = function () {

        };

        google.maps.event.addListener(map, 'mousemove', function (event) {
            self.lat(event.latLng.lat());
            self.lng(event.latLng.lng());
        });

        google.maps.event.addListener(map, 'click', function (event) {

            var circleProperties,
                radius = Math.sqrt(603502) * 100,
                center = { lat: self.lat(), lng: self.lng() };

            new google.maps.Circle({
                strokeColor: "#FF0000",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                //fillColor: "#FF0000",
                fillOpacity: 0.35,
                map,
                center: center,
                radius: radius
            });

            circleProperties = { radius: radius, center: { lat: self.lat(), lng: self.lng(), center }};

            if (!ns.circlesFilter) {
                ns.circlesFilter = [];
            }

            ns.circlesFilter.push(circleProperties);
        });

        return {
            radius: self.radius,
            lat: self.lat,
            lng: self.lng,
            createCircle: self.createCircle
        };
    };
}(window.milosev));