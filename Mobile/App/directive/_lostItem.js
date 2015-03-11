define(['app', 'angularAMD','factory/accountFactory'], function (app, angularAMD) {
    app.register.controller("_lostItemController", ['$scope', 'Account', function ($scope, xAccount) {
        xAccount.UserLostItems().then(function (result) {
            $scope.LostItems = result;            
        });

    } ]);
    app.register.directive('lostitemPanel', function () {
        return {
            restrict: 'E',
            controller: '_lostItemController',
            templateUrl: '../Areas/Mobile/App/directive/templates/account/_lostItem.html',            
        };
    });
});
