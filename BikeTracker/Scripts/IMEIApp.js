var imeiApp = angular.module('imeiApp', ['ngRoute', 'ngResource']);

imeiApp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/IMEIs', {
                templateUrl: 'IMEI/Home',
                controller: 'ImeiListCtrl'
            }).
            otherwise({
                redirectTo: '/IMEIs'
            });
    }
]);

imeiApp.factory('IMEI', ['$resource',
    function ($resource) {
        return $resource('odata/:imeiId', {}, {
            query: {
                method: 'GET',
                params: { imeiId: 'IMEI' },
                isArray: true,
                transformResponse: {
                    function(data, headersGetter) {
                        var d = angular.fromJson(data);
                        return d.value;
                    }
                }
            }
        })
    }
]);

imeiApp.controller('ImeiListCtrl', ['$scope', 'IMEI', function ($scope, IMEI) {
    $scope.imeis = IMEI.query();
}]);