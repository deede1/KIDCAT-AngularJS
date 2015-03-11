define(['app'], function (app) {

    app.register.factory('kidsCatFactory', ['$http', '$q', '$log', function ($http, $q, $log) {

        var factory = {};
        var kidsCatalogBaseUrl = "/mvc/KidsCatalog/";
        var BaseUrl = "/mvc/";

        factory.getCategories = function () {
            var deferedAction = $q.defer();
            $http.get(kidsCatalogBaseUrl + 'loadBrowsingCategories').then(function (response) {
                deferedAction.resolve(response);
            }, function (error) {
                deferedAction.reject(error);
            });
            return deferedAction.promise;
        };
        factory.getFullRecord = function (keyword, searchId, rpollId) {
            var deferedAction = $q.defer();
            $http.post(BaseUrl + 'FullRecord/FullRecordJson', { 'searchId': searchId, 'searchTerm': keyword, 'max': 20, 'pollId': rpollId, 'nextRecordID': '', 'isClusterRecord': "0" }).then(function (response) {
                deferedAction.resolve(response);
            }, function (error) {
                deferedAction.reject(error);
            });
            return deferedAction.promise;


        };
        factory.search = function (rMySearchQuery) {
            var deferedAction = $q.defer();
            $http.post(BaseUrl + 'AdvancedSearch/Search', rMySearchQuery).then(function (response) {
                deferedAction.resolve(response);
            }, function (error) {
                deferedAction.reject(error);
            });
            return deferedAction.promise;
        };

        factory.getSearchResult = function (rKeyword, rStartrecord, rNumberrecord, rSearchId, rsortOption) {
            var deferedAction = $q.defer();
            //            var rMySearchResultQuery = {
            //                Keyword: rKeyword,
            //                startrecord: rStartrecord,
            //                numberrecord: rNumberrecord,
            //                searchId: rSearchId,
            //                sortOption: rsortOption
            //            };
            //            $http.post(BaseUrl + 'Search/PollSearchResults/', rMySearchResultQuery).then(function (response) {
            $http.post(BaseUrl + 'Search/PollSearchResults/' + rKeyword + "/" + rStartrecord + "/" + rNumberrecord + "/" + rSearchId + "/?sortOption=" + rsortOption).then(function (response) {
                deferedAction.resolve(response);
            }, function (error) {
                deferedAction.reject(error);
            });
            return deferedAction.promise;
        };

        factory.getKidsCatalogConfig = function () {
            var deferedAction = $q.defer();
            $http.get(kidsCatalogBaseUrl + 'KidsCatalogConfig').then(function (response) {
                deferedAction.resolve(response);
            }, function (error) {
                deferedAction.reject(error);
            });
            return deferedAction.promise;


        };
        return factory;

        //        return {
        //            getCategories: getCategories,
        //            search: search,
        //            getKidsCatalogConfig: getKidsCatalogConfig,
        //            getSearchResult: getSearchResult,
        //            getFullRecord: getFullRecord
        //        };
    } ]);


});