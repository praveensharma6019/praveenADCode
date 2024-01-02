//UAT Script for ZeroEmi
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
var isincludedquerystring;

var globalurl = window.location.href;
	if(globalurl.includes('?'))
	{
		function GetURLParameter(sParam) {
			var url_string = window.location.href;
			var url = new URL(url_string);
			var c = url.searchParams.get(sParam);
			// alert(c);
			return c
			
		}
		$("#enq_project").change(function (e){
		var val=$('#enq_project').val();
		if(val=="Archway")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = GetURLParameter('AdvertisementId');
		Globalutm_source = GetURLParameter('utm_source');
		Globalutm_placement = GetURLParameter('utm_placement');
		}
		else if(val=="Atrius")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = GetURLParameter('AdvertisementId');
		Globalutm_source = GetURLParameter('utm_source');
		Globalutm_placement = GetURLParameter('utm_placement');
		}
		isincludedquerystring = true;
		
	});
	}
	else{
		$("#enq_project").change(function (e){
		var val=$('#enq_project').val();
		if(val=="Archway")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = "a3t9D0000009y3w";
		Globalutm_source = "Landing_Page_Organic";
		Globalutm_placement = "";
		}
		else if(val=="Atrius")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = "a3t9D0000009y3w";
		Globalutm_source = "Landing_Page_Organic";
		Globalutm_placement = "";
		}
		isincludedquerystring = false;
	});
	}
	
	//Download Brochure
	var GlobalmasterIddown;
	var GlobalAdvIddown;
	var GlobalUTMSubSourcedown;
	if(globalurl.includes('?'))
	{
		function GetURLParameter(sParam) {
			var url_string = window.location.href;
			var url = new URL(url_string);
			var c = url.searchParams.get(sParam);
			// alert(c);
			return c
			
		}
	$("#full_project").change(function (e){
		var val=$('#full_project').val();
		if(val=="Archway")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = GetURLParameter('AdvertisementId');
		Globalutm_source = GetURLParameter('utm_source');
		Globalutm_placement = GetURLParameter('utm_placement');
		}
		else if(val=="Atrius")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = GetURLParameter('AdvertisementId');
		Globalutm_source = GetURLParameter('utm_source');
		Globalutm_placement = GetURLParameter('utm_placement');
		}
		isincludedquerystring = true;
	});
	}
	else
	{
		$("#full_project").change(function (e){
		var val=$('#full_project').val();
		if(val=="Archway")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = "a3t9D0000009y3w";
		Globalutm_source = "Landing_Page_Organic";
		Globalutm_placement = "";
		}
		else if(val=="Atrius")
		{
		GlobalmasterId = "a4S9D000000Cgci";
		GlobalAdvId = "a3t9D0000009y3w";
		Globalutm_source = "Landing_Page_Organic";
		Globalutm_placement = "";
		}
		isincludedquerystring = false;
	});
		
	}

//Footer Form
$("#enq_submit").click(function (e) {
getCaptchaResponseForm();
    e.preventDefault();

});

//Footer Form
$("#form_brochure").click(function (e) {
getCaptchaResponseDownload();
    e.preventDefault();

});


function getCaptchaResponseForm(e) {
        grecaptcha.ready(function() {
          grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', {action: 'ZeroEmiEnquiryNow'}).then(function(token) {
			  $('.g-recaptcha').val(token);

      
    if (!$("#enq_name").val()) {
        $("#enq_name").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#enq_name").next().html("");
    }

   if (!document.getElementById("enq_project").value) {
		$("#enq_project").next().html('Please Select Project!');
        return false;
    } else {
        $("#enq_project").next().html("");
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
    
    if (!$("#html").is(":checked")) {
		$("#html").next().next().html('Please Check the Terms!');
        return false;
    } else {
        $("#html").next().next().html("");
    }
			  document.getElementById("enq_submit").innerHTML =
				  '<div class="loader-wrapper"><span></span><span></span><span></span></div>';

    // var projectname= $('#enq_form #enq_project option:selected').text().trim().split(",")[0];
    
    var savecustomdata = {
                                        full_name: $("#enq_name").val(),
                                        mobile: $("#enq_phone").val(),
                                        email: $("#enq_email").val(),
                                        projectname: $("#enq_project").val().toUpperCase(),
                                        propertytype: "RESIDENTIAL",
                                        PropertyLocation: "Ahmedabad",
                                        Remarks: "",
                                        FormType: "Enquire Form",
                                        PageInfo: "ZeroEmi",
                                        PageURL: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: Globalutm_source,
										UTMPlacement: Globalutm_placement,
                                        RecordType: "0129D000000Ajol",
                                        PropertyCode: GlobalmasterId,
                                        AdvertisementId: GlobalAdvId,
										isincludedquerystring : isincludedquerystring,
										reResponse:$(".g-recaptcha").val()
										
                                    };
	
	


			  $.ajax({
				 
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/ZeroEmiEnquiryNow",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
							
							$(this).prop('disabled', true);
							window.location.href = "/ZeroEMIOffer/thankyou" ;
							document.getElementById("enq_form").reset();
							
                        }
						
						else if(data.status === "401"){
							$("#enq_name").next().html('Please Enter Your Name!');
						}else if(data.status === "403"){
							$("#enq_email").next().html('Please enter proper Email ID!');
						}else if(data.status === "405"){
							$("#enq_phone").next().html('Please Enter Your Mobile Number!');
						}
						else if(data.status === "404"){
							$("#enq_project").next().html('Please Select Project!');
						}

                    },

                    error: function (data) {
                        $("#enq_form").next().html('Some technical issue please try after some time'); 
                    }
                });
	});
});
}


/*Brochure Form Start*/
function getCaptchaResponseDownload(e) {
        grecaptcha.ready(function() {
          grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', {action: 'ZeroEmiEnquiryNow'}).then(function(token) {
			  $('.g-recaptcha').val(token);

      
    if (!$("#full_name").val()) {
        $("#full_name").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#full_name").next().html("");
    }

   if (!document.getElementById("full_project").value) {
		$("#full_project").next().html('Please Select Project!');
        return false;
    } else {
        $("#full_project").next().html("");
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
    
    if (!$("#html").is(":checked")) {
		$("#html").next().next().html('Please Check the Terms!');
        return false;
    } else {
        $("#html").next().next().html("");
    }

			  document.getElementById("form_brochure").innerHTML =
				  '<div class="loader-wrapper"><span></span><span></span><span></span></div>';

    var savecustomdata = {
                                        full_name: $("#full_name").val(),
                                        mobile: $("#full_phone").val(),
                                        email: $("#full_email").val(),
                                        projectname: $("#full_project").val().toUpperCase(),
                                        propertytype: "RESIDENTIAL",
                                        PropertyLocation: "Ahmedabad",
                                        Remarks: "",
                                        FormType: "Download Brochure",
                                        PageInfo: "ZeroEmi",
                                        PageURL: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: Globalutm_source,
										UTMPlacement: Globalutm_placement,
                                        RecordType: "0129D000000Ajol",
                                        PropertyCode: GlobalmasterId,
                                        AdvertisementId: GlobalAdvId,
										isincludedquerystring : isincludedquerystring,
										reResponse:$(".g-recaptcha").val()
										
                                    };

		$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/ZeroEmiEnquiryNow",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
						var val = $('#full_project option:selected').val();
						var userAgent = navigator.userAgent || navigator.vendor || window.opera;

							if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i) || userAgent.match(/iPod/i)) {
								// alert('IOS Device');
								if (val == 'Archway') {

									var link = document.createElement('a');
									link.href = "-/media-archive/Archway Brochure.pdf";
									link.download = "Archway Brochure.pdf";
									setTimeout(function () {
										link.click();
										link.remove();
									}, 500);
								}

								if (val == 'Atrius') {

									var link = document.createElement('a');
									link.href = "-/media-archive/Atrius Brochure.pdf";
									link.download = "Atrius Brochure.pdf";
									setTimeout(function () {
										link.click();
										link.remove();
									}, 500);
								}
							}
							else if (userAgent.toString().includes("Mac OS")) {
								if (val == 'Archway') {

									var link = document.createElement('a');
									link.href = "-/media-archive/Archway Brochure.pdf";
									link.download = "Archway Brochure.pdf";
									setTimeout(function () {
										link.click();
										link.remove();
									}, 50);
									$(this).prop('disabled', true);
									document.getElementById("broc_form").reset();
									setTimeout(function () { document.location.href = "/ZeroEMIOffer/thankyou"; }, 250);
								}

								if (val == 'Atrius') {

									var link = document.createElement('a');
									link.href = "-/media-archive/Atrius Brochure.pdf";
									link.download = "Atrius Brochure.pdf";
									setTimeout(function () {
										link.click();
										link.remove();
									}, 50);
									$(this).prop('disabled', true);
									document.getElementById("broc_form").reset();
									setTimeout(function () { document.location.href = "/ZeroEMIOffer/thankyou"; }, 250);
								}
							}
						// Desktop Version Start
						else{
							
							if(val=='Archway')
							{
							// alert("Working in IOS");
							 var link = document.createElement('a');
								link.href = "-/media-archive/Archway Brochure.pdf";
								link.download = "Archway Brochure.pdf";
								link.click();
								link.remove();
							}
							if(val=='Atrius')
							{		
								 var link = document.createElement('a');
								link.href = "-/media-archive/Atrius Brochure.pdf";
								link.download = "Atrius Brochure.pdf";
								link.click();
								link.remove();
							}
								$(this).prop('disabled', true);
						document.getElementById("broc_form").reset();
								window.location.href = "/ZeroEMIOffer/thankyou" ;
						}
							
                        }
						else if(data.status === "401"){
							$("#full_name").next().html('Please Enter Your Name!');
						}else if(data.status === "403"){
							$("#full_email").next().html('Please enter proper Email ID!');
						}else if(data.status === "405"){
							$("#full_phone").next().html('Please Enter Your Mobile Number!');
						}
						else if(data.status === "404"){
							$("#full_project").next().html('Please Select Project!');
						}
                    },

                    error: function (data) {
                        $("#broc_form").next().html('Some technical issue please try after some time'); 
                    }
                });
	});
});
}

