var Contactrecaptcha;
if (typeof sitekey !== 'undefined' && (sitekey != null && sitekey != '')) {
    var onloadCallback = function () {
        //Render the Contactrecaptcha on the element with ID "Contactrecaptcha"
        if ($('#Contactrecaptcha').length) {
            Contactrecaptcha = grecaptcha.render('Contactrecaptcha', {
                'sitekey': sitekey,
                'theme': 'light'
            });
        }
    }
}
function sendEmailToC() {
    var response = grecaptcha.getResponse(Contactrecaptcha);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    var icount = 0;
    if ($("#txtname").val().trim() == "") {
        icount++;
        $('#txtname').addClass('has-error');
        $("#lblrname").html("* Please enter your name");
        $("#lblrname").show();
    } else {
        $('#txtname').removeClass('has-error');
        $("#lblrname").hide();
    }
    if ($("#txtemail").val().trim() == "") {
        icount++;
        $('#txtemail').addClass('has-error');
        $("#lblemail").html("* Please enter your email");
        $("#lblemail").show();
    } else {
        var e = IsValidEmail($("#txtemail").val());
        if (!e) {
            icount++;
            $('#txtemail').addClass('has-error');
            $("#lblemail").html("* Please enter a valid email address");
            $("#lblemail").show();
        } else {
            $('#txtemail').removeClass('has-error');
            $("#lblemail").hide();
        }
    }
    if ($("#txtcomment").val().trim() == "") {
        icount++;
        $('#txtcomment').addClass('has-error');
        $("#lblcomm").html("* Please enter your message");
        $("#lblcomm").show();
    } else {
        $('#txtcomment').removeClass('has-error');
        $("#lblcomm").hide();
    }
    if (icount > 0) {
        return false;
    } else {

        var obj = {
            'name': $("#txtname").val().trim(),
            'email': $("#txtemail").val().trim(),
            'enquiry': $('#enquiry').val().trim(),
            'message': $("#txtcomment").val().trim()
        };
        var contactvalidationlist = [];

        var objname = {
            'txtFieldName': 'txtname',
            'txtFieldValue': $("#txtname").val().trim(),
            'lblErrorField': 'lblrname',
            'lblErrorFieldMessage': '* Please enter your name'
        }

        contactvalidationlist.push(objname);

        var objemail = {
            'txtFieldName': 'txtemail',
            'txtFieldValue': $("#txtemail").val().trim(),
            'lblErrorField': 'lblemail',
            'lblErrorFieldMessage': '* Please enter your email'
        }

        contactvalidationlist.push(objemail);

        var objcomment = {
            'txtFieldName': 'txtcomment',
            'txtFieldValue': $("#txtcomment").val().trim(),
            'lblErrorField': 'lblcomm',
            'lblErrorFieldMessage': '* Please enter your comment'
        }

        contactvalidationlist.push(objcomment);

        var objenquiry = {
            'txtFieldName': 'enquiry',
            'txtFieldValue': $("#enquiry").val().trim(),
            'lblErrorField': 'lblequiry',
            'lblErrorFieldMessage': '* Please select enquiry'
        }

        contactvalidationlist.push(objenquiry);

        var objJsonResult = {
            'IsSuccess': false,
            'IsValid': true,
            'IsError': false,
            'ErrorMessage': '',
            'ErrorSource': '',
            'contactvalidationlist': contactvalidationlist,
            'objContactUs': obj,
            'reResponse': response
        }
        console.log("object" + objJsonResult);
        $("#contactLoader").show();
        $.ajax({
            url: "/WebtransApi/Email/SendEmail",
            contentType: "application/json; charset=utf-8",
            headers: { "__RequestVerificationToken": $(".antiforgerytoken").find("input").val() },
            data: JSON.stringify(objJsonResult),
            type: "POST",
            cache: false,
            success: function (response) {

                console.log(response.data)
                clearvalidationmessage();
                $("#contactLoader").hide();
                var data = response;
                if (data.IsError) {
                    resetFields();
                    alert(data.ErrorMessage);
                    //alert("Please try aftersome time, we are facing issue. " + response.ErrorMessage);
                    return false;
                }
                if (data.IsValid) {
                    if (data.IsSuccess) {
                        resetFields();
                        $("#messageSuccess").html("Thank you for contacting us.");
                        $("#messageShort").html("We have received your enquiry and shall get back to you shortly.");
                        $('#sentFeedbackSuccess').modal('show');
                    }
                    return true;
                }
                else if (data.ErrorMessage != null && data.ErrorMessage != "") {
                    alert(data.ErrorMessage);
                    grecaptcha.reset();
                }
                else {
                    $.each(data.contactvalidationlist, function (index, object) {
                        if (object.lblErrorField != "") {
                            $('#' + object.txtFieldName).addClass('has-error');
                            $("#" + object.lblErrorField).show();
                            $("#" + object.lblErrorField).html(object.lblErrorFieldMessage);

                        }
                    });
                    grecaptcha.reset();
                    return false;
                }

            },
            error: function (response) {
                resetFields();
                $("#contactLoader").hide();
                $("#messageSuccess").html("Error submitting the form");
                $("#messageShort").html("");
                $('#sentFeedbackSuccess').modal('show');
                return false;
            }
        });
    }
}
function resetFields() {
    $("#txtname").val('');
    $("#txtemail").val('');
    $("#txtcomment").val('');
    $("#enquiry").prop('selectedIndex', 0);
    grecaptcha.reset();
}
function clearvalidationmessage() {
    $('#txtname').removeClass('has-error');
    $("#lblrname").hide();
    $('#txtemail').removeClass('has-error');
    $("#lblemail").hide();
    $('#txtcomment').removeClass('has-error');
    $("#lblcomm").hide();
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
$(document).on('click', '#sentFeedbackSuccess .close,#sentFeedbackSuccess .modal-footer button', function () {
    $('#sentFeedbackSuccess').modal('hide');

});