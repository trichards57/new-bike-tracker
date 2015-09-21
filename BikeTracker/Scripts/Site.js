﻿var map = [];
const updateRate = 30;

$(document).ready(function () {
    $('#callsign-box').change(function () {
        $('#callsign-box option:selected').each(function (i, s) {
            var loc = $(s).val();

            if (loc !== 'empty') {
                var parts = loc.split(' ');
                var lat = parseFloat(parts[0]);
                var lon = parseFloat(parts[1]);
                map.setView(
                  {
                      center: new Microsoft.Maps.Location(lat, lon)
                  });
                $('#callsign-box>option:eq(0)').prop('selected', true);
            }
        });
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(centreLocation);
    }
});

function centreLocation(position) {
    map.setView({
        center: new Microsoft.Maps.Location(position.coords.latitude, position.coords.longitude)
    });
}

function GetMap() {
    $.ajaxSetup({ cache: false });
    map = new Microsoft.Maps.Map(document.getElementById("mapDiv"),
                       {
                           credentials: "ApfWme6djEwhx5JqGqyzMf8PrYvSmspgz_nsCamSsEab7AK46NNwhEGd840O1QH3",
                           center: new Microsoft.Maps.Location(51.45, -2.5833),
                           mapTypeId: Microsoft.Maps.MapTypeId.road,
                           zoom: 14
                       });

    (function worker() {
        $.get('/Map/GetLocations', function (data) {

            map.entities.clear();
            $('#callsign-box').find('option').remove();
            $('#callsign-box').append($('<option>', {
                value: 'empty',
                text: 'Pick Callsign'
            }));


            for (var i = 0; i < data.length; i++) {
                var dat = data[i];

                var readingTime = moment(dat.ReadingTime);

                var timeSinceReading = moment().diff(readingTime, "minutes");

                if (timeSinceReading >= 60)
                    continue;

                var color = "text-success bg-success";

                if (timeSinceReading >= 40)
                    color = "text-danger bg-danger";
                else if (timeSinceReading >= 20)
                    color = "text-warning bg-warning";

                var content = "<div class='callsign-flag " + color + "'>";
                if (dat.Callsign)
                    content += dat.Callsign;
                else
                    content += "WR???";

                content += "</div>";
                var options = { width: null, height: null, htmlContent: content, anchor: new Microsoft.Maps.Point(22.5, 10) };
                var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(dat.Latitude, dat.Longitude), options);

                map.entities.push(pin);

                $('#callsign-box').append($('<option>', {
                    value: dat.Latitude + ' ' + dat.Longitude,
                    text: dat.Callsign
                }));
            }

            setTimeout(worker, updateRate * 1000);
        })
    })();
};

GetMap();