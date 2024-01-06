
$("#btnQuoteSubmit").click(function () {
    $('#btnQuoteSubmit').attr("disabled", "disabled");

    var deliveryName = $("#deliveryName").val();
    if (deliveryName == "") { alert("Please enter your Name"); $("#deliveryName").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var deliveryCompany = $("#deliveryCompany").val();
    if (deliveryCompany == "") { alert("Please enter Company Name"); $("#deliveryCompany").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }


    var deliveryEmail = $("#deliveryEmail").val();
    if (deliveryEmail == "") { alert("Please enter your Email"); $("#deliveryEmail").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }



    var deliveryType = $("#deliveryType").val();
    if (deliveryType == "") { alert("Please enter suitable delivery type"); $("#deliveryType").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }


    var deliveryCommodity = $("#deliveryCommodity").val();
    if (deliveryCommodity == "") { alert("Please enter Commodity"); $("#deliveryCommodity").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var deliveryCommodityType = $("#deliveryCommodityType").val();
    if (deliveryCommodityType == "") { alert("Please enter Commodity Type"); $("#deliveryCommodityType").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var deliveryQuantity = $("#deliveryQuantity").val();
    if (deliveryQuantity == "") { alert("Please enter youe expected Quantity"); $("#deliveryQuantity").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var deliveryArrivalDate = $("#deliveryArrivalDate").val();
    if (deliveryArrivalDate == "") { alert("Please enter valid Arrival Date"); $("#deliveryArrivalDate").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var deliveryMessage = $("#deliveryMessage").val();
    if (deliveryMessage == "") { alert("Please enter your Message"); $("#deliveryMessage").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var mobile = "1010101010";
    var formtype = $("#dFormType").val();

    var pageinfo = window.location.href;
    if (!validateEmail(deliveryEmail)) { alert("Please enter valid email address"); $("#deliveryEmail").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }



    function validateEmail(email) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(email)) { return true; }
        else { return false; }
    }

    var model = {

        Name: deliveryName,
        Mobile: mobile,
        Email: deliveryEmail

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {
        Name: deliveryName,
        Company: deliveryCompany,
        Email: deliveryEmail,
        DeliveryType: deliveryType,
        Commodity: deliveryCommodity,
        CommodityType: deliveryCommodityType,
        ExpectedQuantity: deliveryQuantity,
        ExpectedDate: deliveryArrivalDate,
        Message: deliveryMessage,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/ports/InsertQuotedetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "https://stage.adaniports.com/thankyou";
                //$('#contact_form1').submit();
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnQuoteSubmit').removeAttr("disabled");
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
                                $('#btnQuoteSubmit').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnQuoteSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

