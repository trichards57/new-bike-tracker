var appControllers = angular.module('appControllers', ['appServices']);

appControllers.controller('ControlPanelCtrl', ['$scope', function ($scope) {

}])

appControllers.controller('AdminCtrl', ['$scope', function ($scope) {
    $scope.sortBy = 'IMEI';
    $scope.sortReverse = false;
    $scope.userFilter = '';

    $scope.dialogEmail = "";
    $scope.dialogPassword = "";
    $scope.dialogPasswordConfirm = "";
    $scope.dialogRole = "";

    $scope.validPassword = function (value) {
        // Yes yes, I know this is bad practice, but I can't fix it yet.
        // See work item 426

        if (!value.length >= 6)
            return false; // Not long enough
        if (!value.match(/.*[[:upper:]]+.*/))
            return false; // No upper-case letter
        if (!value.match(/.*[[:lower:]]+.*/))
            return false; // No lower-case letter
        if (!value.match(/.*[^[:alnum:]]+.*/))
            return false; // No non-alphanumeric symbols
        if (!value.match(/.*[^[:digit:]]+.*/))
            return false; // No digits

        return true; // Probably okay
    }
}])

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

    $scope.saveImei = function () {
        var i = new IMEI();
        i.IMEI = $scope.dialogImei;
        i.CallSign = $scope.dialogCallsign;
        i.Type = $scope.dialogType;

        if ($scope.createMode) {
            i.$save([],
                function () { $scope.refresh(); },
                function () {
                    $scope.errorTitle = "Couldn't Create IMEI";
                    $scope.errorMessage = "There was an error creating that IMEI.  Please try again later.";

                    $('#error-dialog').modal();
                    $scope.refresh();
                });
        }
        else {
            i.Id = $scope.updateId;
            i.$update({ imeiId: $scope.updateId },
                function () { $scope.refresh(); },
                function () {
                    $scope.errorTitle = "Couldn't Update IMEI";
                    $scope.errorMessage = "There was an error updating that IMEI.  Please try again later.";

                    $('#error-dialog').modal();
                    $scope.refresh();
                });
        }
    }

    $scope.showUpdateImei = function (id) {
        $scope.createMode = false;

        var imei = $.grep($scope.imeis, function (e) { return e.Id == id; })

        if (imei.length == 1)
            imei = imei[0];
        else
            return;

        $scope.updateId = id;
        $scope.dialogImei = imei.IMEI;
        $scope.dialogCallsign = imei.CallSign;
        $scope.dialogType = imei.Type;

        $scope.editForm.$setPristine();

        $('#edit-dialog').modal();
    }

    $scope.showNewImei = function () {
        $scope.dialogImei = "";
        $scope.dialogCallsign = "";
        $scope.dialogType = "Unknown";
        $scope.createMode = true;

        $scope.editForm.$setPristine();

        $('#edit-dialog').modal();
    }

    $scope.updateSortBy = function (param) {
        if ($scope.sortBy == param)
            $scope.sortReverse = !$scope.sortReverse;
        else {
            $scope.sortBy = param;
            $scope.sortReverse = false;
        }
    }

    $scope.refresh = function () {
        $scope.loading = true;
        $scope.imeis = IMEI.query({}, function () {
            $scope.loading = false;
        },
            function () {
                $scope.errorTitle = "Couldn't Load IMEIs";
                $scope.errorMessage = "There was an error loading the IMEI list.  Please try again later.";

                $('#error-dialog').modal();
            });
    };

    $scope.showDeleteConfirm = function (imeiId) {
        var imei = $.grep($scope.imeis, function (e) { return e.Id == imeiId; });

        if (imei.length == 1) {
            imei = imei[0];
        }
        else
            return;

        $scope.deleteName = imei.CallSign;
        $scope.deleteId = imei.Id;

        $('#delete-dialog').modal();
    }

    $scope.removeItem = function () {
        IMEI.remove({ imeiId: $scope.deleteId },
            function () { $scope.refresh(); },
            function () {
                $scope.errorTitle = "Couldn't Delete IMEI";
                $scope.errorMessage = "There was an error deleting that IMEI.  Please try again later.";

                $('#error-dialog').modal();
            });
    }

    $scope.refresh();
}]);