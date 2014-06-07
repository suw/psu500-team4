var DataServices = angular.module('DataServices', ['ngResource']);

DataServices.factory('Article', ['$resource', function($resource) {
    var Article = $resource('/article/',
        { id: '@id' },
        {
            update: { method: 'PUT' }
        }
    );
    return Article;
    }
]);
