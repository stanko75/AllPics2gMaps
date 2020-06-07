/*global window, ko, $*/

(function (ns) {
    "use strict";
    ns.ApplyFilterViewModel = function () {
        var self = this;
        self.myClick = function () {
            console.log(JSON.stringify({ cities: ns.citiesViewModel.cities.selectedCities() }))
        }

        return {
            myClick: self.myClick
        };
    };
}(window.milosev));