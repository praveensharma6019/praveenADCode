$(document).ready(function () {
    $("#loader-wrapper").hide();
});

//$("#frmPanUpdate").submit(function (event) {
//    //event.preventDefault();
//    $("#loader-wrapper").show();
//    //if (checkFormValidateOrNot()) {
//    //    $("#loader-wrapper").show();
//    //}
//    //if ($('#frmPanUpdate').valid()) {
//    //    $("#loader-wrapper").show();
//    //}
//});


function login(componentid) {
    var logincontrol = jQuery("#" + componentid);
    var usernameField = logincontrol.find("#popupLoginEmail");
    var passwordField = logincontrol.find("#popupLoginPassword");
    jQuery.ajax(
        {
            url: "/api/Accounts/_Login",
            method: "POST",
            data: {
                email: usernameField.val(),
                password: passwordField.val()
            },
            success: function (data) {
                if (data.RedirectUrl != null && data.RedirectUrl != undefined) {
                    window.location.assign(data.RedirectUrl);
                } else {
                    var body = logincontrol.find(".login-body");
                    var parent = body.parent();
                    body.remove();
                    parent.html(data);
                }
            }
        });
}

function ResetElement(formId) {
    var FormIdWithPrefix = jQuery("#" + formId);
    var resetInputField = FormIdWithPrefix.find(".reset-control");
    resetInputField.val("");
}

function ResetPage() {
    window.location = window.location.href;
}

$(document).ready(function () {
    var listItems = $("#accordion li");
    listItems.each(function (idx, li) {
        var page = $(li);
        if (window.location.href.indexOf(page.find("a").attr('href')) > -1) {
            $(page).addClass("active");
            $(page).parent().show();
            $(page).parent().parent().addClass("active");
            $(page).parent().parent().find("a[aria-expanded]").attr('aria-expanded', 'true');
        }

    });
});



//$(document).on('click', '#accordion>li>a', function () {
//    var IsAreaExpanded = $(this).attr('aria-expanded');
//    if (IsAreaExpanded == 'true')
//        $(this).attr('aria-expanded', 'true');
//    else
//        $(this).attr('aria-expanded', 'false');

//    var current_li = $(this).parent('li')


//    var listItems = $("#accordion li").not(current_li);
//    listItems.each(function (li) {
//        var page = $(li);
//        $(page).parent().hide();
//        $(page).parent().parent().find("a[aria-expanded]").attr('aria-expanded', 'false');
//    });

//});

//** Compose Form Submition**//


//$(document).on('click', '#btnSubmit', function (event) {
//    event.preventDefault();
//    composeFormSubmit(event);
//});


function Validate(obj) {
    var ext = $(obj).val().split('.').pop().toLowerCase();
    // if($.inArray(ext, ['jpg','jpeg','png','pdf','doc','docx','xls','xlsx]) == -1) {
    // alert('invalid extension!');
    // }
    if ($.inArray(ext, ['jpg', 'jpeg', 'png', 'pdf', 'doc', 'docx', 'xls', 'xlsx']) == -1) {
        alert('File extension not supported!');
        $(obj).val("");
        return false;
    }
}

