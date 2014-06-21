/**
 * Controllers for the front page
 *
 * @author Su Wang <sxw323@psu.edu>
 */

'use strict';

var FrontPageControllers = angular.module('FrontPageControllers', [
]);

FrontPageControllers.controller('RealTimeAnalysisController',[
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
