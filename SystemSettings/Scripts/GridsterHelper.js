
uxAdmin.PageManager.GridsterHelper = (function () {
    var initGridster = function () {
        return $(".gridster ul").gridster({ widget_margins: [2.987, 2.987], widget_base_dimensions: [93.9123, 93.9123], max_size_x: 12, max_size_y: 12,
            serialize_params: function ($w, wgd) { return { id: wgd.el[0].id, col: wgd.col, row: wgd.row }; }
        }).data('gridster');
    };
    var deleteWidgetOnPage = function (gridster, id) {
        gridster.remove_widget(id);
    };
    var addWidgetToPage = function (koObservablePageHeight, gridster, index, col, row, koObservableTemplate, showPreview) {
        uxAdmin.utilities.ajaxPostJson("SystemSettings/WidgetManager/GetWidgetData/", { 'Index': index }, function (datax) {
            var previewTemplate = "";
            var widgetDatax = $.parseJSON(datax.JsonData);
            var widgetTitleName = widgetDatax.WidgetTitleName;
            var widgetDelete = "widgetDelete";
            var sizeX = widgetDatax.selectedColumnSize;
            var sizeY = widgetDatax.selectedRowSize;
            switch (widgetDatax.WidgetType) {
                case 'ContentWidget':
                    var data = widgetDatax;
                    var html = $("<div></div>").append($(data.WidgetContent).clone());
                    var elem = html.find('#cke-background-fill');  //$(content).find('#cke-background-fill');
                    var color = typeof elem == 'undefined' || elem == null || elem == undefined ? 'transparent' : $(elem).attr('data-color');
                    var style = typeof color == 'undefined' ? ' ' : ' style="background-color:' + color + ';" ';
                    //'style' : 'top:'+ $data.locX +'px;left:'+ $data.locY +'px;width:'+ $data.row +'px;height:'+ $data.col +'px'
                    //style="top:0px;left:300px;width:600px;height:800px"
                    //style="width:590px;height:790px;background:#ccffff"                    
                    if ((((row * 100) + (sizeY * 100)) - 100) > koObservablePageHeight()) {
                        koObservablePageHeight(((row * 100) + (sizeY * 100)) - 100);
                    };
                    if (showPreview) {
                        previewTemplate = "<li style='top:" + ((row - 1) * 100) + "px;left:" + ((col - 1) * 100) + "px;width:" + (sizeX * 100) + "px;height:" + (sizeY * 100) + "px;'><div style='width:" + ((sizeX * 100) - 10) + "px;height:" + ((sizeY * 100) - 10) + "px;background:" + color + "'>" + widgetDatax.WidgetContent + "</div></li>";
                        koObservableTemplate(koObservableTemplate() + previewTemplate);
                    } else {
                        gridster.add_widget('<li class="new"' + style + 'id="' + index + '"> <ul class="list4 widgets4"><li><a class="icon delete ' + widgetDelete + '" title="Remove Widget from This Page" value="' + index + '"></a>' + widgetTitleName.substring(0, sizeX * 15) + "..." + '</li></ul><div style="padding-left:10px; padding-right:10px; height:90%; overflow:auto;">' + widgetDatax.WidgetContent + '</div></li>', sizeX, sizeY, col, row);
                    }
                    break;
                case 'TopicSearchWidget':
                    var topicSearchWidgetList = "<div class='topicPreviewHtml'><h4>" + widgetDatax.WidgetHeadline + "</h4><ul>";
                    $.each(widgetDatax.requestedProducts, function (i, v) { topicSearchWidgetList += "<li><span class='icn'></span><a href='#'>" + v.Name + "</a></li>"; });
                    topicSearchWidgetList += "</ul></div>";
                    if ((((row * 100) + (sizeY * 100)) - 100) > koObservablePageHeight()) {
                        koObservablePageHeight(((row * 100) + (sizeY * 100)) - 100);
                    };
                    if (showPreview) {
                        previewTemplate = "<li style='top:" + ((row - 1) * 100) + "px;left:" + ((col - 1) * 100) + "px;width:" + (sizeX * 100) + "px;height:" + (sizeY * 100) + "px;'><div style='width:" + ((sizeX * 100) - 10) + "px;height:" + ((sizeY * 100) - 10) + "px;'>" + topicSearchWidgetList + "</div></li>";
                        koObservableTemplate(koObservableTemplate() + previewTemplate);
                    } else {
                        gridster.add_widget('<li class="new"  id="' + index + '"> <ul class="list4 widgets4"><li><a class="icon delete ' + widgetDelete + '" title="Remove Widget from This Page" value="' + index + '"></a>' + widgetTitleName.substring(0, sizeX * 15) + "..." + '</li></ul>' + topicSearchWidgetList + '</li>', sizeX, sizeY, col, row);
                    }

                    break;
                case 'FeedbackWidget':
                    var feedbackHtml = '<li style="background-color:' + widgetDatax.WidgetBackground + '" class="new"  id="' + index + '"> <ul class="list4 widgets4"><li><a class="icon delete  ' + widgetDelete + '" title="Remove Widget from This Page" value="' + index + '"></a>' + widgetTitleName.substring(0, sizeX * 15) + "..." + '</li></ul>';
                    var feedbackContent = '<div style="padding-left:10px; height:305px; overflow:auto;">' + widgetDatax.WidgetHeadline + '<textarea style="width:95%;"></textarea>check here if you want to reply <input type="checkbox"> <br> Name: <input type="text" style="width:95%;"> <br> Email: <input type="text" style="width:95%;"> <br> Phone: <input type="text"   style="width:95%;" ><br>' + widgetDatax.InstructionalFeedback + '<br><div style="float:right; padding-right: 10px"><input type="button" value="Submit"> <input type="button" value="Reset" ></div></div>';
                    feedbackHtml += feedbackContent + '</li>';
                    if ((((row * 100) + (sizeY * 100)) - 100) > koObservablePageHeight()) {
                        koObservablePageHeight(((row * 100) + (sizeY * 100)) - 100);
                    };
                    if (showPreview) {
                        previewTemplate = "<li style='top:" + ((row - 1) * 100) + "px;left:" + ((col - 1) * 100) + "px;width:" + (sizeX * 100) + "px;height:" + (sizeY * 100) + "px;'><div style='width:" + ((sizeX * 100) - 10) + "px;height:" + ((sizeY * 100) - 10) + "px;background:" + widgetDatax.WidgetBackground + "'>" + feedbackContent + "</div></li>";
                        koObservableTemplate(koObservableTemplate() + previewTemplate);
                    } else {
                        gridster.add_widget(feedbackHtml, sizeX, sizeY, col, row);
                    }

                    break;
                case 'ShowcaseWidget':
                    var showcaseHtml = '<li style="background-color:' + widgetDatax.WidgetBackground + '" class="new"  id="' + index + '"> <ul class="list4 widgets4"><li><a class="icon delete  ' + widgetDelete + '" title="Remove Widget from This Page" value="' + index + '"></a>' + widgetTitleName.substring(0, sizeX * 15) + "..." + '</li></ul>';
                    var showcaseContent = "";
                    if ($.parseJSON(datax.JsonData).selectedShowcaseDisplayType == '0') {
                        showcaseContent += '<div class="liquid" ><span class="previous"></span><div class="wrapper">';
                    } else {

                        var jacketArtUrl = "";
                        var title = "";
                        var author = "";
                        var pubYear = "";
                        var formatType = "";

                        if ($.parseJSON(datax.JsonData).TempShowcaseItems[$.parseJSON(datax.JsonData).TempShowcaseItems.length - 1]) {
                            jacketArtUrl = $.parseJSON(datax.JsonData).TempShowcaseItems[$.parseJSON(datax.JsonData).TempShowcaseItems.length - 1].JackArtUrl;
                            title = $.parseJSON(datax.JsonData).TempShowcaseItems[$.parseJSON(datax.JsonData).TempShowcaseItems.length - 1].Title;
                            author = $.parseJSON(datax.JsonData).TempShowcaseItems[$.parseJSON(datax.JsonData).TempShowcaseItems.length - 1].Author;
                            pubYear = $.parseJSON(datax.JsonData).TempShowcaseItems[$.parseJSON(datax.JsonData).TempShowcaseItems.length - 1].PubYear;
                            formatType = $.parseJSON(datax.JsonData).TempShowcaseItems[$.parseJSON(datax.JsonData).TempShowcaseItems.length - 1].FormatType;
                        }
                        showcaseContent += '<div class="liquid Oneup" ><div class="OneUpShowcaseTempDivWrapper"><div class="OneUpShowcaseTempDiv"><a href="#"><img src="' + jacketArtUrl + '" width="88" height="126" alt="image"/><span class="name"> ' + title + '</span><span class="name">By :' + author + '</span><span class="name"> ' + pubYear + '</span><span class="name"> ' + formatType + '</span></a></div></div><span class="previous" style="left:330px;top:102px;"></span><div class="wrapper" style="width:' + (($.parseJSON(datax.JsonData).selectedColumnSize * 100) - 400) + 'px">';

                    };
                    showcaseContent += '          <ul class="showcaseStandard">';
                    if (widgetDatax.TempShowcaseItems) {
                        $.each(widgetDatax.TempShowcaseItems, function (i, v) {
                            showcaseContent += '              <li><a href="#"><img src="' + v.JackArtUrl + '" width="88" height="126" alt="image"/>';
                            showcaseContent += '                  <span class="name"> ' + v.Title + '</span>';
                            showcaseContent += '                  </a>';
                            showcaseContent += '              </li>';
                        });    
                    }
                    
                    showcaseContent += '          </ul>';
                    showcaseContent += '      </div>';
                    if ($.parseJSON(datax.JsonData).selectedShowcaseDisplayType == '0') {
                        showcaseContent += '<span class="next"></span>';
                    }
                    else {
                        showcaseContent += '<span class="next" style="top:102px;"></span>';
                    };
                    showcaseContent += '  </div>';
                    showcaseHtml += showcaseContent + '</li>';
                    if ((((row * 100) + (sizeY * 100)) - 100) > koObservablePageHeight()) {
                        koObservablePageHeight(((row * 100) + (sizeY * 100)) - 100);
                    };
                    if (showPreview) {
                        previewTemplate = "<li style='top:" + ((row - 1) * 100) + "px;left:" + ((col - 1) * 100) + "px;width:" + (sizeX * 100) + "px;height:" + (sizeY * 100) + "px;'><div style='width:" + ((sizeX * 100) - 10) + "px;height:" + ((sizeY * 100) - 10) + "px;background:" + widgetDatax.WidgetBackground + "'>" + showcaseContent + "</div></li>";
                        koObservableTemplate(koObservableTemplate() + previewTemplate);
                    } else {
                        gridster.add_widget(showcaseHtml, sizeX, sizeY, col, row);
                    }

                    break;
                //                case 'DrawWidget':                                         
                //                    gridster.add_widget('<li class="new"  id="' + index + '"> <ul class="list4 widgets4"><li><a class="icon delete ' + widgetDelete + '" title="Remove Widget from This Page" value="' + index + '"></a>' + widgetTitleName.substring(0, sizeX * 15) + "..." + '</li></ul><center><img  alt=""  src="' + widgetDatax.widgetDrawImage + '" width="95%" height="95%" /></center></li>', sizeX, sizeY, col, row);                                         
                //                    break;                                                              
                //                case 'ShowcaseWidget':                                         
                //                    showcaseWidgetCounter++;                                          
                //                        var x = '<li class="new"  id="' + index + '"><ul class="list4 widgets4"><li><a class="icon delete ' + widgetDelete + '" title="Remove Widget from This Page" value="' + index + '"></a>' + widgetTitleName.substring(0, sizeX * 15) + "..." + '</li></ul><center>' +                                         
                //                                '<div class="list_carousel responsive">' +                                         
                //                                '<ul id="showCaseWidget' + index + showcaseWidgetCounter + '"></ul>' +                                         
                //                                '</div>' +                                         
                //                                '<a id="prev2' + index + showcaseWidgetCounter + '" class="prev" href="#">&lt;</a>' +                                         
                //                                '<a id="next2' + index + showcaseWidgetCounter + '" class="next" href="#">&gt;</a>' +                                         
                //                                '<div id="pager2' + index + showcaseWidgetCounter + '" class="pager"></div>' +                                         
                //                                '</center></li>'.trim();                                         

                //                            gridster.add_widget(x, sizeX, sizeY, col, row);                                         

                //                            $('#showCaseWidget' + index + showcaseWidgetCounter).carouFredSel({                                         
                //                                circular: false,                                         
                //                                infinite: false,                                         
                //                                responsive: false,                                         
                //                                auto: false,                                         
                //                                prev: '#prev2' + index + showcaseWidgetCounter,                                         
                //                                next: '#next2' + index + showcaseWidgetCounter,                                         
                //                                pagination: '#pager2' + index + showcaseWidgetCounter,                                         
                //                                swipe: { onMouse: true, onTouch: true }                                         
                //                            });                                         
                //                            $.each(widgetDatax.ShowCasePreviewResult, function (i, v) {                                         
                //                                $("#showCaseWidget" + index + showcaseWidgetCounter).trigger("insertItem", getShowCaseOptions(v, true));                                         
                //                            });                                         
                //                            showcaseWidgetCounter++;                                             
                //                    break;                                         

                default:
                    gridster.add_widget('<li class="new"  id="' + index + '"> ' + widgetTitleName + ' - ID: ' + index + '  <a class="icon edit widgetEdit" title="Edit" value="' + index + '"></a><a class="icon delete widgetDelete" title="Delete" value="' + index + '"></a></li>', sizeX, sizeY, col, row);
            }

        });
    };

    return {
        InitGridster: initGridster,
        DeleteWidgetOnPage: deleteWidgetOnPage,
        AddWidgetToPage: addWidgetToPage
    };
})();