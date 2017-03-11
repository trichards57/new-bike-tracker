class ControlPanelController extends BaseController {
    constructor($scope: angular.IScope, $window: angular.IWindowService,
        $uibModal: angular.ui.bootstrap.IModalService) {
        super($scope, $uibModal, $window, "Administrator Control Panel - SJA Tracker");
    }

    static $inject = ["$scope", "$window", "$uibModal"];
}

angular.module("app").controller("ControlPanelCtrl", ControlPanelController);