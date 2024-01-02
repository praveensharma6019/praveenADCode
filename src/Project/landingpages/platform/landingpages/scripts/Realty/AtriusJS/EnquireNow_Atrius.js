var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
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

$(".close_btn").click(function (e) {
    document.getElementById("enq_form").reset();
    document.getElementById("price_form").reset();
    document.getElementById("broc_form").reset();
});

//Header Form
$("#enq_submit, #resend_enq_submit").click(function (e) {
    getCaptchaResponseForm();
    e.preventDefault();

});
$("#otp_submit").click(function (e) {
    $("#eotp_error").html('');
    e.preventDefault();
    getOtpVerified();
})

function getCaptchaResponseForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AtriusEnquiryNow' }).then(function (token) {
            $('.g-recaptcha').val(token);

            $("#eotp_error").html('');

            if (!$("#enq_name").val()) {
                $("#enq_name").next().html('Please Enter Your Name!');
                return false;
            } else {
                $("#enq_name").next().html("");
            }

            if (!$("#enq_email").val()) {
                $("#enq_email").next().html('Please Enter Your Email ID!');
                return false;
            } else {
                $("#enq_email").next().html("");
            }



            if (validateEmail($("#enq_email").val()) == false) {
                $("#enq_email").next().html("Please enter proper Email ID!");
                return false;
            } else {
                $("#enq_email").next().html("");
            }

            if (!$("#enq_phone").val()) {
                $("#enq_phone").next().html('Please Enter Your Mobile Number!');
                return false;
            } else {
                $("#enq_phone").next().html("");
            }


            if ($("#enq_phone").val().length !== 10 || regex_special_num.test($("#enq_phone").val()) == false) {
                $("#enq_phone").next().html("Please enter 10 digit Mobile Number!");
                return false;
            } else {
                $("#enq_phone").next().html("");
            }

            if (!$("#enq_html").is(":checked")) {
                $("#enq_html").next().next().html('Please Check the Terms!');
                return false;
            } else {
                $("#enq_html").next().next().html("");
            }

            document.getElementById("enq_submit").disabled = true;

            var savecustomdata = {

                Name: $("#enq_name").val(),
                MobileNumber: $("#enq_phone").val(),
                Email: $("#enq_email").val(),
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/POSTOTP",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "1") {
                        document.body.classList.add("otpModal");
                        $('.enquire-now h3').text('OTP Verification');
                        $(".enquire-now .modal").addClass("showOTP");
                        const mobileNumber = $("#enq_phone").val()
                        document.getElementById('emobile-number').innerHTML = mobileNumber;
                    }
                    else if (data.status === "503") {
                        $("#enq_html").next().next().html('OTP limit is exceeded! Please try again after 1 hour');
                        document.getElementById("enq_submit").disabled = false;
                    } else if (data.status === "0") {
                        $("#enq_html").next().next().html('Oops, something went wrong');
                        document.getElementById("enq_submit").disabled = false;
                    }
                },
                error: function (data) {
                    $("#enq_form").next().html('Some technical issue please try after some time');
                }
            });
        });
    });
}
function getOtpVerified(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AtriusEnquiryNow' }).then(function (token) {
            $('.g-recaptcha').val(token);

            document.getElementById("otp_submit").disabled = true;

            var savecustomdata = {
                full_name: $("#enq_name").val(),
                mobile: $("#enq_phone").val(),
                email: $("#enq_email").val(),
                OTP: $("#digit1").val() + $("#digit2").val() + $("#digit3").val() + $("#digit4").val() + $("#digit5").val(),
                Projects_Interested__c: "RESIDENTIAL",
                PropertyLocation: "Ahmedabad",
                Remarks: "",
                FormType: "Enquire Form",
                PageInfo: "Atrius Landing",
                ProjectName: "ATRIUS",
                PageURL: window.location.href,
                FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                UTMSource: Globalutm_source,
                UTMPlacement: Globalutm_placement,
                RecordType: GlobalrecordId,
                PropertyCode: GlobalmasterId,
                AdvertisementId: GlobalAdvId,
                reResponse: $(".g-recaptcha").val(),
                isincludedquerystring: isincludedquerystring

            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/AtriusEnquiryNow",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "101") {

                        $(this).prop('disabled', true);
                        window.location.href = "/atrius/thankyou";
                        document.getElementById("price_form").reset();

                    }
                    else if (data.status === "401") {
                        $("#price_name").next().html('Please Enter Your Name!');
                        document.getElementById("otp_submit").disabled = false;
                    } else if (data.status === "403") {
                        $("#price_email").next().html('Please enter proper Email ID!');
                        document.getElementById("otp_submit").disabled = false;
                    } else if (data.status === "405") {
                        $("#price_phone").next().html('Please Enter Your Mobile Number!');
                        document.getElementById("otp_submit").disabled = false;
                    } else if (data.status === "0") {
                        document.getElementById("OTP_form").reset();
                        $("#eotp_error").html('Invalid OTP');
                        document.getElementById("otp_submit").disabled = false;
                    }
                },

                error: function (data) {
                    $("#price_form").next().html('Some technical issue please try after some time');
                }
            });
        });
    });
}

//Price Form
$("#enqprice_submit, #resend_enqprice_submit").click(function (e) {
    getCaptchaResponsePrice();
    e.preventDefault();

});
$("#price_otp_submit").click(function (e) {
    $("#potp_error").html('');
    e.preventDefault();
    getPriceOtpVerified();
})

//Price form Start
function getCaptchaResponsePrice(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AtriusEnquiryNow' }).then(function (token) {
            $('.g-recaptcha').val(token);

            $("#potp_error").html('');

            if (!$("#price_name").val()) {
                $("#price_name").next().html('Please Enter Your Name!');
                return false;
            } else {
                $("#price_name").next().html("");
            }

            if (!$("#price_email").val()) {
                $("#price_email").next().html('Please Enter Your Email ID!');
                return false;
            } else {
                $("#price_email").next().html("");
            }



            if (validateEmail($("#price_email").val()) == false) {
                $("#price_email").next().html("Please enter proper Email ID!");
                return false;
            } else {
                $("#price_email").next().html("");
            }

            if (!$("#price_phone").val()) {
                $("#price_phone").next().html('Please Enter Your Mobile Number!');
                return false;
            } else {
                $("#price_phone").next().html("");
            }


            if ($("#price_phone").val().length !== 10 || regex_special_num.test($("#price_phone").val()) == false) {
                $("#price_phone").next().html("Please enter 10 digit Mobile Number!");
                return false;
            } else {
                $("#price_phone").next().html("");
            }

            if (!$("#price_html").is(":checked")) {
                $("#price_html").next().next().html('Please Check the Terms!');
                return false;
            } else {
                $("#price_html").next().next().html("");
            }

            document.getElementById("enqprice_submit").disabled = true;
            // var projectname= $('#enq_form #enq_project option:selected').text().trim().split(",")[0];

            var savecustomdata = {

                Name: $("#price_name").val(),
                MobileNumber: $("#price_phone").val(),
                Email: $("#price_email").val(),
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/POSTOTP",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "1") {
                        document.body.classList.add("otpModal");
                        $(".price-configuration .modal").addClass("showOTP");
                        const mobileNumber = $("#price_phone").val()
                        document.getElementById('pmobile-number').innerHTML = mobileNumber;
                    }
                    else if (data.status === "503") {
                        $("#price_html").next().next().html('OTP limit is exceeded! Please try again after 1 hour');
                        document.getElementById("enqprice_submit").disabled = false;
                    } else if (data.status === "0") {
                        $("#price_html").next().next().html('Oops, something went wrong');
                        document.getElementById("enqprice_submit").disabled = false;
                    }
                },
                error: function (data) {
                    $("#enq_form").next().html('Some technical issue please try after some time');
                }
            });
        });
    });
}
function getPriceOtpVerified(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AtriusEnquiryNow' }).then(function (token) {
            $('.g-recaptcha').val(token);

            document.getElementById("price_otp_submit").disabled = true;

            var savecustomdata = {
                full_name: $("#price_name").val(),
                mobile: $("#price_phone").val(),
                email: $("#price_email").val(),
                OTP: $("#pdigit1").val() + $("#pdigit2").val() + $("#pdigit3").val() + $("#pdigit4").val() + $("#pdigit5").val(),
                Projects_Interested__c: "RESIDENTIAL",
                PropertyLocation: "Ahmedabad",
                Remarks: "",
                FormType: "Price Enquire Form",
                PageInfo: "Atrius Landing",
                ProjectName: "Atrius",
                PageURL: window.location.href,
                FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                UTMSource: Globalutm_source,
                UTMPlacement: Globalutm_placement,
                RecordType: GlobalrecordId,
                PropertyCode: GlobalmasterId,
                AdvertisementId: GlobalAdvId,
                reResponse: $(".g-recaptcha").val(),
                isincludedquerystring: isincludedquerystring

            };
            

            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/AtriusEnquiryNow",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "101") {

                        $(this).prop('disabled', true);
                        window.location.href = "/Atrius/thankyou";
                        document.getElementById("price_form").reset();

                    }
                    else if (data.status === "401") {
                        $("#price_name").next().html('Please Enter Your Name!');
                        document.getElementById("price_otp_submit").disabled = false;
                    } else if (data.status === "403") {
                        $("#price_email").next().html('Please enter proper Email ID!');
                        document.getElementById("price_otp_submit").disabled = false;
                    } else if (data.status === "405") {
                        $("#price_phone").next().html('Please Enter Your Mobile Number!');
                        document.getElementById("price_otp_submit").disabled = false;
                    } else if (data.status === "0") {
                        document.getElementById("POTP_form").reset();
                        $("#potp_error").html('Invalid OTP');
                        document.getElementById("price_otp_submit").disabled = false;
                    }
                },

                error: function (data) {
                    $("#price_form").next().html('Some technical issue please try after some time');
                }
            });
        });
    });
}


//Brochure Form
$("#form_brochure, #resend_form_brochure").click(function (e) {
    getCaptchaResponseDownload();
    e.preventDefault();

});
$("#brochure_otp_submit").click(function (e) {
    $("#dotp_error").html('');
    e.preventDefault();
    getBroOtpVerified();
})
/*Brochure Form Start*/
function getCaptchaResponseDownload(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AtriusEnquiryNow' }).then(function (token) {
            $('.g-recaptcha').val(token);

            $("#dotp_error").html('');

            if (!$("#full_name").val()) {
                $("#full_name").next().html('Please Enter Your Name!');
                return false;
            } else {
                $("#full_name").next().html("");
            }

            if (!$("#full_email").val()) {
                $("#full_email").next().html('Please Enter Your Email ID!');
                return false;
            } else {
                $("#full_email").next().html("");
            }



            if (validateEmail($("#full_email").val()) == false) {
                $("#full_email").next().html("Please enter proper Email ID!");
                return false;
            } else {
                $("#full_email").next().html("");
            }

            if (!$("#full_phone").val()) {
                $("#full_phone").next().html('Please Enter Your Mobile Number!');
                return false;
            } else {
                $("#full_phone").next().html("");
            }


            if ($("#full_phone").val().length !== 10 || regex_special_num.test($("#full_phone").val()) == false) {
                $("#full_phone").next().html("Please enter 10 digit Mobile Number!");
                return false;
            } else {
                $("#full_phone").next().html("");
            }

            if (!$("#broc_html").is(":checked")) {
                $("#broc_html").next().next().html('Please Check the Terms!');
                return false;
            } else {
                $("#broc_html").next().next().html("");
            }

            document.getElementById("form_brochure").disabled = true;

            var savecustomdata = {

                Name: $("#full_name").val(),
                MobileNumber: $("#full_phone").val(),
                Email: $("#full_email").val(),
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/POSTOTP",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "1") {
                        document.body.classList.add("otpModal");
                        $(".download-brochure .modal").addClass("showOTP");
                        const mobileNumber = $("#full_phone").val()
                        document.getElementById('dmobile-number').innerHTML = mobileNumber;
                    }
                    else if (data.status === "503") {
                        $("#price_html").next().next().html('OTP limit is exceeded! Please try again after 1 hour');
                        document.getElementById("form_brochure").disabled = false;
                    } else if (data.status === "0") {
                        $("#price_html").next().next().html('Oops, something went wrong');
                        document.getElementById("form_brochure").disabled = false;
                    }
                },
                error: function (data) {
                    $("#enq_form").next().html('Some technical issue please try after some time');
                }
            });
        });
    });
}
function getBroOtpVerified(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'AtriusEnquiryNow' }).then(function (token) {
            $('.g-recaptcha').val(token);

            document.getElementById("brochure_otp_submit").disabled = true;

            var savecustomdata = {
                full_name: $("#full_name").val(),
                mobile: $("#full_phone").val(),
                email: $("#full_email").val(),
                OTP: $("#ddigit1").val() + $("#ddigit2").val() + $("#ddigit3").val() + $("#ddigit4").val() + $("#ddigit5").val(),
                Projects_Interested__c: "RESIDENTIAL",
                PropertyLocation: "Ahmedabad",
                Remarks: "",
                FormType: "Download Brochure",
                PageInfo: "Atrius Landing",
                ProjectName: "ATRIUS",
                PageURL: window.location.href,
                FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                UTMSource: Globalutm_source,
                UTMPlacement: Globalutm_placement,
                RecordType: GlobalrecordId,
                PropertyCode: GlobalmasterId,
                AdvertisementId: GlobalAdvId,
                reResponse: $(".g-recaptcha").val(),
                isincludedquerystring: isincludedquerystring
            };


            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/AtriusEnquiryNow",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "101") {

                        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

                        if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i) || userAgent.match(/iPod/i)) {
                            // alert('IOS Device');


                            var link = document.createElement('a');
                            link.href = "images/Realty/AtriusImages/docs/Atrius-Brochure.pdf";
                            link.download = "Atrius Brochure";
                            link.click();
                            link.remove();

                        }
                        // Desktop Version Start
                        else {


                            var link = document.createElement('a');
                            link.href = "images/Realty/AtriusImages/docs/Atrius-Brochure.pdf";
                            link.download = "Atrius Brochure";
                            link.click();
                            link.remove();


                            $(this).prop('disabled', true);
                            document.getElementById("broc_form").reset();
                            window.location.href = "/Atrius/thankyou";
                        }
                    }
                    else if (data.status === "401") {
                        $("#full_name").next().html('Please Enter Your Name!');
                        document.getElementById("brochure_otp_submit").disabled = false;
                    } else if (data.status === "403") {
                        $("#full_email").next().html('Please enter proper Email ID!');
                        document.getElementById("brochure_otp_submit").disabled = false;
                    } else if (data.status === "405") {
                        $("#full_phone").next().html('Please Enter Your Mobile Number!');
                        document.getElementById("brochure_otp_submit").disabled = false;
                    } else if (data.status === "0") {
                        document.getElementById("DOTP_form").reset();
                        $("#dotp_error").html('Invalid OTP');
                        document.getElementById("brochure_otp_submit").disabled = false;
                    }
                },

                error: function (data) {
                    $("#broc_form").next().html('Some technical issue please try after some time');
                }
            });
        });
    });
}
