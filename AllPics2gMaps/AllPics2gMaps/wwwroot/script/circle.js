/*global ko, $, google*/
/*jshint esversion: 6 */

(function (ns) {
    "use strict";
    ns.CircleViewModel = function () {
        var self = this,
            map = ns.map,
            circles = [];

        self.radius = ko.observable(100000);
        self.lat = ko.observable(45.23781111);
        self.lng = ko.observable(19.82721111);
        self.removeCircles = function () {
            circles.forEach(function (circle) {
                circle.setMap(null);
                ns.circlesFilter = [];
            });
        };

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
                var listOfCities = $.parseJSON(data);
                listOfCities.forEach(function (file) {
                    ns.createMarker(file);
                });
            });
        };

        google.maps.event.addListener(map, 'mousemove', function (event) {
            self.lat(event.latLng.lat());
            self.lng(event.latLng.lng());
        });

        google.maps.event.addListener(map, 'click', function (event) {
            var url = new URL(window.location.href);
            var useCircles = url.searchParams.get("useCircles");

            if (useCircles) {

                var circleProperties,
                    center = { lat: self.lat(), lng: self.lng() },
                    circle = new google.maps.Circle({
                        strokeColor: "#000000",
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillOpacity: 0.35,
                        map,
                        center: center,
                        radius: parseFloat(self.radius()),
                        editable: true,
                        draggable: true
                    });

                circles.push(circle);

                google.maps.event.addListener(circle, 'radius_changed', function (event) {
                    var radiuschangedCircle = ns.circlesFilter.find(element => element = circle);

                    if (radiuschangedCircle) {
                        radiuschangedCircle.radius = circle.getRadius();
                    }

                    self.radius(circle.getRadius());

                });

                google.maps.event.addListener(circle, 'dragend', function (event) {
                    var dragedCircle = ns.circlesFilter.find(element => element = circle);

                    if (dragedCircle) {
                        dragedCircle.lat = event.latLng.lat();
                        dragedCircle.lng = event.latLng.lng();
                    }

                    self.lat(event.latLng.lat());
                    self.lng(event.latLng.lng());
                });

                circleProperties = { radius: self.radius(), lat: self.lat(), lng: self.lng() };

                if (!ns.circlesFilter) {
                    ns.circlesFilter = [];
                }

                ns.circlesFilter.push(circleProperties);
            };
        });

        return {
            radius: self.radius,
            lat: self.lat,
            lng: self.lng,
            createCircle: self.createCircle,
            removeCircles: self.removeCircles
        };
    };
}(window.milosev));