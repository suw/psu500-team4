/**
 * Controllers for the front page
 *
 * @author Su Wang <sxw323@psu.edu>
 */

'use strict';

var FrontPageControllers = angular.module('FrontPageControllers', [
    'SentimentServices',
    'DisplayFilters'
]);

FrontPageControllers.controller('RealTimeAnalysisController',[
    '$scope',
    '$parse',
    '$location',
    '$routeParams',
    'SentimentRequest',
    function(
        $scope,
        $parse,
        $location,
        $routeParams,
        SentimentRequest
    ) {

        /**
         * Call the SentimentRequest service to grab past API requests
         */
        var updatePastRequests = function() {
            $scope.pastRequests = SentimentRequest.query();
        }

        /*
         * Update the table with past requests before doing other stuff
         */
        updatePastRequests();

        /*
         * Trigger an article processing request
         */
        $scope.processArticle = function() {

            // Freshen up the page to be ready for new request
            $scope.sendingRequest = true;
            $scope.requestSuccess = false;
            $scope.responseType = null;
            $scope.responseScore = null;

            // Set up the object to be ready for the request
            var articleRequested = new SentimentRequest({
                apikey: "315505f383ab7bc362f60a8c663a51fe2381e71d",
                flavor: "url",
                url: $scope.requestUrl,
                target: "to",
                jsonp: null
            });

            // Send off the request and handle the response data
            articleRequested.$save(
                function(response) {
                    if (response.type == 'negative' || response.type == 'positive') {
                        $scope.sendingRequest = false;
                        $scope.requestSuccess = true;
                        $scope.requestUrl = '';
                        $scope.responseData = response;
                        $scope.responseType = response.type;
                        $scope.responseScore = response.score;
                    } else {
                        $scope.apiError = true;
                    }
                }
            );
        }
    }
]);

FrontPageControllers.controller('DashboardController',[
    '$scope',
    '$parse',
    '$location',
    '$routeParams',
    function(
        $scope,
        $parse,
        $location,
        $routeParams
    ) {
        $.getJSON('http://www.highcharts.com/samples/data/jsonp.php?filename=aapl-c.json&callback=?', function(data) {
            // Create the chart
            angular.element('#container').highcharts('StockChart', {


                rangeSelector : {
                    selected : 1,
                    inputEnabled: $('#container').width() > 480
                },

                title : {
                    text : 'AAPL Stock Price'
                },

                series : [{
                    name : 'AAPL',
                    data : data,
                    tooltip: {
                        valueDecimals: 2
                    }
                }]
            });
        });


    }
]);

FrontPageControllers.controller('SourceNYTController', [
    '$scope',
    '$parse',
    '$http',
    '$location',
    '$routeParams',
    function(
        $scope,
        $parse,
        $http,
        $location,
        $routeParams
    ) {

        var dataPromise = $http.get('http://api.nytimes.com/svc/search/v2/articlesearch.json?fq=news_desk:("Business")&limit=100&api-key=a931fc7951cee1141ff8bdf3f37a49db:16:69480846&fl=web_url,snippet,headline');

        dataPromise.success(function(data, status, headers, config) {
            $scope.data= data;
        });

        dataPromise.error(function(data, status, headers, config) {
            $scope.dataGrabError = true;
        });
    }
]);
