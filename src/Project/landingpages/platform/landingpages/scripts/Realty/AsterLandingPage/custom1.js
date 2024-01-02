var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;
var alphabet = /^[a-zA-Z ]*$/;
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

$("#submitpricepopup").click(function (e) {
    e.preventDefault();

    if (!$("#Name4").val()) {
        $("#Name4").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#Name4").next().html("");
    }
	
	if (alphabet.test($("#Name4").val()) == false) {
        $("#Name4").next().html("Please enter Alphabet only!");
        return false;
    } else {
        $("#Name4").next().html("");
    }


    if (!$("#Email4").val()) {
        $("#Email4").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#Email4").next().html("");
    }



    if (validateEmail($("#Email4").val()) == false) {
        $("#Email4").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#Email4").next().html("");
    }

    if (!$("#Mobile4").val()) {
        $("#Mobile4").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#Mobile4").next().html("");
    }


    if ($("#Mobile4").val().length !== 10 || regex_special_num.test($("#Mobile4").val()) == false) {
        $("#Mobile4").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#Mobile4").next().html("");
    }



    var savecustomdata = {
        full_name: $("#Name4").val(),
        last_name: "",
        FormType: "Contact us",
        PageInfo: "NA",
        UTMSource: "Landing_page",
        mobile: $("#Mobile4").val(),
        email: $("#Email4").val(),
        Budget: "",
        country_code: "India",
        state_code: "Gujarat",
        Remarks: "",
        project_type: "Aster",
        city: "Ahmedabad",

    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/AsterEnquiryNow",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "101") {
                // $( "#submitpricepopup" ).attr('disabled','disabled'); 
				$(this).prop('disabled', true);
				window.location.href = "https://www.adanirealty.com/aster/thankyou" ;
            }
            
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

$("#submitf").click(function (e) {
    e.preventDefault();
    if (!$("#Name2").val()) {
        $("#Name2").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#Name2").next().html("");
    }
	
	if (alphabet.test($("#Name2").val()) == false) {
        $("#Name2").next().html("Please enter Alphabet only!");
        return false;
    } else {
        $("#Name2").next().html("");
    }
	
    if (!$("#Email2").val()) {
        $("#Email2").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#Email2").next().html("");
    }
    if (validateEmail($("#Email2").val()) == false) {
        $("#Email2").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#Email2").next().html("");
    }
    if (!$("#Mobile2").val()) {
        $("#Mobile2").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#Mobile2").next().html("");
    }
    if ($("#Mobile2").val().length !== 10 || regex_special_num.test($("#Mobile2").val()) == false) {
        $("#Mobile2").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#Mobile2").next().html("");
    }
    var savecustomdata = {
        full_name: $("#Name2").val(),
        last_name: "",
        FormType: "Contact us",
        PageInfo: "NA",
        UTMSource: "Landing_page",
        mobile: $("#Mobile2").val(),
        email: $("#Email2").val(),
        Budget: "",
        country_code: "India",
        state_code: "Gujarat",
        Remarks: "",
        project_type: "Aster",
        city: "Ahmedabad",

    };
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/AsterEnquiryNow",
        contentType: "application/json",
        success: function (data) {
           if (data.status === "101") {
                $( "#submitf" ).attr('disabled','disabled'); 
				window.location.href = "https://www.adanirealty.com/aster/thankyou" ;
            }
            
            else if (data.status === "401") {
                $("#Name4").next().html('Please Enter Your Name!');
            } else if (data.status === "403") {
                $("#Email4").next().html('Please enter proper Email ID!');
            } else if (data.status === "405") {
                $("#Mobile4").next().html('Please Enter Your Mobile Number!');
            }
        },
        error: function (data) {
            $("#form_eq").next().html('Some technical issue please try after some time'); // 
        }
    });



});

$("#submitinquiryForm").click(function (e) {
    e.preventDefault();
    if (!$("#Name1").val()) {
        $("#Name1").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#Name1").next().html("");
    }
	if (alphabet.test($("#Name1").val()) == false) {
        $("#Name1").next().html("Please enter Alphabet only!");
        return false;
    } else {
        $("#Name1").next().html("");
    }
    if (!$("#Email1").val()) {
        $("#Email1").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#Email1").next().html("");
    }
    if (validateEmail($("#Email1").val()) == false) {
        $("#Email1").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#Email1").next().html("");
    }
    if (!$("#Mobile1").val()) {
        $("#Mobile1").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#Mobile1").next().html("");
    }
    if ($("#Mobile1").val().length !== 10 || regex_special_num.test($("#Mobile1").val()) == false) {
        $("#Mobile1").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#Mobile1").next().html("");
    }
    var savecustomdata = {
        full_name: $("#Name1").val(),
        last_name: "",
        FormType: "Contact us",
        PageInfo: "NA",
        UTMSource: "Landing_page",
        mobile: $("#Mobile1").val(),
        email: $("#Email1").val(),
        Budget: "",
        country_code: "India",
        state_code: "Gujarat",
        Remarks: "",
        project_type: "Aster",
        city: "Ahmedabad",

    };
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/AsterEnquiryNow",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "101") {
                $( "#submitinquiryForm" ).attr('disabled','disabled'); 
				window.location.href = "https://www.adanirealty.com/aster/thankyou" ;
            }
            
            else if (data.status === "401") {
                $("#Name1").next().html('Please Enter Your Name!');
            } else if (data.status === "403") {
                $("#Email1").next().html('Please enter proper Email ID!');
            } else if (data.status === "405") {
                $("#Mobile1").next().html('Please Enter Your Mobile Number!');
            }
        },
        error: function (data) {
            $("#form_eq").next().html('Some technical issue please try after some time'); // 
        }
    });
});



$("#submitPopupForm").click(function (e) {
    e.preventDefault();
    if (!$("#Name3").val()) {
        $("#Name3").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#Name3").next().html("");
    }
	if (alphabet.test($("#Name3").val()) == false) {
        $("#Name3").next().html("Please enter Alphabet only!");
        return false;
    } else {
        $("#Name3").next().html("");
    }
	
    if (!$("#Email3").val()) {
        $("#Email3").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#Email3").next().html("");
    }
    if (validateEmail($("#Email3").val()) == false) {
        $("#Email3").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#Email3").next().html("");
    }
    if (!$("#Mobile3").val()) {
        $("#Mobile3").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#Mobile3").next().html("");
    }
    if ($("#Mobile3").val().length !== 10 || regex_special_num.test($("#Mobile3").val()) == false) {
        $("#Mobile3").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#Mobile3").next().html("");
    }
    var savecustomdata = {
        full_name: $("#Name3").val(),
        last_name: "",
        FormType: "Contact us",
        PageInfo: "NA",
        UTMSource: "Landing_page",
        mobile: $("#Mobile3").val(),
        email: $("#Email3").val(),
        Budget: "",
        country_code: "India",
        state_code: "Gujarat",
        Remarks: "",
        project_type: "Aster",
        city: "Ahmedabad",

    };
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/AsterEnquiryNow",
        contentType: "application/json",
        success: function (data) {
           if (data.status === "101") {
                $( "#submitPopupForm" ).attr('disabled','disabled'); 
				window.location.href = "https://www.adanirealty.com/aster/thankyou" ;
            }
            
            else if (data.status === "401") {
                $("#Name3").next().html('Please Enter Your Name!');
            } else if (data.status === "403") {
                $("#Email3").next().html('Please enter proper Email ID!');
            } else if (data.status === "405") {
                $("#Mobile3").next().html('Please Enter Your Mobile Number!');
            }
        },
        error: function (data) {
            $("#form_eq").next().html('Some technical issue please try after some time'); // 
        }
    });
});
