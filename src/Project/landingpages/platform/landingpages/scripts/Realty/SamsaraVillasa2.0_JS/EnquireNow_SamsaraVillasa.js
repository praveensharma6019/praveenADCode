var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}
function GetURLParameter(sParam) {
    var url_string = window.location.href;
    var url = new URL(url_string);
    var c = url.searchParams.get(sParam);
    return c
}
var globalurl;
var GlobalmasterId;
var GlobalAdvId;
var Globalutm_source;
var Globalutm_placement;
var GlobalrecordId;
var isincludedquerystring = false;

var globalurl = window.location.href;
if (globalurl.includes('?')) {
    function GetURLParameter(sParam) {
        var url_string = window.location.href;
        var url = new URL(url_string);
        var c = url.searchParams.get(sParam);
        return c
    }
    Globalutm_source = GetURLParameter('utm_source');
    Globalutm_placement = GetURLParameter('utm_placement');
    GlobalAdvId = GetURLParameter('AdvertisementId');
    GlobalrecordId = "0129D000000Ajol";  // for UAT only 
    GlobalmasterId = "a4S9D000000Cgci";  // for UAT only
    isincludedquerystring = true;
}
else {
    Globalutm_source = "";
    Globalutm_placement = "";
    GlobalAdvId = "";  // for UAT only
    GlobalrecordId = "";  // for UAT only
    GlobalmasterId = "";  // for UAT only
    isincludedquerystring = false;
}


const MobilenumberEq = document.getElementById('phone_number_eq')
const EmailEq = document.getElementById('email_eq')
const nameEq = document.getElementById('full_name_eq')

nameEq.addEventListener('change', (event) => {
    event.preventDefault();
    if (!$("#full_name_eq").val()) {
        $("#full_name_eq").next().html('Please Enter valid Name!');
    } else {
        $("#full_name_eq").next().html("");
    }
})

MobilenumberEq.addEventListener('change', (event) => {
    event.preventDefault();
    if ($("#phone_number_eq").val().length !== 10 || regex_special_num.test($("#phone_number_eq").val()) == false) {
        $("#phone_number_eq").next().html("Please enter valid Mobile Number!");
    } else {
        $("#phone_number_eq").next().html("");
    }
})

EmailEq.addEventListener('change', (event) => {
    event.preventDefault();
    if (validateEmail($("#email_eq").val()) == false) {
        $("#email_eq").next().html("Please enter proper Email ID!");
    } else {
        $("#email_eq").next().html("");
    }
})

$("#SVform_EQ").click(function (e) {

    $("#otp_limit").html('');

    e.preventDefault();
    if (!$("#full_name_eq").val()) {
        $("#full_name_eq").next().html('Please Enter valid Name!');
    } else {
        $("#full_name_eq").next().html("");
    }

    if (!$("#email_eq").val()) {
        $("#email_eq").next().html('Please Enter Your Email ID!');
    } else {
        $("#email_eq").next().html("");
    }

    if (!$("#phone_number_eq").val()) {
        $("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#phone_number_eq").next().html("");
    }

    if (validateEmail($("#email_eq").val()) == false) {
        $("#email_eq").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#email_eq").next().html("");
    }


    if ($("#phone_number_eq").val().length !== 10 || regex_special_num.test($("#phone_number_eq").val()) == false) {
        $("#phone_number_eq").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#phone_number_eq").next().html("");
    }

    var savecustomdata = {

        Name: $("#full_name_eq").val(),
        MobileNumber: $("#phone_number_eq").val(),
        Email: $("#email_eq").val(),
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/POSTOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "1") {

                otpTimer(30);

                $("#otp_form")[0].reset();
                $("#otpModal").modal({
                    show: "false",
                });
                const mobileNumber = $("#phone_number_eq").val();
                document.getElementById('mobile-number').innerHTML = mobileNumber;
            } else if (data.status === "503") {
                $("#otp_limit").html('OTP limit is exceeded! Please try again after 1 hour');
            } 
        },
        error: function (data) {
            $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
        }
    });
});



const MobilenumberBr = document.getElementById('broc_mobile')
const EmailBr = document.getElementById('broc_email')
const nameBr = document.getElementById('broc_name')

nameBr.addEventListener('change', (event) => {
    event.preventDefault();
    if (!$("#broc_name").val()) {
        $("#broc_name").next().html('Please Enter valid Name!');
    } else {
        $("#broc_name").next().html("");
    }
})

MobilenumberBr.addEventListener('change', (event) => {
    event.preventDefault();
    if ($("#broc_mobile").val().length !== 10 || regex_special_num.test($("#broc_mobile").val()) == false) {
        $("#broc_mobile").next().html("Please enter valid Mobile Number!");
    } else {
        $("#broc_mobile").next().html("");
    }
})

EmailBr.addEventListener('change', (event) => {
    event.preventDefault();
    if (validateEmail($("#broc_email").val()) == false) {
        $("#broc_email").next().html("Please enter proper Email ID!");
    } else {
        $("#broc_email").next().html("");
    }
})
$("#broc_eqform").click(function (e) {

    $("#otp_limit_broc").html('');

    e.preventDefault();
    if (!$("#broc_name").val()) {
        $("#broc_name").next().html('Please Enter valid Name!');
    } else {
        $("#broc_name").next().html("");
    }


    if (!$("#broc_email").val()) {
        $("#broc_email").next().html('Please Enter Your Email ID!');
    } else {
        $("#broc_email").next().html("");
    }

    if (!$("#broc_mobile").val()) {
        $("#broc_mobile").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#broc_mobile").next().html("");
    }

    if (validateEmail($("#broc_email").val()) == false) {
        $("#broc_email").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#broc_email").next().html("");
    }


    if ($("#broc_mobile").val().length !== 10 || regex_special_num.test($("#broc_mobile").val()) == false) {
        $("#broc_mobile").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#broc_mobile").next().html("");
    }

    var savecustomdata = {
        Name: $("#broc_name").val(),
        MobileNumber: $("#broc_mobile").val(),
        Email: $("#broc_email").val(),
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/POSTOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "1") {

                otpTimer(30);

                $("#otp_form")[0].reset();
                $("#otpModal").modal({
                    show: "false",
                });
                const mobileNumber = $("#broc_mobile").val();
                document.getElementById('mobile-number').innerHTML = mobileNumber;
            } else if (data.status === "503") {
                $("#otp_limit_broc").html('OTP limit is exceeded! Please try again after 1 hour');
            } 
        },
        error: function (data) {
            $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
        }
    });
});

$(".btn-resend-otp a").click(function () {

    if ($("#broc_name").val() !== '') {
        var savecustomdata = {
            Name: $("#broc_name").val(),
            MobileNumber: $("#broc_mobile").val(),
            Email: $("#broc_email").val(),
        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Realty/POSTOTP",
            contentType: "application/json",
            success: function (data) {
                if (data.status === "1") {

                    otpTimer(30);

                    $("#otp_form")[0].reset();
                    $("#otpModal").modal({
                        show: "false",
                    });
                    const mobileNumber = $("#broc_mobile").val();
                    document.getElementById('mobile-number').innerHTML = mobileNumber;
                } else if (data.status === "503") {
                    $("#otp_limit").html('OTP limit is exceeded! Please try again after 1 hour');
                } 
            },
            error: function (data) {
                $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
            }
        });
    }
    if ($("#full_name_fot").val() !== '') {
        var savecustomdata = {
            Name: $("#full_name_fot").val(),
            MobileNumber: $("#phone_number_fot").val(),
            Email: $("#email_fot").val(),
        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Realty/POSTOTP",
            contentType: "application/json",
            success: function (data) {
                if (data.status === "1") {

                    otpTimer(30);

                    $("#otp_form")[0].reset();
                    $("#otpModal").modal({
                        show: "false",
                    });
                    const mobileNumber = $("#phone_number_fot").val();
                    document.getElementById('mobile-number').innerHTML = mobileNumber;
                } else if (data.status === "503") {
                    $("#otp_limit").html('OTP limit is exceeded! Please try again after 1 hour');
                } 
            },
            error: function (data) {
                $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
            }
        });
    }
    if ($("#full_name_eq").val() !== '') {
        var savecustomdata = {

            Name: $("#full_name_eq").val(),
            MobileNumber: $("#phone_number_eq").val(),
            Email: $("#email_eq").val(),
        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Realty/POSTOTP",
            contentType: "application/json",
            success: function (data) {
                if (data.status === "1") {

                    otpTimer(30);

                    $("#otp_form")[0].reset();
                    $("#otpModal").modal({
                        show: "false",
                    });
                    const mobileNumber = $("#phone_number_eq").val();
                    document.getElementById('mobile-number').innerHTML = mobileNumber;
                } else if (data.status === "503") {
                    $("#otp_limit").html('OTP limit is exceeded! Please try again after 1 hour');
                } 
            },
            error: function (data) {
                $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
            }
        });
    }
});

function otpTimer(timeleft) {
    setTimeout(() => {
        const ele = document.querySelectorAll(".form-otp");
        ele[0].focus();
    }, 500);
    var downloadTimer = setInterval(function () {
        timeleft--;
        document.getElementById('countdowntimer').innerHTML = timeleft;
        $(".btn-resend-otp").hide();
        $(".countdown").show();
        if (timeleft <= 0) {
            clearInterval(downloadTimer);
            $(".countdown").hide();
            $(".btn-resend-otp").show();
        }
    }, 1000);

    $(".form-edit").click(function () {
        $("#otpModal").modal("hide");
        $("#btn-resend-otp").hide();
        clearInterval(downloadTimer);
        document.getElementById("countdowntimer").innerHTML = 30;
    });
}

const Mobilenumber = document.getElementById('phone_number_fot')
const Email = document.getElementById('email_fot')
const name = document.getElementById('full_name_fot')

name.addEventListener('change', (event) => {
    event.preventDefault();
    if (!$("#full_name_fot").val()) {
        $("#full_name_fot").next().html('Please Enter valid Name!');
    } else {
        $("#full_name_fot").next().html("");
    }
})

Mobilenumber.addEventListener('change', (event) => {
    event.preventDefault();
    if ($("#phone_number_fot").val().length !== 10 || regex_special_num.test($("#phone_number_fot").val()) == false) {
        $("#phone_number_fot").next().html("Please enter valid Mobile Number!");
    } else {
        $("#phone_number_fot").next().html("");
    }
})

Email.addEventListener('change', (event) => {
    event.preventDefault();
    if (validateEmail($("#email_fot").val()) == false) {
        $("#email_fot").next().html("Please enter proper Email ID!");
    } else {
        $("#email_fot").next().html("");
    }
})

$("#formSV_fot").click(function (e) {

    $("#otp_limit_fut").html('');

    e.preventDefault();

    if (!$("#full_name_fot").val()) {
        $("#full_name_fot").next().html('Please Enter valid Name!');
        return false;
    } else {
        $("#full_name_fot").next().html("");
    }



    if (!$("#email_fot").val()) {
        $("#email_fot").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#email_fot").next().html("");
    }



    if (validateEmail($("#email_fot").val()) == false) {
        $("#email_fot").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#email_fot").next().html("");
    }

    if (!$("#phone_number_fot").val()) {
        $("#phone_number_fot").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#phone_number_fot").next().html("");
    }


    if ($("#phone_number_fot").val().length !== 10 || regex_special_num.test($("#phone_number_fot").val()) == false) {
        $("#phone_number_fot").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#phone_number_fot").next().html("");
    }

    var savecustomdata = {
        Name: $("#full_name_fot").val(),
        MobileNumber: $("#phone_number_fot").val(),
        Email: $("#email_fot").val(),
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/POSTOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "1") {

                otpTimer(30);

                $("#otp_form")[0].reset();
                $("#otpModal").modal({
                    show: "false",
                });
                const mobileNumber = $("#phone_number_fot").val();
                document.getElementById('mobile-number').innerHTML = mobileNumber;
            } else if (data.status === "503") {
                $("#otp_limit_fut").html('OTP limit is exceeded! Please try again after 1 hour');
            } 
        },
        error: function (data) {
            $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
        }
    });
});

$("#formotp_val").click(function (e) {

    e.preventDefault();

    if ($("#full_name_eq").val() !== '') {
        var savecustomdata = {

            full_name: $("#full_name_eq").val(),
            mobile: $("#phone_number_eq").val(),
            email: $("#email_eq").val(),
            OTP: $("#digit1").val() + $("#digit2").val() + $("#digit3").val() + $("#digit4").val() + $("#digit5").val(),
            Projects_Interested__c: "RESIDENTIAL",
            PropertyLocation: "Gurgaon",
            Remarks: "",
            FormType: "Enquire Form",
            Country: "India",
            PageInfo: "Samsara Vilasa2.0",
            ProjectName: "SamsaraVilasa2.0",
            PageURL: window.location.href,
            FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
            UTMSource: Globalutm_source,
            UTMPlacement: Globalutm_placement,
            RecordType: GlobalrecordId,
            PropertyCode: GlobalmasterId,
            AdvertisementId: GlobalAdvId,
            reResponse: $(".g-recaptcha").val(),
            isincludedquerystring: isincludedquerystring,
        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Realty/SamsaraVilasa2EnquiryNow",
            contentType: "application/json",
            success: function (data) {
                if (data.status === "101") {

                    $('#Enquire-now').modal('hide');
                    $('#success').modal('hide');
                    $('#EQ_SVForm #otp-button').hide();
                    $('#EQ_SVForm #timer').hide();
                    //window.location.href = "/thankyou";
                    document.getElementById("EQ_SVForm").reset();
                    $('#loader').show();
                    var delayInMilliseconds = 1000; //1 second
                    setTimeout(function () {
                        //your code to be executed after 1 second
                        window.location.href = "samsaravilasa-2.0/thankyou";
                    }, delayInMilliseconds);
                    document.getElementById("EQ_SVForm").reset();

                }
                else if (data.status === "401") {
                    $("#full_name_eq").next().html('Please Enter Valid Name!');
                } else if (data.status === "403") {
                    $("#email_eq").next().html('Please enter proper Email ID!');
                } else if (data.status === "405") {
                    $("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
                } else if (data.status === "0") {
                    $("#otp_error").html('Invalid OTP');
                    $("#otp_form")[0].reset();
                } else if (data.status === "503") {
                    $("#otp_limit").html('OTP limit exceeded');
                    $("#otp_form")[0].reset();
                }
            },

            error: function (data) {
                $("#SVform_EQ").next().html('Some technical issue please try after some time'); // 
            }

        });
    }
    if ($("#broc_name").val() !== '') {
        var savecustomdata = {
            full_name: $("#broc_name").val(),
            mobile: $("#broc_mobile").val(),
            email: $("#broc_email").val(),
            OTP: $("#digit1").val() + $("#digit2").val() + $("#digit3").val() + $("#digit4").val() + $("#digit5").val(),
            Projects_Interested__c: "RESIDENTIAL",
            PropertyLocation: "Gurgaon",
            Remarks: "",
            FormType: "Brochure Form",
            Country: "India",
            PageInfo: "Samsara Vilasa2.0",
            ProjectName: "SamsaraVillasa2.0",
            PageURL: window.location.href,
            FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
            UTMSource: Globalutm_source,
            UTMPlacement: Globalutm_placement,
            RecordType: GlobalrecordId,
            PropertyCode: GlobalmasterId,
            AdvertisementId: GlobalAdvId,
            reResponse: $(".g-recaptcha").val(),
            isincludedquerystring: isincludedquerystring,
        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Realty/SamsaraVilasa2EnquiryNow",
            contentType: "application/json",
            success: function (data) {
                if (data.status === "101") {

                    var userAgent = navigator.userAgent || navigator.vendor || window.opera;

                    if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i) || userAgent.match(/iPod/i)) {
                        // alert('IOS Device');


                        var link = document.createElement('a');
                        link.href = "images/Realty/SamsaraVillasa2.0_Images/documents/Brochure-samsara.pdf";
                        link.download = "Samsara Brochure.pdf";
                        link.click();
                        link.remove();
                        setTimeout(function () {
                            link.click();
                            link.remove();
                        }, 500);

                    }
                    else if (userAgent.toString().includes("Mac OS")) {
                        var link = document.createElement('a');
                        link.href = "images/Realty/SamsaraVillasa2.0_Images/documents/Brochure-samsara.pdf";
                        link.download = "Samsara Brochure.pdf";
                        setTimeout(function () {
                            link.click();
                            link.remove();
                        }, 50);
                        setTimeout(function () { document.location.href = "/samsaravilasa-2.0/thankyou"; }, 250);
                        $(this).prop('disabled', true);
                    }
                    // Desktop Version Start
                    else {
                        var link = document.createElement('a');
                        link.href = "images/Realty/SamsaraVillasa2.0_Images/documents/Brochure-samsara.pdf";
                        link.download = "Samsara Brochure.pdf";
                        link.click();
                        link.remove();
                        $(this).prop('disabled', true);
                        document.getElementById("broc_form").reset();
                        window.location.href = "/samsaravilasa-2.0/thankyou";
                    }
                }
                else if (data.status === "401") {
                    $("#broc_name").next().html('Please Enter Valid Name!');
                } else if (data.status === "403") {
                    $("#broc_email").next().html('Please enter proper Email ID!');
                } else if (data.status === "405") {
                    $("#broc_mobile").next().html('Please Enter Your Mobile Number!');
                } else if (data.status === "0") {
                    $("#otp_error").html('Invalid OTP');
                    $("#otp_form")[0].reset();
                }

            },

            error: function (data) {
                $("#broc_eqform").next().html('Some technical issue please try after some time'); // 
            }
        });
    }
    if ($("#full_name_fot").val() !== '') {
        var savecustomdata = {
            full_name: $("#full_name_fot").val(),
            mobile: $("#phone_number_fot").val(),
            email: $("#email_fot").val(),
            Projects_Interested__c: "RESIDENTIAL",
            PropertyLocation: "Gurgaon",
            OTP: $("#digit1").val() + $("#digit2").val() + $("#digit3").val() + $("#digit4").val() + $("#digit5").val(),
            Remarks: "",
            FormType: "Enquire Form",
            Country: "India",
            PageInfo: "Samsara Vilasa2.0",
            ProjectName: "SamsaraVillasa2.0",
            PageURL: window.location.href,
            FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
            UTMSource: Globalutm_source,
            UTMPlacement: Globalutm_placement,
            RecordType: GlobalrecordId,
            PropertyCode: GlobalmasterId,
            AdvertisementId: GlobalAdvId,
            reResponse: $(".g-recaptcha").val(),
            isincludedquerystring: isincludedquerystring,

        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Realty/SamsaraVilasa2EnquiryNow",
            contentType: "application/json",
            success: function (data) {
                if (data.status === "101") {

                    $('#success').modal('hide');
                    $('#footer_form #otp-button').hide();
                    $('#footer_form #timer').hide();
                    document.getElementById("footer_form").reset();
                    window.location.href = "/samsaravilasa-2.0/thankyou";
                }
                else if (data.status === "401") {
                    $("#full_name_fot").next().html('Please Enter Valid Name!');
                } else if (data.status === "403") {
                    $("#email_fot").next().html('Please enter proper Email ID!');
                } else if (data.status === "405") {
                    $("#phone_number_fot").next().html('Please Enter Your Mobile Number!');
                } else if (data.status === "0") {
                    $("#otp_error").html('Invalid OTP');
                    $("#otp_form")[0].reset();
                }

            },

            error: function (data) {
                $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
            }
        });
    }
});
$("#otpModal").on("shown.bs.modal", function (e) { otpTimer(30); });