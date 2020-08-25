/*global window, $*/
/* jshint esversion: 8 */

(function (ns) {
    "use strict";
    ns.ApplyFilterViewModel = function () {
        var self = this;
        self.sendFilterJsonClick = async () => {
            var json = JSON.stringify({
                cities: ns.citiesViewModel.cities.selectedCities(), limit: ns.limitViewModel.limit()
            });

            $.ajax({
                url: "api/GoogleMaps/",
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

        return {
            sendFilterJsonClick: self.sendFilterJsonClick
        };
    };
}(window.milosev));