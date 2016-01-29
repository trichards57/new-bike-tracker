var appServices = angular.module('appServices', ['ngResource']);

appServices.factory('IMEI', ['$resource',
    function ($resource) {
        return $resource('/odata/IMEI(:imeiId)', {}, {
            query: {
                method: 'GET',
                params: { imeiId: '' },
                isArray: true,
                transformResponse: {
                    function(data, headersGetter) {
                        var d = angular.fromJson(data);
                        return d.value;
                    }
                }
            },
            update: { method: 'PUT' }
        })
    }
]);

appServices.factory('User', ['$resource',
    function ($resource) {
        return $resource('/odata/User(:userId)', {}, {
            query: {
                method: 'GET',
                params: { userId: '' },
                isArray: true,
                transformResponse: {
                    function(data, headersGetter) {
                        var d = angular.fromJson(data);
                        return d.value;
                    }
                }
            },
            update: { method: 'PUT' }
        })
    }
]);