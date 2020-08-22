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

            var json = JSON.stringify({ circles: ns.circlesFilter });

            $.ajax({
                url: "api/CreateCircle/",
                type: "POST",
                data: json,
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (data) {
                for (var i = 0; i < ns.markers.length; i++) {
                    ns.markers[i].setMap(null);
                }
                ns.markers = [];
                var listOfCities = $.parseJSON(data)
                listOfCities.forEach(function (file) {
                    ns.createMarker(file);
                });
            })
        };

        google.maps.event.addListener(map, 'mousemove', function (event) {
            self.lat(event.latLng.lat());
            self.lng(event.latLng.lng());
        });

        google.maps.event.addListener(map, 'click', function (event) {

            var circleProperties,
                radius = Math.sqrt(self.radius()) * 100,
                center = { lat: self.lat(), lng: self.lng() };

            new google.maps.Circle({
                strokeColor: "#000000",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillOpacity: 0.35,
                map,
                center: center,
                radius: radius
            });

            circleProperties = { radius: self.radius(), lat: self.lat(), lng: self.lng() };

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