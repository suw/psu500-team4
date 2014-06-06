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
        $locationProvider.html5Mode(true);
        $routeProvider
        .when('/:articleId?',
        {
            controller: 'ViewFrontPageController',
            templateUrl: 'app/views/frontpage.html'
        })
        .otherwise(
            { redirectTo: '/' }
        );
    }
]);
