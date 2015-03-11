define(['app', 'angularAMD', 'ngload!ui-bootstrap', 'service/modalboxServices', 'factory/authFactory'], function (app, angularAMD) {
    app.register.controller("navMenuController", ['$scope', '$rootScope', '$window', '$location', 'modalbox', 'focus', 'Auth', function ($scope, $rootScope, $window, $location, xModalbox, focus, xAuth) {
        focus('focusMe');
        $scope.SearchKeyword = "";
        $scope.MobileSearch = function () {
            $location.path("/searchResults").search('q', $scope.SearchKeyword);
        };
        $scope.LostItemBox = function () {
            xModalbox.Modalbox("account", "lostItem", "lostItemController", "", "", function (result) {
                //console.log(result);
            });
        };
        $scope.ReserveItemBox = function () {
            xModalbox.Modalbox("account", "reserveItem", "reserveItemController", "", "", function (result) {
                //console.log(result);
            });
        };
        $scope.CheckoutItem = function () {
            xModalbox.Modalbox("account", "checkoutItem", "checkoutItemController", "", "", function (result) {
                //console.log(result);
            });
        };
        $scope.MyProfileBox = function () {
            xModalbox.Modalbox("account", "myProfile", "myProfileController", "", "", function (result) {
                //console.log(result);
            });
        };
        $scope.LoginBox = function () {
            xModalbox.Modalbox("account", "login", "loginController", "", "", function (result) {
                $scope.loginButtonLabel = "Logout";
                //console.log(result);
            });
        };
        $scope.Logoff = function () {
            xAuth.DestorySession();
            $rootScope.ShowLoginFunction = false;
            $rootScope.loginButtonLabel = "Login";
            $location.path("/home");
        };
        if (xAuth.AuthenticateSession() == null) {
            $rootScope.ShowLoginFunction = false;
            $rootScope.loginButtonLabel = "Login";
        } else {
            $rootScope.ShowLoginFunction = true;
            $rootScope.loginButtonLabel = "Logout";
        }
        $scope.getLoginFunction = function () {
            return $rootScope.ShowLoginFunction;
        };
        $scope.getLoginButtonLabel = function () {
            return $rootScope.loginButtonLabel;
        };
        //        $scope.$watch("loginButtonLabel", function (loginBtn) {
        //            if (loginBtn == "Logout") {
        //                $scope.ShowLoginFunction = true;
        //            } else {
        //                $scope.ShowLoginFunction = false;
        //            }
        //        });


    } ]);

    app.register.directive('navMenux', function () {
        return {
            restrict: 'E',
            controller: 'navMenuController',
            templateUrl: '../Areas/Mobile/App/directive/templates/navMenu.html'
        };
    });
});

