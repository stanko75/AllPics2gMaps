/*global window, ko, $*/

(function (ns) {
    "use strict";
    ns.ApplyFilterViewModel = function () {
        var self = this;
        self.myClick = async () => {
            var json = JSON.stringify({ cities: ns.citiesViewModel.cities.selectedCities() });
            console.log(json)
            const response = await fetch("api/GoogleMaps/" + encodeURIComponent(json));
        }

        return {
            myClick: self.myClick
        };
    };
}(window.milosev));