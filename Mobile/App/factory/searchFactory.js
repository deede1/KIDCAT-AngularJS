define(['app'], function (app) {    
    app.register.factory('Search', ['$http', '$q', '$log', function ($http, $q, $log) {
        var searchUrl = "/mvc/Mobile/MobileSearch/GetSearchResult/";
        var recordUrl = "/mvc/Mobile/MobileSearch/GetRecordDetail/";
        var recordQuery = function(data) {
            var d = $q.defer();
            if (typeof data === "undefined" || data === "") {
                $log.error("Keyword: missing keyword.");
                d.reject("Keyword must be provided");
            } else {
                $http.post(recordUrl, data)
                    .success(function(result) {
                        d.resolve(result);
                    })
                    .error(function(result, status, headers, config) {
                        $log.error("Error: ", headers);
                        d.reject(result);
                    });
            }
            return d.promise;
        };
        var searchQuery = function(data) {
            var d = $q.defer();
            if (typeof data === "undefined" || data === "") {
                $log.error("Keyword: missing keyword.");
                d.reject("Keyword must be provided");
            } else {
                $http.post(searchUrl, data)
                    .success(function(result) {
                        d.resolve(result);
                    })
                    .error(function(result, status, headers, config) {
                        $log.error("Error: ", headers);
                        d.reject(result);
                    });
            }
            return d.promise;
        };

        return {
            xRecordQuery: recordQuery,
            xSearchQuery: searchQuery
        };
    } ]);
});