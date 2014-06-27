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

