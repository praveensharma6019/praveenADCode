
$("#btnQuoteSubmit").click(function() {
    $('#btnQuoteSubmit').attr("disabled", "disabled");

    var freighttype = $("#freighttype").val();
    if (freighttype == "") { alert("Please enter freight type"); $("#freighttype").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }


    var cityofdeparture = $("#cityofdeparture").val();
    if (cityofdeparture == "") { alert("Please enter city of departure"); $("#cityofdeparture").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var deliverycity = $("#deliverycity").val();
    if (deliverycity == "") { alert("Please enter delivery city"); $("#deliverycity").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var incoterms = $("#incoterms").val();
    if (incoterms == "") { alert("Please enter incoterms"); $("#incoterms").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var totalgrossweight = $("#totalgrossweight").val();
    if (totalgrossweight == "") { alert("Please enter total gross weight"); $("#totalgrossweight").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var dimension = $("#dimension").val();
    if (dimension == "") { alert("Please enter dimension"); $("#dimension").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var email = $("#email").val();
    if (email == "") { alert("Please enter email"); $("#email").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var message = $("#message").val();
    if (message == "") { alert("Please enter message"); $("#message").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }

    var mobile = "1010101010";




    var formtype = $("#cFormType").val();

        var pageinfo = window.location.href;
    if (!validateEmail(email)) { alert("Please enter valid email address"); $("#email").focus(); $('#btnQuoteSubmit').removeAttr("disabled"); return false; }



    function validateEmail(email) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(email)) { return true; }
            else { return false; }
        }

        var model = {

            Name: name,
            Mobile: mobile,
            Email: email

        };

        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

        //create json object
        var savecustomdata = {

            FreightType: freighttype,
            CityofDeparture: cityofdeparture,
            DeliveryCity: deliverycity,
            Incoterms: incoterms,
            TotalGrossWeight: totalgrossweight,
            Dimension: dimension,
            Email: email,
            Message: message,
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
                    window.location.href = "/";
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
    
