define(['app', 'angularAMD','factory/accountFactory'], function (app, angularAMD) {
    app.register.controller("_myProfileController", ['$scope', 'Account', function ($scope, xAccount) {

//        $scope.credentials = { username: '', password: '' };
//        $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
//        $scope.ok = function () { $modalInstance.close(); };
        xAccount.UserProfile().then(function (result) {
            $scope.UserProfile = result;            
        });

    } ]);
    app.register.directive('profilePanel', function () {
        return {
            restrict: 'E',
            controller: '_myProfileController',
            templateUrl: '../Areas/Mobile/App/directive/templates/account/_myProfile.html',            
        };
    });
});

