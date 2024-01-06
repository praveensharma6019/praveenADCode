var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

var url = location.href;
var utm_url = url.split("?");
var utm_codes = [];

var utm_source = '';
var utm_medium = '';
var utm_campaign = '';
var utm_adgroup = '';
var utm_term = '';

if (utm_url[1]) {
    utm_codes = utm_url[1].split("&");
    if (utm_codes[0]) {
        utm_source = utm_codes[0].replace('utm_source=', '');
    } else {
        utm_source = 'Generic-Visit';
    }

    if (utm_codes[1]) {
        utm_medium = utm_codes[1].replace('utm_medium=', '');
    }
    else {
        utm_medium = 'Generic-Visit';
    }

    if (utm_codes[2]) {
        utm_campaign = utm_codes[2].replace('utm_campaign=', '');
    } else {
        utm_campaign = 'Generic-Visit';
    }

    if (utm_codes[3]) {
        utm_content = utm_codes[3].replace('utm_content=', '');
    } else {
        utm_content = 'Generic-Visit';
    }

    if (utm_codes[4]) {
        utm_term = utm_codes[4].replace('utm_term=', '');
    } else {
        utm_term = 'Generic-Visit';
    }
} else {
    utm_source = 'Generic-Visit';
    utm_medium = 'Generic-Visit';
    utm_campaign = 'Generic-Visit';
    utm_content = 'Generic-Visit';
    utm_term = 'Generic-Visit';
}

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}
function showOtpButton(event, t) {
    var mobile = $(t).val();
    var attr = $(t).attr("data-id");
    var errorclass = $(t).attr("data-class");
    if (mobile.length === 10 && regex_special_num.test(mobile) === true) {
        $("#" + attr).show();
    } else {
        $("." + errorclass).html("Please enter a valid Mobile Number");
    }


}

function SendOtpFooterMobile(event, t) {
    var mobile = $("#footer-input-mobile").val();
    var self = $(t);
    var count = parseInt($(t).attr("data-click"));
    var attr = $(t).attr("data-id");
    var errorclass = $(t).attr("data-class");
    if (mobile.length === 10 && regex_special_num.test(mobile) === true) {

        $("." + errorclass).html('You will receive an otp on your number<div class="loader"></div>');
        
       
            $.ajax({
                type: 'POST',
                data: { mobile: mobile },
                url: "/api/InspireBkc/genrateOtp",
                success: function (data) {
                    if (data.status === "1") {
                        self.attr('data-click', count + 1);
                        $("." + attr).show();
                        $("#footer-input-otp-button").hide();
                        $("." + errorclass).html("");
                        self.hide();
                        $("#footer-input-mobile").attr('readonly',true);
                    }

                },

                error: function (data) {
                    $("." + errorclass).html("Some technical issue please after some time");  // 
                }
            });
        
    }
    else {
        $("." + errorclass).html("Please enter a valid Mobile Number");
    }

}

function SendOtpMobile(event, t) {
    var mobile = $("#input-mobile").val();
    var self = $(t);
    var count = parseInt($(t).attr("data-click"));
    var attr = $(t).attr("data-id");
    var errorclass = $(t).attr("data-class");
    if (mobile.length === 10 && regex_special_num.test(mobile) === true) {

        $("." + errorclass).html('You will receive an otp on your number<div class="loader"></div>');


        $.ajax({
            type: 'POST',
            data: { mobile: mobile },
            url: "/api/InspireBkc/genrateOtp",
            success: function (data) {
                if (data.status === "1") {
                    self.attr('data-click', count + 1);
                    $("." + attr).show();
                    $("." + errorclass).html("");
                    $("#input-otp-button").hide();
                    $("#input-mobile").attr('readonly',true);
                    self.hide();
                }

            },

            error: function (data) {
                $("." + errorclass).html("Some technical issue please after some time");  // 
            }
        });

    }
    else {
        $("." + errorclass).html("Please enter a valid Mobile Number");
    }

}



$("#submit_form").click(function (e) {
    e.preventDefault();
    if (!$("#input-name").val()) {
        $(".err-name").html('Please Enter Your Name!');
        return false;
    } else {
        $(".err-name").html("");
    }

    if (!$("#input-lastname").val()) {
        $(".err-lname").html('Please Enter Your Last Name!');
        return false;
    } else {
        $(".err-lname").html("");
    }

    if (!$("#input-email").val()) {
        $(".err-email").html('Please Enter Your Email ID!');
        return false;
    } else {
        $(".err-email").html("");
    }

    if (!$("#input-email").val()) {
        $("#err-email").html("Please provide your Email ID!");
        return false;
    } else {
        $(".err-email").html("");
    }

    if (validateEmail($("#input-email").val()) == false) {
        $(".err-email").html("Please enter proper Email ID!");
        return false;
    } else {
        $(".err-email").html("");
    }

    if (!$("#input-mobile").val()) {
        $(".err-mobile").html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $(".err-mobile").html("");
    }


    if ($("#input-mobile").val().length !== 10 || regex_special_num.test($("#input-mobile").val()) == false) {
        $(".err-mobile").html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $(".err-mobile").html("");
    }
    
    
    

    if (!$("#input-city").val()) {
        $(".err-company").html('Please Enter Your Company Name!');
        return false;
    } else {
        $(".err-company").html("");
    }
    
    



    if ($("#g_recaptcha_response").val() !== null && $("#g_recaptcha_response").val() !== '') {

        var mobile = $("#input-mobile").val();
        $(".err_msg").html('<div class="loader"></div>');
         $.ajax({
                type: 'POST',
                data: { mobile: mobile },
                url: "/api/InspireBkc/genrateOtp",
                success: function (data) {
                    if (data.status === "1") {
                        //$("#input-mobile").attr('readonly', true);
                        $("#otpModal").modal('show');
                        $(".err_msg").html('');
                    } else {
                        $(".err_msg").html('Please enter valid mobile number');
                    }

                },

                error: function (data) {
                    //$("." + errorclass).html("Some technical issue please after some time");  // 
                    $(".err_msg").html('Some technical issue please after some time');
                }
            });

        
        
    } else {
        $(".err_msg").html('Please verify Recaptcha');
        return false;
    }

});


$(".verify_otp").click(function (e) {
    
    if (!$("#input-mobile").val()) {
        $(".err_msg_pop").html('Please Enter Your Mobile Number!');
        return false;
    }

    if ($("#input-mobile").val().length != 10 || regex_special_num.test($("#input-mobile").val()) == false) {
        $("#err_msg_pop").html("Please enter 10 digit Mobile Number!");
        return false;
    }

    if (!$("#input-otp").val()) {
        $(".err_msg_pop").html('Please Enter OTP!');
        return false;
    }
    $(".err_msg_pop").html('<div class="loader"></div>');
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    var pageinfo = window.location.href.split('?')[0];
    var savecustomdata = {
        first_name: $("#input-name").val().replaceAll(/[^a-zA-Z ]/g, " "),
        last_name: $("#input-lastname").val().replaceAll(/[^a-zA-Z ]/g, " "),
        mobile: $("#input-mobile").val(),
        email: $("#input-email").val(),
        City: $("#input-city").val().replaceAll(/[^a-zA-Z ]/g, " "),
        FormType: "InspireBKcInquiry",
        PageInfo: pageinfo,
        FormSubmitOn: currentdate,
        LeadSource: "Web to Lead",
        UTMSource: utm_source.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMMedium: utm_medium.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMCampaign: utm_campaign.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMContent: utm_content.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMTerm: utm_term.replaceAll(/[^a-zA-Z ]/g, " ")

    };

    var verifyOtp = {
        mobile: $("#input-mobile").val(),
        OTP: $("#input-otp").val()
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(verifyOtp),
        url: "/api/InspireBkc/VerifyOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "1") {
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/InspireBkc/Insertcontactdetail",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "1") {
                            $(".err_msg_pop").html('Your inquriy submitted sucessfully');

                            window.location.href = "/thankyou";
                        } else {
                            $(".err_msg_pop").html('please enter valid otp');
                        }

                    },

                    error: function (data) {
                        $(".err_msg_pop").html('Some technical issue please try after some time'); // 
                    }
                });
            } else {
                $(".err_msg_pop").html('please enter valid otp');
            }

        },

        error: function (data) {
            $(".err_msg_pop").html('Some technical issue please try after some time'); // 
        }
    });

});

$(".generate_otp").click(function () {


    
    var count = parseInt($(this).attr("data-count"));
    var self = $(this);
    var mobile = $("#input-mobile").val();
   

    $(".err_msg_pop").html('<div class="loader"></div>');


        $.ajax({
            type: 'POST',
            data: { mobile: mobile },
            url: "/api/InspireBkc/genrateOtp",
            success: function (data) {
                if (data.status === "1") {
                    $(".err_msg_pop").html('OTP has been shared on your mobile number, Please verify your mobile number using the OTP!');
                    if (count < 3) {
                        self.attr('data-count', count + 1);
                    } else {
                        self.hide();
                        $(".err_msg_pop").html('You can send otp only 3 times');
                    }

                } else {
                    $(".err_msg_pop").html("Some technical error occured, Please try again later!");
                    return false;
                }

            },

            error: function (data) {
                $(".err_msg_pop").html("Some technical error occured, Please try again later!");
                return false; // 
            }
        });

   
});



$("#input-mobile").on('change', function(e) {
    if ($("#input-mobile").val() == $("#input-mobile-status").val()) {} else {
        $("#input-mobile-verified").val('NO');
        $("#input-mobile-status").val('');
    }
});



$("#footer-submit_form").on('click',function (e) {
    e.preventDefault();
    if (!$("#footer-input-name").val()) {
        $(".err-footer-name").html('Please Enter Your Name!');
        return false;
    } else {
        $(".err-footer-name").html("");
    }

    if (!$("#footer-input-lastname").val()) {
        $(".err-footer-lname").html('Please Enter Your Last Name!');
        return false;
    } else {
        $(".err-footer-lname").html("");
    }

    if (!$("#footer-input-email").val()) {
        $(".err-footer-email").html('Please Enter Your Email ID!');
        return false;
    } else {
        $(".err-footer-email").html("");
    }

    if (!$("#footer-input-email").val()) {
        $(".err-footer-email").html("Please provide your Email ID!");
        return false;
    } else {
        $(".err-footer-email").html("");
    }

    if (validateEmail($("#footer-input-email").val()) == false) {
        $(".err-footer-email").html("Please enter proper Email ID!");
        return false;
    } else {
        $(".err-footer-email").html("");
    }

    if (!$("#footer-input-mobile").val()) {
        $(".err-footer-mobile").html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $(".err-footer-mobile").html("");
    }

    if ($("#footer-input-mobile").val().length != 10 || regex_special_num.test($("#footer-input-mobile").val()) == false) {
        $(".err-footer-mobile").html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $(".err-footer-mobile").html("");
    }
  
    if (!$("#footer-input-city").val()) {
        $(".err-footer-company").html('Please Enter Your Company Name!');
        return false;
    }
    //$("#contactform").submit();

   

    if ($("#g_recaptcha_response").val() !== null && $("#g_recaptcha_response").val() !== '') {
        var mobile = $("#footer-input-mobile").val();
        $(".err_footer-msg").html('<div class="loader"></div>');
        $.ajax({
            type: 'POST',
            data: { mobile: mobile },
            url: "/api/InspireBkc/genrateOtp",
            success: function (data) {
                if (data.status === "1") {
                    //$("#input-mobile").attr('readonly', true);
                    $("#footer-otpModal").modal('show');
                    $(".err_footer-msg").html('');
                } else {
                    $(".err_footer-msg").html('Please enter valid mobile number');
                }

            },

            error: function (data) {
                //$("." + errorclass).html("Some technical issue please after some time");  // 
                $(".err_footer-msg").html('Some technical issue please after some time');
            }
        });
     } else {
        $(".err_footer-msg").html('Please verify Recaptcha');
    return false;
    }

});

$(".footer-verify_otp").click(function (e) {
    
    if (!$("#footer-input-mobile").val()) {
        $(".footer-err_msg_pop").html('Please Enter Your Mobile Number!');
        return false;
    }

    if ($("#footer-input-mobile").val().length != 10 || regex_special_num.test($("#footer-input-mobile").val()) == false) {
        $("#footer-err_msg_pop").html("Please enter 10 digit Mobile Number!");
        return false;
    }

    if (!$("#footer-input-otp").val()) {
        $(".footer-err_msg_pop").html('Please Enter OTP!');
        return false;
    }
    $(".footer-err_msg_pop").html('<div class="loader"></div>');

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    var pageinfo = window.location.href.split('?')[0];
    var savecustomdata = {
        first_name: $("#footer-input-name").val().replaceAll(/[^a-zA-Z ]/g, " "),
        last_name: $("#footer-input-lastname").val().replaceAll(/[^a-zA-Z ]/g, " "),
        mobile: $("#footer-input-mobile").val(),
        email: $("#footer-input-email").val(),
        City: $("#footer-input-city").val().replaceAll(/[^a-zA-Z ]/g, " "),
        FormType: "InspireBKcInquiry",
        PageInfo: pageinfo,
        FormSubmitOn: currentdate,
        LeadSource: "Web to Lead",
        UTMSource: utm_source.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMMedium: utm_medium.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMCampaign: utm_campaign.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMContent: utm_content.replaceAll(/[^a-zA-Z ]/g, " "),
        UTMTerm: utm_term.replaceAll(/[^a-zA-Z ]/g, " ")

    };

    var verifyOtp = {
        mobile: $("#footer-input-mobile").val(),
        OTP: $("#footer-input-otp").val()
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(verifyOtp),
        url: "/api/InspireBkc/VerifyOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "1") {
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/InspireBkc/Insertcontactdetail",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "1") {
                            $(".footer-err_msg_pop").html('Your inquriy submitted sucessfully');

                            window.location.href = "/thankyou";
                        } else {
                            $(".footer-err_msg_pop").html('please enter valid otp');
                        }

                    },

                    error: function (data) {
                        $(".footer-err_msg_pop").html('Some technical issue please try after some time'); // 
                    }
                });
            } else {
                $(".footer-err_msg_pop").html('please enter valid otp');
            }

        },

        error: function (data) {
            $(".footer-err_msg_pop").html('Some technical issue please try after some time'); // 
        }
    });

});

$(".footer-generate_otp").click(function () {



    var count = parseInt($(this).attr("data-count"));
    var self = $(this);
    var mobile = $("#footer-input-mobile").val();


    $(".footer-err_msg_pop").html('<div class="loader"></div>');


    $.ajax({
        type: 'POST',
        data: { mobile: mobile },
        url: "/api/InspireBkc/genrateOtp",
        success: function (data) {
            if (data.status === "1") {
                $(".footer-err_msg_pop").html('OTP has been shared on your mobile number, Please verify your mobile number using the OTP!');
                if (count < 3) {
                    self.attr('data-count', count + 1);
                } else {
                    self.hide();
                    $(".footer-err_msg_pop").html('You can send otp only 3 times');
                }

            } else {
                $(".footer-err_msg_pop").html("Some technical error occured, Please try again later!");
                return false;
            }

        },

        error: function (data) {
            $(".footer-err_msg_pop").html("Some technical error occured, Please try again later!");
            return false; // 
        }
    });


});



