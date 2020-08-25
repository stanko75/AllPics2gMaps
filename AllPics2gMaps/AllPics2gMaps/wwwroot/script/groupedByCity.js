/*global $*/

(function (ns) {
    "use strict";
    ns.GroupedByCityViewModel = function () {
        var self = this;
        self.groupedByCityClick = async () => {
            $.getJSON("/api/GoogleMaps", function (data) {
                data.forEach(function (file) {
                    ns.createMarker(file);
                });
            });
        };
    };

    return {
        groupedByCityClick: self.groupedByCityClick
    };
}(window.milosev));