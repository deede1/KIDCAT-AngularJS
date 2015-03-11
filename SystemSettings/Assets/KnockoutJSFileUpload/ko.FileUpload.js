ko.bindingHandlers.fileUpload = {
    init: function (element, valueAccessor) {
        //$(element).after('<div class="progress"><div class="bar"></div><div class="percent">0%</div></div><div class="progressError"></div>');
        $(element).before("<div style='width:208px; float:left'><input type='text' id='filePathTxt' size='30' readonly > </div>");
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var options = ko.utils.unwrapObservable(valueAccessor()),
            property = ko.utils.unwrapObservable(options.property),
            url = ko.utils.unwrapObservable(options.url),
            imgDiv = ko.utils.unwrapObservable(options.imgDiv);

        if (property && url) {

            $(element).change(function () {
                if (element.files.length) {
                    var $this = $(this),
                        fileName = $this.val();
                    $("#filePathTxt").val(fileName);
                    // this uses jquery.form.js plugin
                    $(element.form).ajaxSubmit({
                        url: url,
                        type: "POST",
                        dataType: "text",
                        headers: { "Content-Disposition": "attachment; filename=" + fileName },
                        beforeSubmit: function () {
                            //  $(".progress").show();
                            //  $(".progressError").hide();
                            //  $(".bar").width("0%");
                            //  $(".percent").html("0%");
                        },
                        uploadProgress: function (event, position, total, percentComplete) {
                            // var percentVal = percentComplete + "%";
                            // $(".bar").width(percentVal);
                            // $(".percent").html(percentVal);

                        },
                        success: function (data) {
                            //alert(data);
                            var myData = JSON.parse(data);
                            $("#" + imgDiv).empty();
                            var img = document.createElement("IMG");
                            img.src = myData.ImagePath;
                            img.width = "300px";
                            var hidden = document.createElement("input");
                            hidden.id = "UploadedImagePath";
                            hidden.type = "hidden";
                            hidden.value = myData.ImagePath;
                            document.getElementById(imgDiv).appendChild(img);
                            document.getElementById(imgDiv).appendChild(hidden);

                            var imageX = document.getElementById('lastPreviewImage');
                            imageX.parentNode.removeChild(imageX);

                            //$(".progress").hide();
                            //$(".progressError").hide();
                            // set viewModel property to filename
                            // $("label[for='upload']").text(data);

                            //console.log(data);
                            //bindingContext.$data[property](data);
                        },
                        error: function (jqXhr, errorThrown) {
                            //$(".progress").hide();
                            //$("div.progressError").html(jqXhr.responseText);
                        }
                    });
                }
            });
        }
    }
}