define(['app', 'factory/authFactory'], function (app) {
    app.register.controller('loginController', ['$scope', '$rootScope', '$cookieStore', '$modalInstance', 'Auth', function ($scope, $rootScope, $cookieStore, $modalInstance, xAuth) {
        $scope.msg = "";
        $scope.credentials = { username: '', password: '' };
        $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
        $scope.ok = function () {
            $scope.credentials.msg = "";
            xAuth.UserAuthenticate($scope.credentials.username,
                                        $scope.credentials.password,
                                        $scope.libraryList.currentActivity.Value,
                                        $scope.userSession.Session).then(function (loginResult) {

                                            if (loginResult.EncounteredError) {
                                                $scope.msg = loginResult.ErrorMessage;
                                            } else {
                                                $rootScope.loginButtonLabel = "Logout";
                                                $rootScope.ShowLoginFunction = true;
                                                $modalInstance.close(loginResult);
                                            }
                                        }, function () {
                                            console.log("fail");
                                        });
        };


        xAuth.InitilizeGuestSession().then(function (sessionResult) {
            $scope.userSession = sessionResult;
            xAuth.LibraryList(sessionResult.Session).then(function (libraryListResult) {
                //                var filter = function (propertyName, propertyValue, collection) {
                //                    var i = 0, len = collection.length;
                //                    for (; i < len; i++) {
                //                        if (collection[i][propertyName] == propertyValue) {
                //                            return collection[i];
                //                        }
                //                    }
                //                    return collection[0];
                //                };                
                $scope.libraryList = {
                    activities: libraryListResult.LibarayList,
                    currentActivity: libraryListResult.LibarayList[0] //filter('IsChecked', true, libraryListResult.LibarayList)
                };
            });
        });

    } ]);

}); 
