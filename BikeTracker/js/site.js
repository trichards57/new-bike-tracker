var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var DeleteFormController = (function () {
    function DeleteFormController($modalInstance, name) {
        this.$modalInstance = $modalInstance;
        this.name = name;
    }
    DeleteFormController.prototype.ok = function () {
        this.$modalInstance.close();
    };
    DeleteFormController.prototype.cancel = function () {
        this.$modalInstance.dismiss("cancel");
    };
    return DeleteFormController;
}());
DeleteFormController.$inject = ["$uibModalInstance", "name"];
angular.module("app").controller("DeleteFormCtrl", DeleteFormController);
var ErrorFormController = (function () {
    function ErrorFormController($modalInstance, title, message) {
        this.$modalInstance = $modalInstance;
        this.title = title;
        this.message = message;
    }
    ErrorFormController.prototype.close = function () {
        this.$modalInstance.close();
    };
    return ErrorFormController;
}());
ErrorFormController.$inject = ["$uibModalInstance", "title", "message"];
angular.module("app").controller("ErrorFormCtrl", ErrorFormController);
/// <reference path="../../../node_modules/bingmaps/scripts/microsoftmaps/microsoft.maps.d.ts" />
/// <reference path="../../moment.d.ts" />
var LocationReportController = (function (_super) {
    __extends(LocationReportController, _super);
    function LocationReportController($scope, $window, $uibModal, $http) {
        var _this = _super.call(this, $scope, $uibModal, $window, "Location Reports - SJA Tracker") || this;
        _this.mapSettings = {
            credentials: "ApfWme6djEwhx5JqGqyzMf8PrYvSmspgz_nsCamSsEab7AK46NNwhEGd840O1QH3",
            center: new Microsoft.Maps.Location(51.45, -2.5833),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 14
        };
        _this.callsigns = [];
        _this.dateOptions = {
            formatYear: "yy",
            startingDay: 1,
            showWeeks: false
        };
        _this.selectedCallsign = "";
        _this.selectedDate = new Date();
        _this.$window = $window;
        _this.$http = $http;
        var vm = _this;
        $http.get("/api/Report/Callsigns").then(function (response) {
            vm.callsigns = response.data;
            vm.selectedCallsign = vm.callsigns[0];
        }, function () {
            vm.showError("Failed to Load Callsigns", "It wasn't possible to load the callsigns from the server.");
        });
        _this.map = new Microsoft.Maps.Map("#mapDiv", _this.mapSettings);
        $("#mapDiv").removeAttr("style");
        return _this;
    }
    LocationReportController.prototype.downloadDates = function () {
        var d = moment(this.selectedDate).format("YYYYMMDD");
        this.$window.open("/api/Report/DownloadCallsignLocations?callsign=" + this.selectedCallsign + "&startDate=" + d + "&endDate=" + d);
    };
    LocationReportController.prototype.openDate = function ($event) {
        $event.stopPropagation();
        $event.preventDefault();
        this.showDate = true;
    };
    LocationReportController.prototype.showDates = function () {
        this.map.entities.clear();
        var d = moment(this.selectedDate).format("YYYYMMDD");
        var svgTemplate = '<svg xmlns="http://www.w3.org/2000/svg" width="100" height="25"><foreignObject width="100%" height="100%"><div xmlns="http://www.w3.org/1999/xhtml">{htmlContent}</div></foreignObject></svg>';
        var vm = this;
        this.$http.get("/api/Report/CallsignLocations?callsign=" + this.selectedCallsign + "&startDate=" + d + "&endDate=" + d).then(function (response) {
            var reports = response.data;
            reports.forEach(function (dat) {
                var content = "<div style='font-size: 12px; font-weight: bold; border: solid 2px; display: inline; white-space: nowrap; color: #3c763d; background-color: #dff0d8; font-family: Arial,Helvetica,sans-serif;'>";
                var reportD = moment(dat.ReadingTime);
                content += reportD.format("HH:mm");
                content += "</div>";
                var options = {
                    icon: svgTemplate.replace('{htmlContent}', content),
                    anchor: new Microsoft.Maps.Point(22.5, 10)
                };
                var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(dat.Latitude, dat.Longitude), options);
                vm.map.entities.push(pin);
            });
        }, function () {
            vm.showError("Failed to Load Location Reports", "It wasn't possible to load the location reports from the server.");
        });
    };
    return LocationReportController;
}(BaseController));
LocationReportController.$inject = ["$scope", "$window", "$uibModal", "$http"];
angular.module("app").controller("LocationReportCtrl", LocationReportController);
var BaseModalController = (function () {
    function BaseModalController() {
    }
    return BaseModalController;
}());
var BaseController = (function () {
    function BaseController($scope, $uibModal, $window, title) {
        this.$uibModal = $uibModal;
        this.previousTitle = $window.document.title;
        $window.document.title = title;
        var vm = this;
        $scope.$on("$destroy", function () {
            $window.document.title = vm.previousTitle;
        });
    }
    BaseController.prototype.showError = function (title, message) {
        this.$uibModal.open({
            animation: true,
            templateUrl: "/Dialog/ErrorForm",
            controller: "ErrorFormCtrl as vm",
            resolve: {
                title: function () {
                    return title;
                },
                message: function () {
                    return message;
                }
            }
        });
    };
    ;
    return BaseController;
}());
var ReportsController = (function (_super) {
    __extends(ReportsController, _super);
    function ReportsController($scope, $window, $uibModal) {
        return _super.call(this, $scope, $uibModal, $window, "Reports Control Panel - SJA Tracker") || this;
    }
    return ReportsController;
}(BaseController));
ReportsController.$inject = ["$scope", "$window", "$uibModal"];
angular.module("app").controller("ReportCtrl", ReportsController);
var ControlPanelController = (function (_super) {
    __extends(ControlPanelController, _super);
    function ControlPanelController($scope, $window, $uibModal) {
        return _super.call(this, $scope, $uibModal, $window, "Administrator Control Panel - SJA Tracker") || this;
    }
    return ControlPanelController;
}(BaseController));
ControlPanelController.$inject = ["$scope", "$window", "$uibModal"];
angular.module("app").controller("ControlPanelCtrl", ControlPanelController);
var EditUserController = (function (_super) {
    __extends(EditUserController, _super);
    function EditUserController($scope, $uibModalInstance, $window, createMode, email, role) {
        var _this = this;
        if (createMode) {
            _this = _super.call(this, $scope, null, $window, "New User - SJA Tracker") || this;
            _this.email = "";
            _this.role = "Normal";
        }
        else {
            _this = _super.call(this, $scope, null, $window, "Edit User - SJA Tracker") || this;
            _this.email = email;
            _this.role = role;
        }
        _this.$uibModalInstance = $uibModalInstance;
        return _this;
    }
    EditUserController.prototype.ok = function () {
        this.$uibModalInstance.close({
            email: this.email,
            role: this.role
        });
    };
    EditUserController.prototype.cancel = function () {
        this.$uibModalInstance.dismiss("cancel");
    };
    return EditUserController;
}(BaseController));
EditUserController.$inject = ["$scope", "$uibModalInstance", "$window", "createMode", "email", "role"];
angular.module("app").controller("EditUserCtrl", EditUserController);
var AdminController = (function (_super) {
    __extends(AdminController, _super);
    function AdminController($scope, $window, $uibModal, User) {
        return _super.call(this, $scope, $uibModal, $window, "Reports Control Panel - SJA Tracker") || this;
    }
    return AdminController;
}(BaseController));
AdminController.$inject = ["$scope", "$window", "$uibModal", "User"];
//angular.module("app").controller("AdminCtrl", AdminController); 
//# sourceMappingURL=site.js.map