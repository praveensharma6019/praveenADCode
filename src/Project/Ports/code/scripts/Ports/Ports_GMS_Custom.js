function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}

function validateEmailCorporate(mailid) {
    //var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}

function validateEmailId(event, t) {
    var emailAddress = $(t).val();
    if (validateEmail(emailAddress)) {
        $("#emailerror").html("");
    }
    else {
        $("#emailerror").html("Please enter a valid Email Address");
    }
}

function validateEmailIdCorporate(event, t) {
    var emailAddress = $(t).val();
    if (validateEmailCorporate(emailAddress)) {
        $("#emailerror").html("");
    }
    else {
        $("#emailerror").html("Please enter a valid email address only allow form adani.com");
    }
}
function SendOtpEmail(event, t) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'InsertContactdetail' }).then(function (token) {
            $('.g-RegistrationGcptchares-res').val(token);
            var emailAddress = $(t).val();
            if (validateEmail(emailAddress)) {
                $("#emailerror").html("");
                var OTPFor = emailAddress;
                var OTPType = "registration";
                var Gcptchares = $('.g-RegistrationGcptchares-res').val();
                var Status = 0;
                if ($('#Email_Verified').val() === "") {
                    $.ajax({
                        type: 'POST',
                        data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, Gcptchares: Gcptchares, IsMobile: false },
                        url: "/api/Ports/PortsGMSGenerateOTP",
                        success: function (data) {
                            if (data.status === "1") {
                                $("#email-otp-ver").modal('show');
                            }
                        },
                        error: function (data) {
                            alert("error!");  // 
                        }
                    });
                }
            }
            else {
                $("#emailerror").html("Please enter a valid Email Address");
            }
        });
    });
}


function SendOtpEmailCorporate(event, t) {
    var emailAddress = $(t).val();
    if (validateEmailCorporate(emailAddress)) {

        $("#emailerror").html("");
        var OTPFor = emailAddress;
        var OTPType = "registration";
        var Status = 0;
        if ($('#Email_Verified').val() === "") {
            $.ajax({
                type: 'POST',
                data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: false },
                url: "/api/Ports/PortsGMSGenerateOTP",
                success: function (data) {
                    if (data.status === "1") {
                        $("#email-otp-ver").modal('show');
                    }

                },

                error: function (data) {
                    alert("error!");  // 
                }
            });
        }
    }
    else {
        $("#emailerror").html("Please enter a valid email address only allow form adani.com");
    }

}


function SendLoginOtpEmail(event, t) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'InsertContactdetail' }).then(function (token) {
            $('.g-cptcha-res').val(token);
            var emailAddress = $(t).val();
            if (validateEmail(emailAddress)) {
                $("#emailerror").html("");
                var OTPFor = emailAddress;
                var OTPType = "login";
                var Status = 0;
                var Gcptchares = $('.g-cptcha-res').val();
                $.ajax({
                    type: 'POST',
                    data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, Gcptchares: Gcptchares, IsMobile: false },
                    url: "/api/Ports/PortsGMSGenerateOTP",
                    success: function (data) {
                        if (data.status === "1") {
                            $('#mail-login-msg').html("Please enter the 5-digit verification code we sent via Email:");
                        }
                    },

                    error: function (data) {
                        alert("error!");  // 
                    }
                });
            }
            else {
                $("#emailerror").html("Please enter a valid Email Address");
            }
        });
    });
}


function validateMobile(event, t) {

    var mobile = $(t).val();

    if (validateMobileNo(mobile)) {
        $("#mobileerror").html("");
    }
    else {
        $("#mobileerror").html("This is not a valid mobile number");
    }
}
function validateNumber(inputtxts) {
    var numbers = /^[0-9]+$/;
    if (numbers.test(inputtxts)) { return true; }
    else { return false; }
};

function onchangeValidateFax(event, t) {
    var fax = $("#fax").val();
    if (fax !== null && fax.trim() !== "") {
        if (!validateFax(fax)) {
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            //$("#fax").focus();
        }
        else {
            $("#faxerror").html("");
        }
    }
    else {
        $("#faxerror").html("");
    }
}

function validateFax(fax) {
    var regex = /^[0-9]{12,12}$/;
    return regex.test(fax);
}

function validateMobileNo(mobile) {
    if (mobile.match(/^[6789]\d{9}$/)) {
        return true;
    }
    else {
        return false;
    }
}




function validateName(sname) {
    var regex = /^[a-zA-Z ]+$/;

    if (regex.test(sname)) { return true; }
    else { return false; }
};

//function isValidDate(s) {
//    var bits = s.split('/');
//    var d = new Date(bits[2] + '/' + bits[1] + '/' + bits[0]);
//    return !!(d && (d.getMonth() + 1) === bits[1] && d.getDate() === Number(bits[0]));
//}
function isValidDate(dateString) {
    // First check for the pattern
    if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
        return false;

    // Parse the date parts to integers
    var parts = dateString.split("/");
    var day = parseInt(parts[1], 10);
    var month = parseInt(parts[0], 10);
    var year = parseInt(parts[2], 10);

    // Check the ranges of month and year
    if (year < 1000 || year > 3000 || month === 0 || month > 12)
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Adjust for leap years
    if (year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0))
        monthLength[1] = 29;

    // Check the range of the day
    return day > 0 && day <= monthLength[month - 1];
};

/*$("#PortsGmsRegistrationBtn").click(function (event) {
    alert("sdsd");
    var mobileNo = $("#Mobile").val();
    var emailAddress = $("#Email").val();
    var fax = $("#Fax_No").val();
    var legalDisclaimer = $("#terms")[0]["checked"];
    if (!validateMobileNo(mobileNo)) {
        event.preventDefault();
        $("#mobileerror").html("Please enter a 10 digit valid mobile number");
        $("#mobileNumber").focus();
        return false;
    }
    else {
        $("#mobileerror").html("");
    }
    if (fax !== null && fax.trim() !== "") {
        if (!validateFax(fax)) {
            event.preventDefault();
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            $("#fax").focus();
            return false;
        }
        else {
            $("#faxerror").html("Please enter valid Fax number");
        }
    }
    else {
        $("#faxerror").html("");
    }
    if (!validateEmail(emailAddress)) {
        event.preventDefault();
        $("#emailerror").html("Please enter a valid Email Address");
        $("#emailAddress").focus();
        return false;
    }
    else {
        $("#emailerror").html("");
    }
    if (legalDisclaimer != true) {
        event.preventDefault();
        $("#legalDisclaimerError").html("Please checked the Terms and condition");
        $("#legalDisclaimerError").focus();
        return false;
    }
    else {
        $("#legalDisclaimerError").html("");
    }
    
});*/

$(document).ready(function () {


});


var body = $('body');

function goToNextInput(e) {
    var key = e.which,
        t = $(e.target),
        sib = t.next('input');

    if (key !== 9 && (key < 48 || key > 57)) {
        e.preventDefault();
        return false;
    }

    if (key === 9) {
        return true;
    }

    if (!sib || !sib.length) {
        sib = body.find('input').eq(0);
    }
    sib.select().focus();
}

function onKeyDown(e) {
    var key = e.which;

    if (key === 9 || (key >= 48 && key <= 57)) {
        return true;
    }

    e.preventDefault();
    return false;
}

function onFocus(e) {
    $(e.target).select();
}



function SendOtpMobileReg(event, t) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'PortsGMSGenerateOTP' }).then(function (token) {
            $('.g-RegistrationGcptchares-res').val(token);
            if ($(".mobile").val().length === 10 && $('#Mobile_Verified').val() === "") {
                var OTPFor = $(".mobile").val();
                var OTPType = "registration";
                var Status = 0;
                var Gcptchares = $('.g-RegistrationGcptchares-res').val();
                $.ajax({
                    type: 'POST',
                    data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, Gcptchares: Gcptchares, IsMobile: true },
                    url: "/api/Ports/PortsGMSGenerateOTP",
                    success: function (data) {
                        if (data.status === "1") {
                            $("#mobile-otp-ver").modal('show');
                        }

                    },
                    error: function (data) {
                        alert("error!");
                    }
                });
            }
        });
    });
}



$('#title').change(function () {
    var select = document.getElementById("title");
    var text = select.options[select.selectedIndex].text;
    if (text == "Select") {
        return false;
    }

});

$('#Location').change(function () {
    var select = document.getElementById("Location");
    var text = select.options[select.selectedIndex].text;
    if (text == "Select..") {
        return false;
    }

});
$('#Nature').change(function () {
    var select = document.getElementById("Nature");
    var text = select.options[select.selectedIndex].text;
    if (text == "Select..") {
        return false;
    }

});
$('#WhoImpacted').change(function () {
    var select = document.getElementById("WhoImpacted");
    var text = select.options[select.selectedIndex].text;
    if (text == "Select..") {
        return false;
    }

});
$('#Company').change(function () {
    var select = document.getElementById("Company");
    var text = select.options[select.selectedIndex].text;
    if (text === "Select..") {
        return false;
    }

});

$('#Title').change(function () {
    var select = document.getElementById("Title");
    var text = select.options[select.selectedIndex].text;
    if (text === "Select") {
        return false;
    }

});

$('#Gender').change(function () {
    var select = document.getElementById("title");
    var text = select.options[select.selectedIndex].text;
    if (text === "Select") {
        return false;
    }

});

$("#WhatsappNumber").bind('keyup', function (e) {
    if ($(this).val().length === 10 && $('#WhatsappNumber_Mobile_Verified').val() === "") {
        var OTPFor = $(this).val();
        var OTPType = "registration";
        var Status = 0;
        $.ajax({
            type: 'POST',
            data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: true },
            url: "/api/Ports/PortsGMSGenerateOTP",
            success: function (data) {
                if (data.status === "1") {
                    $("#mobile-otp-ver").modal('show');
                }
            },
            error: function (data) {
                alert("error!");
            }
        });
    }


});



$(".phone_number").bind('keyup', function (e) {
    if ($(this).val().length === 10 && $('#phone_Mobile_Verified').val() === "") {
        var OTPFor = $(this).val();
        var OTPType = "registration";
        var Status = 0;
        $.ajax({
            type: 'POST',
            data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: true },
            url: "/api/Ports/PortsGMSGenerateOTP",
            success: function (data) {
                if (data.status === "1") {
                    $("#mobile-otp-ver").modal('show');
                }
            },
            error: function (data) {
                alert("error!");
            }
        });
    }


});


function SendLoginOtpMobile(event, t) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'PortsGMSGenerateOTP' }).then(function (token) {
            $('.g-cptcha-res').val(token);
            if ($('#mobile-login').val().length === 10) {
                var OTPFor = $('#mobile-login').val();
                var OTPType = "login";
                var Status = 0;
                $.ajax({
                    type: 'POST',
                    data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, Gcptchares: $('.g-cptcha-res').val(), IsMobile: true },
                    url: "/api/Ports/PortsGMSGenerateOTP",
                    success: function (data) {
                        if (data.status === "1") {
                            $('#mo-login-msg').html("Please enter the 5-digit verification code we sent via SMS:");
                        }

                    },

                    error: function (data) {
                        alert("error!");  // 
                    }
                });
            }
        });
    });
}


$(".mo-otp").bind('keyup', function (e) {
    var txtData = [];
    txtData.push($('#mobile-otp').val());
    txtData.push($(this).val());

    $("#mobile-otp").val(txtData.join(""));
    e.preventDefault();
});
$("#submit-mo-otp").click(function (e) {
    var txtData = [];
    $(".mo-otp").each(function () {
        txtData.push($(this).val());
    });
    var otp = txtData.join("");
    var OTPType = "registration";
    $.ajax({
        type: 'POST',
        data: { OTP: otp, OTPFor: $('#Mobile').val(), OTPType: OTPType },
        url: "/api/Ports/PortsGMSVerifyOTP",
        success: function (data) {
            if (data.status === "1") {
                $("#mobile-otp-ver").modal('hide');
                $(".mo-otp").val("");
                $('#Mobile_Verified').val(otp);
            } else {
                $("#msg-mo-otp").html("Please enter correct OTP or refresh page to change mobie number");
            }

        },

        error: function (data) {
            alert("error!");  // 
        }
    });
    e.preventDefault();
});


$("#submit-email-otp").click(function (e) {
    var txtData = [];
    $(".mail-otp").each(function () {
        txtData.push($(this).val());
    });
    var otp = txtData.join("");
    var OTPType = "registration";
    $.ajax({
        type: 'POST',
        data: { OTP: otp, OTPFor: $('#Email').val(), OTPType: OTPType },
        url: "/api/Ports/PortsGMSVerifyOTP",
        success: function (data) {
            if (data.status === "1") {
                $("#email-otp-ver").modal('hide');
                $(".mail-otp").val("");
                $('#Email_Verified').val(otp);
            } else {
                $("#msg-mail-otp").html("Please enter correct OTP or refresh page to change email");
            }

        },

        error: function (data) {
            alert("error!");  // 
        }
    });
    e.preventDefault();
});

$(".reset").click(function (e) {
    //$('#ContentPlaceHolder1_date').val('');
    //$('#Status').val('');
    //// $('#form-stakholder').trigger("reset");
    window.location.reload();
});

$(".submitToStackholder").click(function (e) {
    var form = $("#PortsGMSLevel0Reply");
    var formdata = $(form).serialize();
    $.ajax({
        type: 'POST',
        data: formdata,
        url: "/api/PortsGrievance/PortsGMSLevel0SubmitToStackholder",
        success: function (data) {
            window.location.href = data;
        },

        error: function (data) {
            alert("error!");  // 
        }
    });
    e.preventDefault();
});



$("#submitStackholderReview").click(function (e) {
    var form = $("#PortsGMSStakHolderGrivanceView");
    //var response = $("input[id='chkResponse']").is(':checked');
    // var response = $("#chkResponse").prop('checked');
    var response = $('#chkResponse').is(':checked');
    var formdata = $(form).serialize();
    $.ajax({
        type: 'POST',
        data: formdata,
        url: "/api/PortsGrievance/PortsGMSStakHolderGrivanceView?chkResponse=" + response + "",
        success: function (data) {

            window.location.href = data;
        },

        error: function (data) {
            alert("error!");  // 
        }
    });
    e.preventDefault();
});

$("#reaasignPointMan").click(function (e) {
    var form = $("#PortsGMSLevel3Reply");
    var defaultValue = $("#AssignPointMan").val();
    //$(`.point option[value=${defaultValue}]`).attr('selected', true);
    var formdata = $(form).serialize();
    $.ajax({
        type: 'POST',
        data: formdata,
        url: "/api/ReassignPointMan/ReassignPointMan?AssignPointMan=" + defaultValue + "",
        success: function (data) {

            window.location.href = data;
        },

        error: function (data) {
            alert("error!");  // 
        }
    });
    e.preventDefault();
});

$("#BusinessGroup").change(function () {
    var targetDrp = '#SiteHead';
    if ($(this).val()) {
        $('' + targetDrp + ' option').remove();
        $.ajax({
            type: 'GET',
            url: "/api/PortsGrievance/SiteHead",
            data: { "site": $(this).val() },
            async: false,
            success: function (response) {
                $('' + targetDrp + '').append($("<option></option>").val('').html('--Select--'));
                $.each(response, function (index, item) {
                    $('' + targetDrp + '').append('<option value="' + item.SiteHeadCode + '">' + item.SiteHeadName + '</option>');
                });
            },
            error: function (response, success, error) {
                alert("Error: " + error);
            }
        });
    }
    else {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">select</option>');
    }
});

$("#portsRegisterbtn").click(function (e) {
    getCaptchaResponseContactForm();
    e.preventDefault();
});

function getCaptchaResponseContactForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'InsertContactdetail' }).then(function (token) {
            $('.g-cptcha-res').val(token);
            $('#portsRegisterbtn').attr("disabled", "disabled");
            $("#PortsGmsLoginForm").submit();
        });
    });
}


$("#portsSubmit").click(function (e) {
    getCaptchaResponseportsSubmit();
    e.preventDefault();
});

function getCaptchaResponseportsSubmit(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'InsertContactdetail' }).then(function (token) {
            $('.g-SubmitGcptchares-res').val(token);
            $('#portsSubmit').attr("disabled", "disabled");
            $("#PortsGmsGrievanceBookingForm").submit();
        });
    });
}

$("#PortsGmsRegistrationBtn").click(function (e) {
    getCaptchaResponseRegistration();
    e.preventDefault();
});

function getCaptchaResponseRegistration(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'InsertContactdetail' }).then(function (token) {
            $('.g-RegistrationGcptchares-res').val(token);
            $('#PortsGmsRegistrationBtn').attr("disabled", "disabled");
            $("#PortsGmsRegistrationForm").submit();
        });
    });
}

$("#AddUsersBtn").click(function (e) {
    getCaptchaResponseAddUser();
    e.preventDefault();
});

function getCaptchaResponseAddUser(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'InsertContactdetail' }).then(function (token) {
            $('.g-AddUsersGcaptchares-res').val(token);
            $('#AddUsersBtn').attr("disabled", "disabled");
            $("#form-add-users").submit();
        });
    });
}

