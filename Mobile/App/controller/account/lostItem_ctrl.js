define(['app', 'directive/_lostItem'], function (app) {
    app.register.controller('lostItemController', ['$scope', '$cookieStore', '$modalInstance', function ($scope, $cookieStore, $modalInstance) {        
        $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
        $scope.ok = function () { $modalInstance.close(); };      
    } ]);

}); 
