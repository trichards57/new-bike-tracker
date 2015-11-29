var app = angular.module('app', [
    'ngRoute',
    'ngResource',
    'appControllers'
]);

app.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/IMEIs', {
                templateUrl: '/IMEI/Home',
                controller: 'ImeiListCtrl'
            }).
            otherwise({
                redirectTo: '/IMEIs'
            });
    }
]);



