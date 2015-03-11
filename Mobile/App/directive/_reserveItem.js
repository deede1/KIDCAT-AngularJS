define(['app', 'angularAMD','factory/accountFactory'], function (app, angularAMD) {
    app.register.controller("_reserveItemController", ['$scope', 'Account', function ($scope, xAccount) {
        $scope.cancelReserveItem = function (data) {           
            xAccount.CancelReserveItem(data.ReserveID).then(function (result) { $scope.ReserveItems = result; });
        };
        xAccount.UserReserveItems().then(function (result) { $scope.ReserveItems = result;});
    } ]);
    app.register.directive('reserveitemPanel', function () {
        return {
            restrict: 'E',
            controller: '_reserveItemController',
            templateUrl: '../Areas/Mobile/App/directive/templates/account/_reserveItem.html',            
        };
    });
});
