/*jslint
    browser: true
*/
/*global angular $ */

var appControllers = angular.module('appControllers', ['appServices']);

appControllers.controller("DeleteFormCtrl", ["$scope", "$modalInstance", "name", function ($scope, $modalInstance, name) {
    "use strict";
    $scope.name = name;

    $scope.ok = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);

appControllers.controller("ErrorFormCtrl", ["$scope", "$modalInstance", "title", "message", function ($scope, $modalInstance, title, message) {
    "use strict";
    $scope.title = title;
    $scope.message = message;

    $scope.close = function () {
        $modalInstance.close();
    };
}]);

appControllers.controller('ControlPanelCtrl', [function () {
    "use strict";

    return;
}]);

appControllers.controller('AdminCtrl', ['$scope', 'User', '$modal', function ($scope, User, $modal) {
    "use strict";
    $scope.sortBy = 'UserName';
    $scope.sortReverse = false;
    $scope.userFilter = '';

    $scope.dialogEmail = "";
    $scope.dialogRole = "";

    $scope.showError = function (title, message) {
        $modal.open({
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

        var modalInstance = $modal.open({
            animation: true,
            templateUrl: "/Dialog/DeleteForm",
            controller: "DeleteFormCtrl",
            resolve: {
                name: function () {
                    return user.UserName;
                }
            }
        });

        modalInstance.result.then(function () {
            User.remove({
                userId: "'" + user.Id + "'"
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Delete User", "There was an error deleting that User.  Please try again later.");
            });
        });
    };

    $scope.refresh();
}]);

appControllers.controller('EditImeiCtrl', ["$scope", "$modalInstance", "createMode", "imei", "callsign", "type", function ($scope, $modalInstance, createMode, imei, callsign, type) {
    "use strict";

    $scope.createMode = createMode;
    $scope.imei = createMode ? "" : imei;
    $scope.callsign = createMode ? "" : callsign;
    $scope.type = createMode ? "Unknown" : type;

    $scope.ok = function () {
        $modalInstance.close({
            imei: $scope.imei,
            callsign: $scope.callsign,
            type: $scope.type
        });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);

appControllers.controller('ImeiListCtrl', ['$scope', 'IMEI', '$modal', function ($scope, IMEI, $modal) {
    "use strict";
    $scope.sortBy = 'IMEI';
    $scope.sortReverse = false;
    $scope.imeiFilter = '';

    $scope.dialogImei = "";
    $scope.dialogCallsign = "";
    $scope.dialogType = 0;
    $scope.createMode = true;

    $scope.showError = function (title, message) {
        $modal.open({
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

    $scope.showUpdateImei = function (id) {
        var imei = $.grep($scope.imeis, function (e) {
            return e.Id === id;
        });

        if (imei.length === 1) {
            imei = imei[0];
        } else {
            $scope.showError("Couldn't Update IMEI", "There was an error updating that IMEI.  Please try again later.");
            return;
        }

        var modalInstance = $modal.open({
            animation: true,
            templateUrl: "/IMEI/EditForm",
            controller: "EditImeiCtrl",
            resolve: {
                createMode: function () {
                    return false;
                },
                imei: function () {
                    return imei.IMEI;
                },
                callsign: function () {
                    return imei.CallSign;
                },
                type: function () {
                    return imei.Type;
                }
            }
        });

        modalInstance.result.then(function (res) {
            var i = new IMEI();
            i.IMEI = res.imei;
            i.CallSign = res.callsign;
            i.Type = res.type;

            i.Id = id;
            i.$update({
                imeiId: id
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Update IMEI", "There was an error updating that IMEI.  Please try again later.");
                $scope.refresh();
            });
        });
    };

    $scope.showNewImei = function () {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: "/IMEI/EditForm",
            controller: "EditImeiCtrl",
            resolve: {
                createMode: function () {
                    return true;
                },
                imei: function () {
                    return "";
                },
                callsign: function () {
                    return "";
                },
                type: function () {
                    return "";
                }
            }
        });

        modalInstance.result.then(function (res) {
            var i = new IMEI();
            i.IMEI = res.imei;
            i.CallSign = res.callsign;
            i.Type = res.type;

            i.$save([], function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Create IMEI", "There was an error creating that IMEI.  Please try again later.");
                $scope.refresh();
            });
        });
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

        var modalInstance = $modal.open({
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
                imeiId: imei.Id
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Delete IMEI", "There was an error deleting that IMEI.  Please try again later.");
            });
        });
    };

    $scope.refresh();
}]);