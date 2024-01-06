$(document).ready(function () {
    if (parseInt($('#advanceAmmount').val()) == 0) {
        $('#divBenefitsContent').show();
    }
    else {
        $('#divBenefitsContent').hide();
    }

    var callfrom = GetParameterValues('callfrom');
    var AccountNumber = GetParameterValues('ca_number');
    //var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    //var decrypted = CryptoJS.AES.decrypt(AccountNumber, key);
    //alert(decrypted);
    //console.log(decrypted.toString(CryptoJS.enc.Utf8));
    if ((AccountNumber != null && AccountNumber != "")) {
    }
    else {
        window.location.href = "http://" + $(location).attr('host');
    }

    $(".PayPaymentGateway").click(function () {        
        var buttonName = $(document.activeElement).attr('name');
        //if (buttonName === "Pay_PaymentGateway") {
        var actualAmount = $("#CheckAmountPayable").val();
        var newAmount = $(".checkPayable").val();
        var emailaddress = $("#emailaddress").val();
        var mobileNumber = $("#mobileNumber").val();
        var mobilenocheck = new RegExp("^[0-9]{10,10}$");
        var Emailregex = new RegExp("^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$");
        var checkvalidation = true;
        
        var captcharesponse = grecaptcha.getResponse(paybillrevamprecaptcha);

        if (newAmount != "") {
            if (newAmount <= 0) {
                $('#paymentErrorMessage').html("Invalid amount payable value.");
                $('.checkPayable')[0].scrollIntoView(true);
                checkvalidation = false;
                $('#paymentErrorMessage').show();
            }
            else {
                $('#paymentErrorMessage').html("");
                $('#paymentErrorMessage').hide();
            }
        }
        else {
            checkvalidation = false;
            $('.checkPayable')[0].scrollIntoView(true);
            $('#paymentErrorMessage').html("Invalid amount payable value.");
            $('#paymentErrorMessage').show();
        }
        if (mobileNumber == "") {
            $('#mobileNumberMessage').html("Please enter a valid Mobile No.");
            $('#mobileNumber')[0].scrollIntoView(true);
            checkvalidation = false;
            $('#mobileNumberMessage').show();
        }
        else if (!mobilenocheck.test(mobileNumber)) {
            $('#mobileNumberMessage').html("Please enter a valid Mobile No.");
            $('#mobileNumber')[0].scrollIntoView(true);
            checkvalidation = false;
            $('#mobileNumberMessage').show();
        }
        else {
            $('#mobileNumberMessage').html("");
            $('#mobileNumberMessage').hide();
        }

        if (emailaddress == "") {
            $('#emailaddressMessage').html("Please enter a valid Email No.");
            $('#emailaddress')[0].scrollIntoView(true);
            checkvalidation = false;
            $('#emailaddressMessage').show();
        }
        else if (!Emailregex.test(emailaddress)) {
            $('#emailaddressMessage').html("Please enter a valid Email No.");
            $('#emailaddress')[0].scrollIntoView(true);
            checkvalidation = false;
            $('#emailaddressMessage').show();
        }
        else {
            $('#emailaddressMessage').html("");
            $('#emailaddressMessage').hide();
        }

        if (actualAmount >= 0 && actualAmount >= 100 && newAmount < 100) {
            $('#paymentErrorMessage').html("Minimum Amount payable Value is 100. Please enter valid amount.");
            $('.checkPayable')[0].scrollIntoView(true);
            // event.preventDefault();
            $('#paymentErrorMessage').show();
            return false;
        }
        else {
            if (newAmount != "" && newAmount > 0) {
                $('#paymentErrorMessage').html("");
                $('#paymentErrorMessage').hide();
            }
        }

        if (!captcharesponse) {
            $('#Captchaerror').html('Please validate captcha to continue');
            return false;
        }
        else {
            $('#Captchaerror').html('');
        }

        if (checkvalidation) {
            return true;
        }
        else {
            return false;
        }
        // }

    });
});

$('.checkPayable').keypress(function (event) {
    return isNumber(event, this)
});

// THE SCRIPT THAT CHECKS IF THE KEY PRESSED IS A NUMERIC OR DECIMAL VALUE.
function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function checkMinimumPayable(AmountPayable) {
    $('SecurityDepositAmountType').val("Actual");
    $('#advanceAmmount').val(AmountPayable.toFixed(2));
    $('#advanceAmmount').siblings('label').addClass('active');
    $('#advanceAmmount').attr("readonly", "readonly");
    $('#advanceAmmount').addClass("ameldisabled");
    
    $('#SecurityDepositAmount').val(AmountPayable.toFixed(2));
    $('#SecurityDepositAmount').siblings('label').addClass('active');
    $('#SecurityDepositAmount').attr("readonly", "readonly");
    $('#SecurityDepositAmount').addClass("ameldisabled");
    $('#amountPayable').val(AmountPayable.toFixed(2));
    $('#amountPayable').siblings('label').addClass('active');
    $('#amountPayable').attr("readonly", "readonly");
    $('#amountPayable').addClass("ameldisabled");
    
}
function checkOtherPayment() {
    $('#SecurityDepositAmountType').val("Partial");
    $('#advanceAmmount').removeAttr('readonly');
    $('#amountPayable').removeAttr('readonly');
    $('#SecurityDepositAmount').removeAttr('readonly');

    $('#advanceAmmount').removeClass('ameldisabled');
    $('#amountPayable').removeClass('ameldisabled');
    $('#SecurityDepositAmount').removeClass('ameldisabled');
   
}

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}


