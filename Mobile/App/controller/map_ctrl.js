define(['app', 'service/mapServices', 'directive/navMenu'], function (app) {
    app.register.controller('MapController', ['$scope', 'MapService', function($scope, MapService) {
        $scope.title = "Where is Auto Graphics Inc?";

        $scope.latitude = 34.067887;
        $scope.longitude = -117.61024;

        // Set the location to be Colosseum
        MapService.initialize($scope, "map-canvas");
    }]);
}); 
