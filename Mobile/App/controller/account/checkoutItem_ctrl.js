
define(['app', 'directive/_checkoutItem'], function (app) {
    app.register.controller('checkoutItemController', ['$scope', '$cookieStore', '$modalInstance', function ($scope, $cookieStore, $modalInstance) {        
        $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
        $scope.ok = function () { $modalInstance.close(); };
    } ]);

}); 
