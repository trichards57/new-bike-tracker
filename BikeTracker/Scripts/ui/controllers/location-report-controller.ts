/// <reference path="../../../node_modules/bingmaps/scripts/microsoftmaps/microsoft.maps.d.ts" />
/// <reference path="../../moment.d.ts" />

interface ICallsignLocationReport {
    Latitude: number;
    Longitude: number;
    ReadingTime: string;
}

class LocationReportController extends BaseController {
    constructor($scope: angular.IScope, $window: angular.IWindowService,
        $uibModal: angular.ui.bootstrap.IModalService, $http: angular.IHttpService) {
        super($scope, $uibModal, $window, "Location Reports - SJA Tracker");
        this.$window = $window;
        this.$http = $http;

        let vm = this;

        $http.get<string[]>("/api/Report/Callsigns").then(function (response) {
            vm.callsigns = response.data;
            vm.selectedCallsign = vm.callsigns[0];
        }, function () {
            vm.showError("Failed to Load Callsigns", "It wasn't possible to load the callsigns from the server.");
        });

        this.map = new Microsoft.Maps.Map("#mapDiv", this.mapSettings);
        $("#mapDiv").removeAttr("style");
    }

    private $window: angular.IWindowService;
    private $http: angular.IHttpService;

    private mapSettings: Microsoft.Maps.IMapLoadOptions = {
        credentials: "ApfWme6djEwhx5JqGqyzMf8PrYvSmspgz_nsCamSsEab7AK46NNwhEGd840O1QH3",
        center: new Microsoft.Maps.Location(51.45, -2.5833),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 14
    };
    private map: Microsoft.Maps.Map;

    static $inject = ["$scope", "$window", "$uibModal", "$http"];

    callsigns: string[] = [];
    dateOptions: angular.ui.bootstrap.IDatepickerConfig = {
        formatYear: "yy",
        startingDay: 1,
        showWeeks: false
    };
    selectedCallsign: string = "";
    selectedDate: Date = new Date();
    showDate: boolean;

    downloadDates() {
        let d = moment(this.selectedDate).format("YYYYMMDD");

        this.$window.open("/api/Report/DownloadCallsignLocations?callsign=" + this.selectedCallsign + "&startDate=" + d + "&endDate=" + d);
    }

    openDate($event: angular.IAngularEvent) {
        $event.stopPropagation();
        $event.preventDefault();

        this.showDate = true;
    }

    showDates() {
        this.map.entities.clear();

        let d = moment(this.selectedDate).format("YYYYMMDD");
        let svgTemplate = '<svg xmlns="http://www.w3.org/2000/svg" width="100" height="25"><foreignObject width="100%" height="100%"><div xmlns="http://www.w3.org/1999/xhtml">{htmlContent}</div></foreignObject></svg>';
        let vm = this;

        this.$http.get<ICallsignLocationReport[]>("/api/Report/CallsignLocations?callsign=" + this.selectedCallsign + "&startDate=" + d + "&endDate=" + d).then(function (response) {
            var reports = response.data;

            reports.forEach(function (dat) {
                var content = "<div style='font-size: 12px; font-weight: bold; border: solid 2px; display: inline; white-space: nowrap; color: #3c763d; background-color: #dff0d8; font-family: Arial,Helvetica,sans-serif;'>";
                var reportD = moment(dat.ReadingTime);
                content += reportD.format("HH:mm");
                content += "</div>";

                var options: Microsoft.Maps.IPushpinOptions = {
                    icon: svgTemplate.replace('{htmlContent}', content),
                    anchor: new Microsoft.Maps.Point(22.5, 10)
                };

                var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(dat.Latitude, dat.Longitude), options);
                vm.map.entities.push(pin);
            });
        }, function () {
            vm.showError("Failed to Load Location Reports", "It wasn't possible to load the location reports from the server.");
        });
    }
}

angular.module("app").controller("LocationReportCtrl", LocationReportController);