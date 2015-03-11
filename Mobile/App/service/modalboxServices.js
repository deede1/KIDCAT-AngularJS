
define(['app', 'angularAMD', 'ngload!ui-bootstrap'], function (app, angularAMD) {
    app.register.service('modalbox', ['$modal', '$log', function ($modal, $log) {
        var modalbox = function (modalDirectory, modalName, controllerName, size, assignRecord, callback) {
            var modalInstance = $modal.open(angularAMD.route({
                controllerUrl: '/mvc/Areas/Mobile/App/controller/' + modalDirectory + '/' + modalName + '_ctrl.js',
                templateUrl: '/mvc/Areas/Mobile/App/views/' + modalDirectory + '/' + modalName + '.html',
                controller: controllerName,
                size: size,
                resolve: {
                    record: function () {
                        return assignRecord;// $scope.record();
                    }
                }
            }));
            modalInstance.result.then(function (userSession) {
                callback(userSession);
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

        return {
            Modalbox: modalbox
        };
    } ]);
});


