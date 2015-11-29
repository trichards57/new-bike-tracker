var appControllers = angular.module('appControllers', ['appServices']);

appControllers.controller('ImeiListCtrl', ['$scope', 'IMEI', function ($scope, IMEI) {
    $scope.sortBy = 'IMEI';
    $scope.sortReverse = false;
    $scope.imeiFilter = '';

    $scope.updateSortBy = function (param) {
        if ($scope.sortBy == param)
            $scope.sortReverse = !$scope.sortReverse;
        else
        {
            $scope.sortBy = param;
            $scope.sortReverse = false;
        }
    }

    $scope.refresh = function () {
        $scope.imeis = IMEI.query();
    };

    $scope.refresh();
}]);