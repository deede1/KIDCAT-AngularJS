define(['app','directive/_reserveItem'], function (app) {
    app.register.controller('reserveItemController', ['$scope', '$cookieStore', '$modalInstance', 'Auth', 'Account', function ($scope, $cookieStore, $modalInstance, xAuth, xAccount) {
        $scope.credentials = { username: '', password: '' };
        $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
        $scope.ok = function () { $modalInstance.close(); };
       
    } ]);

}); 
