define(['app', 'jquery', 'directive/kidsCatalog/header', 'factory/kidsCatFactory'], function (app, jq) {
    app.register.controller('KidsCatHomeController', ['$scope', 'kidsCatFactory', '$location', function ($scope, kidsCatFactory, $location) {


        var catalogConfig;
        function searchterm(index, operator, option, phrasex, pharaseType) {
            var st = this;
            st.index = index;
            st.operator = operator;
            st.option = option;
            st.phrase = phrasex;
            st.phraseType = pharaseType;
        };
        function dataCollection() {

            this.sortOrder = "0";
            this.pageSize = 18;
            this.languages = [];
            this.formats = [];
            this.resources = [catalogConfig.ResourceID]; //[1986913]; //
            this.publication = "All";
            this.pubDate1 = "";
            this.pubDate2 = "";
            this.pubDateOperator = "All";
            this.library = "All Libraries";
            this.libraryLocations = 0;
            this.DaysWithin = "0";
            this.IgnoreFacet = true;

        };


        function disableKidsStyleSheet() {
            jq('#kidsStylesheet').prop("disabled", true); //disable
            jq('link[rel~="stylesheet"]').prop("disabled", false); //re-enable all stylesheets
        };
        function enableKidsStyleSheet() {
            jq('link[rel="stylesheet"]').prop("disabled", true); //disable
            jq('#kidsStylesheet').prop("disabled", false); //re-enable kids stylesheet
        };

        function calculateWidth(categories) {
            var width = 714 * Math.ceil(categories.length / 7);
            jq('.carousel').width(width);

            if (width === 714)
                jq('.carousel-arrow').hide();
            else
                jq('.carousel-arrow.right').show();

        };

        function performSearch(searchData) {

            if ((typeof (searchData.search) !== 'undefined') && (searchData.search !== null)) {
                var searchTerms = [];
                //check if keyword1,2,3 are available before insertion 
                if ((typeof (searchData.search.keyword1) !== 'undefined') && (searchData.search.keyword1 !== null)) {
                    searchTerms.push(new searchterm(searchData.search.keyword1.field.index, searchData.search.operator1, "", searchData.search.keyword1.keywords, searchData.search.keyword1.field.matchType));
                }
                if ((typeof (searchData.search.keyword2) !== 'undefined') && (searchData.search.keyword2 !== null)) {
                    searchTerms.push(new searchterm(searchData.search.keyword2.field.index, searchData.search.operator2, "", searchData.search.keyword2.keywords, searchData.search.keyword2.field.matchType));
                }
                if ((typeof (searchData.search.keyword3) !== 'undefined') && (searchData.search.keyword3 !== null)) {
                    searchTerms.push(new searchterm(searchData.search.keyword3.field.index, "AND", "", searchData.search.keyword3.keywords, searchData.search.keyword3.field.matchType));
                }
                var getMaterialTypes = function () {
                    var MaterialTypes = [];
                    jq.each(searchData.search.materialTypes, function (i, v) { MaterialTypes.push(v.value); });
                    return MaterialTypes;
                }
                var mySearchQuery = new dataCollection();
                dataCollection.prototype.searchTerms = null;
                mySearchQuery.searchTerms = searchTerms; //ko.toJS(searchTerms);

                dataCollection.prototype.materialTypes = null;
                mySearchQuery.materialTypes = getMaterialTypes();

                dataCollection.prototype.TargetAudienceList = null;
                mySearchQuery.TargetAudienceList = searchData.search.targetAudienceList;

                var tempSearchTerm = [];
                jq.each(mySearchQuery.searchTerms, function (i, v) {
                    tempSearchTerm.push(new searchterm(v.index, v.operator, v.option, v.phrase, v.phraseType));
                });

                mySearchQuery.searchTerms = tempSearchTerm;
                Search(searchData.title, mySearchQuery);

            }
        }
        function Search(keyword, rMySearchQuery) {

            kidsCatFactory.search(rMySearchQuery).then(function (result) {
                //debugger;
                var xSearchTermKeyword = "";
                for (var i = 0; i < rMySearchQuery.searchTerms.length; i++) {
                    xSearchTermKeyword += rMySearchQuery.searchTerms[i].phrase;
                    if ((rMySearchQuery.searchTerms[i + 1] != null) && (rMySearchQuery.searchTerms[i + 1].phrase != "") && (i != rMySearchQuery.searchTerms.length - 1)) {
                        xSearchTermKeyword += " " + rMySearchQuery.searchTerms[i].operator + " ";
                    }
                };

                var searchTkw = encodeURIComponent(xSearchTermKeyword.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, ''));
                var url = "/kidsCat/searchResult/" + keyword + "/" + result.data.ResultViewType + "/" + searchTkw + "/0/" + rMySearchQuery.pageSize + "/" + result.data.searchID + "/" + rMySearchQuery.sortOrder;
                console.log(url);
                $location.path(url);
                //                router.resetBreadCrumb();
                //                router.addBreadCrumb(url, xSearchTermKeyword, true);
                //                router.navigate(url, true);

            }, function (err) {

                alert(err);

            });
        };


        $scope.setMouseOverImg = function ($event, obj) {
            jq($event.currentTarget).children('img').attr("src", obj.imageUrlMouseOver);
        };


        $scope.setMouseDownImg = function ($event, obj) {
            jq($event.currentTarget).children('img').attr("src", obj.imageUrlMouseDown);
        };

        $scope.setMouseOutImg = function ($event, obj) {
            jq($event.currentTarget).children('img').attr("src", obj.imageUrl);
        };

        $scope.loadSubCategory = function (category) {
            //            console.log(category);
            if (category.subcategories !== null || category.subcategories != undefined) {
                $scope.subcategories = category.subcategories;
                $scope.carouselTitle = category.title;
                // Calculate width of the carousel container based on the 
                // number of subcategories
                calculateWidth(category.subcategories);
                jq('.carousel-scroll-container').scrollLeft(0);
            }
            else {
                // If no subcategories found, do a POST for the search an
                // don't redcaclulate the carousel widths
                performSearch(category);
            }
        };
        $scope.carouselPrevPage = function () {
            var curScroll = jq('.carousel-scroll-container').scrollLeft();
            var width = jq('.carousel').width();
            jq('.carousel-scroll-container').scrollLeft(curScroll - 714);
            jq('.carousel-arrow.right').show();
            if (curScroll - 714 === 0)
                jq('.carousel-arrow.left').hide();
        };
        $scope.carouselNextPage = function () {
            var curScroll = jq('.carousel-scroll-container').scrollLeft();
            var width = jq('.carousel').width();
            jq('.carousel-scroll-container').scrollLeft(curScroll + 714);
            jq('.carousel-arrow.left').show();
            if (curScroll + 714 === width - 714)
                jq('.carousel-arrow.right').hide();
        };

        function init() {
            enableKidsStyleSheet();
            $scope.carouselTitle = "";
            //load search categories 
            kidsCatFactory.getCategories().then(function (response) {
                $scope.subcategories = response.data.subcategories;
                $scope.carouselTitle = response.data.title;
                // Calculate width of the carousel container based on the 
                // number of subcategories
                calculateWidth(response.data.subcategories);
                kidsCatFactory.getKidsCatalogConfig().then(function (result) {
//                    debugger;
                    catalogConfig = result.data;
                }, function (err) {

                });
            },


            function (error) {

            });
        };

        init();
    } ]);
}); 

