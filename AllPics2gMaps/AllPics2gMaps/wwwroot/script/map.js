(function (ns) {
    /*globals google, ko*/
    "use strict";
    var map,
        citiesViewModel,
        limitViewModel, 
        applyFilterViewModel,
        circleViewModel,
        mapsModel,
        groupedByCityViewModel;

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

        ns.map = map;
        citiesViewModel = new ns.CitiesViewModel();
        limitViewModel = new ns.LimitViewModel();
        applyFilterViewModel = new ns.ApplyFilterViewModel();
        circleViewModel = new ns.CircleViewModel();
        groupedByCityViewModel = new ns.GroupedByCityViewModel();

        mapsModel = {
            citiesViewModel: citiesViewModel,
            circleViewModel: circleViewModel,
            limitViewModel: limitViewModel,
            applyFilterViewModel: applyFilterViewModel,
            groupedByCityViewModel : groupedByCityViewModel
        };

        ns.citiesViewModel = citiesViewModel;
        ns.circleViewModel = circleViewModel;
        ns.limitViewModel = limitViewModel;
        ns.applyFilterViewModel = applyFilterViewModel;
        ns.groupedByCityViewModel = groupedByCityViewModel;
        ko.applyBindings(mapsModel);
    }

    ns.initMap = initMap;
    ns.markers = [];
}(window.milosev));