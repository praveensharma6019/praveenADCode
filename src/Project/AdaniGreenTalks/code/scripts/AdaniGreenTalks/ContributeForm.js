const ErrorContributeMsg = {
    FirstNameNullCheck: "Please enter First Name",
    LastNameNullCheck: "Please enter Last Name",
    EmailNullCheck: "Please enter your email",
    EmailInvalid: "Please enter a valid email address",
    ContactNoNullCheck: "Please enter your Contact No",
    ContactNoInvalid: "Please enter a valid Contact No",
    CityNoValid: "Please enter your city",
    GoalNoValid: "Please enter select Goal",
    fellowNoValid: "Please enter select fellow",
}
const StatusContributeCode = {
    FirstNameErrorCode: "405",
    LastNameErrorCode: "406",
    EmailErrorCode: "407",
    ContactNoCode: "408",
    CityCode: "410",
    GoalCode: "411",
    FellowCode: "412",
}
function showErrorContributeMsg(inputId, Msg) {
    $('#' + escapeHTML(inputId)).addClass('has-error');
    var label = $('#' + escapeHTML(inputId)).next('span');
    if ($('#' + escapeHTML(inputId)).next('span').length > 0) {
        $('#' + escapeHTML(inputId)).next("span").remove();
    }
    label = $('#' + escapeHTML(inputId)).after('<span class="custom-field-validation-error"></span>').next("span");
    label.text(escapeHTML(Msg));
    label.show();
}
function hideErrorContributeMsg(inputId) {
    $('#' + escapeHTML(inputId)).removeClass('has-error');
    $('#' + escapeHTML(inputId)).next("span").remove();
}
function IsNullInputFieldContribute(inputId, Msg) {
    let IsNullInput = false;
    if ($("#" + escapeHTML(inputId)).val().trim() == "") {
        IsNullInput = true;
        showErrorContributeMsg(inputId, Msg);
    }
    else {
        hideErrorContributeMsg(inputId);
    }
    return IsNullInput;
}
function IsNullSelectFieldContribute(inputId, Msg) {
    let IsNullInput = false;
    if ($("#" + escapeHTML(inputId)).val() == -1) {
        IsNullInput = true;
        showErrorContributeMsg(inputId, Msg);
    }
    else {
        hideErrorContributeMsg(inputId);
    }
    return IsNullInput;
}
function ContributeUsForm() {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AdaniGreenTalksContributeFormPage' }).then(function (token) {
            $('#googleCaptchaToken').val(token);


            var icount = 0;
            let IsFormError = jQuery(".attend-form form .custom-field-validation-error").length > 0 ? true : false;
            if (!IsFormError) {
                if (IsNullInputFieldContribute("first-name", escapeHTML(ErrorContributeMsg.FirstNameNullCheck))) {
                    icount++;
                }
                if (IsNullInputFieldContribute("last-name", escapeHTML(ErrorContributeMsg.LastNameNullCheck))) {
                    icount++;
                }
                if (IsNullInputFieldContribute("email-id", escapeHTML(ErrorContributeMsg.EmailNullCheck))) {
                    icount++;
                }
                else {
                    var e = IsValidEmailContribute($('#email-id').val());
                    if (!e) {
                        icount++;
                        showErrorContributeMsg("email-id", escapeHTML(ErrorContributeMsg.EmailInvalid))
                    }
                    else {
                        hideErrorContributeMsg("email-id");
                    }
                }
                if (IsNullInputFieldContribute("contact-number", escapeHTML(ErrorContributeMsg.ContactNoNullCheck))) {
                    icount++;
                }
                else {
                    var e = validateContributePhoneNumber($('#contact-number').val());
                    if (!e) {
                        icount++;
                        showErrorContributeMsg("contact-number", escapeHTML(ErrorContributeMsg.ContactNoInvalid));
                    }
                    else {
                        hideErrorContributeMsg("contact-number");
                    }
                }
                if (IsNullInputFieldContribute("city-name", escapeHTML(ErrorContributeMsg.CityNoValid))) {
                    showErrorContributeMsg("city-number", escapeHTML(ErrorContributeMsg.CityNoValid));
                    icount++;
                }
                if (IsNullSelectFieldContribute("select-Goal", escapeHTML(ErrorContributeMsg.GoalCode))) {
                    showErrorContributeMsg("select-Goal", escapeHTML(ErrorContributeMsg.GoalNoValid));
                    icount++;
                }
                if (IsNullSelectFieldContribute("select-Fellow", escapeHTML(ErrorContributeMsg.FellowCode))) {
                    showErrorContributeMsg("select-Fellow", escapeHTML(ErrorContributeMsg.fellowNoValid));
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
                    'City': $('#city-name').val().trim(),
                    'Goal': $("#select-Goal option:selected").text().trim(),
                    'FellowName': $("#select-Fellow option:selected").text().trim(),
                    'FormType': $('#FormType').val(),
                    'FormUrl': $('#FormUrl').val(),
                    'googleCaptchaToken': $('#googleCaptchaToken').val()
                };

                $("#submitloader").attr("style", "display:block");
                $('#buttontext').css("display", "none");
                clearvalidationContributemessage();
                $('#ContributeFormSubmit_Btn').addClass('disabled');
                $.ajax({
                    url: "/api/AdaniGreenTalks/AdaniGreenTalksContributeFormPage",
                    contentType: "application/json; charset=utf-8",
                    headers: { "__RequestVerificationToken": $(".antiforgerytoken").find("input").val() },
                    data: JSON.stringify(obj),
                    type: "POST",
                    cache: false,
                    success: function (response) {
                        //var response = JSON.parse(data);
                        if (response.errorModel != null && response.errorModel.IsError) {
                            //resetFields();
                            ShowContributeError(response.errorModel.errorMessage);
                        }
                        else if (response.contactvalidationlist != null) {
                            $.each(response.contactvalidationlist, function (index, object) {
                                if (object.StatusCode == StatusContributeCode.FirstNameErrorCode) {
                                    showErrorContributeMsg("first-name", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusContributeCode.LastNameErrorCode) {
                                    showErrorContributeMsg("last-name", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusContributeCode.EmailErrorCode) {
                                    showErrorContributeMsg("email-id", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusContributeCode.ContactNoCode) {
                                    showErrorContributeMsg("contact-number", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusContributeCode.CityCode) {
                                    showErrorContributeMsg("city-name", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusContributeCode.GoalCode) {
                                    showErrorContributeMsg("select-Goal", escapeHTML(object.FieldErrorMessage));
                                }
                                else if (object.StatusCode == StatusContributeCode.FellowCode) {
                                    showErrorContributeMsg("select-Fellow", escapeHTML(object.FieldErrorMessage));
                                }

                            });

                            $("#submitloader").attr("style", "display:none");
                            $('#buttontext').attr("style", "display:block");
                            $('#ContributeFormSubmit_Btn').removeClass('disabled');
                        }
                        else {
                            resetContributeFields();
                            ShowContributeModal();
                        }
                    },
                    error: function (response) {
                        console.log(response);
                        resetContributeFields();
                    }
                });
            }
        });
    });
}
$('.CloseThankyou').on('click', function () {
    HideContributeModal();
});
function ShowContributeError(msg) {
    $('#ContributeFormSubmit_Btn').removeClass('disabled');
    $("#contactLoader").hide();
    $("#submitloader").attr("style", "display:none");
    $('#buttontext').attr("style", "display:block");
    $('#divError').text(escapeHTML(msg));
    $('#divError').show();
}
function HideContributeError() {
    $('#divError').text('');
    $('#divError').hide();
}
function ShowContributeModal() {
    HideContributeError();
    $('#ContributeFormSubmit_Btn').removeClass('disabled');
    $("#contactLoader").hide();
    $("#submitloader").attr("style", "display:none");
    $('#buttontext').attr("style", "display:block");
    $('body').addClass('active-popup');
}
function HideContributeModal() {
    $('body').removeClass('active-popup');
}
function resetContributeFields() {
    $("#first-name").val('');
    $("#last-name").val('');
    $("#email-id").val('');
    $("#contact-number").val('');
    $("#city-name").val('');
    $('#select-Goal').val('-1');
}
function clearvalidationContributemessage() {
    $('#first-name').removeClass('has-error');
    $('#first-name').next('span').remove();
    $('#last-name').removeClass('has-error');
    $('#last-name').next('span').remove();
    $('#email-id').removeClass('has-error');
    $('#email-id').next('span').remove();
    $('#contact-number').removeClass('has-error');
    $('#contact-number').next('span').remove();
    $('#city-name').removeClass('has-error');
    $('#city-name').next('span').remove();
    HideContributeError();
}
function validateContributePhoneNumber(input_str) {
    var re = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;

    return re.test(input_str);
}
function IsValidEmailContribute(email) {
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

function validateContribute(key) {
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
    if (!validateContribute(event)) { event.preventDefault(); return false; }
});