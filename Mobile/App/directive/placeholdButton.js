define(['app', 'angularAMD', 'ngload!ui-bootstrap', 'factory/searchFactory', 'service/modalboxServices', 'factory/authFactory'], function (app, angularAMD) {
    app.register.controller("placeHoldButtonController", ['$scope', 'Auth', 'modalbox', function ($scope, xAuth, xModalbox) {

        $scope.PlaceHoldBox = function () {            
            if (xAuth.AuthenticateSession() != null) {                                
                xModalbox.Modalbox("account", "placeHold", "placeHoldController", '', $scope.record(), function (result) { });
            } else {
                xModalbox.Modalbox("account", "login", "loginController", '', "", function (result) { });
            }
        };

    } ]);
    app.register.directive('placeholdButton', function () {
        return {
            restrict: 'E',
            controller: 'placeHoldButtonController',
            templateUrl: '../Areas/Mobile/App/directive/templates/placeHoldButton.html',
            scope: {
                record: '&record'
            }
        };
    });
});

