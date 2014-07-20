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
        .when('/dashboard/:symbol?',
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
        .when('/source-rss/',
        {
            controller: 'RSSDataFeedController',
            templateUrl: 'app/view/source-rss.html'
        })
        .otherwise(
            { redirectTo: '/' }
        );
    }
]);

predictlyApp.run(function($rootScope) {
    $rootScope.alchemyApiKey = '315505f383ab7bc362f60a8c663a51fe2381e71d';
});
