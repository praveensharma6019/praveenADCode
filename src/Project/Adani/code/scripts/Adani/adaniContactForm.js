$(document).ready(function () {
    if (window.location.href.indexOf("?title") > -1) {
        var title = GetParameterValues('title');
        window.location = "/Newsroom/Media-Release/Media-Statement";
        //return true;
    }


    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }
});

var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}

var recaptcha3;

var onloadCallback1 = function () {
    //Render the recaptcha1 on the element with ID "recaptcha1"
    recaptcha3 = grecaptcha.render('recaptcha3', {
        'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
        'theme': 'light'
    });
};

$("#btnContactUsSubmit1").click(function () {
    var response = grecaptcha.getResponse(recaptcha3);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    $('#btnContactUsSubmit1').attr("disabled", "disabled");
    var name = $("#cname1").val();
    if (name == "") { alert("Please enter your Name"); $("#cname1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid1").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    var ccontactno = $("#ccontactno1").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    if ($("#ccontactno1").val().length !== 10 || regex_special_num.test($("#ccontactno1").val()) == false) { alert("Please enter 10 digit Mobile Number!"); $("#ccontactno1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    var message = $("#cmessage1").val();
    if (message == "") { alert("Please enter any message"); $("#cmessage1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    var cmessageType = $("#cmessageType1").val();
    if (cmessageType == "") { alert("Please select valid subject"); $("#cmessageType1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }

    var formtype = $("#cFormType1").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    var mobile = "1010101010";


    function validateEmail(mailid) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(mailid)) { return true; }
        else { return false; }
    }

    var model = {

        Name: name,
        Mobile: mobile,
        Email: mailid

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {

        Name: name,
        Email: mailid,
        SubjectType: cmessageType,
        Mobile: ccontactno,
        Message: message,
        reResponse: response,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Adani/InsertContactdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                document.getElementById("contact_form").reset();
                //$('#contact_form1').submit();
            }
            else if (data.status == "3") {
                alert("Captcha Failed!!! Please try again");
                return false;
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnContactUsSubmit1').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;


    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Adani/CreateOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                var otp = prompt("Please enter OTP received on your mobile", "");

                if (otp != null) {

                    var generatedOtp = {
                        mobile: mobile,
                        OTP: otp,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(generatedOtp),
                        url: "/api/Adani/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {


                            }

                            else {
                                alert("Invalid OTP");
                                $('#btnContactUsSubmit1').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnContactUsSubmit1').removeAttr("disabled");
            }
        }
    });

    return false;
});

