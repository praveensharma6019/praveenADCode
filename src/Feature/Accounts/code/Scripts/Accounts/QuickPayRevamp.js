$('.icon-holder').click(function () {
    $("#first_name2").val("");
    $('.icon-holder').hide();
});

const capitalize = (phrase) => {
    return phrase
        .toLowerCase()
        .split(' ')
        .map(word => word.charAt(0).toUpperCase() + word.slice(1))
        .join(' ');
};

function getEncriptedKey(stringToEncrypt) {
    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
    var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(stringToEncrypt), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        }).toString();
    return encrypted;
}

function FetchQuickPay() {
    var accountNumber = $('#hdnAccountNumber').val();
    $('.loader-wrap').show();
    $.ajax({
        url: apiSettings + "/AccountsRevamp/PayBillRevamp",
        type: 'GET',
        data: { AccountNumber: getEncriptedKey(accountNumber), AmountPayable: $('#advanceAmmount').val(), Pay_PaymentGateway: "Proceed to Pay" },
        success: function (data) {
            $('.loader-wrap').hide();
            window.location.href = "http://" + $(location).attr('host') + '/Payment/pay-your-bill?ca_number=' + getEncriptedKey(accountNumber);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation--' + errorThrown);
        }
    });
}

function showCrossIcon() {
    $('#lblCANumber').addClass('active');
    $("#first_name2").removeClass("invalid");
    if ($('#first_name2').val().length == 0) {
        $('.icon-holder').hide();
        $('#crossicon').click();
    }
    else {
        $('.icon-holder').show();

    }
}

function ShowDetails() {
    $('.quick-pay-tab #energybill')[0].click();
    $('#Captchaerror').hide().html('');
    var BeforeLoadingText = $('.qproceed').html();
    //$('.qproceed').html('<svg viewBox="0 0 100 100" class="loader-white"> <circle cx="6" cy="50" r="6"> <animateTransform attributeName="transform" dur="1s" type="translate" values="0 15 ; 0 -15; 0 15" repeatCount="indefinite" begin="0.1"></animateTransform> </circle> <circle stroke="none" cx="30" cy="50" r="6"> <animateTransform attributeName="transform" dur="1s" type="translate" values="0 10 ; 0 -10; 0 10" repeatCount="indefinite" begin="0.2"></animateTransform> </circle> <circle stroke="none" cx="54" cy="50" r="6"> <animateTransform attributeName="transform" dur="1s" type="translate" values="0 5 ; 0 -5; 0 5" repeatCount="indefinite" begin="0.3"></animateTransform> </circle> </svg>');
    $('.qproceed').html(`<span class="custom-loader-dots-box button-loader">
                                <span class="custom-loader-dot custom-loader-dot-1"></span>
                                <span class="custom-loader-dot custom-loader-dot-2"></span>
                                <span class="custom-loader-dot custom-loader-dot-3"></span>
                              </span>`);
    var accountNumber = $('#first_name2').val().trim();

    $('#hdnAccountNumber').val(accountNumber);
    if (accountNumber != '' && accountNumber.length == 9) {
        var captcharesponse = false;
        if (accountNumber != '' && accountNumber.length == 9) {
            var callfrom = GetParameterValues('callfrom');
            if (callfrom == undefined) {
                callfrom = '';
                var captcharesponse = grecaptcha.getResponse(recaptcha);
            }
            if (callfrom != '') {

                captcharesponse = 'Home';
                callfrom = "Home";
            }
            if (callfrom == "Home" || (callfrom == '' && (captcharesponse))) {
                $.ajax({
                    url: apiSettings + "/AccountsRevamp/QuickPayRevampHumb",
                    type: 'GET',
                    data: { AccountNumber: getEncriptedKey(accountNumber), recaptcha: captcharesponse },
                    dataType: 'JSON',
                    beforeSend: function () {
                        $('.loader-wrap').show();
                        dataLayer.push({
                            'event': 'quick_bill_payment_proceed',
                            'eventCategory': 'Bill Payment',
                            'eventAction': 'Quick Bill Payment Proceed',
                            'eventLabel': ' Quick Bill Payment | Proceed',
                            'business_user_id': $('#BusinessUserId').val(),
                            'login_status': $('#login_status').val(),
                            'ca_number': $('#first_name2').val().trim(),
                            'page_type': $('head title').text(),
                        });
                    },
                    success: function (data) {

                        $('.qproceed').html(BeforeLoadingText);

                        $('.qproceed').attr("disabled", "disabled");
                        $('.BillDetailsQuick').attr("class", "active");


                        if (data.data.Captcha == undefined || data.data.Captcha == null || data.data.Message == "Please validate captcha to continue") {
                            $('.qproceed').removeAttr("disabled", "disabled");
                            $('#billDetailsContent').hide();
                            $('#Captchaerror').show();
                            $("#Captchaerror").html("Please validate captcha to continue");
                            if ($('#login_status').val() == 'Guest User') {
                                dataLayer.push({
                                    'event': 'quick_bill_payment_proceed_captcha_failed',
                                    'eventCategory': 'Bill Payment',
                                    'eventAction': 'Quick Bill Payment Proceed',
                                    'eventLabel': ' Quick Bill Payment | Proceed',
                                    'business_user_id': $('#BusinessUserId').val(),
                                    'login_status': $('#login_status').val(),
                                    'ca_number': $('#first_name2').val().trim(),
                                    'page_type': $('head title').text()
                                });
                            }
                            else {
                                dataLayer.push({
                                    'event': 'quick_bill_payment_proceed_captcha_failed',
                                    'eventCategory': 'Bill Payment',
                                    'eventAction': 'Quick Bill Payment Proceed',
                                    'eventLabel': ' Quick Bill Payment | Proceed',
                                    'business_user_id': $('#BusinessUserId').val(),
                                    'login_status': $('#login_status').val(),
                                    'ca_number': $('#GACANumber').val(),
                                    'page_type': $('head title').text()
                                });
                            }
                        }
                        else {
                            $('#Captchaerror').hide();
                            $('#billDetailsContent').show();
                            if (data.data["AccountNumber"] == "") {

                                $('.qproceed').removeAttr("disabled", "disabled");
                                $("#first_name2").addClass("invalid");
                                $('.BillDetailsQuick').removeAttr("class", "active");
                                $('#billDetailsContent').hide();
                                $('.ca-number-help').html("Enter valid CA Number and click Proceed");
                                $('.ca-number-help').attr("class", "help-text ca-number-help field-validation-error");
                                //  $('.ca-number-help').attr("class", "error-text");
                            }
                            else {
                                $('.SecurityDepositAmount').html('₹' + parseFloat(data.data["SecurityDeposit"]).toFixed(2));
                                $('.SecurityDepositText').html(data.data["SecurityDepositMsg"]);
                                //$('.SecurityDepositText').html('No Security Amount Pending');

                                $('.PayNowSecurity').hide();
                            }

                            $(".STariffSlab span").html(data.data["TariffSlab"]);
                            $(".SMeterNumber span").html(data.data["MeterNumber"]);
                            $(".SBookNumber span").html(data.data["BookNumber"]);
                            $(".SCycleNumber span").html(data.data["CycleNumber"]);
                            $(".SZone span").html(data.data["Zone"]);
                            /* $(".SAddress span").html(capitalize(data.data["Address"]));*/
                            $(".SSecurityDeposit span").html(data.data["SecurityDeposit"]);

                            if ((parseInt(data.data["SecurityDeposit"])) > 0) {
                                $('.SecurityDepositAmount').html('₹' + parseFloat(data.data["SecurityDeposit"]).toFixed(2));
                                $('.SecurityDepositText').html('Pay Security Deposit Pending');
                                $('.PayNowSecurity').show();
                            }
                            else {
                                $('.SecurityDepositAmount').html('₹' + parseFloat(data.data["SecurityDeposit"]).toFixed(2));
                                $('.SecurityDepositText').html(data.data["SecurityDepositMsg"]);
                                //$('.SecurityDepositText').html('No Security Amount Pending');

                                $('.PayNowSecurity').hide();
                            }

                            $(".STariffSlab span").html(data.data["TariffSlab"]);
                            $(".SMeterNumber span").html(data.data["MeterNumber"]);
                            $(".SBookNumber span").html(data.data["BookNumber"]);
                            $(".SCycleNumber span").html(data.data["CycleNumber"]);
                            $(".SZone span").html(data.data["Zone"]);
                            $(".SAddress span").html(capitalize(data.data["Address"]));
                            $(".SSecurityDeposit span").html(data.data["SecurityDeposit"]);

                            if ((parseInt(data.data["PaymentVDSAmount"])) > 0) {
                                $("#hdnCurrentOutstanding").val(data.data["AmountPayable"]);
                                $('#CurrentOutstanding').html('₹' + parseFloat(data.data["AmountPayable"]).toFixed(2))
                                $('.VDSAmountCurrentOutstanding').show();
                                $('.withoutOutstanding').hide();
                                $('.VDSAmountData').hide();
                            }
                            else {
                                $('.VDSAmountCurrentOutstanding').hide();
                                $('.VDSAmountData').show();
                                $('.withoutOutstanding').show();
                            }

                            $("#spanAverageVDSAmount").html(data.data["AverageVDSAmount"]);
                            $("#spanMaxVDSAmount").html(data.data["MaxVDSAmount"]);
                            $("#AmountPayable").val(data.data["AverageVDSAmount"]);
                            $("#PANNo").val(data.data["PANNo"]);
                            $("#Mobile").val(data.data["Mobile"]);
                            $("#Email").val(data.data["Email"]);
                            $('.spanCANumber').html(data.data["AccountNumber"]);
                            $('.spanName').html(capitalize(data.data["Name"]));
                            $('.VDScheck').attr("class", "active");

                            if ((parseInt(data.data["AmountPayable"])) > 0) {
                                $('#modal2').hide();
                                $('#benefitsContent').hide();
                                $('.QuickBillAmount').html('₹' + parseFloat(data.data["AmountPayable"]).toFixed(2));
                                if (data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00") {
                                    $(".QDuedate").html("Payment Due Date: Not Updated");
                                }
                                else {

                                    if (data.data["DueDateGraterthenFourDays"]) {
                                        $(".QDuedate").html("Due on " + data.data["PaymentDueDate"]);
                                        $(".QDuedate").css("color", "red");
                                    }
                                    else {
                                        $(".QDuedate").html("Due on " + data.data["PaymentDueDate"]);
                                        $(".QDuedate").css("color", "red");
                                    }
                                }

                                $('.AdvanceQuickPayDetailBody').hide();
                                $('.AdvanceQuickPay').hide();
                                $('.BillQuickPayDetailBody').show();
                                $('.BillQuickPay').show();
                                $(".QConsumerName span").html(capitalize(data.data["Name"]));
                                $(".QCANumber span").html(data.data["AccountNumber"]);
                                $(".QBookNumber span").html(data.data["BookNumber"]);
                                $(".QCycleNumber span").html(data.data["CycleNumber"]);
                                $(".QZone span").html(data.data["Zone"]);
                                //$(".QAddress span").html(capitalize(data.data["Address"]));
                                $(".QBillMonth span").html(data.data["BillMonth"]);
                                $(".QBillConsumed span").html(data.data["UnitsConsumed"]);
                                $(".QTBillConsumed span").html(data.data["TotalBillAmount"]);
                                $(".QMinimumAmount span").html(data.data["AmountPayable"]);
                                if ($('#login_status').val() == 'Guest User') {
                                    dataLayer.push({
                                        'event': 'quick_bill_due_amount',
                                        'eventCategory': 'Bill Payment',
                                        'eventAction': 'Quick Bill Payment Proceed',
                                        'eventLabel': ' Quick Bill Payment | Proceed',
                                        'business_user_id': $('#BusinessUserId').val(),
                                        'login_status': $('#login_status').val(),
                                        'ca_number': $('#first_name2').val().trim(),
                                        'page_type': $('head title').text(),
                                        'amount': $(".QMinimumAmount").find('span').text(),
                                        'due_date': data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00" || parseInt(data.data["AmountPayable"]) == 0 ? 'No Bill Due' : data.data["PaymentDueDate"]
                                    });
                                }
                                else {
                                    dataLayer.push({
                                        'event': 'quick_bill_due_amount',
                                        'eventCategory': 'Bill Payment',
                                        'eventAction': 'Quick Bill Payment Proceed',
                                        'eventLabel': ' Quick Bill Payment | Proceed',
                                        'business_user_id': $('#BusinessUserId').val(),
                                        'login_status': $('#login_status').val(),
                                        'ca_number': $('#GACANumber').val(),
                                        'page_type': $('head title').text(),
                                        'amount': $(".QMinimumAmount").find('span').text(),
                                        'due_date': data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00" || parseInt(data.data["AmountPayable"]) == 0 ? 'No Bill Due' : data.data["PaymentDueDate"]
                                    });
                                }
                            }
                            else {
                                $('#modal2').hide();
                                $('#benefitsContent').show();
                                $('.AdvanceQuickPayDetailBody').show();
                                $(".QAConsumerName span").html(capitalize(data.data["Name"]));
                                $(".QABookNumber span").html(data.data["BookNumber"]);
                                $(".QACycleNumber span").html(data.data["CycleNumber"]);
                                $(".QAZone span").html(data.data["Zone"]);
                                // $(".QAAddress span").html(capitalize(data.data["Address"]));
                                $(".QCANumber span").html(data.data["AccountNumber"]);
                                // $(".QLastPaidAmount span").html(data.data["AmountPayable"]);
                                $(".QABillMonth span").html(data.data["BillMonth"]);
                                $(".QABillConsumed span").html(data.data["UnitsConsumed"]);
                                $(".QATBillConsumed span").html(data.data["TotalBillAmount"]);
                                $(".QAMinimumAmount span").html(data.data["AmountPayable"]);
                                $('.BillQuickPay').hide();
                                $('.BillQuickPayDetailBody').hide();
                                $('.AdvanceQuickPay').show();
                                $('.AdvanceQuickPayDetailBody').show();
                                if ($('#login_status').val() == 'Guest User') {
                                    dataLayer.push({
                                        'event': 'quick_bill_due_amount',
                                        'eventCategory': 'Bill Payment',
                                        'eventAction': 'Quick Bill Payment Proceed',
                                        'eventLabel': ' Quick Bill Payment | Proceed',
                                        'business_user_id': $('#BusinessUserId').val(),
                                        'login_status': $('#login_status').val(),
                                        'ca_number': $('#first_name2').val().trim(),
                                        'page_type': $('head title').text(),
                                        'amount': $(".QMinimumAmount").find('span').text(),
                                        'due_date': data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00" || parseInt(data.data["AmountPayable"]) == 0 ? 'No Bill Due' : data.data["PaymentDueDate"]
                                    });
                                }
                                else {
                                    dataLayer.push({
                                        'event': 'quick_bill_due_amount',
                                        'eventCategory': 'Bill Payment',
                                        'eventAction': 'Quick Bill Payment Proceed',
                                        'eventLabel': ' Quick Bill Payment | Proceed',
                                        'business_user_id': $('#BusinessUserId').val(),
                                        'login_status': $('#login_status').val(),
                                        'ca_number': $('#GACANumber').val(),
                                        'page_type': $('head title').text(),
                                        'amount': $(".QMinimumAmount").find('span').text(),
                                        'due_date': data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00" || parseInt(data.data["AmountPayable"]) == 0 ? 'No Bill Due' : data.data["PaymentDueDate"]
                                    });
                                }
                            }
                        }
                        $('.loader-wrap').hide();
                    },
                    error: function (xhr, textStatus, errorThrown, data) {
                        $('.qproceed').html(BeforeLoadingText);
                        console.log("ed" + errorThrown);

                    }
                });
            }
            else {
                $('.qproceed').html(BeforeLoadingText);
                if (!captcharesponse) {
                    $('#Captchaerror').show().html('Please validate captcha to continue');
                    if ($('#login_status').val() == 'Guest User') {
                        dataLayer.push({
                            'event': 'quick_bill_payment_proceed_captcha_failed',
                            'eventCategory': 'Bill Payment',
                            'eventAction': 'Quick Bill Payment Proceed',
                            'eventLabel': ' Quick Bill Payment | Proceed',
                            'business_user_id': $('#BusinessUserId').val(),
                            'login_status': $('#login_status').val(),
                            'ca_number': $('#first_name2').val().trim(),
                            'page_type': $('head title').text()
                        });
                    }
                    else {
                        dataLayer.push({
                            'event': 'quick_bill_payment_proceed_captcha_failed',
                            'eventCategory': 'Bill Payment',
                            'eventAction': 'Quick Bill Payment Proceed',
                            'eventLabel': ' Quick Bill Payment | Proceed',
                            'business_user_id': $('#BusinessUserId').val(),
                            'login_status': $('#login_status').val(),
                            'ca_number': $('#GACANumber').val(),
                            'page_type': $('head title').text()
                        });
                    }
                }
                else {
                    $('#Captchaerror').hide().html('');
                }
                if (accountNumber == '' || accountNumber.length != 9) {
                    $('.ca-number-help').html("Enter valid CA Number and click Proceed");
                    $('.ca-number-help').addClass("field-validation-error");
                    $("#first_name2").addClass("invalid");
                }
            }
        }

    }
    else {
        var captcharesponse = grecaptcha.getResponse(recaptcha);
        $('.qproceed').html(BeforeLoadingText);
        if (!captcharesponse) {
            $('#Captchaerror').show().html('Please validate captcha to continue');
            if ($('#login_status').val() == 'Guest User') {
                dataLayer.push({
                    'event': 'quick_bill_payment_proceed_captcha_failed',
                    'eventCategory': 'Bill Payment',
                    'eventAction': 'Quick Bill Payment Proceed',
                    'eventLabel': ' Quick Bill Payment | Proceed',
                    'business_user_id': $('#BusinessUserId').val(),
                    'login_status': $('#login_status').val(),
                    'ca_number': $('#first_name2').val().trim(),
                    'page_type': $('head title').text()
                });
            }
            else {
                dataLayer.push({
                    'event': 'quick_bill_payment_proceed_captcha_failed',
                    'eventCategory': 'Bill Payment',
                    'eventAction': 'Quick Bill Payment Proceed',
                    'eventLabel': ' Quick Bill Payment | Proceed',
                    'business_user_id': $('#BusinessUserId').val(),
                    'login_status': $('#login_status').val(),
                    'ca_number': $('#GACANumber').val(),
                    'page_type': $('head title').text()
                });
            }
        }
        else {
            $('#Captchaerror').hide().html('');
        }
        if (accountNumber == '' || accountNumber.length != 9) {
            $('.ca-number-help').html("Enter valid CA Number and click Proceed");
            $('.ca-number-help').addClass("field-validation-error");
            $("#first_name2").addClass("invalid");
        }
    }
}

function viewSampleBill() {
    let billModals = M.Modal.init(document.getElementById('sampleBillModal'), { opacity: 0.7 });
    document.querySelector('.hide-sample-bill').addEventListener('click', function () {
        billModals.close();
    })
    billModals.open();
}

$('#crossicon').click(function (e) {
    $('#first_name2').val('');
    $("#first_name2").removeClass("invalid");
    $('#lblCANumber').removeClass('active');
    $('.qproceed').removeAttr("disabled");
    $("#billDetailsContent").hide();
    $('.ca-number-help').removeClass("field-validation-error");
    $('.ca-number-help').html("Please enter your 9 digit consumer account number");
    $('#AmountPayable').removeClass('validate invalid');
    $('#errorPaymentAmount').html("");
    $('#errorPANNo').html("");
    $('#PANNo').removeClass('validate invalid');
    $('#errorMobileNumber').html("");
    $('#Mobile').removeClass('validate invalid');
    $('#errorEmailAddress').html("");
    $('#Email').removeClass('validate invalid');
});

function FetchQuickPaySecurity() {
    var accountNumber = $('#hdnAccountNumber').val();
    var SecurityDeposit = $(".SSecurityDeposit span").html();

    window.location.href = "http://" + $(location).attr('host') + '/Payment/pay-your-bill?ca_number=' + getEncriptedKey(accountNumber) + '&SecurityDeposit=' + getEncriptedKey(SecurityDeposit);


}

$('.PayOutstandingBill').click(function () {
    var accountNumber = $('#hdnAccountNumber').val();
    $('.loader-wrap').show();
    $.ajax({
        url: apiSettings + "/AccountsRevamp/PayBillRevamp",
        type: 'GET',
        data: { AccountNumber: getEncriptedKey(accountNumber), AmountPayable: $("#hdnCurrentOutstanding").val(), Pay_PaymentGateway: "Proceed to Pay" },
        success: function (data) {
            $('.loader-wrap').hide();

            window.location.href = "http://" + $(location).attr('host') + '/Payment/pay-your-bill?ca_number=' + getEncriptedKey(accountNumber);

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation--' + errorThrown);

        }
    });

});

$(".PayVDSBill").click(function () {

    var AmountPayable = parseFloat($('#AmountPayable').val());
    var pANNo = $('#PANNo').val();
    var mobile = $('#Mobile').val();
    var email = $('#Email').val();
    var VDSAmount = parseFloat($('#spanAverageVDSAmount').html());
    var maxVDSAmount = parseFloat($('#spanMaxVDSAmount').html());
    var Pancheck = new RegExp("^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$");
    var mobilenocheck = new RegExp("^[0-9]{10,10}$");
    var Emailregex = new RegExp("^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$");
    var checkvalidation = true;
    if (AmountPayable != 0) {
        if (AmountPayable <= 0) {
            $('#errorPaymentAmount').html("Please enter a valid VDS amount in multiples of ₹500");
            $('#AmountPayable').addClass('validate invalid');
            $('#errorPaymentAmount').show();
            checkvalidation = false;
            $('#AmountPayable')[0].scrollIntoView(true);
        }
        else if ((AmountPayable) % 500 != 0) {
            $('#errorPaymentAmount').html("Please enter a valid VDS amount in multiples of ₹500");
            $('#AmountPayable').addClass('validate invalid');
            checkvalidation = false;
            $('#errorPaymentAmount').show();
            $('#AmountPayable')[0].scrollIntoView(true);
        }
        else if (AmountPayable < VDSAmount || AmountPayable > maxVDSAmount) {
            $('#errorPaymentAmount').html("Please enter a valid VDS Amount Value, it should be in between min and max values.");
            $('#AmountPayable').addClass('validate invalid');
            checkvalidation = false;
            $('#errorPaymentAmount').show();
            $('#AmountPayable')[0].scrollIntoView(true);
        }
        else {
            $('#AmountPayable').removeClass('validate invalid');
            $('#errorPaymentAmount').html("");
            $('#errorPaymentAmount').hide();
        }
    }
    else {
        $('#errorPaymentAmount').html("Please enter a valid VDS amount in multiples of ₹500");
        $('#AmountPayable').addClass('validate invalid');
        $('#errorPaymentAmount').show();
        checkvalidation = false;
    }
    if (pANNo != "") {
        if (!Pancheck.test(pANNo)) {
            $('#errorPANNo').html("Please enter a valid PAN No.");
            $('#PANNo').addClass('validate invalid');
            checkvalidation = false;
            $('#errorPANNo').show();
            $('#PANNo')[0].scrollIntoView(true);
        }
        else {
            $('#errorPANNo').html("");
            $('#errorPANNo').hide();
            $('#PANNo').removeClass('validate invalid');

        }
    }
    if (mobile == "") {
        $('#errorMobileNumber').html("Please enter a valid Mobile No.");
        $('#Mobile').addClass('validate invalid');
        checkvalidation = false;
        $('#errorMobileNumber').show();
        $('#Mobile')[0].scrollIntoView(true);
    }
    else if (!mobilenocheck.test(mobile)) {
        $('#errorMobileNumber').html("Please enter a valid Mobile No.");
        $('#Mobile').addClass('validate invalid');
        checkvalidation = false;
        $('#errorMobileNumber').show();
        $('#Mobile')[0].scrollIntoView(true);
    }
    else {
        $('#errorMobileNumber').html("");
        $('#Mobile').removeClass('validate invalid');
        $('#errorMobileNumber').hide();
    }

    if (email == "") {
        $('#errorEmailAddress').html("Please enter a valid Email No.");
        $('#Email').addClass('validate invalid');
        checkvalidation = false;
        $('#errorEmailAddress').show();
        $('#Email')[0].scrollIntoView(true);
    }
    else if (!Emailregex.test(email)) {
        $('#errorEmailAddress').html("Please enter a valid Email No.");
        $('#Email').addClass('validate invalid');
        checkvalidation = false;
        $('#errorEmailAddress').show();
        $('#Email')[0].scrollIntoView(true);
    }
    else {
        $('#errorEmailAddress').html("");
        $('#Email').removeClass('validate invalid');
        $('#errorEmailAddress').hide();
    }

    if ($("#checkcheckbox")[0].checked) {
        $('.ca-checkcheckbox-help').hide();
    }
    else {
        $('.ca-checkcheckbox-help').show();
        checkvalidation = false;
    }

    if (checkvalidation)
        return true;
    else
        return false;
});


$(document).ready(function () {
    var callfrom = GetParameterValues('callfrom');
    var AccountNumber = GetParameterValues('ca_number');
    //var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    //var decrypted = CryptoJS.AES.decrypt(AccountNumber, key);
    //alert(decrypted);
    //console.log(decrypted.toString(CryptoJS.enc.Utf8));
    if ((AccountNumber != null && AccountNumber != "") && (callfrom != null && callfrom != "")) {
        if (callfrom == "home") {
            $('.loader-wrap').show();

            $('#first_name2').val(AccountNumber);
            $('.qproceed').click();
            $('#lblCANumber').addClass('active');

        }
    }
});

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}
