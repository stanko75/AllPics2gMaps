﻿/*global window, ko, $*/

(function (ns) {
    "use strict";
    ns.CitiesViewModel = function () {
        var self = this,
            getCities = new GetCities(),
            city = ko.observable();

        function GetCities() {
            self.cities = ko.observableArray();

            $.getJSON("api/cities", function (data) {
                data.forEach(function (item, index) {
                    city({ name: item.Name, id: item.ID });
                    self.cities.push(city());
                });
            });

            return self.cities;
        }

        return {
            cities: getCities
        };
    };


}(window.milosev));
