var recaptcha3;
var recaptcha31;

//var registerCallBack = function () {


//    //Render the recaptcha2 on the element with ID "recaptcha2"
//    recaptcha3 = grecaptcha.render('recaptcha3', {
//        'sitekey': '6LcAkasUAAAAAGmIZTUyEhNafee2adaz0BpZyrOw', //Replace this with your Site key
//        'theme': 'light'
//});};


//var registerCallBack1 = function () {
//    //Render the recaptcha2 on the element with ID "recaptcha2"
//    recaptcha31 = grecaptcha.render('recaptcha31', {
//        'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc', //Replace this with your Site key
//        'theme': 'light'
//    });


//};

$("#btnContactUsSubmit").click(function (e) {
    getCaptchaResponseContactForm();
    e.preventDefault();
});
	
function getCaptchaResponseContactForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'InsertContactdetail' }).then(function (token) {
            $('.g-recaptcha').val(token);
            $('#btnContactUsSubmit').attr("disabled", "disabled");
            var name = $("#cname").val();
            if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var mailid = $("#cmailid").val();
            if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var messagetype = $("#cmessageType").val();
            if (messagetype == "") { alert("Please specify your subject to the message"); $("#cmessageType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var ccontactno = $("#ccontactno").val();
            if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

            if (ccontactno.length != 10) {
                alert("Contact Number should be of 10 digit"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false;
            }

            var message = $("#cmessage").val();
            if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var formtype = $("#cFormType").val();
            var pageinfo = window.location.href;
            if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

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
                MessageType: messagetype,
                Mobile: ccontactno,
                Message: message,
                FormType: formtype,
                PageInfo: pageinfo,
                FormSubmitOn: currentdate,
                reResponse: token
            };

            //ajax calling to insert  custom data function
            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/ports/InsertContactdetail",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == "1") {
                        window.location.href = "/thankyou";
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
    });
}
