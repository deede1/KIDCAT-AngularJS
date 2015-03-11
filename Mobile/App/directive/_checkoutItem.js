define(['app', 'angularAMD','factory/accountFactory'], function (app, angularAMD) {
    app.register.controller("_checkoutitemController", ['$scope', 'Account', function ($scope, xAccount) {
       
        $scope.renewCheckoutItem = function (data) {            
            xAccount.RenewCheckoutItem(data.HoldingsID).then(function (result) { $scope.CheckoutItems = result;});
        };
        
        xAccount.UserCheckoutItems().then(function (result) { $scope.CheckoutItems = result;});
        
    } ]);
    app.register.directive('checkoutitemPanel', function () {
        return {
            restrict: 'E',
            controller: '_checkoutitemController',
            templateUrl: '../Areas/Mobile/App/directive/templates/account/_checkoutItem.html',            
        };
    });
});










