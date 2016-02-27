/*jslint
    browser: true
*/
/*global angular $ */

var appControllers = angular.module('appControllers', ['appServices']);

appControllers.controller("DeleteFormCtrl", ["$scope", "$uibModalInstance", "name", function ($scope, $uibModalInstance, name) {
    "use strict";
    $scope.name = name;

    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };
}]);

appControllers.controller("ErrorFormCtrl", ["$scope", "$uibModalInstance", "title", "message", function ($scope, $uibModalInstance, title, message) {
    "use strict";
    $scope.title = title;
    $scope.message = message;

    $scope.close = function () {
        $uibModalInstance.close();
    };
}]);

appControllers.controller('ControlPanelCtrl', [function () {
    "use strict";

    return;
}]);

appControllers.controller('AdminCtrl', ['$scope', 'User', '$uibModal', function ($scope, User, $uibModal) {
    "use strict";
    $scope.sortBy = 'UserName';
    $scope.sortReverse = false;
    $scope.userFilter = '';

    $scope.dialogEmail = "";
    $scope.dialogRole = "";

    $scope.showError = function (title, message) {
        $uibModal.open({
            animation: true,
            templateUrl: "/Dialog/ErrorForm",
            controller: "ErrorFormCtrl",
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

    $scope.showAscending = function (param) {
        return ($scope.sortBy !== param || ($scope.sortBy === param && !$scope.sortReverse));
    };

    $scope.saveUser = function () {
        var i = new User();

        if ($scope.createMode) {
            i.email = $scope.dialogEmail;
            i.role = $scope.dialogRole;

            i.$save([], function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Create User", "There was an error creating that user.  Please try again later.");
                $scope.refresh();
            });
        } else {
            i.Id = $scope.updateId;
            i.EmailAddress = $scope.dialogEmail;
            i.Role = $scope.dialogRole;
            i.$update({
                userId: "'" + $scope.updateId + "'"
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Create User", "There was an error creating that user.  Please try again later.");
                $scope.refresh();
            });
        }
    };

    $scope.showUpdateUser = function (id) {
        $scope.createMode = false;

        var user = $.grep($scope.users, function (e) {
            return e.Id === id;
        });

        if (user.length === 1) {
            user = user[0];
        } else {
            return;
        }

        $scope.updateId = id;
        $scope.dialogEmail = user.EmailAddress;
        $scope.dialogRole = user.Role;

        $scope.editForm.$setPristine();

        $('#create-dialog').modal();
    };

    $scope.showNewUser = function () {
        $scope.dialogEmail = "";
        $scope.dialogPassword = "";
        $scope.dialogPasswordConfirm = "";
        $scope.dialogRole = "";
        $scope.createMode = true;

        $scope.editForm.$setPristine();

        $('#create-dialog').modal();
    };

    $scope.updateSortBy = function (param) {
        if ($scope.sortBy === param) {
            $scope.sortReverse = !$scope.sortReverse;
        } else {
            $scope.sortBy = param;
            $scope.sortReverse = false;
        }
    };

    $scope.refresh = function () {
        $scope.loading = true;
        $scope.users = User.query({}, function () {
            $scope.loading = false;
        }, function () {
            $scope.showError("Couldn't Load Users", "There was an error loading the User list.  Please try again later.");
        });
    };

    $scope.showDeleteConfirm = function (userId) {
        var user = $.grep($scope.users, function (e) {
            return e.Id === userId;
        });

        if (user.length === 1) {
            user = user[0];
        } else {
            $scope.showError("Couldn't Delete User", "There was an error deleting that User.  Please try again later.");
            return;
        }

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: "/Dialog/DeleteForm",
            controller: "DeleteFormCtrl",
            resolve: {
                name: function () {
                    return user.Name;
                }
            }
        });

        modalInstance.result.then(function () {
            User.remove({
                userId: "'" + $scope.deleteId + "'"
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Delete User", "There was an error deleting that User.  Please try again later.");
            });
        });
    };

    $scope.refresh();
}]);

appControllers.controller('ImeiListCtrl', ['$scope', 'IMEI', '$uibModal', function ($scope, IMEI, $uibModal) {
    "use strict";
    $scope.sortBy = 'IMEI';
    $scope.sortReverse = false;
    $scope.imeiFilter = '';

    $scope.dialogImei = "";
    $scope.dialogCallsign = "";
    $scope.dialogType = 0;
    $scope.createMode = true;

    $scope.showError = function (title, message) {
        $uibModal.open({
            animation: true,
            templateUrl: "/Dialog/ErrorForm",
            controller: "ErrorFormCtrl",
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

    $scope.showAscending = function (param) {
        return ($scope.sortBy !== param || ($scope.sortBy === param && !$scope.sortReverse));
    };

    $scope.saveImei = function () {
        var i = new IMEI();
        i.IMEI = $scope.dialogImei;
        i.CallSign = $scope.dialogCallsign;
        i.Type = $scope.dialogType;

        if ($scope.createMode) {
            i.$save([], function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Create IMEI", "There was an error creating that IMEI.  Please try again later.");
                $scope.refresh();
            });
        } else {
            i.Id = $scope.updateId;
            i.$update({
                imeiId: $scope.updateId
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Update IMEI", "There was an error updating that IMEI.  Please try again later.");
                $scope.refresh();
            });
        }
    };

    $scope.showUpdateImei = function (id) {
        $scope.createMode = false;

        var imei = $.grep($scope.imeis, function (e) {
            return e.Id === id;
        });

        if (imei.length === 1) {
            imei = imei[0];
        } else {
            return;
        }

        $scope.updateId = id;
        $scope.dialogImei = imei.IMEI;
        $scope.dialogCallsign = imei.CallSign;
        $scope.dialogType = imei.Type;

        $scope.editForm.$setPristine();

        $('#edit-dialog').modal();
    };

    $scope.showNewImei = function () {
        $scope.dialogImei = "";
        $scope.dialogCallsign = "";
        $scope.dialogType = "Unknown";
        $scope.createMode = true;

        $scope.editForm.$setPristine();

        $('#edit-dialog').modal();
    };

    $scope.updateSortBy = function (param) {
        if ($scope.sortBy === param) {
            $scope.sortReverse = !$scope.sortReverse;
        } else {
            $scope.sortBy = param;
            $scope.sortReverse = false;
        }
    };

    $scope.refresh = function () {
        $scope.loading = true;
        $scope.imeis = IMEI.query({}, function () {
            $scope.loading = false;
        }, function () {
            $scope.showError("Couldn't Load IMEIs", "There was an error loading the IMEI list.  Please try again later.");
        });
    };

    $scope.showDeleteConfirm = function (imeiId) {
        var imei = $.grep($scope.imeis, function (e) {
            return e.Id === imeiId;
        });

        if (imei.length === 1) {
            imei = imei[0];
        } else {
            $scope.showError("Couldn't Delete IMEI", "There was an error deleting that IMEI.  Please try again later.");
            return;
        }

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: "/Dialog/DeleteForm",
            controller: "DeleteFormCtrl",
            resolve: {
                name: function () {
                    return imei.CallSign;
                }
            }
        });

        modalInstance.result.then(function () {
            IMEI.remove({
                imeiId: $scope.deleteId
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Delete IMEI", "There was an error deleting that IMEI.  Please try again later.");
            });
        });
    };

    $scope.refresh();
}]);