
$("#btnLogisticsQuoteSubmit").click(function () {
    $('#btnLogisticsQuoteSubmit').attr("disabled", "disabled");

    var requestorName = $("#requestorName").val();
    if (requestorName == "") { alert("Please enter your Name"); $("#requestorName").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false; }

    var requestorOrganization = $("#requestorOrganization").val();
    if (requestorOrganization == "") { alert("Please enter Organization Name"); $("#requestorOrganization").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false; }


    var requestorEmail = $("#requestorEmail").val();
    if (requestorEmail == "") { alert("Please enter your Email"); $("#requestorEmail").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false; }



    var requestorNumber = $("#requestorNumber").val();
    if (requestorNumber == "") { alert("Please enter your Contact Number"); $("#requestorNumber").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false; }
    if (requestorNumber.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#requestorNumber").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false;

    }

    var requestorService = $("#requestorService").val();
    if (requestorService == "") { alert("Please select valid Service"); $("#requestorService").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false; }

    var requestorDetails = $("#requestorDetails").val();
    if (requestorDetails == "") { alert("Please enter Details"); $("#requestorDetails").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false; }

    var mobile = "1010101010";
    var formtype = $("#lFormType").val();

    var pageinfo = window.location.href;
    if (!validateEmail(requestorEmail)) { alert("Please enter valid email address"); $("#requestorEmail").focus(); $('#btnLogisticsQuoteSubmit').removeAttr("disabled"); return false; }



    function validateEmail(email) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(email)) { return true; }
        else { return false; }
    }

    var model = {

        Name: requestorName,
        Mobile: requestorNumber,
        Email: requestorEmail

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        Name: requestorName,
        Organization: requestorOrganization,
        Email: requestorEmail,
        ContactNumber: requestorNumber,
        Service: requestorService,
        Details: requestorDetails,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/ports/InsertLogisticsQuotedetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "https://stage.adaniports.com/thankyou";
                //$('#contact_form1').submit();
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnLogisticsQuoteSubmit').removeAttr("disabled");
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
                                $('#btnLogisticsQuoteSubmit').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnLogisticsQuoteSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

