var recaptcha1;
var registerCallBack = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc', //Replace this with your Site key
        'theme': 'light'
    });


};
function validateEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function validateMobileNo(mobile) {
    if (mobile.match(/^[6789]\d{9}$/)) {
        return true;
    }
    else {
        return false;
    }
}

$("#btnSubmitOtp1").on('click', function () {
    //check if tearms and cinditions are checked and confrim
    if (!$('input[name="termsCb"]').is(':checked')) {
        $("#termsErrorMessage").html("Please confirm terms and conditions.");
        return false;
    }
});

$("#btnSubmitOtp2").on('click', function () {
    //check if tearms and cinditions are checked and confrim
    if (!$('input[name="termsCb"]').is(':checked')) {
        $("#termsErrorMessage").html("Please confirm terms and conditions.");
        return false;
    }
});


$("#ValidateInput").on('click', function () {

    var message = '';
    var error = false;
    
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        message = message + "Captcha required";
        $("#captchaErrorMessage").html(message);
        $("#captchaErrorMessage").focus();
        error = true;
    }
    else $("#captchaErrorMessage").html("");

    $("#reResponse").val(response);

    if (error) {
        return false;
    }
    else
        return true;
});

$("#ClaimAnOffer").on('click', function () {

    var message = '';
    var error = false;

    //var emailAddress= $("#emailAddressForClaim").val();
    //var mobileNo  = $("#mobilenumberForClaim").val();

    //if (!validateMobileNo(mobileNo)) {
    //    event.preventDefault();
    //    $("#mobileerror").html("Please enter a 9 digit valid mobile number");
    //    $("#mobilenumberForClaim").focus();
    //    error = true;
    //}
    //else {
    //    $("#mobileerror").html("");
    //}

    //if (!validateEmail(emailAddress)) {
    //    event.preventDefault();
    //    $("#emailerror").html("Please enter a valid Email Address");
    //    $("#emailAddressForClaim").focus();
    //    error = true;
    //}
    //else {
    //    $("#emailerror").html("");
    //}

    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        message = message + "Captcha required";
        $("#captchaErrorMessage").html(message);
        $("#captchaErrorMessage").focus();
        error = true;
    }
    else $("#captchaErrorMessage").html("");

    $("#reResponse").val(response);

    if (error) {
        return false;
    }
    else
        return true;
    
});



























$(document).ready(function () {

    $('.hero-banner').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        items: 1,
        nav: false,
        dots: true
    });

    $(document).on("click", ".claim-offer", function () {
        Id = $(this).data('id');
        $("#OfferId").val(Id);

        var OfferCode = $("#OfferCodeId").text();
        $("#OfferCode").val(OfferCode);

        var offerName = $("#OfferHeading").text();
        $("#OfferName").val(offerName);

        var offerCompany = $("#OfferCompanyName").text();
        $("#OfferCompany").val(offerCompany);

        var offerLink = $("#OfferLinkToRedirect").text();
        $("#OfferLink").val(offerLink);
    });


    $("#btnsubmit").click(function () {

        $('#btnsubmit').attr("disabled", "disabled");
        var offerName = $("#OfferNames").val();

        var offerId = $("#ooferids").val();

        var consumerName = $("#consumernames").val();




        var model = {

            OfferName: offerName,
            OfferId: offerId,
            ConsumerName: consumerName

        };

        var ClaimedDates = new Date().toISOString().slice(0, 19).replace('T', ' ');

        //create json object
        var savecustomdata = {


            OfferName: offerName,
            OfferId: offerId,
            ConsumerName: consumerName,
            ConsumerAccountNumber: "abc",
            ConsumerMobileNumber: "99834775",

            PageInfo: pageinfo,
            ClaimedDate: ClaimedDates


        };

        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/AdaniCare/ClaimedOfferDetails",
            contentType: "application/json",
            success: function (data) {
                //////////////

                if (data.status == "1") {
                    window.location.href = "/thankyou";
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
    });
});