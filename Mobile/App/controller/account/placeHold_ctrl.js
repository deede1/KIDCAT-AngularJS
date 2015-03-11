define(['app', 'angular-animate', 'factory/accountFactory'], function (app) {
    app.register.controller('placeHoldController', ['$scope', '$cookieStore', '$modalInstance', 'Account', 'record',
        function ($scope, $cookieStore, $modalInstance, xAccount, xRecord) {
            $scope.showMsg = false;
            $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
            $scope.ok = function () {                
                xAccount.PlaceTitleReserve(xRecord.AgControlId, $scope.libraryList.currentActivity.PickupLocationId).then(function(result) {
                    $scope.showMsg = true;                    
                });
            };
            xAccount.PickupLocation().then(function (pickupLocationResult) {
                $scope.libraryList = {
                    activities: pickupLocationResult.PickupLocations,
                    currentActivity: pickupLocationResult.PickupLocations[0]
                };
            });

        } ]);
}); 
