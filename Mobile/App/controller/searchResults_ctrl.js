define(['app', 'angularAMD', 'ngload!ui-bootstrap', 'directive/navMenu', , 'directive/placeHoldButton', 'factory/searchFactory', 'factory/authFactory'], function (app, angularAMD) {
    app.register.controller('SearchResultsController', ['$scope', '$modal', '$log', '$location', 'Search', 'Auth', function ($scope, $modal, $log, $location, search, xAuth) {

        $scope.totalItems = 64;
        $scope.currentPage = 0;
        $scope.prevPage = -1;
        $scope.totalHits = 0;
        $scope.pageChanged = function () {
            var navId = "";
            if ($scope.prevPage > $scope.currentPage) {
                navId = $scope.SearchResults.MobileSearchQueryResult.PrevSetId;
            } else {
                navId = $scope.SearchResults.MobileSearchQueryResult.NextSetId;
            }
            search.xSearchQuery({ 'keyword': $location.search()['q'], 'navId': navId }).then(function (result) {
                $scope.SearchResults = result;
            });
            $scope.prevPage = $scope.currentPage;
        };

        $scope.SearchKeyword = $location.search()['q'];
        $scope.recordDetail = function (agControlId) {
            $location.path("/recordDetail").search("r", agControlId);
        };

        search.xSearchQuery({ 'keyword': $location.search()['q'] }).then(
            function (result) {
                $scope.SearchResults = result;
                $scope.totalHits = result.MobileSearchQueryResult.TotalHits;
            },
            function (err) {
                $scope.totalHits = 0;
            }


            );
    } ]);
}); 
