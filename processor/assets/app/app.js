var marketApp= angular.module('marketApp', [
    'ngRoute',
    'FrontPageControllers'
]);

marketApp.config( [
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
            controller: 'ViewFrontPageController',
            templateUrl: 'app/views/frontpage.html'
        })
        .otherwise(
            { redirectTo: '/' }
        );
    }
]);
