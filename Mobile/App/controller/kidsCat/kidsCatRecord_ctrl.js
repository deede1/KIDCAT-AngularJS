define(['app', 'jquery', 'directive/kidsCatalog/header', 'factory/kidsCatFactory'], function (app, jq) {
    app.register.controller('KidsCatRecordController', ['$scope', 'kidsCatFactory', '$location', '$routeParams', '$sce', function ($scope, kidsCatFactory, $location, $routeParams, $sce) {




        function disableKidsStyleSheet() {
            jq('#kidsStylesheet').prop("disabled", true); //disable
            jq('link[rel~="stylesheet"]').prop("disabled", false); //re-enable all stylesheets
        };
        function enableKidsStyleSheet() {
            jq('link[rel="stylesheet"]').prop("disabled", true); //disable
            jq('#kidsStylesheet').prop("disabled", false); //re-enable kids stylesheet
        };

        //        function LabelField(data) {
        //            this.Title = ko.observable(data.Title);
        //            this.Value = ko.observable(data.Value);
        //            this.IsHotLink = ko.observable(data.IsHotLink);
        //            this.SearchValue = ko.observable(data.SearchValue);
        //            this.Index = ko.observable(data.Index);
        //            this.hotLinkSearch = function (searchId, searchTerm, searchIndex) {

        //                utilities.ajaxPostJson("/Search/FullRecordHotLink/", { 'guidSearchId': searchId, 'searchTerm': searchTerm, 'searchIndex': searchIndex }, function (datax) {
        //                    if (!datax.showError) {
        //                        var lastView;
        //                        if (router.getLastView().length > 0) { lastView = router.getLastView()[0].view; }
        //                        else { lastView = datax.viewType; }
        //                        var url = "kidsCatalog/searchResult/" + datax.searchKeyword + "/" + "gallery" + "/" + datax.searchKeyword + "/0/18" + "/" + datax.searchCacheID + "/0";
        //                        router.resetBreadCrumb();
        //                        router.addBreadCrumb(url, datax.searchKeyword, true);
        //                        router.navigate(url, true);
        //                    } else { alert("Sorry, We are experiencing technical difficulties, Please try your search again."); }

        //                });
        //            }
        //            this.HotLink = function (data, event) {
        //                var memberId = event.view.location.hash;
        //                var searchId = memberId.split('/')[4].split(',')[0];
        //                if ((memberId) && (searchId) && (data.SearchValue) && (data.Index)) {
        //                    this.hotLinkSearch(searchId, data.SearchValue, data.Index);
        //                } else { alert("Sorry, We are experiencing technical difficulties, Please try your search again."); }
        //            };
        //        }

        var goBack = function () {
            var url = '';
            if ((typeof (router.breadCrumbs) !== 'undefined') && (router.breadCrumbs !== null)) {
                if (router.breadCrumbs.length > 0) {
                    var SearchResultHderTitle = router.breadCrumbs[0].text;
                    router.resetBreadCrumb();
                    if (Isac()) {
                        url = "kidsCatalog/home/";
                    }
                    else {
                        url = "kidsCatalog/searchResult/" + SearchResultHderTitle + "/gallery/" + router.activeInstruction().params[1] + "/" + router.activeInstruction().params[2].split(",")[1] + "/" + router.activeInstruction().params[2].split(",")[2] + "/" + router.activeInstruction().params[2].split(",")[0] + "/0";
                    }
                    router.addBreadCrumb(url, SearchResultHderTitle, true);
                    router.navigate(url, true);
                }
            }
        };

        var printRecord = function () {
            $("#print-preview").jqprint({ importCSS: true });
        };

        var getItemLocation = function (startedRecord, numberOfRecords, memberId, pollId) {

            utilities.ajaxPostJson("/Search/GetItemLocation", {
                memberId: memberId,
                fromZipCode: "",
                pollId: pollId,
                startRecord: startedRecord,
                numberOfRecords: numberOfRecords

            },
            function (result) {
                if (result.InvalidFromZipCode) {
                    alert("This is Invalid Zip Code");
                }
                else {
                    //console.log(result);
                    TotalLocations(result.TotalLocations);
                    ItemAvailabilities(result.ItemAvailabilities);

                    $.each(result.ItemAvailabilities, function (index, item) {
                        $.each(item.Holdings, function (index, item) {

                            Holdings.push(item);
                        });
                    });
                }

            });
        };

        var ilsPlaceHold = function () {
            var placeHoldingModel = function () {
                var that = this;
                if (fullRecord().FullRecord.ItemAvailabilities.length > 0) {
                    that.AvailableCopies = fullRecord().FullRecord.ItemAvailabilities[0].AvailableCopies;
                } else {
                    that.AvailableCopies = null;
                }
                that.ItemID = fullRecord().FullRecord.ItemId;
                that.MemberID = fullRecord().FullRecord.MemeberId;
                that.ResourceId = fullRecord().FullRecord.ResourceId;
                that.ResourceName = fullRecord().FullRecord.ResourceName;
                that.ResourceType = fullRecord().FullRecord.ResourceType;
                that.TotalCopies = 1;
                that.linkHoldings = fullRecord().FullRecord.ItemAvailabilities.length > 0;
                that.AllowItemLevelReserve = fullRecord().AllowItemLevelReserve;
                that.AllowPatronReserveNote = fullRecord().AllowPatronReserveNote;
                that.TitleAuthor = fullRecord().FullRecord.Title + " / " + fullRecord().FullRecord.Author;
                that.callBack = resetStyles;
                that.sourceId = 1;
            };
            disableKidsStyleSheet();
            var x = holdingReservesx.show(new placeHoldingModel(), "");
            console.log(x);
        };
        var resetStyles = function () {
            enableKidsStyleSheet();
            //window.location.reload();
        };

        var getAgControlFullRecord = function (agControlId, libraryDbPoolKey, rpollId, waitTime) {
            if ((agControlId) && (libraryDbPoolKey)) {
                setTimeout(function () {
                    xhr = utilities.ajaxPostSearch("/FullRecord/FulRecordlAgControlJson/", { 'agControlId': agControlId, 'libraryDbPoolKey': libraryDbPoolKey });
                    xhr.done(function (datax) {
                        mapFullrecordInfo(datax, libraryDbPoolKey, rpollId);
                        showSmallLoader(false);
                    });
                    xhr.error(function (xhrx, ajaxOptions, thrownError) {
                        alert(thrownError);
                        showSmallLoader(false);
                    });
                }, waitTime);
            } else if (agControlId) {
                setTimeout(function () {
                    xhr = utilities.ajaxPostSearch("/FullRecord/FulRecordlAgControlOnlyJson/", { 'agControlId': agControlId });
                    xhr.done(function (datax) {
                        mapFullrecordInfo(datax);

                    });
                    xhr.error(function (xhrx, ajaxOptions, thrownError) {
                        alert(thrownError);
                        showSmallLoader(false);
                    });
                }, waitTime);
            }

        }

        var getFullRecord = function (keyword, searchId, rpollId) {
            kidsCatFactory.getFullRecord(keyword, searchId, rpollId).then(function (data) {
                $scope.fullRecord = data.data;
                $scope.RecordTitle = data.data.FullRecord.Title;
                // mapFullrecordInfo(searchId, rpollId);
                $.each(data.data.FullRecord.LabelFields, function (index, item) {
                    $.each(item, function (index, item) {
//                        console.log(item);
                        $sce.trustAsHtml(item.toString());
                    });
                    //getItemLocation(1, 20, searchId, rpollId);
                    console.log(data.data.FullRecord.TotalLocations);
                });
            }, function (err) {

            })

        };

        var mapFullrecordInfo = function (searchId, rpollId) {

            $.each($scope.fullRecord.FullRecord.LabelFields, function (index, item) {
                $.each(item, function (index, item) {
                    $sce.trustAsHtml(item);
                });
                //getItemLocation(1, 20, searchId, rpollId);

            });

            //location(s) 
            //            $scope.TotalLocations = datax.FullRecord.TotalLocations;
            //            $scope.ItemAvailabilities = datax.FullRecord.ItemAvailabilities;



        }

        var init = function (src, keyword, searchId, rpollId, isPlaceHold) {

            $scope.JacketArtUrl = "";
            $scope.RecordTitle = "";
            //$scope.LabelFields([]);
            $scope.Isac = false;
            $scope.TotalLocations = 0;
            if (($routeParams.keyword) && ($routeParams.searchId) && ($routeParams.pollId)) {
                switch (src) {
                    case "ac":
                        $scope.Isac = true;
                        if ($routeParams.pollId == 1) {
                            getAgControlFullRecord($routeParams.keyword, $routeParams.searchId, $routeParams.pollId, 1000);
                        }
                        break;
                    default:
                        getFullRecord($routeParams.keyword, $routeParams.searchId, $routeParams.pollId);
                        break;
                }
            }
        }

        init();



    } ]);

});

