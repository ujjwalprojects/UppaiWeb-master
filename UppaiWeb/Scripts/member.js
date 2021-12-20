$(function () {
    //**************** JS to show loading progress during ajax call *********************//
    $(document).ajaxStart(function () {
        $("#ajaxLoading").css("display", "block");
        $("#ajaxLoading").css("top", "50%");
        $("#ajaxLoading").css("left", "50%");
        $("#ajaxLoading").css("position", "fixed");
        $("#ajax-backdrop").css("display", "block");
    });
    $(document).ajaxComplete(function () {
        $("#ajaxLoading").css("display", "none");
        $("#ajax-backdrop").css("display", "none");

    });
    highlightMenu();
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-container",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    ShowToast();
    //************************************************************************************//

    //******************************** JS for Grid paging *********************************//
    var getPage = function () {
        var $a = $(this);

        if ($a.attr("href").trim() == undefined || $a.attr("href").trim() == "") {
            return;
        }
        var $form = $('form[data-search="true"]');
        var options = {
            url: $a.attr("href")
            , data: $form.serialize()
            , type: "get"
        }

        $.ajax(options).done(function (data) {
            var $target = $($a.parents("div.ns-grid-pager").attr("data-otf-target"));
            $target.replaceWith(data);
        }).fail(function (xhr, msg, err) {
            toastr.error(msg, "Error");
        });
        return false;
    };
    $("#wrapper").on("click", "a.ns-page-link", getPage);

    var getPageForDDL = function () {
        var TargetURL = $(this).parent().attr("data-otf-actionlink");
        if (TargetURL.indexOf("?") > -1) {
            TargetURL = TargetURL + "&PageSize=" + $('.page-size').val() + "&PageNo=" + $(this).val()
        }
        else {
            TargetURL = TargetURL + "?PageSize=" + $('.page-size').val() + "&PageNo=" + $(this).val()
        }
        //TargetURL = TargetURL + "?PageSize=" + $('.page-size').val()
        var $form = $('form[data-search="true"]');
        var options = {
            url: TargetURL
            , data: $form.serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-otf-target");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        }).fail(function (xhr, msg, err) {
            toastr.error(msg, "Error");
        });
    };
    $("#wrapper").on("change", ".page-number", getPageForDDL);

    var getPageSizeForDDL = function () {
        var TargetURL = $(this).parent().attr("data-otf-actionlink");
        //TargetURL = TargetURL + "?PageSize=" + $(this).val()
        if (TargetURL.indexOf("?") > -1) {
            TargetURL = TargetURL + "&PageSize=" + $('.page-size').val()
        }
        else {
            TargetURL = TargetURL + "?PageSize=" + $('.page-size').val()
        }
        var $form = $('form[data-search="true"]');
        var options = {
            url: TargetURL
            , data: $form.serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-otf-target");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        }).fail(function (xhr, msg, err) {
            toastr.error(msg, "Error");
        });
    };
    $("#wrapper").on("change", ".page-size", getPageSizeForDDL);

    //************************************************************************************//

    //************** Dropdown Changed event for Cascading Dropdown list ******************//

    var getDropDownList = function () {

        var targeturl = $(this).attr("dataotflink")
        var elem = $(this);
        if (elem.val() != '' && elem.val() != null) {
            targeturl = targeturl + "/" + elem.val();
            var target = $(this).attr("dataotftarget");
            var options = {
                url: targeturl
                , data: $("form").serialize()
                , type: "get"
                , dataType: 'json'
            }

            $.ajax(options).done(function (data) {
                $(target).empty();
                $(target).append('<option value="">--- Select ---</option>');
                $.each(data, function (i, item) {
                    $(target).append('<option value="' + item.Value + '">' + item.Text + '</option>');
                    // here we are adding option for employee dropdown
                });
                $("#select2-" + $(target).attr('id') + "-container").replaceWith('<span id="select2-' + $(target).attr('id') + '-container" class="select2-selection__rendered" title="--- Select ---">--- Select ---</span>')
            }).fail(function (xhr, msg, err) {
                toastr.error(msg, "Error");
            });

        }
        else {
            var target = $(elem.attr("dataotftarget"));
            target.empty();
            $("#select2-" + $(target).attr('id') + "-container").replaceWith('<span id="select2-' + $(target).attr('id') + '-container" class="select2-selection__rendered" title="--- Select ---">--- Select ---</span>')
            $(elem.attr("dataotftarget")).append('<option value="">--- Select ---</option>');
        }
        return false;
    };

    $("#wrapper").on("change", ".dropdown-change", getDropDownList);

    //************************************************************************************//

    $("#wrapper").on("click", ".publish", ShowPublishMessageBox);
    $("#wrapper").on("click", ".unpublish", ShowUnPublishMessageBox);
    $("#wrapper").on("click", ".delete", ShowWarningMessageBox);
    $("#wrapper").on("click", ".reset", ShowResetMessageBox);
    $("#wrapper").on("click", ".status", ShowStatusMessageBox);

    var ajaxFormSubmit = function () {
        var $form = $(this);
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $($form.attr("data-otf-target"));
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        }).fail(function (xhr, msg, err) {
            toastr.error(msg, "Error");
        });
        return false;
    };
    $("#wrapper").on("submit", "form[data-modal-ajax='true']", function () {
        $('#actionModal').modal('hide');
        var $form = $(this);
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $($form.attr("data-otf-target"));
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
            ShowToast();
        }).fail(function (xhr, msg, err) {
            toastr.error(msg, "Error");
        });
        return false;
    });
    $("form[data-otf-ajax='true']").submit(ajaxFormSubmit);
});

// ************************* This is to show success/error message ********************************//

// Function to call custom Ajax form submit
function CustomAjaxFormSubmit(sender, url) {
    if (url == "#") { return false; }
    var $form = $('a[href="' + decodeURI(url) + '"]').closest('form');
    if ($form.attr("data-otf-ajax") == 'true') {
        var options = {
            url: decodeURI(url)
            , type: $form.attr("method")
            , data: $form.serialize()
        }
        var target = $($form.attr("data-otf-target"));
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);

            ShowToast();
        }).fail(function (xhr, msg, err) {
            toastr.error(msg, "Error");
        });
        return false;
    }
    else {
        $form.submit();
        return true;
    }
};

//function ShowToast() {
//    alert($('#ErrMsgHiddenField').val());
//    if ($('#ErrMsgHiddenField').val().length > 1) {
//        var msgId = $('#ErrMsgHiddenField').val();
//        if (msgId != null && msgId.toString().trim() != "") {
//            if (msgId.toString().toLowerCase().indexOf('error') != -1) {
//                toastr.error(msgId, "Operation Failed");
//            }
//            else if (msgId.toString().toLowerCase().indexOf('success') != -1) {
//                toastr.success(msgId, 'Success');
//            } else {
//                toastr.info(msgId, 'Information');
//            }
//        }
//    }
//}
function ShowToast() {
    //alert($('#ErrMsgHiddenField').val());
    if ($('#ErrMsgHiddenField').val().length > 1) {
        toastr.options.timeOut = 10000;
        toastr.options.extendedTimeOut = 1000; //1000;
        toastr.options.positionClass = 'toast-bottom-right';
        toastr.options.fadeOut = 250;
        toastr.options.fadeIn = 250;
        var msgId = $('#ErrMsgHiddenField').val();
        if (msgId != null && msgId.toString().trim() != "") {
            if (msgId.toString().toLowerCase().indexOf('error') != -1) {
                toastr.error(msgId, "Operation Failed");
            }
            else if (msgId.toString().toLowerCase().indexOf('success') != -1) {
                toastr.success(msgId, "Success");
            } else {
                toastr.info(msgId, "Information");
            }
        }
    }
}

//// This is to show warning message before delete operation
var ShowWarningMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        var record = $(this).attr('data-record');
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Confirmation');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').html('Are you sure you want to delete the record..?');

        $('#myErroMsgModalYesButton').removeClass('invisible');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}

//// This is to show warning message before Publish operation
var ShowPublishMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        var record = $(this).attr('data-record');
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Confirmation');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').html('Are you sure you want to Publish this record of <b>' + record + '</b>..?');

        $('#myErroMsgModalYesButton').removeClass('invisible');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}

//// This is to show warning message before UnPublish operation
var ShowUnPublishMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        var record = $(this).attr('data-record');
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Confirmation');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').html('Are you sure you want to UnPublish this record of <b>' + record + '</b>..?');

        $('#myErroMsgModalYesButton').removeClass('invisible');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}

//// This is to show reset password message 
var ShowResetMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        var record = $(this).attr('data-record');
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Confirmation');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').html('Are you sure you want to reset account password of <b>' + record + '</b>..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}
//// This is to show status message 
var ShowStatusMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        var record = $(this).attr('data-record');
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Confirmation');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').html('Are you sure you want to change the status of <b>' + record + '</b>..?');

        $('#myErroMsgModalYesButton').removeClass('invisible');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}


// Close message box
function CloseMyModal() {
    $('#myErroMsgModalYesButton').addClass('hidden');
    $('#ErrMsgHiddenField').val("");
    $("#myErroMsgModal").hide();
}
// close message box and procceed for intended action.
function OkMyModal() {
    $("#myErroMsgModal").hide();
    $('#ErrMsgHiddenField').val("");
    // Retrieve the sender infromation from hidden field and pass it to the function
    CustomAjaxFormSubmit($("#eventSender").val().split('|')[1], $("#eventSender").val().split('|')[0]);
}

function highlightMenu() {
    var url = window.location.href.split('?')[0],
        urlRegExp = new RegExp(url.replace(/\/$/, '') + "$");

    url = url.substr(url.indexOf("/", 7))
    var IsActivated = false;
    //alert($("#ActiveURL").data("value").toLowerCase());
    var linkElement = $('.sidebarMenu a[href="' + url.toLowerCase() + '"]');
    if (linkElement.length != 0) {
        linkElement.parents('li').addClass('nav-item').addClass('active');
        //linkElement.addClass('active');
        var IsActivated = true;
    }
    else {
        var linkElement = $('.sidebarMenu a[href="' + $("#ActiveURL").data("value").toLowerCase() + '"]');
        if (linkElement.length != 0) {
            linkElement.parents('li').addClass('nav-item').addClass('active');
            var IsActivated = true;
        }
    }
}
