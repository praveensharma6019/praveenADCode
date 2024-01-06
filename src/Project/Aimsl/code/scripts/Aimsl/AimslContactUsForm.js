





$("#btnContactUsSubmit").click(function () {
   //var response = grecaptcha.getResponse(recaptcha1);
 

    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
 
    var mailid = $("#cmailid").val();
   

    var ccontactno = $("#ccontactno").val();
    

    var message = $("#cmessage").val();
   
    var messagetype = $("#cmessageType").val();
  


    var formtype = $("#cFormType").val();
    var pageinfo = window.location.href;
  
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
        //reResponse: response,
        MessageType: messagetype,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Aimsl/InsertContact",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {s
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }
            else if (data.status == "4") {
                alert("Please provide proper input values.");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;


    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Aimsl/CreateOTP",
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
                        url: "/api/Wilmar/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {
                                window.location.href = "/thankyou";
                                //$('#contact_form1').submit();
                            }
                            else if (data.status == "2") {
                                alert("OOPS! You have missed Captcha Validation. Kindly validate to proceed further.");
                                $('#btnContactUsSubmit').removeAttr("disabled");
                                return false;
                            }
                            else {
                                alert("Sorry Operation Failed!!! Please try again later");
                                $('#btnContactUsSubmit').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnContactUsSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

