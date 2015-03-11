
define(['app', 'service/sessionServices'], function (app) {
    app.register.factory('Auth', ['$http', '$q', '$log', 'Session', function ($http, $q, $log, xSession) {
        var baseUrl = "/mvc/Mobile/MobileAccount/";
        var initilizeGuestSession = function () {
            var d = $q.defer();
            //console.log(xSession.GetSession());
            $http.post(baseUrl + "InitilizeGuestSession")
                .success(function (result) {
                    d.resolve(result);
                })
                .error(function (result, status, headers, config) {
                    $log.error("Error: ", headers);
                    d.reject(result);
                });
            return d.promise;
        };
        //        var getCurrentLibraryList = function () {
        //            var session = xSession.getSession;
        //            var d = $q.defer();
        //            if (!session) {
        //                initilizeGuestSession().then(function (result) {
        //                    console.log(result);
        //                    this.libraryList(result.Session).then(function (data) {
        //                        console.log(data);
        //                        d.resolve(data);
        //                    }, function (data) {
        //                        d.reject(data);
        //                    });
        //                });
        //            }
        //            return d.promise;
        //        };
        var libraryList = function (session) {

            var d = $q.defer();
            if (typeof session === "undefined" || session === "") {
                $log.error("session: missing username.");
                d.reject("session must be provided");
            } else {
                $http.post(baseUrl + 'LibraryList', session)
                        .success(function (result) {
                            d.resolve(result);
                        })
                        .error(function (result, status, headers, config) {
                            $log.error("Error: ", headers);
                            d.reject(result);
                        });
            }
            return d.promise;
        };
        var userAuthenticate = function (username, password, libraryId, session) {
            var d = $q.defer();
            if (typeof username === "undefined" || username === "") {
                $log.error("Login: missing username.");
                d.reject("username must be provided");
            } else {
                $http.post(baseUrl + 'UserAuthenticate', { 'mobileUser': { 'Username': username, 'Password': password, 'LibraryId': libraryId }, 'agEnzoSession': session })
                        .success(function (result) {
                            d.resolve(result);
                            xSession.SetSession(result);
                        })
                        .error(function (result, status, headers, config) {
                            $log.error("Error: ", headers);
                            d.reject(result);
                        });
            }
            return d.promise;
        };
        var authenticateSession = function () {
            return xSession.GetSession();
        };
        var destorySession = function() {
            return xSession.DestorySession();
        };
        return {
            InitilizeGuestSession: initilizeGuestSession,
            LibraryList: libraryList,
            UserAuthenticate: userAuthenticate,
            AuthenticateSession: authenticateSession,
            DestorySession: destorySession
        };
    } ]);
});