$(function () {
    //**************** JS to show loading progress during ajax call *********************//
    $(document).bind("ajaxStart.box-body", function () {
        $("#ajaxLoading").css("display", "block");
        $("#ajaxLoading").css("top", "50%");
        $("#ajaxLoading").css("left", "50%");
        $("#ajaxLoading").css("position", "fixed");
        $("#ajax-backdrop").css("display", "block");
    });
    $(document).bind("ajaxStop.box-body", function () {
        $("#ajaxLoading").css("display", "none");
        $("#ajax-backdrop").css("display", "none");
    })

    //$('.box-body').ajaxStart(function () {
    //    $("#ajaxLoading").css("display", "block");
    //    $("#ajaxLoading").css("top", $(window).height() / 1.8);
    //    $("#ajaxLoading").css("left", $(window).width() / 2);
    //    $("#ajaxLoading").css("position", "fixed");
    //    $("#ajax-backdrop").css("display", "block");
    //});
    //$('.box-body').ajaxStop(function () {
    //    $("#ajaxLoading").css("display", "none");
    //    $("#ajax-backdrop").css("display", "none");
    //});
    //************************************************************************************//

    //*************************  Highlight Active Menu  ***********************************//
    jQuery(function () {
        var url = window.location.pathname,
        urlRegExp = new RegExp(url.replace(/\/$/, '') + "$");
        var IsActivated = false;
        $("#sidebar ul li").each(function () {
            $(this).removeClass("active");
            try {
                if ($(this).children().attr("href").toLowerCase() == url.toLowerCase()) {
                    $(this).addClass("active");
                    $(this).closest('.submenu').addClass("active");
                    $(this).closest('.submenu-second-level').addClass("active");
                    $(this).closest('.submenu-third-level').addClass("active");
                    $(this).closest('.nav-second-level').addClass("in");
                    $(this).closest('.nav-third-level').addClass("in");
                    
                    //$(this).closest('.submenu').addClass("open");
                    IsActivated = true;
                }
            } catch (e) {

            }
        })
        if (IsActivated == false) {
            $("#sidebar ul li").each(function () {
                $(this).removeClass("active");
                try {
                    if ($(this).children().attr("href").toLowerCase() == $("#ActiveURL").data("value").toLowerCase()) {
                        $(this).addClass("active");
                        $(this).closest('.submenu').addClass("active");
                        $(this).closest('.nav-second-level').addClass("in");
                        $(this).closest('.nav-third-level').addClass("in");
                        $(this).closest('.submenu-second-level').addClass("active");
                        $(this).closest('.submenu-third-level').addClass("active");
                        //$(this).closest('.submenu').addClass("open");
                        IsActivated = true;
                    }
                } catch (e) {

                }

            })
        }
    });
    //************************************************************************************//

    ShowMessageBox();
    //************************************************************************************//

    //******************************** JS for Grid paging *********************************//
    var getPage = function () {
        var $a = $(this);

        if ($a.attr("href").trim() == undefined || $a.attr("href").trim() == "") {
            return;
        }
        var options = {
            url: $a.attr("href")
            , data: $("form").serialize()
            , type: "get"
        }

        $.ajax(options).done(function (data) {
            var $target = $($a.parents("div.ns-grid-pager").attr("data-otf-target"));
            //$target.replaceWith(data);
            $target.empty().append(data);
        });
        return false;
    };
    $(".container-fluid").on("click", "a.ns-page-link", getPage);

    var getPageForDDL = function () {
        var TargetURL = $(this).parent().attr("data-otf-actionlink");
        if (TargetURL.indexOf("?") > -1) {
            TargetURL = TargetURL + "&PageSize=" + $('.page-size').val() + "&PageNo=" + $(this).val()
        }
        else {
            TargetURL = TargetURL + "?PageSize=" + $('.page-size').val() + "&PageNo=" + $(this).val()
        }
        //TargetURL = TargetURL + "?PageSize=" + $('.page-size').val()
        var options = {
            url: TargetURL
            , data: $("form").serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-otf-target");
        $.ajax(options).done(function (data) {
            //$(target).replaceWith(data);
            $(target).empty().append(data);
        });
    };
    $(".container-fluid").on("change", ".page-number", getPageForDDL);

    var getPageSizeForDDL = function () {
        var TargetURL = $(this).parent().attr("data-otf-actionlink");
        //TargetURL = TargetURL + "?PageSize=" + $(this).val()
        if (TargetURL.indexOf("?") > -1) {
            TargetURL = TargetURL + "&PageSize=" + $('.page-size').val()
        }
        else {
            TargetURL = TargetURL + "?PageSize=" + $('.page-size').val()
        }
        var options = {
            url: TargetURL
            , data: $("form").serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-otf-target");
        $.ajax(options).done(function (data) {
            //$(target).replaceWith(data);
            $(target).empty().append(data);
        });
    };
    $(".container-fluid").on("change", ".page-size", getPageSizeForDDL);

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
    $(".container-fluid").on("change", ".dropdown-change", getDropDownList);

    //************************************************************************************//

    //************** Dropdown Changed event for Cascading Html ******************//

    var getPartial = function () {

        var targeturl = $(this).attr("dataotflink")
        targeturl = targeturl + "&EmpCode=" + $(this).val();
        var target = $(this).attr("dataotftarget");
        var options = {
            url: targeturl
            , data: $("form").serialize()
            , type: "get"
        }
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
            ShowMessageBox();
        });
        return false;
    };
    $(".container-fluid").on("change", ".partial-change", getPartial);

    //************************************************************************************//

    $(".container-fluid").on("click", ".delete", ShowWarningMessageBox);
    $(".container-fluid").on("click", ".approve", ShowApproveMessageBox);
    $(".container-fluid").on("click", ".reject", ShowRejectMessageBox);
    $(".container-fluid").on("click", ".revertreject", ShowRevRejectMessageBox);
    $(".container-fluid").on("click", ".revertapprove", ShowRevApproveMessageBox);
    $(".container-fluid").on("click", ".publish", ShowPublishMessageBox);
    //$(".container-fluid").on("click", "#btnPublish", ShowPublishMessageBox);
    $(".container-fluid").on("click", ".unallocate", ShowWarningUnallocateMessageBox);
    $(".container-fluid").on("click", ".customdelete", CustomDeleteMessageBox);
    
    var ajaxFormSubmit = function () {
        var $form = $(this);
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $($form.attr("data-otf-target"));
        $.ajax(options).done(function (data) {
            $(target).empty().append(data);
        });
        return false;
    };
    $("form[data-otf-ajax='true']").submit(ajaxFormSubmit);

});

// ************************* This is to show success/error message ********************************//

// Function to call custom Ajax form submit
function CustomAjaxFormSubmit(sender, url) {
    if (url == "#")
    { return false; }
    if (url == "delContentForm") {
        var $form = $('#delContentForm');
        $form.submit();
        return true;
    }
    var $form = $('a[href="' + decodeURI(url) + '"]').closest('form')
    if ($form.attr("data-otf-ajax") == 'true') {
        var options = {
            url: decodeURI(url)
            , type: $form.attr("method")
            , data: $form.serialize()
        }
        var target = $($form.attr("data-otf-target"));
        $.ajax(options).done(function (data) {
            if (data.success) {
                $(target).load(data.url, function () { ShowMessageBox(); });
            }
            else {
                $(target).empty().append(data);
                ShowMessageBox();
            }
        });
        return false;
    }
    else {
        $form.submit();
        return true;
    }
};
function ShowMessageBox() {
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
//function ShowMessageBox() {
//    if ($('#ErrMsgHiddenField').val().length > 1) {
//        var msgId = $('#ErrMsgHiddenField').val().substring(0, 4);
//        var errMsg = $('#ErrMsgHiddenField').val().toString();
//    }
//    else {
//        var msgId = $('#ErrMsgHiddenField').val();
//    }
//    if (msgId != null && msgId.toString().trim() != "") {

//        if (msgId == "0") {
//            $('#myErroModalLabel').text("Success");
//            $('#Msg').text("Operation success...");
//            $('#myErroMsgModalNoButton').val("Done");
//        }
//        else if (msgId == "7") {
//            $('#myErroModalLabel').text('Success');
//            $('#Msg').text("A confirmation email has been sent to respective user.");
//            $('#myErroMsgModalNoButton').val("Done");
//        }

//        else if (msgId == "9") {
//            $('#myErroModalLabel').text('Success');
//            $('#Msg').text("Ticket posted successfully...");
//            $('#myErroMsgModalNoButton').val("Close");
//        }

//        else if (msgId == "3") {
//            $('#myErroModalLabel').text('Information');
//            $('#Msg').text('Session expired, redirecting to login page...');
//        }
//        else if (msgId == "4") {
//            $('#myErroModalLabel').text('Error');
//            $('#Msg').text('An error occured while processing your request, please try again...');
//        }
//        else {
//            $('#myErroMsgModalNoButton').val("Close");
//            $('#myErroModalLabel').text('Information');
//            $('#Msg').text(errMsg);
//        }

//        $('#myErroMsgModalYesButton').addClass('hidden');
//        $('#myErroMsgModal').show();
//    }
//}

//// This is to show warning message before delete operation
var ShowWarningMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Are you sure you want to delete the record..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModalNoButton').val('Cancel');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}
var ShowApproveMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Do you want to approve this request..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModalNoButton').val('Cancel');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}
var ShowRejectMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Are you sure you want to reject this request..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModalNoButton').val('Cancel');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}
var ShowRevRejectMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Are you sure you want to revert reject this request..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModalNoButton').val('Cancel');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}
var ShowRevApproveMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Are you sure you want to revert approve this request..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModalNoButton').val('Cancel');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}
var ShowPublishMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Are you sure you want to Publish this record..?');
        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModalNoButton').val('Cancel');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}



var CustomDeleteMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        var name = $(this).attr("name");
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        $('#myErroMsgModalNoButton').val("Cancel");
        //$('#Msg').text('Are you sure you want to delete the record for '+id+'...?');
        $('#Msg').replaceWith('<h4 id="Msg"><div><span style="font-size:20px;">Are you sure you want to delete <span class="text-primary">' + name + '</span>...?</span></div></h4>');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}



var ShowWarningUnallocateMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        $('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Are you sure you want to UnAllocate this Employee from the task..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
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

//**************************** Model Ajax *********************************
$(function () {
    $.ajaxSetup({ cache: false });
    $(".container-fluid").on("click", "a[data-modal]", function (e) {
        // hide dropdown if any (this is used wehen invoking modal from link in bootstrap dropdown )
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#AddEditModalContent').load(this.href, function () {
            $('#AddEditModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });

});
$(function () {
    $.ajaxSetup({ cache: false });

    $(".container-fluid").on("click", "button[data-modal]", function (e) {
        // hide dropdown if any (this is used wehen invoking modal from link in bootstrap dropdown )
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#AddEditModalContent').load(this.data_href, function () {
            $('#AddEditModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});


function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#AddEditModal').modal('hide');
                    $('#replacetarget').load(result.url); //  Load data from the server and place the returned HTML into the matched element
                } else {
                    $('#AddEditModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });
        return false;
    });
}

function loadDownloadDialog(e) {
    e.preventDefault();
    var options = {
        url: '/admin/home/getdownloadlogdialog',
        type: 'get',
    };
    $.ajax(options).success(function (data) {
        $('#actionContent').empty.append(data);
        $('#actionModal').modal('show');
    }).error(function () {
        alert('Error');
    });
    //$.ajax(options).done(function (data) {
    //    $('#actionContent').empty.append(data);
    //    $('#actionModal').modal('show');

    //    //$('.date').datetimepicker({
    //    //    format: "dd M yyyy",
    //    //    weekStart: 1,
    //    //    todayBtn: 1,
    //    //    autoclose: 1,
    //    //    todayHighlight: 1,
    //    //    startView: 2,
    //    //    minView: 2,
    //    //    forceParse: 0
    //    //});
    //}).error(function () {
    //    alert('Error');
    //});
}


