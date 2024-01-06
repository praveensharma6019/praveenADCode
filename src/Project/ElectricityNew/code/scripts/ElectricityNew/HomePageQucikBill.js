$(document).ready(function () {
    $('#iconholderid').hide();


});
$(document).keypress(function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13' && $('#caNumber').val().trim() != "") {
        ShowDetails();
    }
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

$('#iconholderid').click(function () {
    $("#caNumber").val("");
});
function showCrossIcon() {
    $('#lblCANumber').addClass('active');
    $('#caNumber').removeClass("validate invalid");
    if ($('#caNumber').val().length == 0) {
        $('#iconholderid').hide();
        $('#icrossid').click();
    }
    else {
        $('#iconholderid').show();
    }
}
function CloseFetchQuickPay() {
    $('#icrossid').click();
}
function FetchQuickPay(caNumber) {
    $('.loader-wrap').show();
    var accountNumber = caNumber == '' ? $('#caNumber').val() : caNumber;
    var myQuickBillPaymentUrl = $('#myQuickBillPaymentUrl').val();
    $.ajax({
        url: apiSettings + "/AccountsRevamp/PayBillRevamp",
        type: 'GET',
        data: { AccountNumber: getEncriptedKey(accountNumber), AmountPayable: $('#advanceAmmount').val(), Pay_PaymentGateway: "Proceed to Pay" },
        success: function (data) {
            $('.loader-wrap').hide();

            window.location.href = "http://" + $(location).attr('host') + '' + myQuickBillPaymentUrl + '?ca_number=' + getEncriptedKey(accountNumber);



        },
        error: function (xhr, textStatus, errorThrown) {

        }
    });
}

function ShowDetails() {
    if (window.innerWidth < 992) {

        $('#sampleBillModal').hide();
        // Mobile Specific Change
        var accountNumber = $('#caNumber').val();
        if (accountNumber != '' && $('#caNumber').val().length == 9) {


            ShowBillAndAdvanceBill();
            // let billModal = M.Modal.init(document.getElementById('billDetailsMobile'), { opacity: 0.7 });
            // billModal.open();
        }
        else {
            $('#caNumber').addClass("validate invalid");
        }

    }
    else {

        //console.log(decrypted.toString(CryptoJS.enc.Utf8));

        ShowBillAndAdvanceBill();
    }

}



function ShowBillAndAdvanceBill() {

    var BeforeLoadingText = $('#fetchBill').html();
    //$('#fetchBill').html('<svg viewBox="0 0 100 100" class="loader-white"> <circle cx="6" cy="50" r="6"> <animateTransform attributeName="transform" dur="1s" type="translate" values="0 15 ; 0 -15; 0 15" repeatCount="indefinite" begin="0.1"></animateTransform> </circle> <circle stroke="none" cx="30" cy="50" r="6"> <animateTransform attributeName="transform" dur="1s" type="translate" values="0 10 ; 0 -10; 0 10" repeatCount="indefinite" begin="0.2"></animateTransform> </circle> <circle stroke="none" cx="54" cy="50" r="6"> <animateTransform attributeName="transform" dur="1s" type="translate" values="0 5 ; 0 -5; 0 5" repeatCount="indefinite" begin="0.3"></animateTransform> </circle> </svg>');
    $('#fetchBill').html(`<span class="custom-loader-dots-box button-loader">
                                    <span class="custom-loader-dot custom-loader-dot-1"></span>
                                    <span class="custom-loader-dot custom-loader-dot-2"></span>
                                    <span class="custom-loader-dot custom-loader-dot-3"></span>
                                  </span>`);
    var accountNumber = $('#caNumber').val();
    $('.QBillDetails').hide();
    if (accountNumber != '' && $('#caNumber').val().length == 9) {
        var captcharesponse = grecaptcha.getResponse(recaptcha);
        $.ajax({
            url: apiSettings + "/AccountsRevamp/QuickPayRevampNew",
            type: 'GET',
            data: { AccountNumber: getEncriptedKey(accountNumber), recaptcha: captcharesponse },
            dataType: 'JSON',
            beforeSend: function () {
                $('.loader-wrap').show();
                dataLayer.push({
                    'event': 'quick_hp_bill_payment_proceed',
                    'eventCategory': 'Bill Payment',
                    'eventAction': 'Quick Bill Payment Proceed',
                    'eventLabel': 'Hero Form',
                    'business_user_id': $('#BusinessUserId').val(),
                    'login_status': $('#login_status').val(),
                    'ca_number': $('#caNumber').val(),
                    'page_type': $('head title').text()
                });
            },
            success: function (data) {

                $('#fetchBill').html(BeforeLoadingText);

                $('.qproceed').attr("disabled", "disabled");

                $(".QPayable").html(data.data["AmountPayable"]);

                if (data.data["Captcha"] == "Invalid") {
                    $('.qproceed').removeAttr("disabled", "disabled");
                    $('.QBillDetails').hide();
                    $("#Captchaerror").html("Please validate captcha to continue");
                    if ($('#login_status').val() == 'Guest User') {
                        dataLayer.push({
                            'event': 'quick_bill_payment_proceed_captcha_failed',
                            'eventCategory': 'Bill Payment',
                            'eventAction': 'Quick Bill Payment Proceed',
                            'eventLabel': ' Quick Bill Payment | Proceed',
                            'business_user_id': $('#BusinessUserId').val(),
                            'login_status': $('#login_status').val(),
                            'ca_number': $('#caNumber').val(),
                            'page_type': $('head title').text()
                        });
                    } 
                }
                else {
                    if (data.data["AccountNumber"] == "") {

                        $('#caNumber').addClass("validate invalid");

                        $('.qproceed').removeAttr("disabled", "disabled");
                        $('.QBillDetails').hide();
                        $('.ca-number-help').html("Enter valid CA Number and click Proceed");
                        $('.ca-number-help').attr("class", "help-text ca-number-help error-text");

                    }
                    else {
                        $('#caNumber').removeClass("validate invalid");
                        $(".QDuedate").css("color", "red");
                        if (window.innerWidth < 992) {
                            let billModal = M.Modal.init(document.getElementById('billDetailsMobile'), { opacity: 0.7 });
                            billModal.open();
                        }

                        $('.QPayAdvance').show();
                        $('.QBillDetails').removeClass("hide");
                        $('.ca-number-help').removeClass("error-text");
                        $('.QBillDetails').attr('style', 'display:block');
                        //$('.QBillDetails').removeClass(display);

                        if ((parseInt(data.data["SecurityDeposit"])) > 0) {
                            $('.SecurityDepositAmount').html(parseFloat(data.data["SecurityDeposit"]).toFixed(2));
                            $('.SecurityDepositText').html('Pay Security Deposit Pending');
                            $('.PayNowSecurity').show();
                        }
                        else {
                            $('.SecurityDepositAmount').html(parseFloat(data.data["SecurityDeposit"]).toFixed(2));
                            //$('.SecurityDepositText').html('No Security Amount Pending');
                            $('.SecurityDepositText').html(data.data["SecurityDepositMsg"]);
                            $('.PayNowSecurity').hide();
                        }
                        $(".SCANumber").html(data.data["AccountNumber"]);
                        $(".SConsumerName").html(capitalize(data.data["Name"]));
                        $(".SBookNumber").html(data.data["BookNumber"]);
                        $(".SCycleNumber").html(data.data["CycleNumber"]);
                        $(".SZone").html(data.data["Zone"]);
                        $('.hdnSecurityDepositAmount').val(data.data["SecurityDeposit"]);

                        if ((parseInt(data.data["PaymentVDSAmount"])) > 0) {
                            $(".hdnCurrentOutstanding").val(data.data["AmountPayable"]);
                            $('.CurrentOutstanding').html(parseFloat(data.data["AmountPayable"]).toFixed(2))
                            $('.VDSAmountCurrentOutstanding-hero').show();
                            $('.withoutOutstanding-hero').hide();
                            $('.VDSAmountData').hide();
                        }
                        else {
                            $('.VDSAmountCurrentOutstanding-hero').hide();
                            $('.VDSAmountData').show();
                            $('.withoutOutstanding-hero').show();
                        }
                        $(".spanAverageVDSAmount").html(data.data["AverageVDSAmount"]);
                        $(".spanMaxVDSAmount").html(data.data["MaxVDSAmount"]);

                        $('#fetchBill').attr("disabled", "disabled");
                        if ((parseInt(data.data["AmountPayable"])) > 0) {
                            $('.QPayBill').show();
                            $('.QPayAdvance').hide();
                            $(".QBillDetails .QAConsumerName").html(capitalize(data.data["Name"]));
                            $(".QBillDetails .QACANumber span").html(data.data["AccountNumber"]);
                            $(".QBillDetails .QACANumber").html(data.data["AccountNumber"]);


                            if (data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00") {
                                $(".QDuedate").html("Payment Due Date: Not Updated");
                            }
                            else {
                                if (data.data["DueDateGraterthenFourDays"]) {
                                    $(".QDuedate").html("Due on " + data.data["PaymentDueDate"]);
                                    // $(".QDuedate").css("color", "black");
                                }
                                else {
                                    $(".QDuedate").html("Due on " + data.data["PaymentDueDate"]);
                                }
                            }
                            $('.tooltipNoBillDue').show();
                            $(".QBillMonth").html(data.data["BillMonth"]);
                            $(".QUnitConsumed").html(data.data["UnitsConsumed"]);
                            $(".QTBillAmount").html("&#8377 " + data.data["TotalBillAmount"]);

                            $(".QMinimumPayable").html("&#8377 " + data.data["AmountPayable"]);
                            if ($('#login_status').val() == 'Guest User') {
                                dataLayer.push({
                                    'event': 'quick_bill_due_amount',
                                    'eventCategory': 'Bill Payment',
                                    'eventAction': 'Quick Bill Payment Proceed',
                                    'eventLabel': ' Quick Bill Payment | Proceed',
                                    'business_user_id': $('#BusinessUserId').val(),
                                    'login_status': $('#login_status').val(),
                                    'ca_number': $('#caNumber').val(),
                                    'page_type': $('head title').text(),
                                    'amount': data.data["AmountPayable"],
                                    'due_date': data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00" || parseInt(data.data["AmountPayable"]) == 0 ? 'No Bill Due' : data.data["PaymentDueDate"]
                                });
                            }                        
                        }
                        else {

                            $('.QPayAdvance').show();
                            $('.QPayBill').hide();
                            $('.QBillDetails').removeClass("hide");
                            $('.tooltipNoBillDue').hide();
                            // $('.logged-out hide').removeClass();
                            $(".QBillDetails .QAConsumerName").html(capitalize(data.data["Name"]));
                            $(".QBillDetails .QACANumber span").html(data.data["AccountNumber"]);
                            $(".QBillDetails .QACANumber").html(data.data["AccountNumber"]);
                            if (data.data["PaymentDueDate"] == null || data.data["PaymentDueDate"] == "0000-00-00") {
                                $(".QDuedate").html("No Bill Due");
                                $(".QDuedate").css("color", "black");
                                $(".tooltip-wrapper").hide();
                            }
                            else {

                                $(".QDuedate").html("No Bill Due");
                                $(".QDuedate").css("color", "black");
                            }

                            $(".QBillMonth").html(data.data["BillMonth"]);
                            $(".QUnitConsumed").html(data.data["UnitsConsumed"]);
                            $(".QTBillAmount").html("&#8377 " + data.data["TotalBillAmount"]);
                            $(".QMinimumPayable").html("&#8377 " + data.data["AmountPayable"]);
                            if ($('#login_status').val() == 'Guest User') {
                                dataLayer.push({
                                    'event': 'quick_bill_due_amount',
                                    'eventCategory': 'Bill Payment',
                                    'eventAction': 'Quick Bill Payment Proceed',
                                    'eventLabel': ' Quick Bill Payment | Proceed',
                                    'business_user_id': $('#BusinessUserId').val(),
                                    'login_status': $('#login_status').val(),
                                    'ca_number': $('#caNumber').val(),
                                    'page_type': $('head title').text(),
                                    'amount': data.data["AmountPayable"],
                                    'due_date': 'No Bill Due'
                                });
                            }          
                        }
                    }
                }
                $('.loader-wrap').hide();
            },
            error: function (xhr, textStatus, errorThrown, data) {
                $('#fetchBill').html(BeforeLoadingText);


            }
        });

    }
    else {
        $('#caNumber').addClass("validate invalid");
        $('#fetchBill').html(BeforeLoadingText);
    }
}

$('.hide-bill-details, #icrossid').click(function (e) {
    $('#caNumber').val('');
    $('#lblCANumber').removeClass('active');
    $('#caNumber').removeClass("validate invalid");
    // $('.ca-number-help').removeClass("error-text");
    $('#fetchBill').removeAttr("disabled");
    $(".QPayable").val('');
    $(".QBillDetails .QAConsumerName").val('');
    $(".QBillDetails .QACANumber span").val('');
    $(".QBillDetails .QACANumber").val('');
    $(".QDuedate").val('');
    $(".QBillMonth").val('');
    $(".QTBillAmount").val('');
    $(".QMinimumPayable").val('');
    $('.QPayBill').hide();
    $('.QPayAdvance').hide();
    $('.QBillDetails').hide();

    $('#iconholderid').hide();
    $('.ca-number-help').removeClass("error-text");
    $('.ca-number-help').html("Please enter your 9 digit consumer account number");

});

$('#billDetailsMobile .hide-bill-details, #billDetailsMobile + .modal-overlay').click(function () {
    $('#billDetailsMobile').hide();
    $('.modal-overlay').hide();
    $('body').css('overflow', 'visible');
});
function FetchQuickPaySecurity() {
    $('.loader-wrap').show();
    var accountNumber = $('.SCANumber').html();
    var SecurityDeposit = $(".hdnSecurityDepositAmount").val();
    var myQuickBillPaymentUrl = $('#myQuickBillPaymentUrl').val();
    //$.ajax({
    //    url: apiSettings + "/AccountsRevamp/PayBillRevamp",
    //    type: 'GET',
    //    // data: { AccountNumber: '@Model.AccountNumber', SecurityDeposit: parseInt($('#securityDepositPartial').val()) },
    //    data: { AccountNumber: accountNumber, SecurityDeposit: SecurityDeposit },
    //    dataType: 'text',
    //    success: function (data) {

    window.location.href = "http://" + $(location).attr('host') + '' + myQuickBillPaymentUrl + '?ca_number=' + getEncriptedKey(accountNumber) + '&SecurityDeposit=' + getEncriptedKey(SecurityDeposit);

    $('.loader-wrap').hide();
    //    },
    //    error: function (xhr, textStatus, errorThrown) {
    //        console.log('Error in Operation--' + errorThrown);
    //    }
    //});

}

$('.PayOutstandingBill').click(function () {
    var accountNumber = $('.SCANumber').html();
    var myQuickBillPaymentUrl = $('#myQuickBillPaymentUrl').val();
    $('.loader-wrap').show();
    $.ajax({
        url: apiSettings + "/AccountsRevamp/PayBillRevamp",
        type: 'GET',
        data: { AccountNumber: getEncriptedKey(accountNumber), AmountPayable: $("#hdnCurrentOutstanding").val(), Pay_PaymentGateway: "Proceed to Pay" },
        success: function (data) {

            window.location.href = "http://" + $(location).attr('host') + '' + myQuickBillPaymentUrl + '?ca_number=' + getEncriptedKey(accountNumber);
            $('.loader-wrap').hide();
        },
        error: function (xhr, textStatus, errorThrown) {

        }
    });

});

$('.btn-add-money').click(function () {
    var accountNumber = $('.SCANumber').html();
    var myCurrentBillUrl = $('#myCurrentBillUrl').val();
    window.location.href = "http://" + $(location).attr('host') + '' + myCurrentBillUrl + '?ca_number=' + accountNumber + '&callfrom=home';

});