define(['app', 'directive/_myProfile'], function (app) {
    app.register.controller('myProfileController', ['$scope', '$cookieStore', '$modalInstance', function ($scope, $cookieStore, $modalInstance) {
        $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
        $scope.ok = function () { $modalInstance.close(); };
    } ]);
}); 
