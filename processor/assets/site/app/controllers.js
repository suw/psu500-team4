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
    '$rootScope',
    '$parse',
    '$location',
    '$routeParams',
    'SentimentRequest',
    function(
        $scope,
        $rootScope,
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
                apikey: $rootScope.alchemyApiKey,
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

        /**
         * Display data
         */
        var dataSeriesOptions = [],
        yAxisOptions = [],
        seriesCounter = 0,
        names = ['Actual', 'Predicted'],
        colors = Highcharts.getOptions().colors;

        $.each(names, function(i, name) {
            $.getJSON('/site/fake-data/json/'+ name.toLowerCase() +'.json', function(data) {
                dataSeriesOptions[i] = {
                    name: name,
                    data: data
                };
                // As we're loading the data asynchronously, we don't know what order it will arrive. So
                // we keep a counter and create the chart when all the data is loaded.
                seriesCounter++;

                if (seriesCounter == names.length) {
                    createDataChart();
                }
            }).error( function(jqXHR, textStatus, errorThrown) {
                console.log("error " + textStatus);
                console.log("incoming Text " + jqXHR.responseText);
            });
        });

        // Create data chart
        function createDataChart() {

            angular.element('#data').highcharts('StockChart', {
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

                series: dataSeriesOptions
            });
        }


        /**
         * Display correlation data
         */
        var myData;
        var corrSeriesOptions = [];

        $.getJSON('/site/fake-data/json/jpm-corr.json', function(data) {
            myData = data;
            corrSeriesOptions[0] = {
                name: 'Correlation',
                data: data
            };

            createCorrChart();
        }).error( function(jqXHR, textStatus, errorThrown) {
            console.log("error " + textStatus);
            console.log("incoming Text " + jqXHR.responseText);
        });


        var createCorrChart = function() {
            angular.element('#data-corr').highcharts({
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Data Correlation'
                },
                xAxis: {
                    type: 'datetime'
                },
                yAxis: {
                     title: null,
                     label: { enabled: false },
                     offset: -20,
                     min: -1,
                     max: 1
                },
                series: corrSeriesOptions
            });
        }
    }
]);

FrontPageControllers.controller('SourceNYTController', [
    '$scope',
    '$parse',
    '$rootScope',
    '$http',
    '$location',
    '$routeParams',
    'SentimentRequest',
    function(
        $scope,
        $parse,
        $rootScope,
        $http,
        $location,
        $routeParams,
        SentimentRequest
    ) {

        $scope.processArticle = function(requestUrl, index) {

            // Set up the object to be ready for the request
            var articleRequested = new SentimentRequest({
                apikey: $rootScope.alchemyApiKey,
                flavor: "url",
                url: requestUrl,
                target: "to",
                jsonp: null
            });

            // Send off the request and handle the response data
            articleRequested.$save(
                function(response) {
                    if (response.type == 'negative' || response.type == 'positive') {
                        $scope.data.response.docs[index].sentiment = response.type;
                        $scope.data.response.docs[index].score = response.score;
                        angular.element('#collapse-'+index).collapse('show');
                    } else {
                        $scope.dataGrabError = true;
                    }
                }
            );
        }


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
    '$rootScope',
    '$http',
    '$location',
    '$routeParams',
    'FeedService',
    'SentimentRequest',
    function(
        $scope,
        $parse,
        $rootScope,
        $http,
        $location,
        $routeParams,
        FeedService,
        SentimentRequest
    ) {

        $scope.url = 'http://finance.yahoo.com/rss/industry?s=msft';

        $scope.processFeed = function() {
            FeedService.parseFeed($scope.url).then(function(response) {
                $scope.feeds = response.data.responseData.feed.entries;
            });
        }

        $scope.processArticle = function(requestUrl, index) {


            // Set up the object to be ready for the request
            var articleRequested = new SentimentRequest({
                apikey: $rootScope.alchemyApiKey,
                flavor: "url",
                url: requestUrl,
                target: "to",
                jsonp: null
            });

            // Send off the request and handle the response data
            articleRequested.$save(
                function(response) {

                    if (response.type == 'negative' || response.type == 'positive') {
                        $scope.feeds[index].sentiment = response.type;
                        $scope.feeds[index].score = response.score;
                        angular.element('#collapse-'+index).collapse('show');
                    } else {
                        $scope.dataGrabError = true;
                    }
                }
            );
        }
    }
]);
