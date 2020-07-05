(function (ns) {
    /*globals google, $*/
    "use strict";

    var markers = [];

    function createMarker(file) {
        try {
            var picsLatLng = new google.maps.LatLng(parseFloat(file.Latitude.replace(',', '.')), parseFloat(file.Longitude.replace(',', '.')));
            var bounds = new google.maps.LatLngBounds();

            var marker = new google.maps.Marker({
                position: picsLatLng,
                map: ns.map,
                title: file.FileName,
                url: file.FileName
            });

            marker.addListener('click', function () {
                window.open(marker.url, "_target");
            });

            markers.push(marker);

            bounds.extend(picsLatLng);
            ns.map.fitBounds(bounds);

            var zoomChangeBoundsListener =
                google.maps.event.addListenerOnce(ns.map, 'bounds_changed', function (event) {
                    if (ns.map.getZoom()) {
                        ns.map.setZoom(5);  // set zoom here
                    }
                });

            setTimeout(function () {
                google.maps.event.removeListener(zoomChangeBoundsListener);
            }, 2000);
        }
        catch (e) {
            console.log(e);
            if (typeof google !== 'object') {
                setTimeout(function () {
                    location.reload();
                }, 3000);
            }
        }
    }

    $.getJSON("/api/GoogleMaps", function (data) {
        data.forEach(function (file) {
            createMarker(file);
        });
    }).done(function () {
        ns.markers = markers;
    });

    ns.createMarker = createMarker;
})(window.milosev);