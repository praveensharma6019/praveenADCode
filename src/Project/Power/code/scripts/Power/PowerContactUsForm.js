
var recaptchav3;



var onloadCallback1 = function () {

    recaptcha3 = grecaptcha.render('recaptcha3', {
        'sitekey': '6Ld0fbAUAAAAAEqyKQ0mTbEpPWZY-czpfI0d66P5', //Replace this with your Site key
        'theme': 'light'
    });

};


$("#btnContactUsSubmit1").click(function () {
    var response = grecaptcha.getResponse(recaptcha3);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }

    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname1").val();
    if (name == "") { alert("Please enter your Name"); $("#cname1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid1").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }

    var ccontactno = $("#ccontactno1").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }

    if (ccontactno.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#ccontactno1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false;

    }

    var message = $("#cmessage1").val();
    if (message == "") { alert("Please enter any message"); $("#cmessage1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }
    var messagetype = $("#cmessageType1").val();
    if (messagetype == "") { alert("Please specify your subject to the message"); $("#cmessageType1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }


    var formtype = $("#cFormType1").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid1").focus(); $('#btnContactUsSubmit1').removeAttr("disabled"); return false; }



    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    var model = {

        Name: name,
        Mobile: ccontactno,
        Email: mailid

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        Name: name,
        Email: mailid,
        Mobile: ccontactno,
        Message: message,
        MessageType: messagetype,
        reResponse: response,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Power/InsertContactFormdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }
            else if (data.status == "2") {
                alert("Please validate Captcha before submitting");
                $('#btnContactUsSubmit1').removeAttr("disabled");
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
        url: "/api/Power/CreateOTP",
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
                        url: "/api/Power/VerifyOTP",
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


