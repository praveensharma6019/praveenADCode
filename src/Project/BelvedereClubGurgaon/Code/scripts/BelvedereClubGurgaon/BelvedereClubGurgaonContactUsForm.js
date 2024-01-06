$(document).ready(function () {

    var SelectedRadioVal = "";


    $('#btnContactUsSubmit').click(function () {



        $('#btnContactUsSubmit').attr("disabled", "disabled");
        var name = $("#cname").val();
        if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var mailid = $("#cmailid").val();
        if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var interestedIn = $("#cinterestedIn").val();
        if (interestedIn == "") { alert("Please specify your interest in field"); $("#cinterestedIn").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var city = $("#ccity").val();
        if (city == "") { alert("Please enter your city"); $("#ccity").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var message = $("#cmessage").val();
        if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
       var formtype  = $("#cFormType").val();

        if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; } 

        var mobile = $("#cmobile").val();
        if (mobile == "") {
            alert("Please provide your Mobile Number");
            $("#cmobile").focus();
            $('#btnContactUsSubmit').removeAttr("disabled");
            return false;
        }
        else {
            if (document.contact_form1.cmobile.value.length != 10) {
                alert("Mobile Number sould be 10 digit!");
                document.contact_form1.cmobile.focus();
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
        }
        function validateEmail(sEmail) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (filter.test(sEmail)) { return true; }
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
            InterestedIn: interestedIn,
            City: city,
            Mobile: mobile,
            Message: message,
            FormType: formtype,
            FormSubmitOn: currentdate
        };

        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/BelvedereClubGurgaon/InsertEnquiryNowdetail",
            contentType: "application/json",
            success: function (data) {
                //////////////

                if (data.status == "1") {
                    window.location.href = "/ThankYou";
                    //$('#contact_form1').submit();
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
                url: "/api/BelvedereClubGurgaon/CreateOTP",
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
                                url: "/api/BelvedereClubGurgaon/VerifyOTP",
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


