/*property
    $on, $save, $update, CallSign, EmailAddress, IMEI, Id, Location, Map,
    MapTypeId, Maps, Role, Type, UserName, animation, callsign, callsigns,
    cancel, center, close, controller, createMode, credentials, data,
    dateOptions, dialogCallsign, dialogEmail, dialogImei, dialogRole,
    dialogType, dismiss, document, email, formatYear, get, getElementById,
    grep, imei, imeiFilter, imeiId, imeis, initialize, length, loading,
    mapTypeId, message, module, name, ok, open, openDate, preventDefault,
    previousTitle, query, refresh, remove, resolve, result, road, role,
    selectedCallsign, selectedDate, showAscending, showDate, showDates,
    showDeleteConfirm, showError, showNewImei, showNewUser, showUpdateImei,
    showUpdateUser, showWeeks, sortBy, sortReverse, startingDay,
    stopPropagation, templateUrl, then, title, type, updateSortBy, userFilter,
    userId, users, zoom
*/
/*jslint
    browser: true
*/
/*global angular $ Microsoft */

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

appControllers.controller('LocationReportCtrl', ['$scope', '$window', '$modal', '$http', function ($scope, $window, $modal, $http) {
    "use strict";

    $scope.initialize = function () {
        $scope.previousTitle = $window.document.title;
        $window.document.title = "Location Reports - SJA Tracker";

        $scope.$on("$destroy", function () {
            $window.document.title = $scope.previousTitle;
        });
    };

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

    $scope.callsigns = [];

    $scope.selectedCallsign = '';

    $scope.selectedDate = new Date();

    $scope.openDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.showDate = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: false
    };

    $http.get("/api/Report/Callsigns").then(function (response) {
        $scope.callsigns = response.data;
        $scope.selectedCallsign = $scope.callsigns[0];
    }, function () {
        $scope.showError("Failed to Load Callsigns", "It wasn't possible to load the callsigns from the server.")
    });

    var mapSettings = {
        credentials: "ApfWme6djEwhx5JqGqyzMf8PrYvSmspgz_nsCamSsEab7AK46NNwhEGd840O1QH3",
        center: new Microsoft.Maps.Location(51.45, -2.5833),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 14
    };
    var map = new Microsoft.Maps.Map(document.getElementById("mapDiv"), mapSettings);

    $scope.showDates = function () {

        map.entities.clear();

        var d = moment($scope.selectedDate).format("YYYYMMDD");

        $http.get("/api/Report/CallsignLocations?callsign=" + $scope.selectedCallsign + "&startDate=" + d + "&endDate=" + d).then(function (response) {
            var reports = response.data;

            reports.forEach(function (dat) {
                var content = "<div class='callsign-flag text-success bg-success'>";
                var reportD = moment(dat.ReadingTime);
                content += reportD.format('HH:mm');
                content += "</div>";

                var options = {
                    width: null, height: null, htmlContent: content, anchor: new Microsoft.Maps.Point(22.5, 10)
                };

                var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(dat.Latitude, dat.Longitude), options);
                map.entities.push(pin);
            });
        }, function () {
            $scope.showError("Failed to Load Location Reports", "It wasn't possible to load the location reports from the server.")
        });
    };

    $scope.initialize();
}]);

appControllers.controller('ReportCtrl', ["$scope", "$window", function ($scope, $window) {
    "use strict";

    $scope.initialize = function () {
        $scope.previousTitle = $window.document.title;
        $window.document.title = "Reports Control Panel - SJA Tracker";

        $scope.$on("$destroy", function () {
            $window.document.title = $scope.previousTitle;
        });
    };

    $scope.initialize();
}]);

appControllers.controller('ControlPanelCtrl', ["$scope", "$window", function ($scope, $window) {
    "use strict";

    $scope.initialize = function () {
        $scope.previousTitle = $window.document.title;
        $window.document.title = "Administrator Control Panel - SJA Tracker";

        $scope.$on("$destroy", function () {
            $window.document.title = $scope.previousTitle;
        });
    };

    $scope.initialize();
}]);

appControllers.controller('EditUserCtrl', ["$scope", "$modalInstance", "$window", "createMode", "email", "role", function ($scope, $modalInstance, $window, createMode, email, role) {
    "use strict";

    $scope.initialize = function () {
        $scope.previousTitle = $window.document.title;

        if (createMode) {
            $window.document.title = "New User - SJA Tracker";
        } else {
            $window.document.title = "Edit User - SJA Tracker";
        }

        $scope.$on("$destroy", function () {
            $window.document.title = $scope.previousTitle;
        });
    };

    $scope.createMode = createMode;
    $scope.email = createMode
        ? ""
        : email;
    $scope.role = createMode
        ? "Normal"
        : role;

    $scope.ok = function () {
        $modalInstance.close({
            email: $scope.email,
            role: $scope.role
        });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };

    $scope.initialize();
}]);

appControllers.controller('AdminCtrl', ['$scope', 'User', '$modal', '$window', function ($scope, User, $modal, $window) {
    "use strict";

    $scope.initialize = function () {
        $scope.previousTitle = $window.document.title;
        $window.document.title = "User Control Panel - SJA Tracker";

        $scope.$on("$destroy", function () {
            $window.document.title = $scope.previousTitle;
        });
    };

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

    $scope.showUpdateUser = function (id) {
        var user = $.grep($scope.users, function (e) {
            return e.Id === id;
        });

        if (user.length === 1) {
            user = user[0];
        } else {
            $scope.showError("Couldn't Update User", "There was an error updating that user.  Please try again later.");
            return;
        }

        var modalInstance = $modal.open({
            animation: true,
            templateUrl: "/Admin/EditForm",
            controller: "EditUserCtrl",
            resolve: {
                createMode: function () {
                    return false;
                },
                email: function () {
                    return user.EmailAddress;
                },
                role: function () {
                    return user.Role;
                }
            }
        });

        modalInstance.result.then(function (res) {
            var i = new User();
            i.Id = user.Id;
            i.EmailAddress = res.email;
            i.Role = res.role;

            i.$update({
                userId: "'" + i.Id + "'"
            }, function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Update User", "There was an error updating that user.  Please try again later.");
                $scope.refresh();
            });
        });
    };

    $scope.showNewUser = function () {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: "/Admin/EditForm",
            controller: "EditUserCtrl",
            resolve: {
                createMode: function () {
                    return true;
                },
                email: function () {
                    return "";
                },
                role: function () {
                    return "";
                }
            }
        });

        modalInstance.result.then(function (res) {
            var i = new User();
            i.email = res.email;
            i.role = res.role;

            i.$save([], function () {
                $scope.refresh();
            }, function () {
                $scope.showError("Couldn't Create User", "There was an error creating that user.  Please try again later.");
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

    $scope.initialize();
    $scope.refresh();
}]);

appControllers.controller('EditImeiCtrl', ["$scope", "$modalInstance", "$window", "createMode", "imei", "callsign", "type", function ($scope, $modalInstance, $window, createMode, imei, callsign, type) {

    "use strict";

    $scope.initialize = function () {
        $scope.previousTitle = $window.document.title;

        if (createMode) {
            $window.document.title = "New IMEI - SJA Tracker";
        } else {
            $window.document.title = "Edit IMEI - SJA Tracker";
        }

        $scope.$on("$destroy", function () {
            $window.document.title = $scope.previousTitle;
        });
    };

    $scope.createMode = createMode;
    $scope.imei = createMode
        ? ""
        : imei;
    $scope.callsign = createMode
        ? ""
        : callsign;
    $scope.type = createMode
        ? "Unknown"
        : type;

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

    $scope.initialize();
}]);

appControllers.controller('ImeiListCtrl', ['$scope', 'IMEI', '$modal', '$window', function ($scope, IMEI, $modal, $window) {
    "use strict";

    $scope.initialize = function () {
        $scope.previousTitle = $window.document.title;
        $window.document.title = "IMEI Control Panel - SJA Tracker";

        $scope.$on("$destroy", function () {
            $window.document.title = $scope.previousTitle;
        });
    };

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
        $scope.imeis = IMEI.query({
        }, function () {
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


    $scope.initialize();
    $scope.refresh();
}]);