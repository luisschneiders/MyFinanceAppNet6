(() => {
    'use strict'

    window.initializeGoogleMap = (mapdId, center, zoom, googleMapdId) => {
        window[mapdId] = new google.maps.Map(document.getElementById(mapdId), {
            center,
            zoom,
            mapId: googleMapdId
            // mapId: "DEMO_MAP_ID"
        });
    }

    window.addGoogleMapMarker = (mapId, title, position) => {
        const marker = new google.maps.marker.AdvancedMarkerElement({
            map: window[mapId],
            position,
            title,
        });
    
        window[title] = marker;
        return marker;
    }

    window.fitGoogleMapMarkerToView  = (mapId, markerPositions) => {
        if (!markerPositions || markerPositions.length === 0) {
            return;
        }

        const map = window[mapId];
        const bounds = new google.maps.LatLngBounds();

        markerPositions.forEach(pos => {
            bounds.extend(new google.maps.LatLng(pos.latitude, pos.longitude));
        });

        map.fitBounds(bounds);
    }

})()
