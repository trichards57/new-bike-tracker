class AdminController extends BaseController {
    constructor($scope: angular.IScope, $window: angular.IWindowService,
        $uibModal: angular.ui.bootstrap.IModalService, User: angular.resource.IResource<{}>) {
        super($scope, $uibModal, $window, "Reports Control Panel - SJA Tracker");
    }

    static $inject = ["$scope", "$window", "$uibModal", "User"];
}

//angular.module("app").controller("AdminCtrl", AdminController);