$(document).ready(function () {
    $("#city").change(function () {
        var targetDrp = '#Area';
        if ($(this).val()) {
            $('' + targetDrp + ' option').remove();
            $('' + targetDrp + '').append('<option value="">select</option>');
            $.getJSON('/api/AdaniGas/CngArea', { city: $(this).val() }, function (response) {
                $.each(response, function (index, item) {
                    $('' + targetDrp + '').append('<option value="' + item.Value + '">' + item.Text + '</option>');
                });
            });
        }
        else {
            $('' + targetDrp + ' option').remove();
            $('' + targetDrp + '').append('<option value="">select</option>');
        }
        reloadEmptyMap();
    });
    $('#city').trigger('change');


    $("#Area").change(function () {
        if ($(this).val()) {
            //// API Call and fetch necessary data and push into array.

            var parameters = { "city": $("#city").val(), "area": $(this).val() }
            $.ajax({
                type: 'post',
                url: '/api/AdaniGas/CNGStations',
                data: JSON.stringify(parameters),
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                success: function (response) {
                    reloadPlacemark(response);
                },
                error: function (response, success, error) {
                    alert("Error: " + error);
                }
            });
        }
        else {
            reloadEmptyMap();
        }
    });
    reloadEmptyMap();
});

function reloadEmptyMap() {
    locations = [];
    initialize();
    $("#dvDetail").html('');
}

function reloadPlacemark(centerlist) {
    locations = [];
    var centerlisthtml = '';
    var startRow = '<div class="row">';
    var endRow = '</div>';
    centerlisthtml += startRow;
    $.each(centerlist, function (index, center) {
        title = center.AddrLine1;
        address = '<p>' + center.AddrLine2 + '</p><br/><p>' + center.AddrLine3 + '</p><br/><p>' + center.AddrLine4 + '</p><br/><p>' + center.AddrLine5 + '</p><br/><p>' + center.AddrLine6 + '</p><br/><p>' + center.AddrLine7 + '</p><br/><p>' + center.AddrLine8 + '</p>'; // AddressLine2 + 3+4+5+6+7+8
        lat = center.Latitude;
        long = center.Longitude;

        var data = [title, address, lat, long];
        locations.push(data);

        ////Generate DIV HTML
        centerlisthtml = centerlisthtml +
            '<div class="col-md-4 mb-3">' +
            '<p><span><b>' + center.AddrLine1 + '</b></span></p>' +
            '<p><span>' + center.AddrLine2 + '</span></p >' +
            '<p><span>' + center.AddrLine3 + '</span></p >' +
            '<p><span>' + center.AddrLine4 + '</span></p>' +
            //'<p><span>' + center.AddrLine5 + '</span></p>' +
            // '<p><span>' + center.AddrLine6 + '</span></p>' +
            // '<p><span>' + center.AddrLine7 + '</span></p>' +
            //'<p><span>' + center.AddrLine8 + '</span></p>' +
            '<p><span>' + center.NearArea + '</span></p>' +
            '<p><span>' + center.City + '</span></p>';

        if (center.PinCode != "000000") {
            centerlisthtml = centerlisthtml + '<p><span>' + center.PinCode + '</span></p>';
        }
        if (center.MobileNo != "0000000000") {
            centerlisthtml = centerlisthtml + '<p><span>Mobile No:' + center.MobileNo + '</span></p>';
        }
        if (center.MobileNo2 != "0000000000") {
            centerlisthtml = centerlisthtml + '<p><span>Mobile No:' + center.MobileNo2 + '</span></p>';
        }
        if (center.Landline != "") {
            centerlisthtml = centerlisthtml + '<p><span>Landline No:' + center.Landline + '</span></p>';
        }
        centerlisthtml = centerlisthtml + '</div>';
    });
    centerlisthtml += endRow;
    $("#dvDetail").html(centerlisthtml);
    initialize();
}

var origin = new google.maps.LatLng(72.3979500, 72.39795009);
function initialize() {
    var mapOptions = {
        zoom: 8,
        center: origin
    };

    map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
    google.maps.event.addListenerOnce(map, 'bounds_changed', function () {
        map.setZoom(8);
    });
    infowindow = new google.maps.InfoWindow();

    ////Create LatLngBounds object.
    var latlngbounds = new google.maps.LatLngBounds();

    for (i = 0; i < locations.length; i++) {
        var position = new google.maps.LatLng(locations[i][2], locations[i][3]);

        var marker = new google.maps.Marker({
            position: position,
            map: map,
        });
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                contentString = locations[i][0] + "<br>" + locations[i][1];
                infowindow.setContent(contentString);
                infowindow.setOptions({
                    maxWidth: 200
                });
                infowindow.open(map, marker);

                map.panTo(marker.getPosition());
            }
        })(marker, i));

        ////Extend each marker's position in LatLngBounds object.
        latlngbounds.extend(marker.position);
    }

    ////Get the boundaries of the Map.
    var bounds = new google.maps.LatLngBounds();

    ////Center map and adjust Zoom based on the position of all markers.
    map.setCenter(latlngbounds.getCenter());
    map.fitBounds(latlngbounds);
}


