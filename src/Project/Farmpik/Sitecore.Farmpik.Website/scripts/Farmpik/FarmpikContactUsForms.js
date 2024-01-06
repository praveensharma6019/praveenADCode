$("#btnContactUsSubmit").click(function (e) {
    getCaptchaResponseForm();
    e.preventDefault();

});
function getCaptchaResponseForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6LdmYcUgAAAAACDRSFWl8yk7VeQCgJs29I93eYaG', { action: 'InsertContact' }).then(function (token) {
            $('.g-recaptcha').val(token);
            $('#btnContactUsSubmit').attr("disabled", "disabled");
            var name = $("#cname").val();
            if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var mailid = $("#cmailid").val();
            if (mailid == "") { alert("Email is btnContactUsSubmit"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var message = $("#cmessage").val();
            if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var messagetype = $("#cmessageType").val();
            if (messagetype == "") { alert("Please specify your subject to the message"); $("#cmessageType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            var formtype = $("#cFormType").val();
            var pageinfo = window.location.href;
            if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
            function validateEmail(sEmail) {
                var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
                if (filter.test(sEmail)) { return true; }
                else { return false; }
            }
            var model =
            {
                Name: name,
                Mobile: "",
                Email: mailid
            };
            var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

            //create json object
            var savecustomdata = {
                Name: name,
                Email: mailid,
                Mobile: "",
                Message: message,
                reResponse: $('.g-recaptcha').val(),
                MessageType: messagetype,
                FormType: formtype,
                PageInfo: pageinfo,
                FormSubmitOn: currentdate
            };

            //ajax calling to insert  custom data function
            $.ajax({
                type: "POST",
                dataType: "json",
                headers: { "__RequestVerificationToken": $(".antiforgerytoken").find("input").val() },
                Cookies: { "key": "__RequestVerificationToken", "value": $(".antiforgerytoken").find("input").val() },
                data: JSON.stringify(savecustomdata),
                url: "/api/Farmpik/InsertContact",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == "1") {
                        $('#contact_form')[0].reset()
                        window.location.href = "/thankyou";
                    }
                    else if (data.status == "2") {
                        alert("captcha failed");
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
                url: "/api/Farmpik/CreateOTP",
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
                                url: "/api/Farmpik/VerifyOTP",
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

        });
    });
}