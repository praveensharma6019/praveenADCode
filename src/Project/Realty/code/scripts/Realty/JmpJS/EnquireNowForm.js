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
var utm_source = GetURLParameter('utm_source');
var utm_medium = GetURLParameter('utm_medium');
var utm_campaign = GetURLParameter('utm_campaign');

$("#form_eq").click(function (e) {
    e.preventDefault();

    if (!$('#agreeCheck').is(':checked')) {
        $("#agreeCheck").next().html('Please check this box if you want to proceed!');
        return false;
    }

    if (!$("#full_name_eq").val()) {
        $("#full_name_eq").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#full_name_eq").next().html("");
    }



    if (!$("#email_eq").val()) {
        $("#email_eq").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#email_eq").next().html("");
    }



    if (validateEmail($("#email_eq").val()) == false) {
        $("#email_eq").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#email_eq").next().html("");
    }

    if (!$("#phone_number_eq").val()) {
        $("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#phone_number_eq").next().html("");
    }


    if ($("#phone_number_eq").val().length !== 10 || regex_special_num.test($("#phone_number_eq").val()) == false) {
        $("#phone_number_eq").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#phone_number_eq").next().html("");
    }




    if (!$("#otp_eq").val()) {
    $("#otp_eq").next().show();
     $("#otp_eq").next().html('Please Enter OTP!');
     return false;
     } else {
    $("#otp_eq").next().html("");
     }

    var propType = $('#propertyType').val();
    if (propType == '') {
        $("#PropertyError").html('Please select propertyt type!');
        $("#dloader").css("display", "none");
        return false;
    }
    else if (propType == 'Premium - 2, 2.5, 3 BHK') {
        var recordTypeId = "0122v000001uNyy";
        var PropertyCodeId = "a4F2v000000IEFN";
        var AdvertisementId = "";
    }
    else if (propType == 'Luxurious - 3 & 4 BHK') {
        var recordTypeId = "0122v000001uNyy";
        var PropertyCodeId = "a4F2v000000IEFN";
        var AdvertisementId = "a3t2u0000005JL3";
    }

    var savecustomdata = {
        full_name: $("#full_name_eq").val(),
        last_name: "",
        FormType: "Contact us",
        PageInfo: "NA",
        UTMSource: "Landing_page",
        mobile: $("#phone_number_eq").val(),
        email: $("#email_eq").val(),
        OTP: $("#otp_eq").val(),
        Budget: "",
        country_code: "India",
        state_code: "Gujrat",
        Remarks: "",
        PropertyCode: "a4F2v000000IEFN",
        project_type: propType,
        city: "Ahmedabad",
        AdvertisementId: AdvertisementId,
        RecordType: "0122v000001uNyy",
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/JMPEnquiryNow",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "101") {

                //$('#Enquire-now').modal('hide');
                //$('#success').modal('hide');
               // $('#eq_form #otp-button').hide();
                //$('#eq_form #timer').hide();
                //window.location.href = "/thankyou";
                //document.getElementById("eq_form").reset();
                window.location.href = "http://realty.dev.local/JMP/thank-you" ;
            }
            // else if (data.status === "102"){
            // $("#form_eq").next().html('please enter valid otp');
            // }
            else if (data.status === "401") {
                $("#full_name_eq").next().html('Please Enter Your Name!');
            } else if (data.status === "403") {
                $("#email_eq").next().html('Please enter proper Email ID!');
            } else if (data.status === "405") {
                $("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
            }
             else if(data.status === "406"){
             $("#otp_eq").next().html('Please Enter OTP!');
             }else if(data.status === "103"){
             $("#form_eq").next().html('You Entered 3 times wrong otp please send otp again');
             $('#eq_form #otp-button').hide();
             $("#phone_number_eq").next().next().show();
             $("#phone_number_eq").next().next().children().show();
             }
             else if(data.status === "108"){
             $("#form_eq").next().html('Please send otp on your mobile number');
             $('#eq_form #otp-button').hide();
             $("#phone_number_eq").next().next().show();
             $("#phone_number_eq").next().next().children().show();
             }

        },

        error: function (data) {
            $("#form_eq").next().html('Some technical issue please try after some time'); // 
        }
    });



});


function SendOtpMobile(event,t) {
    var self = $(t);
    var valattr = $("#phone_number_eq").val();
    if (valattr != "") {
        var mobile = $("#phone_number_eq").val();
    } else {
        var mobile = $(t).attr("data-value");
    }

    //var count = parseInt($(t).attr("data-click"));
    var attr = $(t).attr("data-id");

    // var errorclass = $(t).attr("data-class");
    //if (mobile.length === 10 && regex_special_num.test(mobile) === true) {
    if (mobile.length === 10) {

        self.prev().show();

        self.prev().html('Please Wait....');


       // if (count < 3) {
            $.ajax({
                type: 'POST',
                data: { mobile: mobile },
                url: "/api/Realty/JMPRealtySendOtp",
                success: function (data) {
                    if (data.status === "1") {
                        //self.attr('data-click', count + 1);
                        //$("." + attr).show();
                        //$("." + errorclass).html("");
                        //$("#input-otp-button").hide();
                        //self.attr('readonly',true);
                        self.hide();
                        self.prev().html('You will receive an otp on your number');
                        //$("#" + attr + ' #otp-button a').attr("data-value", mobile);
                        //OTPTimer("#" + attr);
                        //$("#otp_eq_div").css("display", "none");
                    } else if (data.status === "503") {
                        alert("You have sent 3 times OTP Please try again after 30 minutes")
                    }

                },

                error: function (data) {
                    self.prev().html("Some technical issue please after some time");  // 
                }
            });
        /*} else {


            $("#" + attr + ' #otp-button').text("Otp limit reached");
        }*/

    }
    else {
        self.prev().html("Please enter a valid Mobile Number");
    }

}