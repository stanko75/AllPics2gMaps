(function (ns) {
    /*globals google, map*/
    "use strict";
    var map,
        citiesViewModel,
        applyFilterViewModel,
        mapsModel;

    function initMap() {
        try {
            map = new google.maps.Map(document.getElementById('map-canvas'), {
                center: { lat: 34.397, lng: 150.644 },
                scrollwheel: true,
                zoom: 2
            });
        }
        catch (e) {
            console.log(e);
            setTimeout(function () {
                if (typeof google !== 'object') {
                    location.reload();
                }
            }, 1000);
        }

        citiesViewModel = new ns.CitiesViewModel();
        applyFilterViewModel = new ns.ApplyFilterViewModel();

        mapsModel = {
            citiesViewModel: citiesViewModel,
            applyFilterViewModel: applyFilterViewModel
        };

        ns.citiesViewModel = citiesViewModel;
        ns.applyFilterViewModel = applyFilterViewModel;
        ns.map = map;
        ko.applyBindings(mapsModel);
    }

    ns.initMap = initMap;
})(window.milosev);