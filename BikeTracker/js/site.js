var app = angular.module("app", [
    "ngRoute",
    "ngResource",
    "ui.validate",
    "appControllers",
    "ui.bootstrap",
    "chart.js"
]);
app.config(["$routeProvider", function ($routeProvider) {
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
var CompareToDirective = (function () {
    function CompareToDirective() {
        this.scope = {
            otherModelValue: "=compareTo"
        };
    }
    CompareToDirective.prototype.link = function (scope, element, attributes, ngModel) {
        var val = ngModel.$validators;
        val.compareTo = function (modelValue) {
            return modelValue === scope.otherModelValue;
        };
        scope.$watch("otherModelValue", function () {
            ngModel.$validate();
        });
    };
    CompareToDirective.Factory = function () {
        var directive = function () {
            return new CompareToDirective();
        };
        return directive;
    };
    return CompareToDirective;
}());
app.directive("compareTo", CompareToDirective.Factory());
//# sourceMappingURL=site.js.map