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
        var seriesOptions = [],
        yAxisOptions = [],
        seriesCounter = 0,
        names = ['MSFT', 'AAPL', 'GOOG'],
        colors = Highcharts.getOptions().colors;

        $.each(names, function(i, name) {

            $.getJSON('http://www.highcharts.com/samples/data/jsonp.php?filename='+ name.toLowerCase() +'-c.json&callback=?',   function(data) {

                seriesOptions[i] = {
                    name: name,
                    data: data
                };

                // As we're loading the data asynchronously, we don't know what order it will arrive. So
                // we keep a counter and create the chart when all the data is loaded.
                seriesCounter++;

                if (seriesCounter == names.length) {
                    createChart();
                }
            });
        });



        // create the chart when all data is loaded
        function createChart() {

            angular.element('#container').highcharts('StockChart', {

                rangeSelector: {
                    inputEnabled: $('#container').width() > 480,
                    selected: 4
                },

                yAxis: {
                    labels: {
                        formatter: function() {
                            return (this.value > 0 ? '+' : '') + this.value + '%';
                        }
                    },
                    plotLines: [{
                        value: 0,
                        width: 2,
                        color: 'silver'
                    }]
                },

                plotOptions: {
                    series: {
                        compare: 'percent'
                    }
                },

                tooltip: {
                    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
                    valueDecimals: 2
                },

                series: seriesOptions
            });
        }

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

        /**
         * Get NYT data from API
         *
         * @param int Page we want to load
         */
        $scope.getData = function(page) {
            $scope.isLoading = true;

            // Set up API URI
            var apiString = 'http://api.nytimes.com/svc/search/v2/articlesearch.json?fq=news_desk:("Business")&limit=100&api-key=a931fc7951cee1141ff8bdf3f37a49db:16:69480846&fl=web_url,snippet,headline'
            apiString += '&page=' + page;

            // Do it!
            var dataPromise = $http.get(
                apiString
            );

            dataPromise.success(function(data, status, headers, config) {
                $scope.data = data;
                $scope.isLoading = false;
            });

            dataPromise.error(function(data, status, headers, config) {
                $scope.dataGrabError = true;
            });

            $scope.currentPage = page;
        }

        // Initial data load of page 1
        $scope.getData(0);
    }
]);



FrontPageControllers.controller('RSSDataFeedController', [
    '$scope',
    '$parse',
    '$http',
    '$location',
    '$routeParams',
    'FeedService',
    function(
        $scope,
        $parse,
        $http,
        $location,
        $routeParams,
        FeedService
    ) {

        $scope.url = 'http://finance.yahoo.com/rss/industry?s=msft';

        $scope.processFeed = function() {
            FeedService.parseFeed($scope.url).then(function(response) {
                $scope.feeds = response.data.responseData.feed.entries;
            });
        }
    }
]);
