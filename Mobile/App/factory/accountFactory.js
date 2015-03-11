define(['app', 'factory/authFactory'], function (app) {
    app.register.factory('Account', ['$http', '$q', '$log', 'Auth', function ($http, $q, $log, xAuth) {
        var baseUrl = "/mvc/Mobile/MobileAccount/";
        var httpPost = function (xMethod, xData) {
            var d = $q.defer();
            if (typeof xMethod === "undefined" || xMethod === "") {
                $log.error("Missing controller name.");
                d.reject("controller name must be provided");
            } else {
                $http.post(baseUrl + xMethod, xData)
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
        var initilizeGuestSession = function () {
            httpPost("InitilizeGuestSession", "");
        };
        var libraryList = function (session) {
            httpPost("LibraryList", session);
        };
        var userAuthenticate = function (username, password, libraryId, session) {
            httpPost("UserAuthenticate", { 'mobileUser': { 'Username': username, 'Password': password, 'LibraryId': libraryId }, 'agEnzoSession': session });
        };
        var userLostItems = function () {
            return httpPost("GetAllLostItemsUnderPatron", { 'agEnzoSession': xAuth.AuthenticateSession().Session });
        };
        var userReserveItems = function () {
            return httpPost("GetAllReservesUnderPatron", { 'agEnzoSession': xAuth.AuthenticateSession().Session });
        };
        var userCheckoutItems = function () {
            return httpPost("GetAllCheckedOutItemsUnderPatron", { 'agEnzoSession': xAuth.AuthenticateSession().Session });
        };
        var userProfile = function () {
            return httpPost("GetUserProfile", { 'agEnzoSession': xAuth.AuthenticateSession().Session });
        };
        var renewCheckoutItem = function (holdingId) {
            return httpPost("RenewCheckoutItem", { 'agEnzoSession': xAuth.AuthenticateSession().Session, 'holdingId': holdingId });
        };
        var cancelReserveItem = function (reserveId) {
            return httpPost("CancelReserveItem", { 'agEnzoSession': xAuth.AuthenticateSession().Session, 'reserveId': reserveId });
        };
        var placeTitleReserve = function (agControlId, pickupLocationId) {
            return httpPost("PlaceTitleReserve", { 'agEnzoSession': xAuth.AuthenticateSession().Session, 'agControlId': agControlId, 'pickupLocationId': pickupLocationId });
        };
        var pickupLocation = function () {
            return httpPost("GetPickupLocation", { 'agEnzoSession': xAuth.AuthenticateSession().Session });
        };
        return {
            InitilizeGuestSession: initilizeGuestSession,
            LibraryList: libraryList,
            UserAuthenticate: userAuthenticate,
            UserLostItems: userLostItems,
            UserReserveItems: userReserveItems,
            UserCheckoutItems: userCheckoutItems,
            UserProfile: userProfile,
            PickupLocation: pickupLocation,
            PlaceTitleReserve: placeTitleReserve,
            CancelReserveItem: cancelReserveItem,
            RenewCheckoutItem: renewCheckoutItem
        };
    } ]);
});