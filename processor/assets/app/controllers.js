var FrontPageControllers = angular.module('FrontPageControllers', [
    'DataServices'
]);

FrontPageControllers.controller('ViewFrontPageController',[
    '$scope',
    '$parse',
    '$location',
    '$routeParams',
    'Article',
    function(
        $scope,
        $parse,
        $location,
        $routeParams,
        Article
    ) {
        var articleId = ('articleId' in $routeParams) ? $routeParams.articleId : null;

        if (articleId == null) {
            var article = new Article({
                id: null,
                title: "Demo Article"
            });

            $scope.article = article;
        } else {
            $scope.article = Article.get(
                {articleId:articleId},
                function() {
                    $scope.isLoading = false;
                }, function() {

                }
            );

        }

    }
]);
