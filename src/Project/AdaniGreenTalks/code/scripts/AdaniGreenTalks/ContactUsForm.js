const ErrorMsg = {
    FirstNameNullCheck: "Please enter First Name",
    LastNameNullCheck: "Please enter Last Name",
    EmailNullCheck: "Please enter your email",
    EmailInvalid: "Please enter a valid email address",
    ContactNoNullCheck: "Please enter your Contact No",
    ContactNoInvalid: "Please enter a valid Contact No",
    MsgNullCheck: "Please enter your message",
}
const StatusCode = {
    FirstNameErrorCode: "405",
    LastNameErrorCode: "406",
    EmailErrorCode: "407",
    ContactNoCode: "408",
    MsgErrorCode: "409",
}
function showErrorMsg(inputId, Msg) {
    $('#' + escapeHTML(inputId)).addClass('has-error');
    var label = $('#' + escapeHTML(inputId)).next('span');
    if ($('#' + escapeHTML(inputId)).next('span').length > 0) {
        $('#' + escapeHTML(inputId)).next("span").remove();
    }
    label = $('#' + escapeHTML(inputId)).after('<span class="custom-field-validation-error"></span>').next("span");
    label.text(escapeHTML(Msg));
    label.show();
}
function hideErrorMsg(inputId) {
    $('#' + escapeHTML(inputId)).removeClass('has-error');
    $('#' + escapeHTML(inputId)).next("span").remove();
}
function IsNullInputField(inputId, Msg) {
    let IsNullInput = false;
    if ($("#" + escapeHTML(inputId)).val().trim() == "") {
        IsNullInput = true;
        showErrorMsg(inputId, Msg);
    }
    else {
        hideErrorMsg(inputId);
    }
    return IsNullInput;
}
function ContactUsForm() {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AdaniGreenTalksContactFormPage' }).then(function (token) {
            $('#googleCaptchaToken').val(token);

            var icount = 0;
            let IsFormError = jQuery(".contact-form form .custom-field-validation-error").length > 0 ? true : false;
            if (!IsFormError) {
                HideError();

                if (IsNullInputField("first-name", escapeHTML(ErrorMsg.FirstNameNullCheck))) {
                    icount++;
                }
                if (IsNullInputField("last-name", escapeHTML(ErrorMsg.LastNameNullCheck))) {
                    icount++;
                }
                if (IsNullInputField("email-id", escapeHTML(ErrorMsg.EmailNullCheck))) {
                    icount++;
                }
                else {
                    var e = IsValidEmail($('#email-id').val());
                    if (!e) {
                        icount++;
                        showErrorMsg("email-id", escapeHTML(ErrorMsg.EmailInvalid))
                    }
                    else {
                        hideErrorMsg("email-id");
                    }
                }
                if (IsNullInputField("contact-number", escapeHTML(ErrorMsg.ContactNoNullCheck))) {
                    icount++;
                }
                else {
                    var e = validatePhoneNumber($('#contact-number').val());
                    if (!e) {
                        icount++;
                        showErrorMsg("contact-number", escapeHTML(ErrorMsg.ContactNoInvalid));
                    }
                    else {
                        hideErrorMsg("contact-number");
                    }
                }
                if (IsNullInputField("customerQuery", escapeHTML(ErrorMsg.MsgNullCheck))) {
                    icount++;
                }
            }
            if (icount > 0) {
                return false;
            }
            else if (!IsFormError) {
                var obj = {
                    'FirstName': $('#first-name').val().trim(),
                    'LastName': $('#last-name').val().trim(),
                    'Email': $('#email-id').val().trim(),
                    'ContactNumber': $('#contact-number').val().trim(),
                    'CustomerQuery': escapeHTML($('#customerQuery').val().trim()),
                    'FormType': $('#FormType').val(),
                    'FormUrl': $('#FormUrl').val(),
                    'googleCaptchaToken': $('#googleCaptchaToken').val()
                };

                $("#submitloader").attr("style", "display:block");
                $('#buttontext').css("display", "none");
                clearvalidationmessage();
                $('#contactFormSubmit_Btn').addClass('disabled');
                $.ajax({
                    url: "/api/AdaniGreenTalks/AdaniGreenTalksContactFormPage",
                    contentType: "application/json; charset=utf-8",
                    headers: { "__RequestVerificationToken": $(".antiforgerytoken").find("input").val() },
                    data: JSON.stringify(obj),
                    type: "POST",
                    cache: false,
                    success: function (response) {
                        //var response = JSON.parse(data);
                        if (response.errorModel != null && response.errorModel.IsError) {
                            //resetFields();

                            ShowError(escapeHTML(response.errorModel.errorMessage));
                        }
                        else if (response.contactvalidationlist != null) {
                            $.each(response.contactvalidationlist, function (index, object) {
                                if (object.StatusCode == StatusCode.FirstNameErrorCode) {
                                    showErrorMsg("first-name", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusCode.LastNameErrorCode) {
                                    showErrorMsg("last-name", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusCode.EmailErrorCode) {
                                    showErrorMsg("email-id", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusCode.ContactNoCode) {
                                    showErrorMsg("contact-number", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusCode.MsgErrorCode) {
                                    showErrorMsg("customerQuery", escapeHTML(object.FieldErrorMessage));
                                }

                            });

                            $("#submitloader").attr("style", "display:none");
                            $('#buttontext').attr("style", "display:block");
                            $('#contactFormSubmit_Btn').removeClass('disabled');
                        }
                        else {

                            resetFields();
                            ShowModal();
                        }
                    },
                    error: function (response) {
                        console.log(response);
                        resetFields();
                    }
                });
            }
        });
    });
}
$('.CloseThankyou').on('click', function () {
    HideModal();
});
function ShowError(msg) {
    $('#contactFormSubmit_Btn').removeClass('disabled');
    $("#contactLoader").hide();
    $("#submitloader").attr("style", "display:none");
    $('#buttontext').attr("style", "display:block");
    $('#divError').text(escapeHTML(msg));
    $('#divError').show();
}
function HideError() {
    $('#divError').text('');
    $('#divError').hide();
}
function ShowModal() {
    HideError();
    $('#contactFormSubmit_Btn').removeClass('disabled');
    $("#contactLoader").hide();
    $("#submitloader").attr("style", "display:none");
    $('#buttontext').attr("style", "display:block");
    $('.contact-section').addClass('active-popup');
}
function HideModal() {
    $('.contact-section').removeClass('active-popup');
}
function resetFields() {
    $("#first-name").val('');
    $("#last-name").val('');
    $("#email-id").val('');
    $("#contact-number").val('');
    $("#customerQuery").val('');
    $("#submitloader").attr("style", "display:none");
    $('#buttontext').attr("style", "display:block");
    //grecaptcha.reset();
}
function clearvalidationmessage() {
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
    HideError();
}
function validatePhoneNumber(input_str) {
    var re = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;

    return re.test(input_str);
}
function IsValidEmail(email) {
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

function validate(key) {
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
    if (!validate(event)) { event.preventDefault(); return false; }
});
$(document).on('keyup', '#customerQuery', function (event) {
    if ($(this).val().trim() != '') {
        hideErrorMsg('customerQuery')
        return true;
    }
});