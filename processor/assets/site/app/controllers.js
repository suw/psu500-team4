/**
 * Controllers for the front page
 *
 * @author Su Wang <sxw323@psu.edu>
 */

'use strict';

var FrontPageControllers = angular.module('FrontPageControllers', [
    'SentimentServices'
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

        $scope.processArticle = function() {
            var articleRequested = new SentimentRequest({
                apikey: "315505f383ab7bc362f60a8c663a51fe2381e71d",
                flavor: "url",
                url: "http://www.nytimes.com/2014/06/22/technology/yahoo-wants-you-to-linger-on-the-ads-too.html?ref=business&_r=0",
                target: "to",
                jsonp: null
            });

            articleRequested.$save(
                function(response) {
                    console.log(response);
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
