const ErrorSpeakMsg = {
    FirstNameNullCheck: "Please enter First Name",
    LastNameNullCheck: "Please enter Last Name",
    EmailNullCheck: "Please enter your email",
    EmailInvalid: "Please enter a valid email address",
    MoblieNoNullCheck: "Please enter moblie no",
    MoblieNoInvalid: "Please enter a valid moblie no",
    NomineeMyselfNoNullCheck: "Please check Myself nominee",
    FirstNameNomineeNullCheck: "Please enter Nominee First Name",
    LastNameNomineeNullCheck: "Please enter Nominee Last Name",
    EmailNomineeNullCheck: "Please enter your Nominee email",
    EmailNomineeInvalid: "Please enter a valid Nominee email address",
    ContactNoNullCheck: "Please enter contact no",
    ContactNoInvalid: "Please enter a valid contact no",
    CityNoValid: "Please enter your city",
    CountryNoValid: "Please enter enter Country",
    GenderValid: "Please enter select gender",
    UploadPhotoNoNullCheck: "Please Upload Photo",
    UploadPhotoValidCheck: "Please upload the valid photo (png,jpg,gif,jpeg)",
    UploadoriginalconceptNoNullCheck: "Please Upload original concept",
    UploadBiographyNoNullCheck: "Please Upload Biography",
    UploadoriginalconceptNoValid: "Please Upload (.pdf) file for original concept",
    UploadBiographyNoValid: "Please Upload  (.pdf) file for  Biography",
    keytakeawayNoValid: "Please enter enter key take away",
    audvidNoValid: "Please enter enter audio video",
    articleNoValid: "Please enter article",
    GoalNoValid: "Please select Goal",
    checkconsentNoValid: "Please check consent"
}
const StatusSpeakCode = {
    FirstNameErrorCode: "405",
    LastNameErrorCode: "406",
    EmailErrorCode: "407",
    MoblieNoCode: "413",
    Goal: "411",
    FirstNameNomineeCode: "414",
    LastNameNomineeCode: "415",
    EmailNomineeCode: "416",
    ContactNoNomineeCode: "408",
    CityCode: "410",
    CountryCode: "417",
    GenderCode: "418",
    UploadPhotoCodeCode: "419",
    UploadoriginalconceptCode: "420",
    UploadBiographyCode: "421",
    TakeawayCode: "422",
    linkforarticleCode: "423",
    linkaudioorvideo: "424",
}

function showErrorSpeakFileMsg(inputId, Msg, Hint) {
    $('#' + escapeHTML(inputId)).parent().removeClass('error');
    $('#' + escapeHTML(inputId)).parent().addClass('error');

    if (Hint == "hint") {
        if ($('#' + escapeHTML(inputId)).parent().next('div').find('span').length > 0) {
            $('#' + escapeHTML(inputId)).parent().next('div').find('span').eq(0).remove();
        }
        label = $('#upl-bio').parent().next('div').prepend('<span class="custom-field-validation-error"></span>').next("span");
        label.text(escapeHTML(Msg));
        label.show();
    }
    else if (Hint == "directshow") {

        if ($('#' + escapeHTML(inputId)).parent().parent().next('span').length > 0) {
            $('#' + escapeHTML(inputId)).parent().parent().next("span").eq(0).remove();
        }
        label = $('#' + escapeHTML(inputId)).parent().parent().append('<span class="custom-field-validation-error filevalidation">' + escapeHTML(Msg) + '</span>').next("span");
        label.show();
    }
    else {
        if ($('#' + escapeHTML(inputId)).parent().next('span').length > 0) {
            $('#' + escapeHTML(inputId)).parent().next("span").eq(0).remove();
        }
        label = $('#' + escapeHTML(inputId)).parent().after('<span class="custom-field-validation-error"></span>').next("span");
        label.text(escapeHTML(Msg));
        label.show();
    }

}
function hideErrorSpeakFileMsg(inputId, Hint) {
    $('#' + escapeHTML(inputId)).parent().removeClass('error');

    if (Hint == "hint") {
        if ($('#' + escapeHTML(inputId)).parent().next('div').find('span').length > 0) {
            $('#' + escapeHTML(inputId)).parent().next('div').find('span').eq(0).remove();
        }
    }
    else if (Hint == "directshow") {
        if ($('#' + escapeHTML(inputId)).parent().parent().find('.filevalidation').length > 0) {
            $('#' + escapeHTML(inputId)).parent().parent().find('.filevalidation').eq(0).remove();
        }
    }
    else {
        if ($('#' + escapeHTML(inputId)).parent().next('span').length > 0) {
            $('#' + escapeHTML(inputId)).parent().next("span").eq(0).remove();
        }
    }
}


function showErrorSpeakMsg(inputId, Msg) {
    $('#' + escapeHTML(inputId)).addClass('has-error');
    var label = $('#' + escapeHTML(inputId)).next('span');
    if ($('#' + escapeHTML(inputId)).next('span').length > 0) {
        $('#' + escapeHTML(inputId)).next("span").remove();
    }
    label = $('#' + escapeHTML(inputId)).after('<span class="custom-field-validation-error"></span>').next("span");
    label.text(escapeHTML(Msg));
    label.show();
}
function hideErrorSpeakMsg(inputId) {
    $('#' + escapeHTML(inputId)).removeClass('has-error');
    $('#' + escapeHTML(inputId)).next("span").remove();
}
function IsNullInputFieldSpeak(inputId, Msg) {
    let IsNullInput = false;
    if ($("#" + escapeHTML(inputId)).val().trim() == "") {
        IsNullInput = true;
        showErrorSpeakMsg(inputId, Msg);
    }
    else {
        hideErrorSpeakMsg(inputId);
    }
    return IsNullInput;
}
function IsNullCheckboxFieldSpeak(inputId, labelId, Msg) {
    let IsNullInput = false;
    if (!$("#" + escapeHTML(inputId)).is(":checked")) {
        IsNullInput = true;
        showErrorSpeakMsg(labelId, Msg);
    }
    else {
        hideErrorSpeakMsg(labelId);
    }
    return IsNullInput;
}

function IsNullFileTypeFieldSpeak(inputId, Msg) {
    let IsNullInput = false;
    //var extension = file.substr( (file.lastIndexOf('.') +1) );
    if ($("#" + escapeHTML(inputId))[0].files.length == 0) {
        IsNullInput = true;
        showErrorSpeakMsg(inputId, Msg);
    }
    else {
        hideErrorSpeakMsg(inputId);
    }
    return IsNullInput;
}
function IsNullFileTypeExtensionFieldSpeak(inputId, Msg) {
    let IsNullInput = false;
    var extension = $("#" + escapeHTML(inputId)).val().substr(($("#" + escapeHTML(inputId)).val().lastIndexOf('.') + 1));
    if (extension != "pdf") {

        IsNullInput = true;
        showErrorSpeakMsg(inputId, Msg);
    }
    else {
        hideErrorSpeakMsg(inputId);
    }
    return IsNullInput;
}

function IsNullFileTypeExtensionFieldPhoto(inputId, Msg) {
    let IsNullInput = false;
    var extension = $("#" + escapeHTML(inputId)).val().substr(($("#" + escapeHTML(inputId)).val().lastIndexOf('.') + 1)).toUpperCase();
    if (extension != "PNG" && extension != "JPG" && extension != "GIF" && extension != "JPEG") {

        IsNullInput = true;
        showErrorSpeakMsg(inputId, Msg);
    }
    else {
        hideErrorSpeakMsg(inputId);
    }
    return IsNullInput;
}




$('#checknominatingmyself').click(function () {
    if (!$(this).is(':checked')) {
        $(".nominee").show();
    }
    else {
        $(".nominee").hide();
        $('.nominee span.custom-field-validation-error').remove();
        $('.nominee input.error').removeClass('error')
    }
});
$('#check-consent').click(function () {
    if (!$(this).is(':checked')) {
        showErrorSpeakMsg(escapeHTML($('#check-consent').next('label').attr('id')), escapeHTML(ErrorSpeakMsg.checkconsentNoValid));
    }
    else {
        hideErrorSpeakMsg(escapeHTML($('#check-consent').next('label').attr('id')));
    }
});
function IsNullSelectFieldSpeak(inputId, Msg) {
    let IsNullInput = false;
    if ($("#" + escapeHTML(inputId)).val().trim() == "") {
        IsNullInput = true;
        showErrorSpeakMsg(inputId, Msg);
    }
    else {
        hideErrorSpeakMsg(inputId);
    }
    return IsNullInput;
}
function SpeakUsForm() {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AdaniGreenTalksSpeakFormPage' }).then(function (token) {
            console.log("google token", token)
            $('#googleCaptchaToken').val(token);
            SpeakUsFormSubmit();
        });
    });


}

// left: 37, up: 38, right: 39, down: 40,
// spacebar: 32, pageup: 33, pagedown: 34, end: 35, home: 36
var keys = { 37: 1, 38: 1, 39: 1, 40: 1, 32: 1 };
function preventDefault(e) {
    e.preventDefault();
}
function preventDefaultForScrollKeys(e) {
    if (keys[e.keyCode]) {
        preventDefault(e);
        return false;
    }
}
// modern Chrome requires { passive: false } when adding event
var supportsPassive = false;
try {
    window.addEventListener("test", null, Object.defineProperty({}, 'passive', {
        get: function () { supportsPassive = true; }
    }));
} catch (e) { }
var wheelOpt = supportsPassive ? { passive: false } : false;
var wheelEvent = 'onwheel' in document.createElement('div') ? 'wheel' : 'mousewheel';
// call this to Disable
function disableScroll() {
    window.addEventListener('DOMMouseScroll', preventDefault, false); // older FF
    window.addEventListener(wheelEvent, preventDefault, wheelOpt); // modern desktop
    window.addEventListener('touchmove', preventDefault, wheelOpt); // mobile
    window.addEventListener('keydown', preventDefaultForScrollKeys, false);
}
// call this to Enable
function enableScroll() {
    window.removeEventListener('DOMMouseScroll', preventDefault, false);
    window.removeEventListener(wheelEvent, preventDefault, wheelOpt);
    window.removeEventListener('touchmove', preventDefault, wheelOpt);
    window.removeEventListener('keydown', preventDefaultForScrollKeys, false);
}

function SpeakUsFormSubmit() {
    var icount = 0;
    if ($('#checknominatingmyself').is(':checked')) {
        $('.nominee span.custom-field-validation-error').remove();
        $('.filevalidation').remove();
        $('.nominee input.error').removeClass('error')
    }

    let IsFormError = jQuery(".form-section form .custom-field-validation-error").length > 0 ? true : false;
    if (!IsFormError) {
        setTimeout(function () {
            $("#submitloader").css("display", "block");
            $('#buttontext').css("display", "none");
            disableScroll();
        }, 10);
        if (IsNullInputFieldSpeak("first-name", escapeHTML(ErrorSpeakMsg.FirstNameNullCheck))) {
            icount++;
        }
        if (IsNullInputFieldSpeak("last-name", escapeHTML(ErrorSpeakMsg.LastNameNullCheck))) {
            icount++;
        }
        if (IsNullInputFieldSpeak("email-id", escapeHTML(ErrorSpeakMsg.EmailNullCheck))) {
            icount++;
        }
        else {
            var e = IsValidEmailSpeak($('#email-id').val());
            if (!e) {
                icount++;
                showErrorSpeakMsg("email-id", escapeHTML(ErrorSpeakMsg.EmailInvalid))
            }
            else {
                hideErrorSpeakMsg("email-id");
            }
        }

        if (IsNullInputFieldSpeak("mobile-number", escapeHTML(ErrorSpeakMsg.MoblieNoNullCheck))) {
            icount++;
        }
        else {
            var e = validateSpeakPhoneNumber($('#mobile-number').val());
            if (!e) {
                icount++;
                showErrorSpeakMsg("mobile-number", escapeHTML(ErrorSpeakMsg.MoblieNoInvalid));
            }
            else {
                hideErrorSpeakMsg("mobile-number");
            }
        }

        if (!$("#checknominatingmyself").is(":checked")) {


            if (IsNullInputFieldSpeak("nominee-first-name", escapeHTML(ErrorSpeakMsg.FirstNameNomineeNullCheck))) {
                icount++;
            }
            if (IsNullInputFieldSpeak("nominee-last-name", escapeHTML(ErrorSpeakMsg.LastNameNomineeNullCheck))) {
                icount++;
            }
            if (IsNullInputFieldSpeak("nominee-email-address", escapeHTML(ErrorSpeakMsg.EmailNomineeNullCheck))) {
                icount++;
            }
            else {
                var e = IsValidEmailSpeak($('#nominee-email-address').val());
                if (!e) {
                    icount++;
                    showErrorSpeakMsg("nominee-email-address", escapeHTML(ErrorSpeakMsg.EmailNomineeInvalid))
                }
                else {
                    hideErrorSpeakMsg("nominee-email-address");
                }
            }

            if (IsNullInputFieldSpeak("contact-number", escapeHTML(ErrorSpeakMsg.ContactNoNullCheck))) {
                icount++;
            }
            else {
                var e = validateSpeakPhoneNumber($('#contact-number').val());
                if (!e) {
                    icount++;
                    showErrorSpeakMsg("contact-number", escapeHTML(ErrorSpeakMsg.ContactNoInvalid));
                }
                else {
                    hideErrorSpeakMsg("contact-number");
                }
            }
        }
        if (IsNullInputFieldSpeak("city-name", escapeHTML(ErrorSpeakMsg.CityNoValid))) {
            showErrorSpeakMsg("city-number", escapeHTML(ErrorSpeakMsg.CityNoValid));
            icount++;
        }
        if (IsNullInputFieldSpeak("country", escapeHTML(ErrorSpeakMsg.CountryNoValid))) {
            showErrorSpeakMsg("country", escapeHTML(ErrorSpeakMsg.CountryNoValid));
            icount++;
        }
        if (IsNullSelectFieldSpeak("Gender", escapeHTML(ErrorSpeakMsg.GenderValid))) {
            showErrorSpeakMsg("Gender", escapeHTML(ErrorSpeakMsg.GenderValid));
            icount++;
        }
        if (IsNullFileTypeFieldSpeak("upl-photo", escapeHTML(ErrorSpeakMsg.UploadPhotoNoNullCheck))) {
            showErrorSpeakFileMsg("upl-photo", escapeHTML(ErrorSpeakMsg.UploadPhotoNoNullCheck), "");
            icount++;
        }
        else {
            var e = IsNullFileTypeExtensionFieldPhoto("upl-photo");
            if (e) {
                icount++;
                // showErrorSpeakFileMsg("upl-bio", ErrorSpeakMsg.UploadBiographyNoValid, "hint");
                showErrorSpeakFileMsg("upl-photo", escapeHTML(ErrorSpeakMsg.UploadPhotoValidCheck), "directshow");
            }
            else {
                hideErrorSpeakFileMsg("upl-photo", "directshow");
            }

            //hideErrorSpeakFileMsg("upl-photo");
        }
        if (IsNullFileTypeFieldSpeak("upl-bio", escapeHTML(ErrorSpeakMsg.UploadBiographyNoNullCheck))) {
            showErrorSpeakFileMsg("upl-bio", escapeHTML(ErrorSpeakMsg.UploadBiographyNoNullCheck), "hint");
            icount++;
        }
        else {
            var e = IsNullFileTypeExtensionFieldSpeak("upl-bio");
            if (e) {
                icount++;
                // showErrorSpeakFileMsg("upl-bio", ErrorSpeakMsg.UploadBiographyNoValid, "hint");
                showErrorSpeakFileMsg("upl-bio", escapeHTML(ErrorSpeakMsg.UploadBiographyNoValid), "directshow", "showerrorBiography");
            }
            else {
                hideErrorSpeakFileMsg("upl-bio", "directshow", "showerrorBiography");
            }
        }
        if (IsNullFileTypeFieldSpeak("upl-concept", escapeHTML(ErrorSpeakMsg.UploadoriginalconceptNoNullCheck))) {
            showErrorSpeakFileMsg("upl-concept", escapeHTML(ErrorSpeakMsg.UploadoriginalconceptNoValid), "hint");
            icount++;
        }
        else {
            var e = IsNullFileTypeExtensionFieldSpeak("upl-concept");
            if (e) {
                icount++;
                showErrorSpeakFileMsg("upl-concept", escapeHTML(ErrorSpeakMsg.UploadoriginalconceptNoValid), "directshow", "showerroruploadoriginalconcept");
                // showErrorSpeakFileMsg("upl-concept", ErrorSpeakMsg.UploadoriginalconceptNoValid, "hint");
            }
            else {
                hideErrorSpeakFileMsg("upl-bio", "directshow", "showerroruploadoriginalconcept");
            }
        }
        if (IsNullInputFieldSpeak("key-takeaway", escapeHTML(ErrorSpeakMsg.keytakeawayNoValid))) {
            showErrorSpeakMsg("key-takeaway", escapeHTML(ErrorSpeakMsg.keytakeawayNoValid));
            icount++;
        }
        if (IsNullInputFieldSpeak("link-for-aud-vid", escapeHTML(ErrorSpeakMsg.audvidNoValid))) {
            showErrorSpeakMsg("link-for-aud-vid", escapeHTML(ErrorSpeakMsg.audvidNoValid));
            icount++;
        }
        if (IsNullInputFieldSpeak("link-for-article", escapeHTML(ErrorSpeakMsg.articleNoValid))) {
            showErrorSpeakMsg("link-for-article", escapeHTML(ErrorSpeakMsg.articleNoValid));
            icount++;
        }
        if (IsNullSelectFieldSpeak("goals", escapeHTML(ErrorSpeakMsg.GoalNoValid))) {
            showErrorSpeakMsg("goals", escapeHTML(ErrorSpeakMsg.GoalNoValid));
            icount++;
        }
        if (IsNullCheckboxFieldSpeak("check-consent", "lbl-check-consent", ErrorSpeakMsg.checkconsentNoValid)) {
            showErrorSpeakMsg("lbl-check-consent", escapeHTML(ErrorSpeakMsg.checkconsentNoValid));
            icount++;
        }
        else {
            if ($('#lbl-check-consent').next('span').length > 0) {
                $('#lbl-check-consent').next("span").remove();
            }
        }

    }
    if (icount > 0) {
        setTimeout(function () {
            $("#submitloader").css("display", "none");
            $('#buttontext').css("display", "block");
            enableScroll();
        }, 100);
        return false;
    }
    else if (!IsFormError) {

        var objlist = [];
        objlist.push({
            name: "FirstName",
            value: escapeHTML($('#first-name').val().trim())
        });
        objlist.push({
            name: "LastName",
            value: escapeHTML($('#last-name').val().trim())
        });
        objlist.push({
            name: "Email",
            value: escapeHTML($('#email-id').val().trim())
        });
        objlist.push({
            name: "mobileNumber",
            value: escapeHTML($('#mobile-number').val().trim())
        });
        objlist.push({
            name: "checknominatingmyself",
            value: $("#checknominatingmyself").is(":checked")
        });

        if (!$("#checknominatingmyself").is(":checked")) {
            objlist.push({
                name: "nomineefirstname",
                value: escapeHTML($('#nominee-first-name').val().trim())
            });
            objlist.push({
                name: "nomineelastname",
                value: escapeHTML($('#nominee-last-name').val().trim())
            });
            objlist.push({
                name: "NomineeEmail",
                value: escapeHTML($('#nominee-email-address').val().trim())
            });
            objlist.push({
                name: "contactNumber",
                value: escapeHTML($('#contact-number').val().trim())
            });
        }
        else {
            objlist.push({
                name: "nomineefirstname",
                value: escapeHTML($('#first-name').val().trim())
            });
            objlist.push({
                name: "nomineelastname",
                value: escapeHTML($('#last-name').val().trim())
            });
            objlist.push({
                name: "NomineeEmail",
                value: escapeHTML($('#email-id').val().trim())
            });
            objlist.push({
                name: "contactNumber",
                value: escapeHTML($('#mobile-number').val().trim())
            });
        }


        objlist.push({
            name: "City",
            value: escapeHTML($('#city-name').val().trim())
        });
        objlist.push({
            name: "country",
            value: escapeHTML($('#country').val().trim())
        });
        objlist.push({
            name: "takeaway",
            value: escapeHTML($('#key-takeaway').val().trim())
        });

        objlist.push({
            name: "linkaudioorvideo",
            value: escapeHTML($('#link-for-aud-vid').val().trim())
        });
        objlist.push({
            name: "linkforarticle",
            value: escapeHTML($('#link-for-article').val().trim())
        });
        objlist.push({
            name: "Gender",
            value: $("#Gender option:selected").text().trim()
        });
        objlist.push({
            name: "Goal",
            value: $("#goals option:selected").text().trim()
        });
        objlist.push({
            name: "FormType",
            value: escapeHTML($('#FormType').val())
        });
        objlist.push({
            name: "FormUrl",
            value: escapeHTML($('#FormUrl').val())
        });
        objlist.push({
            name: "googleCaptchaToken",
            value: $('#googleCaptchaToken').val()
        });

        var formdata = new FormData(); //FormData object

        $(objlist).each(function (key, item) {
            formdata.append(item.name, item.value);
        });


        var fileInput = document.getElementById('upl-photo');
        //Iterating through each files selected in fileInput
        for (i = 0; i < fileInput.files.length; i++) {
            //Appending each file to FormData object
            formdata.append(fileInput.files[i].name, fileInput.files[i]);
            formdata.append("fileUploadPhotoName", fileInput.files[i].name);

        }
        var fileInputbio = document.getElementById('upl-bio');
        //Iterating through each files selected in fileInput
        for (i = 0; i < fileInputbio.files.length; i++) {
            //Appending each file to FormData object
            formdata.append(fileInputbio.files[i].name, fileInputbio.files[i]);
            formdata.append("fileUploadbiographName", fileInputbio.files[i].name);
        }

        var fileInputconcept = document.getElementById('upl-concept');
        //Iterating through each files selected in fileInput
        for (i = 0; i < fileInputconcept.files.length; i++) {
            //Appending each file to FormData object
            formdata.append(fileInputconcept.files[i].name, fileInputconcept.files[i]);
            formdata.append("fileOriginalConceptName", fileInputconcept.files[i].name);
        }



        clearvalidationSpeakmessage();
        $('#SpeakFormSubmit_Btn').addClass('disabled');
        $.ajax({
            url: "/api/AdaniGreenTalks/AdaniGreenTalksSpeakFormPage",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: formdata,
            //async: false,
            headers: { "__RequestVerificationToken": $(".antiforgerytoken").find("input").val() },
            type: "POST",
            cache: false,
            success: function (response) {
                //var response = JSON.parse(data);

                if (response.errorModel != null && response.errorModel.IsError) {
                    //resetFields();
                    ShowSpeakError(escapeHTML(response.errorModel.errorMessage));
                }
                else if (response.contactvalidationlist != null) {
                    $.each(response.contactvalidationlist, function (index, object) {
                        if (object.StatusCode == StatusSpeakCode.FirstNameErrorCode) {
                            showErrorSpeakMsg("first-name", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.LastNameErrorCode) {
                            showErrorSpeakMsg("last-name", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.EmailErrorCode) {
                            showErrorSpeakMsg("email-id", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.MoblieNoCode) {
                            showErrorSpeakMsg("moblie-number", escapeHTML(object.FieldErrorMessage));
                        }
                        if (object.StatusCode == StatusSpeakCode.FirstNameNomineeCode) {
                            showErrorSpeakMsg("nominee-first-name", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.LastNameNomineeCode) {
                            showErrorSpeakMsg("nominee-last-name", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.EmailNomineeCode) {
                            showErrorSpeakMsg("nominee-email-address", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.ContactNoNomineeCode) {
                            showErrorSpeakMsg("contact-number", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.CityCode) {
                            showErrorSpeakMsg("city-name", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.CountryCode) {
                            showErrorSpeakMsg("country", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.GenderCode) {
                            showErrorSpeakMsg("Gender", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.Goal) {
                            showErrorSpeakMsg("Goal", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.linkaudioorvideo) {
                            showErrorSpeakMsg("linkaudioorvideo", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.linkforarticleCode) {
                            showErrorSpeakMsg("link-for-article", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.UploadPhotoCodeCode) {
                            showErrorSpeakFileMsg("upl-photo", object.FieldErrorMessage, "directshow");
                            // showErrorSpeakMsg("Goal", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.UploadoriginalconceptCode) {
                            showErrorSpeakFileMsg("upl-concept", object.FieldErrorMessage, "directshow", "showerroruploadoriginalconcept");
                            // showErrorSpeakMsg("linkaudioorvideo", escapeHTML(object.FieldErrorMessage));
                        }
                        else if (object.StatusCode == StatusSpeakCode.UploadBiographyCode) {
                            showErrorSpeakFileMsg("upl-bio", object.FieldErrorMessage, "directshow", "showerrorBiography");

                            // showErrorSpeakMsg("link-for-article", escapeHTML(object.FieldErrorMessage));
                        }
                    });
                    setTimeout(function () {
                        $("#submitloader").css("display", "none");
                        $('#buttontext').css("display", "block");
                        enableScroll();
                        $('form .form-control, form .form-select').each(function () {
                            if ($(this).closest('.nominee').length === 0) {
                                if ($(this).val() === '' || (($(this).val() === '-1' || $(this).val() === '') && $(this).prop('tagName') === 'SELECT')) {
                                    var getTop = $(this).offset().top;
                                    var headerHeight = $('.header').outerHeight();
                                    var finalScroll = getTop - headerHeight - 20;
                                    $('html, body').animate({ scrollTop: finalScroll });
                                    return false
                                }
                            }
                        });
                    }, 100);
                    // $("#submitloader").attr("style","display:none")
                    $('#SpeakFormSubmit_Btn').removeClass('disabled');
                }
                else {
                    resetSpeakFields();
                    ShowSpeakModal();
                }
            },
            error: function (response) {
                console.log(response);
                resetSpeakFields();
            }
        });
    }
    else {
        setTimeout(function () {
            $("#submitloader").css("display", "none");
            $('#buttontext').css("display", "block");
            enableScroll();
        }, 1000);
    }

}
$('.CloseThankyou').on('click', function () {
    HideSpeakModal();
    location.reload();
    scrollTo(0, 0);
});
function ShowSpeakError(msg) {
    $('#SpeakFormSubmit_Btn').removeClass('disabled');
    // $("#contactLoader").hide();
    setTimeout(function () {
        $("#submitloader").css("display", "none");
        $('#buttontext').css("display", "block");
        enableScroll();
    }, 1000);
    $('#divError').text(escapeHTML(msg));
    $('#divError').show();
}
function HideSpeakError() {
    $('#divError').text('');
    $('#divError').hide();
}
function ShowSpeakModal() {
    HideSpeakError();
    $('#SpeakFormSubmit_Btn').removeClass('disabled');
    //$("#contactLoader").hide();
    setTimeout(function () {
        $("#submitloader").css("display", "none");
        $('#buttontext').css("display", "block");
        enableScroll();
    }, 1000);
    $('body').addClass('active-popup');
}
function HideSpeakModal() {
    $('body').removeClass('active-popup');
}
function resetSpeakFields() {
    $("#first-name").val('');
    $("#last-name").val('');
    $("#email-id").val('');
    $('#mobile-number').val('');
    $("#city-name").val('');
    $('#country').val('');
    $('#Gender').val('-1');
    $('#Goal').val('-1');
    $('#linkaudioorvideo').val('');
    $('#link-for-article').val('');
    $('#key-takeaway').val('');
    $('#link-for-aud-vid').val('');
    $('#upl-photo').val('');
    $('#upl-bio').val('');
    $('#upl-concept').val('');
}
function clearvalidationSpeakmessage() {
    $('#first-name').removeClass('has-error');
    $('#first-name').next('span').remove();
    $('#last-name').removeClass('has-error');
    $('#last-name').next('span').remove();
    $('#email-id').removeClass('has-error');
    $('#email-id').next('span').remove();
    $('#mobile-number').removeClass('has-error');
    $('#mobile-number').next('span').remove();
    $('#city-name').removeClass('has-error');
    $('#city-name').next('span').remove();
    $('#country').removeClass('has-error');
    $('#country').next('span').remove();
    $('#Gender').removeClass('has-error');
    $('#Gender').next('span').remove();
    $('#Goal').removeClass('has-error');
    $('#Goal').next('span').remove();
    $('#linkaudioorvideo').removeClass('has-error');
    $('#linkaudioorvideo').next('span').remove();
    $('#link-for-article').removeClass('has-error');
    $('#link-for-article').next('span').remove();
    $('#key-takeaway').removeClass('has-error');
    $('#key-takeaway').next('span').remove();
    $('#upl-photo').parent().removeClass('error');
    $('#upl-photo').parent().next('span').remove();
    $('#upl-bio').parent().removeClass('error');
    $('#upl-bio').parent().next('span').remove();
    $('#upl-concept').parent().removeClass('error');
    $('#upl-concept').parent().next('span').remove();
    $('#link-for-aud-vid').parent().removeClass('error');
    $('#link-for-aud-vid').parent().next('span').remove();
    HideSpeakError();
}
function validateSpeakPhoneNumber(input_str) {
    /*var re = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;*/

    var validateMobNum = /^\d*(\?:\.\d{1,2})?$/;
    return validateMobNum.test(input_str) && input_str.length == 10;
}
function IsValidEmailSpeak(email) {
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

function validateSpeak(key) {
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
$(document).on('keypress', '#contact-number,#mobile-number', function (event) {
    if (!validateSpeak(event)) { event.preventDefault(); return false; }
});