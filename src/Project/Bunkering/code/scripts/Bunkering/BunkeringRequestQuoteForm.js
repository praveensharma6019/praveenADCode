var recaptcha1;

var onloadCallback = function () {
    //Render the recaptcha1 on the element with ID "recaptcha1"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6LcIPrAUAAAAAJmE86xjLjek5yX1T4lRRqqwO3gt',
        'theme': 'light'
    });
};
$("#btnRequestQuoteSubmit").click(function () {

    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    $('#btnRequestQuoteSubmit').attr("disabled", "disabled");
    var Company_Name_B = $("#Company_Name_B").val();
    if (Company_Name_B == "") { alert("Please enter Company Name"); $("#Company_Name_B").focus(); $('#btnRequestQuoteSubmit').removeAttr("disabled"); return false; }

    var Contact_Person_B = $("#Contact_Person_B").val();
    if (Contact_Person_B == "") { alert("Please enter Contact Person Name"); $("#Contact_Person_B").focus(); $('#btnRequestQuoteSubmit').removeAttr("disabled"); return false; }

    var Telephone_B = $("#Telephone_B").val();
    if (Telephone_B == "") { alert("Please specify your Telephone Number"); $("#Telephone_B").focus(); $('#btnRequestQuoteSubmit').removeAttr("disabled"); return false; }

    if (Telephone_B.length != 10) {
        alert("Telephone Number should be of 10 digit"); $("#Telephone_B").focus(); $('#btnRequestQuoteSubmit').removeAttr("disabled"); return false;
    }

    var Email_B = $("#Email_B").val();
    if (Email_B == "") { alert("Email is Required"); $("#Email_B").focus(); $('#btnRequestQuoteSubmit').removeAttr("disabled"); return false; }
    if (!validateEmail(Email_B)) { alert("Please enter valid email address"); $("#Email_B").focus(); $('#btnRequestQuoteSubmit').removeAttr("disabled"); return false; }


    var ProductType = $("input[name='ProductType']:checked").val();

    var ProductSpec = $("input[name='ProductSpec']:checked").val();

    var Quantity_B = $("#Quantity_B").val();

    var Vessel_Name_B = $("#Vessel_Name_B").val();
    var Port_B = $("#Port_B").val();
    var Berth_B = $("#Berth_B").val();
    var Date_of_Arrival_B = $("#Date_of_Arrival_B").val();
    var Time_of_Arrival_B = $("#Time_of_Arrival_B").val();
    var Date_of_Departure_B = $("#Date_of_Departure_B").val();
    var Time_of_Departure_B = $("#Time_of_Departure_B").val();
    var Agent_B = $("#Agent_B").val();

    var Other_Detail_B = $("#Other_Detail_B").val();


    var formtype = $("#bFormType").val();
    var pageinfo = window.location.href;



    function validateEmail(mailid) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(mailid)) { return true; }
        else { return false; }
    }

    var model = {

        Name: Contact_Person_B,
        Mobile: Telephone_B,
        Email: Email_B

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        CompanyName: Company_Name_B,
        ContactPerson: Contact_Person_B,
        Mobile: Telephone_B,
        Email: Email_B,
        ProductType: ProductType,
        ProductSpec: ProductSpec,
        ProductQuantity: Quantity_B,
        VesselName: Vessel_Name_B,
        Port: Port_B,
        Berth: Berth_B,
        Estimated_DOA: Date_of_Arrival_B,
        Estimated_TOA: Time_of_Arrival_B,
        Estimated_DOD: Date_of_Departure_B,
        Estimated_TOD: Time_of_Departure_B,
        Agent: Agent_B,
        reResponse: response,
        OtherDetails: Other_Detail_B,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Bunkering/InsertRequestQuotedetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                document.getElementById("Bunkering").reset();
                //$('#contact_form1').submit();
            }
            else if (data.status == "3") {
                alert("Captcha Failed!!! Please try again");
                return false;
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnRequestQuoteSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;


    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Bunkering/CreateOTP",
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
                        url: "/api/Bunkering/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {


                            }

                            else {
                                alert("Invalid OTP");
                                $('#btnRequestQuoteSubmit').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnRequestQuoteSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

