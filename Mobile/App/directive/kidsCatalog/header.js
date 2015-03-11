define(['app'], function (app) {
    app.register.controller("kidsheaderController", ['$scope', '$location', function ($scope, $location) {

        function disableKidsStyleSheet() {
            jq('#kidsStylesheet').prop("disabled", true); //disable
            jq('link[rel~="stylesheet"]').prop("disabled", false); //re-enable all stylesheets
        }
        function enableKidsStyleSheet() {
            jq('link[rel="stylesheet"]').prop("disabled", true); //disable
            jq('#kidsStylesheet').prop("disabled", false); //re-enable kids stylesheet
        }

        $scope.goHome = function () {
            // load the home page 
            console.log($location.path('/kidsCat/home/'));
            $location.path('/kidsCat/home/');
        };

        $scope.exit = function () {
            disableKidsStyleSheet();
            $location.path('/kidsCat/home/');
        };

    } ]);



    app.register.directive('kidsheader', function () {
        return {
            restrict: 'E',
            controller: 'kidsheaderController',
            templateUrl: '../Areas/Mobile/App/directive/kidsCatalog/header.html'
        }
    });

});