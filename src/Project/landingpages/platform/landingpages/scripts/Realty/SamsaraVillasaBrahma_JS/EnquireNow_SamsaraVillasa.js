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

$("#SVform_EQ").click(function (e) {
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
                                        FormType: "SV Enquire Form Header",
                                        PageInfo: "Samsara Villasa",
                                        PageURL: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/SamsaraVilasaEnquiryNow",
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
								setTimeout(function() {
								  //your code to be executed after 1 second
								  window.location.href = "/samsaravillasa-brahma/thank-you?utm_source=" + utm_source;
								}, delayInMilliseconds);
							document.getElementById("EQ_SVForm").reset();
							
                        }
						else if(data.status === "401"){
							$("#full_name_eq").next().html('Please Enter Your Name!');
						}else if(data.status === "403"){
							$("#email_eq").next().html('Please enter proper Email ID!');
						}else if(data.status === "405"){
							$("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
						}
                    },

                    error: function (data) {
                        $("#SVform_EQ").next().html('Some technical issue please try after some time'); // 
                    }
                });

    

});

$("#broc_eqform").click(function (e) {
    e.preventDefault();
    if (!$("#broc_name").val()) {
        $("#broc_name").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#broc_name").next().html("");
    }

   

    if (!$("#broc_email").val()) {
        $("#broc_email").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#broc_email").next().html("");
    }

    

    if (validateEmail($("#broc_email").val()) == false) {
        $("#broc_email").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#broc_email").next().html("");
    }

    if (!$("#broc_mobile").val()) {
        $("#broc_mobile").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#broc_mobile").next().html("");
    }


    if ($("#broc_mobile").val().length !== 10 || regex_special_num.test($("#broc_mobile").val()) == false) {
        $("#broc_mobile").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#broc_mobile").next().html("");
    }    
    
    var savecustomdata = {
                                        full_name: $("#broc_name").val(),
                                        last_name: "",
										mobile: $("#broc_mobile").val(),
                                        email: $("#broc_email").val(),
                                        Budget: "",
                                        country_code: "",
                                        state_code: "",
                                        Projects_Interested__c: "RESIDENTIAL",
                                        PropertyLocation: "gurgaon",
                                        sale_type: "",
                                        Remarks: "",
                                        FormType: "SV Brochure from",
										PageInfo: "Samsara Villasa",
                                        PageURL: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/SamsaraVilasaEnquiryNow",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
                            
								$('#success').modal('hide');
								$('#broch_form #otp-button').hide();
								$('#broch_form #timer').hide();
								document.getElementById("broch_form").reset();
                            window.location.href = "/samsaravillasa-brahma/thank-you?utm_source=" + utm_source;
                        } 
						else if(data.status === "401"){
							$("#broc_name").next().html('Please Enter Your Name!');
						}else if(data.status === "403"){
							$("#broc_email").next().html('Please enter proper Email ID!');
						}else if(data.status === "405"){
							$("#broc_mobile").next().html('Please Enter Your Mobile Number!');
						}
                    },

                    error: function (data) {
                        $("#broc_eqform").next().html('Some technical issue please try after some time'); // 
                    }
                });


});

$("#formSV_fot").click(function (e) {
    e.preventDefault();
    if (!$("#full_name_fot").val()) {
        $("#full_name_fot").next().html('Please Enter Your Name!');
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
                                        full_name: $("#full_name_fot").val(),
                                        last_name: "",
										mobile: $("#phone_number_fot").val(),
                                        email: $("#email_fot").val(),
                                        Budget: "",
                                        country_code: "",
                                        state_code: "",
                                        Projects_Interested__c: "RESIDENTIAL",
                                        PropertyLocation: "gurgaon",
                                        sale_type: "",
                                        Remarks: "",
                                        FormType: "SV Enquire form Footer",
										PageInfo: "Samsara Villasa",
                                        PageURL: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/SamsaraVilasaEnquiryNow",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
                            
								$('#success').modal('hide');
								$('#footer_form #otp-button').hide();
								$('#footer_form #timer').hide();
								document.getElementById("footer_form").reset();
                            window.location.href = "/samsaravillasa-brahma/thank-you?utm_source=" + utm_source;
                        } 
						else if(data.status === "401"){
							$("#full_name_fot").next().html('Please Enter Your Name!');
						}else if(data.status === "403"){
							$("#email_fot").next().html('Please enter proper Email ID!');
						}else if(data.status === "405"){
							$("#phone_number_fot").next().html('Please Enter Your Mobile Number!');
						}

                    },

                    error: function (data) {
                        $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
                    }
                });
});