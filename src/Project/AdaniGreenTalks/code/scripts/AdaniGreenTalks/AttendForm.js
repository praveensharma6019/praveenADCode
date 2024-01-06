const ErrorAttendMsg = {
    FirstNameNullCheck: "Please enter First Name",
    LastNameNullCheck: "Please enter Last Name",
    EmailNullCheck: "Please enter your email",
    EmailInvalid: "Please enter a valid email address",
    ContactNoNullCheck: "Please enter your Contact No",
    ContactNoInvalid: "Please enter a valid Contact No",
    MsgNullCheck: "Please write about yourself",
}
const StatusAttendCode = {
    FirstNameErrorCode: "405",
    LastNameErrorCode: "406",
    EmailErrorCode: "407",
    ContactNoCode: "408",
    MsgErrorCode: "409",
}
function showErrorAttendMsg(inputId, Msg) {
    $('#' + escapeHTML(inputId)).addClass('has-error');
    var label = $('#' + escapeHTML(inputId)).next('span');
    if ($('#' + escapeHTML(inputId)).next('span').length > 0) {
        $('#' + escapeHTML(inputId)).next("span").remove();
    }
    label = $('#' + escapeHTML(inputId)).after('<span class="custom-field-validation-error"></span>').next("span");
    label.text(escapeHTML(Msg));
    label.show();
}
function hideErrorAttendMsg(inputId) {
    $('#' + escapeHTML(inputId)).removeClass('has-error');
    $('#' + escapeHTML(inputId)).next("span").remove();
}
function IsNullInputFieldAttend(inputId, Msg) {
    let IsNullInput = false;
    if ($("#" + escapeHTML(inputId)).val().trim() == "") {
        IsNullInput = true;
        showErrorAttendMsg(inputId, Msg);
    }
    else {
        hideErrorAttendMsg(inputId);
    }
    return IsNullInput;
}
function AttendUsForm() {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AdaniGreenTalksAttendFormPage' }).then(function (token) {
            $('#googleCaptchaToken').val(token);
            var icount = 0;
            let IsFormError = jQuery(".attend-form form .custom-field-validation-error").length > 0 ? true : false;
            if (!IsFormError) {
                HideAttendError();
                setTimeout(function () {
                    $("#submitloader").css("display", "block");
                    $('#buttontext').css("display", "none");
                }, 100);
                if (IsNullInputFieldAttend("first-name", escapeHTML(ErrorAttendMsg.FirstNameNullCheck))) {
                    icount++;
                }
                if (IsNullInputFieldAttend("last-name", escapeHTML(ErrorAttendMsg.LastNameNullCheck))) {
                    icount++;
                }
                if (IsNullInputFieldAttend("email-id", escapeHTML(ErrorAttendMsg.EmailNullCheck))) {
                    icount++;
                }
                else {
                    var e = IsValidEmailAttend($('#email-id').val());
                    if (!e) {
                        icount++;
                        showErrorAttendMsg("email-id", escapeHTML(ErrorAttendMsg.EmailInvalid))
                    }
                    else {
                        hideErrorAttendMsg("email-id");
                    }
                }
                if (IsNullInputFieldAttend("contact-number", escapeHTML(ErrorAttendMsg.ContactNoNullCheck))) {
                    icount++;
                }
                else {
                    var e = validateAttendPhoneNumber($('#contact-number').val());
                    if (!e) {
                        icount++;
                        showErrorAttendMsg("contact-number", escapeHTML(ErrorAttendMsg.ContactNoInvalid));
                    }
                    else {
                        hideErrorAttendMsg("contact-number");
                    }
                }
                if (IsNullInputFieldAttend("customerQuery", escapeHTML(ErrorAttendMsg.MsgNullCheck))) {
                    icount++;
                }
            }
            if (icount > 0) {
                setTimeout(function () {
                    $("#submitloader").css("display", "none");
                    $('#buttontext').css("display", "block");
                }, 10);
                return false;
            }
            else if (!IsFormError) {
                var obj = {
                    'FirstName': $('#first-name').val().trim(),
                    'LastName': $('#last-name').val().trim(),
                    'Email': $('#email-id').val().trim(),
                    'ContactNumber': $('#contact-number').val().trim(),
                    'CustomerQuery': $('#customerQuery').val().trim(),
                    'FormType': $('#FormType').val(),
                    'FormUrl': $('#FormUrl').val(),
                    'googleCaptchaToken': $('#googleCaptchaToken').val()
                };


                clearvalidationattendmessage();
                $('#AttendFormSubmit_Btn').addClass('disabled');
                $.ajax({
                    url: "/api/AdaniGreenTalks/AdaniGreenTalksAttendFormPage",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    headers: { "__RequestVerificationToken": $(".antiforgerytoken").find("input").val() },
                    data: JSON.stringify(obj),
                    type: "POST",
                    cache: false,
                    success: function (response) {
                        //var response = JSON.parse(data);
                        if (response.errorModel != null && response.errorModel.IsError) {
                            //resetFields();
                            ShowAttendError(escapeHTML(response.errorModel.errorMessage));
                        }
                        else if (response.contactvalidationlist != null) {
                            $.each(response.contactvalidationlist, function (index, object) {
                                if (object.StatusCode == StatusAttendCode.FirstNameErrorCode) {
                                    showErrorAttendMsg("first-name", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusAttendCode.LastNameErrorCode) {
                                    showErrorAttendMsg("last-name", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusAttendCode.EmailErrorCode) {
                                    showErrorAttendMsg("email-id", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusAttendCode.ContactNoCode) {
                                    showErrorAttendMsg("contact-number", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusAttendCode.MsgErrorCode) {
                                    showErrorAttendMsg("customerQuery", escapeHTML(object.FieldErrorMessage));
                                }

                            });

                            setTimeout(function () {
                                $("#submitloader").css("display", "none");
                                $('#buttontext').css("display", "block");
                            }, 10);
                            $('#AttendFormSubmit_Btn').removeClass('disabled');
                        }
                        else {
                            resetAttendFields();
                            ShowAttendModal();
                        }
                    },
                    error: function (response) {
                        console.log(response);
                        resetAttendFields();
                    }
                });
            }
        });
    });
}
$('.CloseThankyou').on('click', function () {
    HideAttendModal();
});
function ShowAttendError(msg) {
    $('#AttendFormSubmit_Btn').removeClass('disabled');
    $("#submitloader").attr("style", "display:none");
    $('#buttontext').attr("style", "display:block");
    $('#divError').text(escapeHTML(msg));
    $('#divError').show();
}


function escapeHtml1(text) {
    'use strict';
    return text.replace(/[\"&<>]/g, function (a) {
        return { '"': '&quot;', '&': '&amp;', '<': '&lt;', '>': '&gt;' }[a];
    });
}

function HideAttendError() {
    $('#divError').text('');
    $('#divError').hide();
}
function ShowAttendModal() {
    HideAttendError();
    $('#AttendFormSubmit_Btn').removeClass('disabled');
    $("#contactLoader").hide();
    $('body').addClass('active-popup');
    $("#submitloader").css("display", "none");
    $('#buttontext').css("display", "block");
}
function HideAttendModal() {
    $('body').removeClass('active-popup');
}
function resetAttendFields() {
    $("#first-name").val('');
    $("#last-name").val('');
    $("#email-id").val('');
    $("#contact-number").val('');
    $("#customerQuery").val('');
    $("#submitloader").css("display", "none");
    $('#buttontext').css("display", "block");
    //grecaptcha.reset();
}
function clearvalidationattendmessage() {
    $('#first-name').removeClass('has-error');
    $('#first-name').next('span').remove();
    $('#last-name').removeClass('has-error');
    $('#last-name').next('span').remove();
    $('#email-id').removeClass('has-error');
    $('#email-id').next('span').remove();
    $('#contact-number').removeClass('has-error');
    $('#contact-number').next('span').remove();
    $('#customerQuery').removeClass('has-error');
    $('#customerQuery').next('span').remove();
    HideAttendError();
}
function validateAttendPhoneNumber(input_str) {
    var re = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;

    return re.test(input_str);
}
function IsValidEmailAttend(email) {
    var spliter = [];
    if (email.toString().indexOf('@') >= 0) {
        spliter = email.toString().split("@");
        if (spliter.length > 2) {
            return false;
        } else {
            if (spliter[0].toString() == "")
                return false;
            if (email.toString().indexOf('.') >= 0) {
                spliter = spliter[1].toString().split('.');
                if (spliter.length > 2)
                    return false;
                else {
                    if (spliter[1].toString() == "")
                        return false;
                    else
                        return true;
                }
            } else
                return false;
        }
    } else
        return false;
}

function validateAttend(key) {
    var keycode = (key.which) ? key.which : key.keyCode;
    var phn = document.getElementById('contact-number');
    if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
        return false;
    } else {
        if (phn.value.length < 10) {
            return true;
        } else {
            return false;
        }
    }
}


$(document).on('keypress', '#first-name,#last-name', function (event) {
    var regex = new RegExp("^[a-zA-Z ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode); if (!regex.test(key)) { event.preventDefault(); return false; }
});
$(document).on('keypress', '#contact-number', function (event) {
    if (!validateAttend(event)) { event.preventDefault(); return false; }
});
$(document).on('keyup', '#customerQuery', function (event) {
    if ($(this).val().trim() != '') {
        hideErrorMsg('customerQuery')
        return true;
    }
});