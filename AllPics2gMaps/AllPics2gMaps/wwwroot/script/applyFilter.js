/*global window, ko, $*/

(function (ns) {
    "use strict";
    ns.ApplyFilterViewModel = function () {
        var self = this;
        self.myClick = async () => {
            var json = JSON.stringify({ cities: ns.citiesViewModel.cities.selectedCities() });

            const response = await fetch("api/GoogleMaps/" + encodeURIComponent(json));
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
                var listOfCities = $.parseJSON(data)
                listOfCities.forEach(function (file) {
                    ns.createMarker(file);
                });
            })
        }

        return {
            myClick: self.myClick
        };
    };
}(window.milosev));