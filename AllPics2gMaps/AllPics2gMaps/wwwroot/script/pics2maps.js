(function (ns) {
    /*globals google, $*/
    "use strict";

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

            ns.markers.push(marker);

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

    ns.createMarker = createMarker;
})(window.milosev);