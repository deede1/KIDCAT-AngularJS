define(['app', 'ngload!ui-bootstrap', 'directive/navMenu', 'service/sessionServices', 'service/modalboxServices', 'factory/authFactory'], function (app) {
    app.register.controller('HomeController', ['$scope', '$window', '$routeParams', '$modal', '$log', '$location', 'Session', 'modalbox', 'Auth', function ($scope, $window, $routeParams, $modal, $log, $location, xSession,xModalbox,xAuth) {
        $scope.myInterval = 5000;
        var slides = $scope.slides = [];
        $scope.Calender = function () {
            $location.path("/map");
        };
        //        $scope.addSlide = function () {
        //            var newWidth = 600 + slides.length;
        //            slides.push({
        //                image: 'http://placekitten.com/' + newWidth + '/300',
        //                text: ['More', 'Extra', 'Lots of', 'Surplus'][slides.length % 4] + ' ' + ['Cats', 'Kittys', 'Felines', 'Cutes'][slides.length % 4]
        //            });
        //        };
        //        for (var i = 0; i < 4; i++) {
        //            $scope.addSlide();
        //        }

        slides.push({ image: '/mvc/Areas/Mobile/Public/images/mainContent01.jpg', text: "1" });
        slides.push({ image: '/mvc/Areas/Mobile/Public/images/mainContent02.jpg', text: "2" });
        slides.push({ image: '/mvc/Areas/Mobile/Public/images/mainContent03.jpg', text: "3" });

        $scope.MyAccount = function () {
            if (xAuth.AuthenticateSession() != null) {                
                $location.path("/myAccount");
            } else {
                xModalbox.Modalbox("account", "login", "loginController", '', "", function (result) { $location.path("/myAccount");  });
            }            
        };
        $scope.Events = function () {            
            $location.path("/events");
        };

        //        xAuth.InitilizeGuestSession().then(function (sessionResult) {
        //            $scope.userSession = sessionResult;
        //            xAuth.LibraryList(sessionResult.Session).then(function (libraryListResult) {
        //                //                var filter = function (propertyName, propertyValue, collection) {
        //                //                    var i = 0, len = collection.length;
        //                //                    for (; i < len; i++) {
        //                //                        if (collection[i][propertyName] == propertyValue) {
        //                //                            return collection[i];
        //                //                        }
        //                //                    }
        //                //                    return collection[0];
        //                //                };
        //                console.log(libraryListResult);
        //                $scope.libraryList = {
        //                    activities: libraryListResult.LibarayList,
        //                    currentActivity: libraryListResult.LibarayList[0] //filter('IsChecked', true, libraryListResult.LibarayList)
        //                };
        //            });
        //        });


    } ]);
}); 
