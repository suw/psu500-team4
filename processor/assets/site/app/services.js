'use strict';

/**
 * Service for requesting sentiment analysis for a given URL
 *
 * @author Su Wang <sxw323@psu.edu>
 */


var SentimentServices = angular.module('SentimentServices', ['ngResource']);

SentimentServices.factory('SentimentRequest', ['$resource', function($resource) {
    var SentimentRequest = $resource('/sentiment/',
        { id: '@id' },
        {
            update: { method: 'PUT' }
        }
    );
    return SentimentRequest;
    }
]);

SentimentServices.factory('FeedService', ['$http', function($http) {
    return {
        parseFeed: function (url) {
            return $http.jsonp('//ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=50&callback=JSON_CALLBACK&q=' + encodeURIComponent(url));
        }
    }
}]);
