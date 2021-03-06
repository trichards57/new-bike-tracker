﻿/*jslint
    browser: true
*/
/*global angular */

var app = angular.module("app", [
    "ngRoute",
    "ngResource",
    "ui.validate",
    "appControllers",
    "ui.bootstrap",
    "chart.js"
]);

app.config(["$routeProvider", function ($routeProvider) {
    "use strict";
    $routeProvider.when("/", {
        templateUrl: "/ControlPanel/Home",
        controller: "ControlPanelCtrl"
    }).when("/IMEIs", {
        templateUrl: "/IMEI/Home",
        controller: "IMEIListCtrl"
    }).when("/Admin", {
        templateUrl: "/Admin/Home",
        controller: "AdminCtrl"
    }).when("/Reports", {
        templateUrl: "/Report/Home",
        controller: "ReportCtrl"
    }).when("/Reports/UserActivity", {
        templateUrl: "/Report/UserActivity",
        controller: "LogListCtrl"
    }).when("/Reports/Locations", {
        templateUrl: "/Report/Locations",
        controller: "LocationReportCtrl"
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

var compareTo = function () {
    "use strict";
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {
            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue === scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
};

app.directive("compareTo", compareTo);