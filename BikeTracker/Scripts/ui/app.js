var app = angular.module('app', [
    'ngRoute',
    'ngResource',
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
            otherwise({
                redirectTo: '/'
            });
    }
]);



