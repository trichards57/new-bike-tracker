var appControllers = angular.module('appControllers', ['appServices']);

appControllers.controller('ImeiListCtrl', ['$scope', 'IMEI', function ($scope, IMEI) {
    $scope.refresh = function () {
        $scope.imeis = IMEI.query();
    };

    $scope.refresh();
}]);