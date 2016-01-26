var app = angular.module('app', [
    'ngRoute',
    'ngResource',
    'ui.validate',
    'appControllers'
]);

app.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/', {
                templateUrl: '/ControlPanel/Home',
                controller: 'ControlPanelCtrl'
            }).
            when('/IMEIs', {
                templateUrl: '/IMEI/Home',
                controller: 'ImeiListCtrl'
            }).
            when('/Admin', {
                templateUrl: '/Admin/Home',
                controller: 'AdminCtrl'
            }).
            otherwise({
                redirectTo: '/'
            });
    }
]);

var compareTo = function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
};

app.directive("compareTo", compareTo);
