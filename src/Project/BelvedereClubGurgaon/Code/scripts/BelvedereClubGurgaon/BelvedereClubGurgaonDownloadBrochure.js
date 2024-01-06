

$(document).ready(function () {

   


    $('#btnDownloadBrochureSubmit').click(function () {



        $('#btnDownloadBrochureSubmit').attr("disabled", "disabled");
        var formtype = $("#DBFormType").val();

        var name = $("#DBname").val();
        if (name == "") { alert("Please enter your Name"); $("#DBname").focus(); $('#btnDownloadBrochureSubmit').removeAttr("disabled"); return false; }
        var mailid = $("#DBmailid").val();
        if (mailid == "") { alert("Email is Required"); $("#DBmailid").focus(); $('#btnDownloadBrochureSubmit').removeAttr("disabled"); return false; }

        if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#DBmailid").focus(); $('#btnDownloadBrochureSubmit').removeAttr("disabled"); return false; } 

        var mobile = $("#DBmobile").val();


        if (mobile == "") {
            alert("Please provide your Mobile Number");
            $("#DBmobile").focus();
            $('#btnDownloadBrochureSubmit').removeAttr("disabled");
            return false;
        }
        else {
            if (document.brochure_form1.DBmobile.value.length != 10) {
                alert("Mobile Number sould be 10 digit!");
                document.brochure_form1.DBmobile.focus();
                $('#btnDownloadBrochureSubmit').removeAttr("disabled");
                return false;
            }
        }
         var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

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

                                    //create json object
                                    var savecustomdata = {

                                        Name: name,
                                        Email: mailid,
                                        Mobile: mobile,
                                        FormType: formtype,
                                        FormSubmitOn: currentdate
                                    };

                                    //ajax calling to insert  custom data function
                                    $.ajax({
                                        type: "POST",
                                        data: JSON.stringify(savecustomdata),
                                        url: "/api/BelvedereClubGurgaon/InsertBrochuredetail",
                                        contentType: "application/json",
                                        success: function (data) {
                                            //////////////

                                            if (data.status == "1") {
                                                window.location.href = "/Download-Brochure-ThankYou";
                                                //$('#enquiry_form1').submit();
                                            }
                                            else {
                                                alert("Sorry Operation Failed!!! Please try again later");
                                                $('#btnDownloadBrochureSubmit').removeAttr("disabled");
                                                return false;
                                            }
                                        }
                                    });



                                }

                                else {
                                    alert("Invalid OTP");
                                    $('#btnDownloadBrochureSubmit').removeAttr("disabled");
                                    return false;
                                }
                            }
                        });

                    }
                }
                else if (data == "-1") {
                    alert("Invalid Mobile Number");
                    $('#btnDownloadBrochureSubmit').removeAttr("disabled");
                }
            }
        });

        return false;
    });
});


