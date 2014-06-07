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

        /**
         * Helper function to reload articles from API
         */
        var reloadArticles = function() {
            $scope.articles = Article.query();
        }

        // Trigger initial load of articles to get content
        reloadArticles();

        // Handle functionality for adding a new article to API
        $scope.addArticleButton = function() {
            if ($scope.title) {
                var articleToAdd = new Article({
                    title: $scope.title,
                    source: $scope.source,
                    content: $scope.content
                });

                articleToAdd.$save(
                    function(response) {
                        $scope.title = '';
                        $scope.source = '';
                        $scope.content = '';
                        $scope.displaySuccessAddMessage = true;
                        reloadArticles();
                    }
                );
            }
        }

        // Handle deleting an article from API
        $scope.deleteButton = function(id) {
            var articleToDelete = Article.get({id:id}, function() {
                articleToDelete.$delete();
                reloadArticles();
                $scope.displaySuccessDeleteMessage = true;
            });
        };

    }
]);
