define(['app', 'ngload!ui-bootstrap', 'directive/navMenu', 'directive/placeHoldButton', 'factory/searchFactory'], function (app) {
    app.register.controller('recordDetailsController', ['$scope', '$log', '$location', 'Search', function ($scope, $log, $location, search) {
        // console.log($scope.SearchResults);
        $scope.SearchKeyword = $location.search()['r'];

        search.xRecordQuery({ 'agControlId': $scope.SearchKeyword }).then(function (result) {
            $scope.Availabilities = result.Availabilities;
            $scope.RecordResult = result.RecordModel;            
        });
    } ]);
}); 
