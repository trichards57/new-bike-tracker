﻿var map = [];
var updateRate = 30;
var hideUnknown = false;
var refreshTimeout = [];
var fadeTime = 5;

$(document).ready(function () {
    "use strict";

    $("#callsign-box").change(function () {
        $("#callsign-box option:selected").each(function (i, s) {
            var loc = $(s).val();

            if (loc !== "empty") {
                var parts = loc.split(" ");
                var lat = parseFloat(parts[0]);
                var lon = parseFloat(parts[1]);
                map.setView(
                  {
                      center: new Microsoft.Maps.Location(lat, lon)
                  });
                $("#callsign-box>option:eq(0)").prop("selected", true);
            }
        });
        e.preventDefault();
    });

    $("#delete-landmark").click(function () {
        $("#landmark-box option:selected").each(function (i, s) {
            var id = $(s).val();

            if (id !== "empty") {
                var url = "Map/ClearLandmark?id=" + id;

                $.get(url).done(function () {
                    $("#landmark-delete-alert").removeClass("collapse").addClass("in");
                    setTimeout(function () { $("#landmark-delete-alert").removeClass("in"); }, fadeTime * 1000);
                    setTimeout(function () { $("#landmark-delete-alert").addClass("collapse"); }, (fadeTime + 2) * 1000);
                    refresh();
                });
            }
        });
        e.preventDefault();
    });

    $("#hide-unknown").change(function (e) {
        hideUnknown = $("#hide-unknown").prop("checked");
        clearTimeout(refreshTimeout);
        refresh();
    });

    $("#refresh-button").click(function (e) {
        clearTimeout(refreshTimeout);
        refresh();
        e.preventDefault();
    });

    $("#landmarkSave").click(function (e) {
        if ($("#landmarkName").val() == null || $.trim($("#landmarkName").val()) === "") {
            return;
        }

        var name = $.trim($("#landmarkName").val());
        var lat = $.trim($("#landmarkLatitude").val());
        var lon = $.trim($("#landmarkLongitude").val());

        var url = "Map/AddLandmark?name=" + name + "&lat=" + lat + "&lon=" + lon;

        $("#landmarkModal").modal("hide");

        $.get(url).done(function () {
            $("#landmark-success-alert").addClass("in").removeClass("collapse");
            setTimeout(function () { $("#landmark-success-alert").removeClass("in"); }, fadeTime * 1000);
            setTimeout(function () { $("#landmark-success-alert").addClass("collapse"); }, (fadeTime + 2) * 1000);
            clearTimeout(refreshTimeout);
            refresh();
        }).fail(function () {
            $("#landmark-fail-alert").addClass("in").removeClass("collapse");
            setTimeout(function () { $("#landmark-fail-alert").removeClass("in"); }, fadeTime * 1000);
            setTimeout(function () { $("#landmark-fail-alert").addClass("collapse"); }, (fadeTime + 2) * 1000);
            clearTimeout(refreshTimeout);
            refresh();
        });
    });

    $("#landmarkName").on("input", function () {
        if ($("#landmarkName").val() == null || $.trim($("#landmarkName").val()) === "") {
            $("#landmarkSave").prop("disabled", true).addClass("disabled");
        }
        else {
            $("#landmarkSave").prop("disabled", false).removeClass("disabled");
        }
    });

    $("#landmarkName").keydown(function () {
        if ($("#landmarkName").val() == null || $.trim($("#landmarkName").val()) === "") {
            $("#landmarkSave").prop("disabled", true).addClass("disabled");
        }
        else {
            $("#landmarkSave").prop("disabled", false).removeClass("disabled");
        }
    });

    $("#add-landmarks-button").click(function (e) {
        BeginPlaceLandmark();
        $("#add-landmarks-button").addClass("hide");
        $("#finish-landmarks-button").removeClass("hide");
        e.preventDefault();
    });

    $("#finish-landmarks-button").click(function (e) {
        EndPlaceLandmark();
        $("#add-landmarks-button").removeClass("hide");
        $("#finish-landmarks-button").addClass("hide");
        e.preventDefault();
    });
});

function centreLocation(position) {
    "use strict";

    map.setView({
        center: new Microsoft.Maps.Location(position.coords.latitude, position.coords.longitude)
    });
}

var svgTemplate = '<svg xmlns="http://www.w3.org/2000/svg" width="100" height="25"><foreignObject width="100%" height="100%"><div xmlns="http://www.w3.org/1999/xhtml">{htmlContent}</div></foreignObject></svg>';

function refresh() {
    "use strict";

    $.get("/Map/GetLocations", function (data) {
        map.entities.clear();
        $("#callsign-box").find("option").remove();
        $("#landmark-box").find("option").remove();
        $("#callsign-box").append($("<option>", {
            value: "empty",
            text: "Pick Callsign"
        }));

        for (var i = 0; i < data.length; i++) {
            var dat = data[i];

            var readingTime = moment(dat.ReadingTime);

            var timeSinceReading = moment().diff(readingTime, "minutes");

            if (timeSinceReading >= 60)
                continue;

            if (hideUnknown && dat.Callsign.indexOf("?") > -1)
                continue;

            var color = "color: #3c763d; background-color: #dff0d8;";

            if (timeSinceReading >= 40)
                color = "color: #a94442; background-color: #f2dede;";
            else if (timeSinceReading >= 20)
                color = "color: #8a6d3b; background-color: #f7ecb5;";

            var content = "<div style='font-size: 12px; font-weight: bold; border: solid 2px; display: inline; white-space: nowrap; " + color + " font-family: Arial,Helvetica,sans-serif;'>";

            switch (dat.Type) {
                case 1: // Bike
                    content += "<img src='/Content/bike15.png' />";
                    break;
                case 2: // Foot Patrol
                    content += "<img src='/Content/man-silhouette1.png' />";
                    break;
                case 3: // Ambulance
                    content += "<img src='/Content/transport66.png' />";
                    break;
                case 4: // Other
                    content += "<img src='/Content/medical96.png' />";
                    break;
                default: // Unknown type
                    content += "<img src='/Content/question30.png' />";
                    break;
            }

            if (dat.Callsign)
                content += dat.Callsign;
            else
                content += "WR???";

            content += "</div>";
            var options = { width: null, height: null, icon: svgTemplate.replace('{htmlContent}', content), anchor: new Microsoft.Maps.Point(22.5, 10) };
            var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(dat.Latitude, dat.Longitude), options);

            map.entities.push(pin);

            $("#callsign-box").append($("<option>", {
                value: dat.Latitude + " " + dat.Longitude,
                text: dat.Callsign
            }));
        }

        refreshTimeout = setTimeout(refresh, updateRate * 1000);

        $.get("/Map/GetLandmarks", function (data) {
            for (var i = 0; i < data.length; i++) {
                var dat = data[i];

                var color = "color: #31708f; background-color: #d9edf7;";

                var content = "<div style='font-size: 12px; font-weight: bold; border: solid 2px; display: inline; white-space: nowrap; " + color + " font-family: Arial,Helvetica,sans-serif;'>" + dat.Name + "</div>"
                var options = { width: null, height: null, icon: svgTemplate.replace('{htmlContent}', content), anchor: new Microsoft.Maps.Point(22.5, 10) };
                var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(dat.Latitude, dat.Longitude), options);

                map.entities.push(pin);

                $("#landmark-box").append($("<option>", {
                    value: dat.Id,
                    text: dat.Name
                }));
            }
        }).fail(function (xhr) {
            if (xhr.status == 401) // Unauthorised
            {
                $("#loggedOutAlert").show();
            }
        })
    }).fail(function (xhr) {
        if (xhr.status == 401) // Unauthorised
        {
            $("#loggedOutAlert").show();
        }
    })
}

function ShowAddLandmark(e) {
    if (e.targetType == "map") {
        $("#landmarkName").val("");
        $("#landmarkSave").prop("disabled", true).addClass("disabled");
        $("#landmarkModal").modal("show");

        var point = new Microsoft.Maps.Point(e.getX(), e.getY());
        var loc = e.target.tryPixelToLocation(point);
        $("#landmarkLatitude").val(loc.latitude)
        $("#landmarkLongitude").val(loc.longitude);
    }
};

function GetMap() {
    $.ajaxSetup({ cache: false });
    map = new Microsoft.Maps.Map("#mapDiv",
                       {
                           credentials: "ApfWme6djEwhx5JqGqyzMf8PrYvSmspgz_nsCamSsEab7AK46NNwhEGd840O1QH3",
                           center: new Microsoft.Maps.Location(51.45, -2.5833),
                           mapTypeId: Microsoft.Maps.MapTypeId.road,
                           zoom: 14
                       });
    $("#mapDiv").removeAttr("style");


    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(centreLocation);
    }

    refresh();
};

var handler = {};

function BeginPlaceLandmark() {
    handler = Microsoft.Maps.Events.addHandler(map, "click", ShowAddLandmark);
}

function EndPlaceLandmark() {
    Microsoft.Maps.Events.removeHandler(handler);
}