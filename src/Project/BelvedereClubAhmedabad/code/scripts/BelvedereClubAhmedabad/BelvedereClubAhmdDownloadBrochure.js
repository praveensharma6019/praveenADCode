

$(document).ready(function () {

   


    $('#btnDownloadBrochureSubmit').click(function () {



        $('#btnDownloadBrochureSubmit').attr("disabled", "disabled");
        var formtype = $("#FormType").val();

        var name = $("#name").val();
        if (name == "") { alert("Please enter your Name"); $("#name").focus(); $('#btnDownloadBrochureSubmit').removeAttr("disabled"); return false; }
        var mailid = $("#mailid").val();
        if (mailid == "") { alert("Email is Required"); $("#mailid").focus(); $('#btnDownloadBrochureSubmit').removeAttr("disabled"); return false; }
        var city = $("#city").val();
        if (city == "") { alert("Please enter your city"); $("#city").focus(); $('#btnDownloadBrochureSubmit').removeAttr("disabled"); return false; }

        var mobile = $("#mobile").val();
        if (mobile == "") {
            alert("Please provide your Mobile Number");
            $("#mobile").focus();
            $('#btnDownloadBrochureSubmit').removeAttr("disabled");
            return false;
        }
        else {
            if (document.brochure_form1.mobile.value.length != 10) {
                alert("Mobile Number sould be 10 digit!");
                document.brochure_form1.mobile.focus();
                $('#btnDownloadBrochureSubmit').removeAttr("disabled");
                return false;
            }
        }
         var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

       
        var model = {

            Name: name,
            Mobile: mobile,
            Email: mailid

        };


        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: "/api/BelvedereClubAhmedabad/RoomAvailability",
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
                            url: "/api/BelvedereClubAhmedabad/VerifyOTP",
                            contentType: "application/json",
                            success: function (data) {
                                if (data.status == "1") {

                                    //create json object
                                    var savecustomdata = {

                                        Name: name,
                                        Email: mailid,
                                        City: city,
                                        Mobile: mobile,
                                        FormType: formtype,
                                        FormSubmitOn: currentdate
                                    };

                                    //ajax calling to insert  custom data function
                                    $.ajax({
                                        type: "POST",
                                        data: JSON.stringify(savecustomdata),
                                        url: "/api/BelvedereClubAhmedabad/InsertBrochuredetail",
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


