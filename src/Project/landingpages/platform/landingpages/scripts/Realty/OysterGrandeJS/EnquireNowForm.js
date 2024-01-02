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
    
    
    

    // if (!$("#otp_eq").val()) {
		// $("#otp_eq").next().show();
        // $("#otp_eq").next().html('Please Enter OTP!');
        // return false;
    // } else {
        // $("#otp_eq").next().html("");
    // }
    
    var savecustomdata = {
                                        full_name: $("#full_name_eq").val(),
                                        last_name: "",
										mobile: $("#phone_number_eq").val(),
                                        email: $("#email_eq").val(),
                                        Budget: "",
                                        country_code: "",
                                        state_code: "",
                                        Projects_Interested__c: "RESIDENTIAL",
                                        PropertyLocation: "gurgaon",
                                        sale_type: "",
                                        Remarks: "",
                                        FormType: "OG enquire form Header",
                                        PageInfo: "Oyster Grande",
                                        PageURL: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source,
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/OysterGrandeEnquiryNow",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
							
								$('#Enquire-now').modal('hide');
								$('#success').modal('hide');
								$('#eq_form #otp-button').hide();
								$('#eq_form #timer').hide();
                            //window.location.href = "/thankyou";
								
							document.getElementById("eq_form").reset();
							window.location.href = "/oystergrande-penthouse/thank-you?utm_source=" + utm_source;
                        }
						// else if (data.status === "102"){
                            // $("#form_eq").next().html('please enter valid otp');
                        // }
						else if(data.status === "401"){
							$("#full_name_eq").next().html('Please Enter Your Name!');
						}else if(data.status === "403"){
							$("#email_eq").next().html('Please enter proper Email ID!');
						}else if(data.status === "405"){
							$("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
						}
						// else if(data.status === "406"){
							// $("#otp_eq").next().html('Please Enter OTP!');
						// }else if(data.status === "103"){
							 // $("#form_eq").next().html('You Entered 3 times wrong otp please send otp again');
							 // $('#eq_form #otp-button').hide();
							 // $("#phone_number_eq").next().next().show();
							 // $("#phone_number_eq").next().next().children().show();
						// }
						// else if(data.status === "108"){
							// $("#form_eq").next().html('Please send otp on your mobile number');
							 // $('#eq_form #otp-button').hide();
							 // $("#phone_number_eq").next().next().show();
							 // $("#phone_number_eq").next().next().children().show();
						// }

                    },

                    error: function (data) {
                        $("#form_eq").next().html('Some technical issue please try after some time'); // 
                    }
                });

    

});

/*Footer Form start*/

$("#form_fot").click(function (e) {
    e.preventDefault();
    if (!$("#name_foot").val()) {
        $("#name_foot").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#name_foot").next().html("");
    }

   

    if (!$("#email_foot").val()) {
        $("#email_foot").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#email_foot").next().html("");
    }

    

    if (validateEmail($("#email_foot").val()) == false) {
        $("#email_foot").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#email_foot").next().html("");
    }

    if (!$("#phone_number_foot").val()) {
        $("#phone_number_foot").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#phone_number_foot").next().html("");
    }


    if ($("#phone_number_foot").val().length !== 10 || regex_special_num.test($("#phone_number_foot").val()) == false) {
        $("#phone_number_foot").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#phone_number_foot").next().html("");
    }
    
    
    

    // if (!$("#otp_eq").val()) {
		// $("#otp_eq").next().show();
        // $("#otp_eq").next().html('Please Enter OTP!');
        // return false;
    // } else {
        // $("#otp_eq").next().html("");
    // }
    
    var savecustomdata = {
                                        full_name: $("#name_foot").val(),
                                        last_name: "",
										mobile: $("#phone_number_foot").val(),
                                        email: $("#email_foot").val(),
                                        Budget: "",
                                        country_code: "",
                                        state_code: "",
                                        Projects_Interested__c: "RESIDENTIAL",
                                        PropertyLocation: "gurgaon",
                                        sale_type: "",
                                        Remarks: "",
                                        FormType: "OG enquire form Footer",
                                        PageInfo: "Oyster Grande",
                                        PageURL: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source,
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/OysterGrandeEnquiryNow",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
							
								$('#Enquire-now').modal('hide');
								$('#success').modal('hide');
								$('#footer_form #otp-button').hide();
								$('#footer_form #timer').hide();
                            //window.location.href = "/thankyou";
							document.getElementById("footer_form").reset();
							window.location.href = "/oystergrande-penthouse/thank-you?utm_source=" + utm_source;
                        }
						// else if (data.status === "102"){
                            // $("#form_fot").next().html('please enter valid otp');
                        // }
						else if(data.status === "401"){
							$("#name_foot").next().html('Please Enter Your Name!');
						}else if(data.status === "403"){
							$("#email_foot").next().html('Please enter proper Email ID!');
						}else if(data.status === "405"){
							$("#phone_number_foot").next().html('Please Enter Your Mobile Number!');
						}
						// else if(data.status === "406"){
							// $("#otp_eq").next().html('Please Enter OTP!');
						// }else if(data.status === "103"){
							 // $("#form_fot").next().html('You Entered 3 times wrong otp please send otp again');
							 // $('#footer_form #otp-button').hide();
							 // $("#phone_number_foot").next().next().show();
							 // $("#phone_number_foot").next().next().children().show();
						// }
						// else if(data.status === "108"){
							// $("#form_fot").next().html('Please send otp on your mobile number');
							 // $('#footer_form #otp-button').hide();
							 // $("#phone_number_foot").next().next().show();
							 // $("#phone_number_foot").next().next().children().show();
						// }

                    },

                    error: function (data) {
                        $("#form_fot").next().html('Some technical issue please try after some time'); // 
                    }
                });

    

});

/*Footer Form end*/