var appControllers = angular.module('appControllers', ['appServices']);

appControllers.controller('ImeiListCtrl', ['$scope', 'IMEI', function ($scope, IMEI) {
    $scope.sortBy = 'IMEI';
    $scope.sortReverse = false;
    $scope.imeiFilter = '';

    $scope.dialogImei = "";
    $scope.dialogCallsign = "";
    $scope.dialogType = 0;
    $scope.createMode = true;

    $scope.showAscending = function (param) {
        return ($scope.sortBy != param || ($scope.sortBy == param && !$scope.sortReverse))
    }

    $scope.showNewImei = function () {
        $scope.dialogImei = "";
        $scope.dialogCallsign = "";
        $scope.dialogType = 0;
        $scope.createMode = true;

        $('#edit-dialog').modal();
    }

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