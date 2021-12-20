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
}
//$(function () {

    var $image = $(".img-container img"),
        $dataX = $("#dataX"),
        $dataY = $("#dataY"),
        $dataHeight = $("#dataHeight"),
        $dataWidth = $("#dataWidth"),
        options = {
            aspectRatio: 100 / 50,
            data: {
                x: 1600,
                y: 640,
                width: 1600,
                height: 640
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
        var dataURLNormal = $image.cropper("getDataURL", {
            width: 1600,
            height: 657
        }, "image/jpeg", 0.8);
        var dataURLThumb = $image.cropper("getDataURL", {
            width: 70,
            height: 82
        }, "image/jpeg", 0.8);

        $(PhotoNormal).text(dataURLNormal);
        $(PhotoThumb).text(dataURLThumb);
        $(dataURLView).html('<img style="width: 533px; height: 213px;" src="' + dataURLNormal + '">');
        $(imgPhotoThumb).html('<img style="width: 533px; height: 213px;" src="' + dataURLNormal + '">');
    });
