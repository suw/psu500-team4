/**
 * Primary app module for Predictly
 *
 * @author Su Wang <sxw323@psu.edu>
 */

'use strict';

var predictlyApp = angular.module('predictlyApp', [
    'ngRoute',
    'FrontPageControllers'
]);

predictlyApp.config([
    '$routeProvider',
    '$locationProvider',
    function (
        $routeProvider,
        $locationProvider
    ) {
        $locationProvider
            .html5Mode(false)
            .hashPrefix('!');
        $routeProvider
        .when('/',
        {
            templateUrl: 'app/view/frontpage.html'
        })
        .when('/dashboard/',
        {
            controller: 'DashboardController',
            templateUrl: 'app/view/dashboard.html'
        })
        .when('/real-time/',
        {
            controller: 'RealTimeAnalysisController',
            templateUrl: 'app/view/real-time-analysis.html'
        })
        .when('/source-nyt/',
        {
            controller: 'SourceNYTController',
            templateUrl: 'app/view/source-nyt.html'
        })
        .otherwise(
            { redirectTo: '/' }
        );
    }
]);
