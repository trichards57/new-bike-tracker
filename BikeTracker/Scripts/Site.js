var map = [];
var updateRate = 30;
var hideUnknown = false;
var refreshTimeout = [];
var fadeTime = 5;

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

    $('#delete-landmark').click(function () {
        $('#landmark-box option:selected').each(function (i, s) {
            var id = $(s).val();

            if (id !== 'empty') {
                var url = "Map/ClearLandmark?id=" + id;

                $.get(url).done(function () {
                    $('#landmark-delete-alert').addClass('in');
                    setTimeout(function () { $('#landmark-delete-alert').removeClass('in'); }, fadeTime * 1000);
                    setTimeout(function () { $('#landmark-delete-alert').addClass('collapse'); }, fadeTime * 1000 + 2);
                    refresh();
                })
            }
        });
    })

    $('#hide-unknown').change(function (e) {
        hideUnknown = $('#hide-unknown').prop('checked');
        clearTimeout(refreshTimeout);
        refresh();
    });

    $('#refresh-button').click(function (e) {
        clearTimeout(refreshTimeout);
        refresh();
    });

    $('#landmarkSave').click(function (e) {
        if ($('#landmarkName').val() == null || $('#landmarkName').val().trim() === '')
            return;

        var name = $('#landmarkName').val().trim();
        var lat = $('#landmarkLatitude').val().trim();
        var lon = $('#landmarkLongitude').val().trim();

        var url = "Map/AddLandmark?name=" + name + "&lat=" + lat + "&lon=" + lon;

        $('#landmarkModal').modal('hide');

        $.get(url).done(function () {
            $('#landmark-success-alert').addClass('in');
            setTimeout(function () { $('#landmark-success-alert').removeClass('in'); }, fadeTime * 1000);
            clearTimeout(refreshTimeout);
            refresh();
        }).fail(function () {
            $('#landmark-fail-alert').addClass('in');
            setTimeout(function () { $('#landmark-fail-alert').removeClass('in'); }, fadeTime * 1000);
            clearTimeout(refreshTimeout);
            refresh();
        });
    })

    $('#landmarkName').on('input', function () {
        if ($('#landmarkName').val() == null || $('#landmarkName').val().trim() === '')
        {
            $('#landmarkSave').prop('disabled', true).addClass('disabled');
        }
        else
        {
            $('#landmarkSave').prop('disabled', false).removeClass('disabled');
        }
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

function refresh() {
    $.get('/Map/GetLocations', function (data) {

        map.entities.clear();
        $('#callsign-box').find('option').remove();
        $('#landmark-box').find('option').remove();
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

            if (hideUnknown && dat.Callsign.indexOf('?') > -1)
                continue;

            var color = "text-success bg-success";

            if (timeSinceReading >= 40)
                color = "text-danger bg-danger";
            else if (timeSinceReading >= 20)
                color = "text-warning bg-warning";

            var content = "<div class='callsign-flag " + color + "' data-type='callsign' data-id='" + dat.Id + "'>";
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

        refreshTimeout = setTimeout(refresh, updateRate * 1000);

        $.get('/Map/GetLandmarks', function (data) {
            for (var i = 0; i < data.length; i++) {
                var dat = data[i];

                var content = "<div class='callsign-flag text-info bg-info data-type='landmark' data-id='" + dat.Id + "'>" + dat.Name + "</div>"
                var options = { width: null, height: null, htmlContent: content, anchor: new Microsoft.Maps.Point(22.5, 10) };
                var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(dat.Latitude, dat.Longitude), options);

                map.entities.push(pin);

                $('#landmark-box').append($('<option>', {
                    value: dat.Id,
                    text: dat.Name
                }));
            }
        }).fail(function (xhr) {
            if (xhr.status == 401) // Unauthorised
            {
                location.reload(true); // Refresh the page, which will redirect to the login page.
            }
        })

    }).fail(function (xhr) {
        if (xhr.status == 401) // Unauthorised
        {
            location.reload(true); // Refresh the page, which will redirect to the login page.
        }
    })
}

function ShowAddLandmark(e) {
    if (e.targetType == 'map' && e.isPrimary) {
        $('#landmarkName').val('');
        $('#landmarkSave').prop('disabled', true).addClass('disabled');
        $('#landmarkModal').modal('show');

        var point = new Microsoft.Maps.Point(e.getX(), e.getY());
        var loc = e.target.tryPixelToLocation(point);
        $('#landmarkLatitude').val(loc.latitude)
        $('#landmarkLongitude').val(loc.longitude);
        e.handled = true;
    }
};

function GetMap() {
    $.ajaxSetup({ cache: false });
    map = new Microsoft.Maps.Map(document.getElementById("mapDiv"),
                       {
                           credentials: "ApfWme6djEwhx5JqGqyzMf8PrYvSmspgz_nsCamSsEab7AK46NNwhEGd840O1QH3",
                           center: new Microsoft.Maps.Location(51.45, -2.5833),
                           mapTypeId: Microsoft.Maps.MapTypeId.road,
                           zoom: 14
                       });

    Microsoft.Maps.Events.addHandler(map, 'dblclick', ShowAddLandmark);

    refresh();
};

GetMap();