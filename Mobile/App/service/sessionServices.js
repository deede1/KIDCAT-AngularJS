
define(['app'], function (app) {
    app.register.service('Session', ['$cookieStore', function ($cookieStore) {
        var session = {};
        var destorySession = function () {            
            $cookieStore.remove('session', session);
        };
        var setSession = function (data) {            
            session = data;
            $cookieStore.put('session', data);
        };
        var getSession = function () {            
            session = $cookieStore.get('session');
            return session;
        };
        return {
            DestorySession: destorySession,
            SetSession: setSession,
            GetSession: getSession
        };


        //        var create = function (sessionId, userId, userRole) {
        //            this.id = sessionId;
        //            this.userId = userId;
        //            this.userRole = userRole;
        //        };
        //        var destroy = function () {
        //            this.id = null;
        //            this.userId = null;
        //            this.userRole = null;
        //        };
        //        return {
        //            Create: create,
        //            Destory: destroy
        //        };
    } ]);
});