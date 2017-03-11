let app = angular.module("app", [
    "ngRoute",
    "ngResource",
    "ui.validate",
    "appControllers",
    "ui.bootstrap",
    "chart.js"
]);

app.config(["$routeProvider", function ($routeProvider: angular.route.IRouteProvider) {
    $routeProvider.when("/", {
        templateUrl: "/ControlPanel/Home",
        controller: "ControlPanelCtrl",
        controllerAs: "vm"
    }).when("/IMEIs", {
        templateUrl: "/IMEI/Home",
        controller: "IMEIListCtrl"
    }).when("/Admin", {
        templateUrl: "/Admin/Home",
        controller: "AdminCtrl"
    }).when("/Reports", {
        templateUrl: "/Report/Home",
        controller: "ReportCtrl",
        controllerAs: "vm"
    }).when("/Reports/UserActivity", {
        templateUrl: "/Report/UserActivity",
        controller: "LogListCtrl"
    }).when("/Reports/Locations", {
        templateUrl: "/Report/Locations",
        controller: "LocationReportCtrl",
        controllerAs: "vm"
    }).when("/Reports/Rates", {
        templateUrl: "/Report/Rates",
        controller: "CheckInRateCtrl"
    }).when("/Reports/Success", {
        templateUrl: "/Report/Rates",
        controller: "SuccessRateCtrl"
    }).otherwise({
        redirectTo: "/"
    });
}]);