define(['app', 'jquery', 'directive/kidsCatalog/header', 'factory/kidsCatFactory'], function (app, jq) {
    app.register.controller('KidsCatResultController', ['$scope', '$location', '$routeParams', 'kidsCatFactory', function ($scope, $location, $routeParams, kidsCatFactory) {

        var clusters = [];
        function disableKidsStyleSheet() {
            jq('#kidsStylesheet').prop("disabled", true); //disable
            jq('link[rel~="stylesheet"]').prop("disabled", false); //re-enable all stylesheets
        };
        function enableKidsStyleSheet() {
            jq('link[rel="stylesheet"]').prop("disabled", true); //disable
            jq('#kidsStylesheet').prop("disabled", false); //re-enable kids stylesheet
        };
        var displaytSearchResult = function (rKeyword, rStartrecord, rNumberrecord, rSearchId, rsortOption) {
            //console.log(rKeyword, rStartrecord, rNumberrecord, rSearchId, rsortOption);
            kidsCatFactory.getSearchResult(rKeyword, rStartrecord, rNumberrecord, rSearchId, rsortOption).then(

                function (result) {
                    pollId = result.data.PollID;
                    $scope.clusters = result.data.Clusters;
                    $scope.TotalMatchedRecords = parseInt(result.data.TotalMatchedRecords);
                    $scope.TotalMergedRecords = parseInt(result.data.TotalMergedRecords);

                    if (result.data.IsActive) {
                        displaytSearchResult(rKeyword, rStartrecord, rNumberrecord, rSearchId, rsortOption);
                    }
                },
                function (error) {
                });
        };
        $scope.hasPrev = function () {
            return $scope.startRecord >= $scope.pageSize;
        };
        $scope.hasNext = function () {
            return $scope.startRecord + $scope.pageSize < $scope.TotalMergedRecords;
        };
        $scope.loadPrev = function () {
            if ($scope.hasPrev()) {
                $scope.PageNum = $scope.PageNum - 1;
                $scope.startRecord = $scope.startRecord - $scope.pageSize;
                displaytSearchResult($routeParams.keyword, $scope.startRecord, $scope.pageSize, $routeParams.searchId, $routeParams.sortOption);
            }
        };
        $scope.loadNext = function () {
            if ($scope.hasNext()) {
                $scope.PageNum = $scope.PageNum + 1;
                $scope.startRecord = $scope.startRecord + $scope.pageSize;
                displaytSearchResult($routeParams.keyword, $scope.startRecord, $scope.pageSize, $routeParams.searchId, $routeParams.sortOption);
            }
        };
        $scope.getFullRecord = function (cluster) {
            //var url = "kidsCatalog/fullRecord/ac/"    + keyWord() + "/" + data.Groupings()[0].Items[0].MemberID + "/" + data.Groupings()[0].Items[0].PollId + "/" + data.Groupings()[0].Items[0].Title;
            var url = "/kidsCat/fullRecord/default/" + $routeParams.keyword + "/" + cluster.Groupings[0].Items[0].MemberID + "/" + cluster.Groupings[0].Items[0].PollId + "/0";
            $location.path(url);
//            router.resetBreadCrumb();
//            router.addBreadCrumb(url, pageHeaderTitle(), true);
//            router.navigate(url, true);

        };
        var init = function () {
            //  debugger;
            enableKidsStyleSheet();
            $scope.pageHeaderTitle = $routeParams.pageHeaderTitle;
            $scope.PageNum = parseInt($routeParams.startrecord) / parseInt($routeParams.numberrecord) + 1;
            $scope.startRecord = parseInt($routeParams.startrecord);
            $scope.pageSize = parseInt($routeParams.numberrecord);

            displaytSearchResult($routeParams.keyword, $routeParams.startrecord, $routeParams.numberrecord, $routeParams.searchId, $routeParams.sortOption);
        };

        init();
    } ]);

});
