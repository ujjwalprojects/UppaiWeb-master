
//$(function () {

    var $image = $(".img-container img"),
        $dataX = $("#dataX"),
        $dataY = $("#dataY"),
        $dataHeight = $("#dataHeight"),
        $dataWidth = $("#dataWidth"),
        options = {
            aspectRatio: 1 / 1,
            data: {
                x: 280,
                y: 280,
                width: 280,
                height: 280
            },
            preview: ".img-preview",
            done: function (data) {
                $dataX.val(data.x);
                $dataY.val(data.y);
                $dataHeight.val(data.height);
                $dataWidth.val(data.width);
            }
        };

    $image.cropper(options).on({
        "build.cropper": function (e) {
            console.log(e.type);
        },
        "built.cropper": function (e) {
            console.log(e.type);
        }
    });

    $(document).on("click", "[data-method]", function () {
        var data = $(this).data();

        if (data.method) {
            $image.cropper(data.method, data.option);
        }
    });

    var $inputImage = $("#inputImage");

    if (window.FileReader) {
        $inputImage.change(function () {
            var fileReader = new FileReader(),
                files = this.files,
                file;

            if (files.length) {
                file = files[0];

                if (/^image\/\w+$/.test(file.type)) {
                    fileReader.readAsDataURL(file);
                    fileReader.onload = function () {
                        $image.cropper("reset", true).cropper("replace", this.result);
                    };
                }
            }
        });
    } else {
        $inputImage.addClass("hide");
    }

    var $setDataX = $("#setDataX"),
        $setDataY = $("#setDataY"),
        $setDataWidth = $("#setDataWidth"),
        $setDataHeight = $("#setDataHeight");


    var $dataURLInto = $("#dataURLInto"),
        $dataURLView = $("#dataURLView");

    $("#getDataURL").click(function () {
        $('#DoneBtn').removeClass('disabled');
        var dataURLThumb = $image.cropper("getDataURL", {
            width: 140,
            height: 140
        }, "image/jpeg", 0.8);
        
        $(profilePhotoThumb).text(dataURLThumb);
        $(dataURLView).html('<img style="width: 100%;" class="rounded-circle" src="' + dataURLThumb + '">');
        $(imgPhotoThumb).html('<img style="width: 140px;" class="rounded-circle img-thumbnail" src="' + dataURLThumb + '">');
    });
//});
    function ShowFileMsg() {
        if ($('#inputImage').val() == "") {
            $("#ImageSpan").text("Upload photo...");
            $('#getDataURL').css("pointer-events", "none");
            $('#getDataURL').css("opacity:", ".3");
        }
        else {
            $("#ImageSpan").text("");
            $('#getDataURL').css("pointer-events", "");
            $('#getDataURL').css("opacity", "1");
        }

        if ($('#dataURLInto').val() == "") {
            $('#ModelDone').css("pointer-events", "none");
            $('#ModelDone').css("opacity:", ".3");
        }
        else {
            $('#ModelDone').css("pointer-events", "");
            $('#ModelDone').css("opacity", "1");
        }
        //var imageData = canvas.toDataURL('image/png');

    }