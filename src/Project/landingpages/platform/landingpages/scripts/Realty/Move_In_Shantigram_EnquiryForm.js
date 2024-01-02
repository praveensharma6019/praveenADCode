 $(document).ready(function () {
 // Read a page's GET URL variables and return them as an associative array.
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }

   var utm_source = getUrlVars()["utm_source"];
	if(utm_source!="" && utm_source!=null)
    $(".utm_source").val(window.location.href);
 });

$('#btnMoveInShantigramEnquirySubmit').click(function () {
    
    var firstname = $("#ContactForm #firstname").val();
    if (firstname == "") { alert("Please enter your Firstname"); $("#ContactForm #firstname").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var enuiryemail = $("#ContactForm #enuiry-email").val();
    if (enuiryemail == "") { alert("Email is Required"); $("#ContactForm #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
    if (!ValidateEmail(enuiryemail)) {
        alert("Email is not Valid"); $("#ContactForm #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;
    }
    function ValidateEmail(mail) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(mail);
    }

    var enuirymobile = $("#ContactForm #enuiry-mobile").val();
    if (enuirymobile == "") {
        alert("Please provide your Mobile Number");
        $("#ContactForm #enuiry-mobile").focus();
        $('#btnEnquirySubmit').removeAttr("disabled");
        return false;
    }
    else {
        if (document.ContactForm.CountryCode.value == '91' && document.ContactForm.mobile.value.length != 10) {
            alert("Mobile Number sould be 10 digit!");
            document.ContactForm.mobile.focus();
            $('#btnEnquirySubmit').removeAttr("disabled");
            return false;
        }
    }

    var country = $("#ContactForm #drpcountry").val();
    if (country == "") { alert("Please select your country"); $("#ContactForm #drpcountry").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    // if (!captchavarified) {
    // alert("please click on captcha field");
    // $('#btnenquirysubmit').removeattr("disabled");
    // return false;
    // }
    var locations = $("#ContactForm #Location").val();
    if (locations == "") { alert("Please select your location"); $("#ContactForm #Location").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var model = {
        mobile: enuirymobile,
        first_name: firstname,

        email: enuiryemail

    };
    $('#btnMoveInShantigramEnquirySubmit').attr("disabled", "disabled");
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Realty/Enquiry",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                var otp = prompt("Please enter OTP received on your mobile", "");

                if (otp != null) {

                    var generatedOtp = {
                        mobile: enuirymobile,
                        OTP: otp,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(generatedOtp),
                        url: "/api/Realty/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {
                                //create json object
                                var countryname = $('#ContactForm #drpcountry :selected').val();

                                var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                var formtype = $('#ContactForm #FormType').val();
                                var pageinfo = ($('#ContactForm #PageInfo').val());

                                var RecordTypes = ($('#ContactForm #RecordType').val());

                                var utmSource = $('#ContactForm #utm_source').val();
                                var leadSource = $('#ContactForm #lead_source').val();
                                var advertisementId = $('#ContactForm #advertisement_id').val();


                                var savecustomdata = {
                                    first_name: firstname,
                                    PropertyCode: "a4S9D000000Cbqo",
                                    mobile: enuirymobile,
                                    email: enuiryemail,
                                    PropertyLocation: locations,
                                    country_code: countryname,
                                    Projects_Interested__c: "Shantigram",
                                    FormType: formtype,
                                    PageInfo: pageinfo,
                                    FormSubmitOn: currentdate,
                                    UTMSource: utmSource,
                                    RecordType: RecordTypes,
                                    LeadSource: leadSource,
                                    AdvertisementId: advertisementId
                                };

                                //ajax calling to insert  custom data function
                                $.ajax({
                                    type: "POST",
                                    data: JSON.stringify(savecustomdata),
                                    url: "/api/Realty/InsertDetailMoveInShantigram",
                                    contentType: "application/json",
                                    success: function (data) {
                                        //////////////

                                        if (data.status == "1") {
                                            //var selectedproperty = $('#ContactForm #describe').val();
                                            var selectedproperty = $('#ContactForm #describe option:selected').text().trim();
                                            $('#ContactForm #retURL').val($('#ContactForm #retURL').val());
                                            //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                            window.location.href = $('#ContactForm #retURL').val();
                                            //$('#ContactForm').submit();
                                        }
                                        else {
                                            alert("Sorry Operation Failed!!! Please try again later");
                                            $('#btnEnquirySubmit').removeAttr("disabled");
                                            return false;
                                        }
                                    }
                                });
                            }
                            else {
                                alert("Invalid OTP");
                                $('#btnMoveInShantigramEnquirySubmit').removeAttr("disabled");
                                return false;
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(textStatus, errorThrown);
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnMoveInShantigramEnquirySubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

$('#btnInstantCallbackSubmit').click(function () {
    
    var firstname = $("#InstantCallback #firstname").val();
    if (firstname == "") { alert("Please enter your Firstname"); $("#InstantCallback #firstname").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var enuiryemail = $("#InstantCallback #enuiry-email").val();
    if (enuiryemail == "") { alert("Email is Required"); $("#InstantCallback #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
    if (!ValidateEmail(enuiryemail)) {
        alert("Email is not Valid"); $("#InstantCallback #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;
    }
    function ValidateEmail(mail) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(mail);
    }

    var enuirymobile = $("#InstantCallback #enuiry-mobile").val();
    if (enuirymobile == "") {
        alert("Please provide your Mobile Number");
        $("#InstantCallback #enuiry-mobile").focus();
        $('#btnEnquirySubmit').removeAttr("disabled");
        return false;
    }
    else {
        if (document.InstantCallback.CountryCode.value == '91' && document.InstantCallback.mobile.value.length != 10) {
            alert("Mobile Number sould be 10 digit!");
            document.InstantCallback.mobile.focus();
            $('#btnEnquirySubmit').removeAttr("disabled");
            return false;
        }
    }

    var country = $("#InstantCallback #drpcountry").val();
    if (country == "") { alert("Please select your country"); $("#InstantCallback #drpcountry").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    // if (!captchavarified) {
    // alert("please click on captcha field");
    // $('#btnenquirysubmit').removeattr("disabled");
    // return false;
    // }
    var locations = $("#InstantCallback #Location").val();
    if (locations == "") { alert("Please select your location"); $("#InstantCallback #Location").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var model = {
        mobile: enuirymobile,
        first_name: firstname,

        email: enuiryemail

    };
    $('#btnInstantCallbackSubmit').attr("disabled", "disabled");
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Realty/Enquiry",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                var otp = prompt("Please enter OTP received on your mobile", "");

                if (otp != null) {

                    var generatedOtp = {
                        mobile: enuirymobile,
                        OTP: otp,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(generatedOtp),
                        url: "/api/Realty/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {
                                //create json object
                                var countryname = $('#InstantCallback #drpcountry :selected').val();

                                var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                var formtype = $('#InstantCallback #FormType').val();
                                var pageinfo = ($('#InstantCallback #PageInfo').val());

                                var RecordTypes = ($('#InstantCallback #RecordType').val());

                                var utmSource = $('#InstantCallback #utm_source').val();
                                var leadSource = $('#InstantCallback #lead_source').val();
                                var advertisementId = $('#InstantCallback #advertisement_id').val();


                                var savecustomdata = {
                                    first_name: firstname,
                                    PropertyCode: "a4S9D000000Cbqo",
                                    mobile: enuirymobile,
                                    email: enuiryemail,
                                    PropertyLocation: locations,
                                    country_code: countryname,
                                    Projects_Interested__c: "Shantigram",
                                    FormType: formtype,
                                    PageInfo: pageinfo,
                                    FormSubmitOn: currentdate,
                                    UTMSource: utmSource,
                                    RecordType: RecordTypes,
                                    LeadSource: leadSource,
                                    AdvertisementId: advertisementId
                                };


                                //ajax calling to insert  custom data function
                                $.ajax({
                                    type: "POST",
                                    data: JSON.stringify(savecustomdata),
                                    url: "/api/Realty/InsertDetailMoveInShantigram",
                                    contentType: "application/json",
                                    success: function (data) {
                                        //////////////

                                        if (data.status == "1") {
                                            //var selectedproperty = $('#InstantCallback #describe').val();
                                            var selectedproperty = $('#InstantCallback #describe option:selected').text().trim();
                                            $('#InstantCallback #retURL').val($('#InstantCallback #retURL').val());
                                            //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                            window.location.href = $('#InstantCallback #retURL').val();
                                            //$('#InstantCallback').submit();
                                        }
                                        else {
                                            alert("Sorry Operation Failed!!! Please try again later");
                                            $('#btnInstantCallbackSubmit').removeAttr("disabled");
                                            return false;
                                        }
                                    }
                                });
                            }
                            else {
                                alert("Invalid OTP");
                                $('#btnInstantCallbackSubmit').removeAttr("disabled");
                                return false;
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(textStatus, errorThrown);
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnInstantCallbackSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

$('#btnPopupFormSubmit').click(function () {
   
    var firstname = $("#PopupForm #firstname").val();
    if (firstname == "") { alert("Please enter your Firstname"); $("#PopupForm #firstname").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var enuiryemail = $("#PopupForm #enuiry-email").val();
    if (enuiryemail == "") { alert("Email is Required"); $("#PopupForm #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
    if (!ValidateEmail(enuiryemail)) {
        alert("Email is not Valid"); $("#PopupForm #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;
    }
    function ValidateEmail(mail) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(mail);
    }

    var enuirymobile = $("#PopupForm #enuiry-mobile").val();
    if (enuirymobile == "") {
        alert("Please provide your Mobile Number");
        $("#PopupForm #enuiry-mobile").focus();
        $('#btnEnquirySubmit').removeAttr("disabled");
        return false;
    }
    else {
        if (document.PopupForm.CountryCode.value == '91' && document.PopupForm.mobile.value.length != 10) {
            alert("Mobile Number sould be 10 digit!");
            document.PopupForm.mobile.focus();
            $('#btnEnquirySubmit').removeAttr("disabled");
            return false;
        }
    }

    var country = $("#PopupForm #drpcountry").val();
    if (country == "") { alert("Please select your country"); $("#PopupForm #drpcountry").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    // if (!captchavarified) {
    // alert("please click on captcha field");
    // $('#btnenquirysubmit').removeattr("disabled");
    // return false;
    // }
    var locations = $("#PopupForm #Location").val();
    if (locations == "") { alert("Please select your location"); $("#PopupForm #Location").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var model = {
        mobile: enuirymobile,
        first_name: firstname,

        email: enuiryemail

    };
    $('#btnPopupFormSubmit').attr("disabled", "disabled");
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Realty/Enquiry",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                var otp = prompt("Please enter OTP received on your mobile", "");

                if (otp != null) {

                    var generatedOtp = {
                        mobile: enuirymobile,
                        OTP: otp,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(generatedOtp),
                        url: "/api/Realty/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {
                                //create json object
                                var countryname = $('#PopupForm #drpcountry :selected').val();

                                var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                var formtype = $('#PopupForm #FormType').val();
                                var pageinfo = ($('#PopupForm #PageInfo').val());

                                var RecordTypes = ($('#PopupForm #RecordType').val());

                                var utmSource = $('#PopupForm #utm_source').val();

                                var leadSource = $('#PopupForm #lead_source').val();
                                var advertisementId = $('#PopupForm #advertisement_id').val();

                                
                                var savecustomdata = {
                                    first_name: firstname,
                                    PropertyCode: "a4S9D000000Cbqo",
                                    mobile: enuirymobile,
                                    email: enuiryemail,
                                    PropertyLocation: locations,
                                    country_code: countryname,
                                    Projects_Interested__c: "Shantigram",
                                    FormType: formtype,
                                    PageInfo: pageinfo,
                                    FormSubmitOn: currentdate,
                                    UTMSource: utmSource,
                                    RecordType: RecordTypes,
                                    LeadSource: leadSource,
                                    AdvertisementId: advertisementId
                                };

                                //ajax calling to insert  custom data function
                                $.ajax({
                                    type: "POST",
                                    data: JSON.stringify(savecustomdata),
                                    url: "/api/Realty/InsertDetailMoveInShantigram",
                                    contentType: "application/json",
                                    success: function (data) {
                                        //////////////

                                        if (data.status == "1") {
                                            //var selectedproperty = $('#PopupForm #describe').val();
                                            var selectedproperty = $('#PopupForm #describe option:selected').text().trim();
                                            $('#PopupForm #retURL').val($('#PopupForm #retURL').val());
                                            //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                            window.location.href = $('#PopupForm #retURL').val();
                                            //$('#PopupForm').submit();
                                        }
                                        else {
                                            alert("Sorry Operation Failed!!! Please try again later");
                                            $('#btnPopupFormSubmit').removeAttr("disabled");
                                            return false;
                                        }
                                    }
                                });
                            }
                            else {
                                alert("Invalid OTP");
                                $('#btnPopupFormSubmit').removeAttr("disabled");
                                return false;
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(textStatus, errorThrown);
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnPopupFormSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

$('#btnInquiryFormSubmit').click(function () {
    
    var firstname = $("#inquiryForm #firstname").val();
    if (firstname == "") { alert("Please enter your Firstname"); $("#inquiryForm #firstname").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var enuiryemail = $("#inquiryForm #enuiry-email").val();
    if (enuiryemail == "") { alert("Email is Required"); $("#inquiryForm #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
    if (!ValidateEmail(enuiryemail)) {
        alert("Email is not Valid"); $("#inquiryForm #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;
    }
    function ValidateEmail(mail) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(mail);
    }

    var enuirymobile = $("#inquiryForm #enuiry-mobile").val();
    if (enuirymobile == "") {
        alert("Please provide your Mobile Number");
        $("#inquiryForm #enuiry-mobile").focus();
        $('#btnEnquirySubmit').removeAttr("disabled");
        return false;
    }
    else {
        if (document.inquiryForm.CountryCode.value == '91' && document.inquiryForm.mobile.value.length != 10) {
            alert("Mobile Number sould be 10 digit!");
            document.inquiryForm.mobile.focus();
            $('#btnEnquirySubmit').removeAttr("disabled");
            return false;
        }
    }

    var country = $("#inquiryForm #drpcountry").val();
    if (country == "") { alert("Please select your country"); $("#inquiryForm #drpcountry").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    // if (!captchavarified) {
    // alert("please click on captcha field");
    // $('#btnenquirysubmit').removeattr("disabled");
    // return false;
    // }
    var locations = $("#inquiryForm #Location").val();
    if (locations == "") { alert("Please select your location"); $("#inquiryForm #Location").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }

    var model = {
        mobile: enuirymobile,
        first_name: firstname,

        email: enuiryemail

    };
    $('#btnInquiryFormSubmit').attr("disabled", "disabled");
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Realty/Enquiry",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                var otp = prompt("Please enter OTP received on your mobile", "");

                if (otp != null) {

                    var generatedOtp = {
                        mobile: enuirymobile,
                        OTP: otp,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(generatedOtp),
                        url: "/api/Realty/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {
                                //create json object
                                var countryname = $('#inquiryForm #drpcountry :selected').val();

                                var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                var formtype = $('#inquiryForm #FormType').val();
                                var pageinfo = ($('#inquiryForm #PageInfo').val());

                                var RecordTypes = ($('#inquiryForm #RecordType').val());

                                var utmSource = $('#inquiryForm #utm_source').val();

                                var leadSource = $('#inquiryForm #lead_source').val();
                                var advertisementId = $('#inquiryForm #advertisement_id').val();


                                var savecustomdata = {
                                    first_name: firstname,
                                    PropertyCode: "a4S9D000000Cbqo",
                                    mobile: enuirymobile,
                                    email: enuiryemail,
                                    PropertyLocation: locations,
                                    country_code: countryname,
                                    Projects_Interested__c: "Shantigram",
                                    FormType: formtype,
                                    PageInfo: pageinfo,
                                    FormSubmitOn: currentdate,
                                    UTMSource: utmSource,
                                    RecordType: RecordTypes,
                                    LeadSource: leadSource,
                                    AdvertisementId: advertisementId
                                };

                                //ajax calling to insert  custom data function
                                $.ajax({
                                    type: "POST",
                                    data: JSON.stringify(savecustomdata),
                                    url: "/api/Realty/InsertDetailMoveInShantigram",
                                    contentType: "application/json",
                                    success: function (data) {
                                        //////////////

                                        if (data.status == "1") {
                                            //var selectedproperty = $('#inquiryForm #describe').val();
                                            var selectedproperty = $('#inquiryForm #describe option:selected').text().trim();
                                            $('#inquiryForm #retURL').val($('#inquiryForm #retURL').val());
                                            //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                            window.location.href = $('#inquiryForm #retURL').val();
                                            //$('#inquiryForm').submit();
                                        }
                                        else {
                                            alert("Sorry Operation Failed!!! Please try again later");
                                            $('#btnInquiryFormSubmit').removeAttr("disabled");
                                            return false;
                                        }
                                    }
                                });
                            }
                            else {
                                alert("Invalid OTP");
                                $('#btnInquiryFormSubmit').removeAttr("disabled");
                                return false;
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(textStatus, errorThrown);
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnInquiryFormSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});



