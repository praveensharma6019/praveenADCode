


var recaptcha1;
var onloadCallback = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6LdkC64UAAAAALc0FqqrHPlTizoP8x8WT7VoH_sI', //Replace this with your Site key
        'theme': 'light'
    });


};

$("#vendorsubmitbtn").on('click', function () {

    var message = '';
    var error = false;
   var response = grecaptcha.getResponse(recaptcha1);
   if (response.length == 0) {
        message = message + "Captcha required";
       error = true;
    }
    $("#reResponse").val(response);

    var EmailId = $("#cmailid").val();
    if (EmailId != "") {
        var IsValidMail = validateEmail(EmailId);
        if (!IsValidMail) {
            message = message + "Invalid Email Address </br>";
            error = true;
        }
    }
    var name = $("#cname").val();
    if (name != "") {
        var IsValidName = validateName(name);
        if (!IsValidName) {
            message = message + "Invalid Name </br>";
            error = true;
        }
    }

     // var CompanyName = $("#cCompanyName").val();
     // if (CompanyName != "") {
         // var IsValidCompanyName = validateCompanyName(CompanyName);
         // if (!IsValidCompanyName) {
             // message = message + "Invalid Company Name </br>";
             // error = true;
         // }
     // }
  
     var Message = $("#cmessage").val();
    if (Message != "") {
         var IsValidMessage = validateMessage(Message);
        if (!IsValidMessage) {
            Message = Message + "Invalid Message </br>";
             error = true;
         }
   }
    function validateMessage(smessage) {
        var letterNumber = /[a-zA-Z0-9,. ]/;
        if (letterNumber.test(smessage)) { return true; }
        else { return false; }
    }



    if (error) {
        $("#docErrorMessage").html(message);
        $("#docErrorMessage").focus();
        return false;
    }
    else $("#docErrorMessage").html("");
    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    };
    function validateName(sname) {
        var regex = /^[a-zA-Z ]+$/;

        if (regex.test(sname)) { return true; }
        else { return false; }
    };
    function validateCompanyName(sname) {
        var regex = /^[A-Za-z0-9]+$/;

        if (regex.test(sname)) { return true; }
        else { return false; }
    };

});


$("#Inqbtnsubmit").on('click', function () {

    var message = '';
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        message = message + "Captcha required";
        error = true;
    }
    $("#reResponse").val(response);   
    if (error) {
        $("#docErrorMessage").html(message);
        $("#docErrorMessage").focus();
        return false;
    }
});

// window.onload = function(){
 // document.getElementById("vendorInqNo").value = "";
// }

$("#btnContactUsSubmit").click(function () {
   var  response  =  grecaptcha.getResponse(recaptcha1);
    if (response.length  ==  0) {
        alert("Captcha required.");
        return false;
    }

    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
    
    var mailid = $("#cmailid").val();
  
   
    var ccontactno = $("#ccontactno").val();
    

    

    var message = $("#cmessage").val();
   
     var messagetype = $("#cmessageType").val();
   

	
	var formtype = $("#cFormType").val();
    var pageinfo = window.location.href;
   



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
		reResponse:response,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Transmission/InsertContactFormdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }
			else if (data.status == "2"){
                alert("OOPS! You have missed Captcha Validation. Kindly validate to proceed further.");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
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
        url: "/api/Transmission/CreateOTP",
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
                        url: "/api/Transmission/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {


                            }

                            else {
                                alert("Invalid OTP");
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

