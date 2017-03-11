class ReportsController extends BaseController {
    constructor($scope: angular.IScope, $window: angular.IWindowService,
        $uibModal: angular.ui.bootstrap.IModalService) {
        super($scope, $uibModal, $window, "Reports Control Panel - SJA Tracker");
    }

    static $inject = ["$scope", "$window", "$uibModal"];
}

angular.module("app").controller("ReportCtrl", ReportsController);