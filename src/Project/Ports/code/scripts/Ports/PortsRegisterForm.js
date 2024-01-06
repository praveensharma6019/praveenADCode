var recaptcha3;
var registerCallBack = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha3 = grecaptcha.render('recaptcha3', {
        'sitekey': '6LcAkasUAAAAAGmIZTUyEhNafee2adaz0BpZyrOw', //Replace this with your Site key
        'theme': 'light'
    });


};



$("#btnRegistrationFormSubmit").click(function () {
    var  response  =  grecaptcha.getResponse(recaptcha3);
    if (response.length  ==  0) {
        alert("Captcha required.");
        return false;
    }

    $('#btnRegistrationFormSubmit').attr("disabled", "disabled");

    var rName = $("#rName").val();
    if (rName == "") { alert("Please enter your Name"); $("#rName").focus(); $('#btnRegistrationFormSubmit').removeAttr("disabled"); return false; }

    var rCompany = $("#rCompany").val();
    if (rCompany == "") { alert("Please enter Company Name"); $("#rCompany").focus(); $('#btnRegistrationFormSubmit').removeAttr("disabled"); return false; }

    var rNumber = $("#rNumber").val();
    if (rNumber == "") { alert("Please enter your Contact Number"); $("#rNumber").focus(); $('#btnRegistrationFormSubmit').removeAttr("disabled"); return false; }
    if (rNumber.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#rNumber").focus(); $('#btnRegistrationFormSubmit').removeAttr("disabled"); return false;

    }

    var requestorEmail = $("#requestorEmail").val();
    if (requestorEmail == "") { alert("Please enter your Email"); $("#requestorEmail").focus(); $('#btnRegistrationFormSubmit').removeAttr("disabled"); return false; }
    if (!validateEmail(requestorEmail)) { alert("Please enter valid email address"); $("#requestorEmail").focus(); $('#btnRegistrationFormSubmit').removeAttr("disabled"); return false; }

    var mobile = "1010101010";
    var formtype = $("#rFormType").val();

    var pageinfo = window.location.href;
   
    function validateEmail(email) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(email)) { return true; }
        else { return false; }
    }

    

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');


    var model = {

        Name: rName,
        Mobile: rNumber
       

    };


    //create json object
    var savecustomdata = {


        Name: rName,
        Organization: rCompany,
        ContactNumber: rNumber,
        Email: requestorEmail,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/ports/InsertRegistrationdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "https://www.adaniports.com/thankyou";
                //$('#contact_form1').submit();
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnRegistrationFormSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;


    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Ports/CreateOTP",
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
                        url: "/api/Ports/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {


                            }

                            else {
                                alert("Invalid OTP");
                                $('#btnRegistrationFormSubmit').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnRegistrationFormSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

