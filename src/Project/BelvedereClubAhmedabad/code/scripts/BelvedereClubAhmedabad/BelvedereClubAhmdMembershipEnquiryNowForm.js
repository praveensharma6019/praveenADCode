

$(document).ready(function () {

    var SelectedRadioVal = "";


    $('#btnMembershipEnquiryNowSubmit').click(function () {



        $('#btnMembershipEnquiryNowSubmit').attr("disabled", "disabled");
        var name = $("#name").val();
        if (name == "") { alert("Please enter your Name"); $("#name").focus(); $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled"); return false; }
        var mailid = $("#mailid").val();
        if (mailid == "") { alert("Email is Required"); $("#mailid").focus(); $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled"); return false; }
        var city = $("#city").val();
        if (city == "") { alert("Please enter your city"); $("#city").focus(); $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled"); return false; }
        var message = $("#message").val();
        if (message == "") { alert("Please enter any message"); $("#message").focus(); $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled"); return false; }
        var formType = $("#FormType").val();
             
        var mobile = $("#mobile").val();
        if (mobile == "") {
            alert("Please provide your Mobile Number");
            $("#mobile").focus();
            $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled");
            return false;
        }
        else {
            if (document.membershipenquirynow_form1.mobile.value.length != 10) {
                alert("Mobile Number sould be 10 digit!");
                document.membershipenquirynow_form1.mobile.focus();
                $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled");
                return false;
            }
        }
     
        var model = {

            Name: name,
            Mobile: mobile,
            Email: mailid

        };

       var  currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

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
                                        Message: message,
                                        FormType: formType,
                                        FormSubmitOn: currentdate
                                    };

                                    //ajax calling to insert  custom data function
                                    $.ajax({
                                        type: "POST",
                                        data: JSON.stringify(savecustomdata),
                                        url: "/api/BelvedereClubAhmedabad/InsertMembershipEnquiryNowdetail",
                                        contentType: "application/json",
                                        success: function (data) {
                                            //////////////

                                            if (data.status == "1") {
                                                window.location.href = "/ThankYou";
                                                //$('#membershipenquirynow_form1').submit();
                                            }
                                            else {
                                                alert("Sorry Operation Failed!!! Please try again later");
                                                $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled");
                                                return false;
                                            }
                                        }
                                    });



                                }

                                else {
                                    alert("Invalid OTP");
                                    $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled");
                                    return false;
                                }
                            }
                        });

                    }
                }
                else if (data == "-1") {
                    alert("Invalid Mobile Number");
                    $('#btnMembershipEnquiryNowSubmit').removeAttr("disabled");
                }
            }
        });

        return false;
    });
});


