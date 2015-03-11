define(['angularAMD', 'angular-route', 'angular-cookie', 'ui-bootstrap', 'angular-sanitize'], function (angularAMD) {
    var app = angular.module("ngreq-app", ['ngRoute', 'ngCookies', 'ui.bootstrap', 'ngSanitize']);
    /**
    * Configure Angular ngApp with route and cache the needed providers
    */
    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when("/home/", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/home_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/home.html',
                controller: 'HomeController',
                navTab: "home"
            }))
            .when("/searchResults", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/searchResults_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/searchResults.html',
                controller: 'SearchResultsController',
                navTab: "SearchResult"
            }))
            .when("/recordDetail", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/recordDetail_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/recordDetail.html',
                controller: 'recordDetailsController',
                navTab: "recordDetail"
            }))
            .when("/myAccount", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/account/account_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/account/account.html',
                controller: 'myAccountController',
                navTab: "myAccount"
            }))
            .when("/events", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/events_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/events.html',
                controller: 'eventsController',
                navTab: "myAccount"
            }))
            .when("/pictures", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/pictures_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/pictures.html',
                controller: 'PicturesController',
                navTab: "pictures"
            }))
            .when("/modules", angularAMD.route({
                controllerUrl: 'App/controller/modules_ctrl.js',
                templateUrl: 'App/views/modules.html',
                controller: 'ModulesController',
                navTab: "modules"
            }))
            .when("/map", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/map_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/map.html',
                controller: 'MapController',
                navTab: "map"
            }))
            .when("/kidsCat/home/", angularAMD.route({
                controllerUrl: '../Areas/Mobile/App/controller/kidsCat/kidsCatHome_ctrl.js',
                templateUrl: '../Areas/Mobile/App/views/kidsCat/kidsCatHome.html',
                controller: 'KidsCatHomeController'
            }))
            .when("/kidsCat/searchResult/:pageHeaderTitle/:viewType/:keyword/:startrecord/:numberrecord/:searchId/:sortOption/:newsearch?/:facetsearch?/:linkSearch?/:pollIdx?", angularAMD.route({
                 controllerUrl: '../Areas/Mobile/App/controller/kidsCat/kidsCatResult_ctrl.js',
                 templateUrl: '../Areas/Mobile/App/views/kidsCat/kidsCatResult.html',
                 controller: 'KidsCatResultController'
             }))
             .when("/kidsCat/fullRecord/:src/:keyword/:searchId/:pollId/:isPlaceHold", angularAMD.route({
                  controllerUrl: '../Areas/Mobile/App/controller/kidsCat/kidsCatRecord_ctrl.js',
                  templateUrl: '../Areas/Mobile/App/views/kidsCat/kidsCatRecord.html',
                  controller: 'KidsCatRecordController'
             }))
            .otherwise({ redirectTo: '/home' });
    } ]);

    app.run(function ($rootScope) {
        $rootScope.ShowLoginFunction = false;
        $rootScope.loginButtonLabel = "Login";
        //$rootScope.test = new Date();
    });

    // Define constant to be used by Google Analytics

    app.constant("SiteName", "/angularAMD");
    // Add support for pretty print
    app.directive('prettyprint', function () {
        return {
            restrict: 'C',
            link: function postLink(scope, element, attrs) {
                element.html(prettyPrint(scope.dom));
            }
        };
    });
    app.directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });

    app.directive('focusOn', function () {
        return function (scope, elem, attr) {
            scope.$on('focusOn', function (e, name) {
                if (name === attr.focusOn) {
                    elem[0].focus();
                }
            });
        };
    });

    app.factory('focus', function ($rootScope, $timeout) {
        return function (name) {
            $timeout(function () {
                $rootScope.$broadcast('focusOn', name);
            });
        };
    });
    // Bootstrap Angular when DOM is ready




    angularAMD.bootstrap(app);
    return app;
});
