/*global window, ko*/

(function (ns) {
    "use strict";
    ns.CitiesViewModel = function () {
        var self = this,
            getCities = new GetCities(),
            city = ko.observable();

        ns.cities = ko.observableArray();

        function GetCities() {
            $.getJSON("api/cities", function (data) {
                data.forEach(function (item, index) {
                    city({ name: item.Name, id: item.ID });
                    ns.cities.push(city());
                });

            });
        }

        return {
            myCities: getCities
        }
    }


}(window.milosev));

