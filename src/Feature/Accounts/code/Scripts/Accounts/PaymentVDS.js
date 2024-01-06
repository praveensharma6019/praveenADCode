function AddMoneyVDS() {
    location.reload();
}

$('.PayOutstandingBill').click(function () {
    var accountNumber = $('#first_name2').val();
    $('.loader-wrap').show();
    $.ajax({
        url: apiSettings + "/AccountsRevamp/PayBillRevamp",
        type: 'GET',
        data: { AccountNumber: getEncriptedKey(accountNumber), AmountPayable: $('#currentBillAmount').val(), Pay_PaymentGateway: "Proceed to Pay" },
        success: function (data) {
            $('.loader-wrap').hide();
            console.log(data);
            window.location.href = "http://" + $(location).attr('host') + '/Payment/pay-your-bill?ca_number=' + getEncriptedKey(accountNumber);

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation--' + errorThrown);

            //window.location.href = "http://" + $(location).attr('host') + '/Payment/pay-your-bill?ca_number=' + accountNumber;
        }
    });

});
$("#frmPaymentVDS").submit(function (event) {
    if ($("#checkcheckbox")[0].checked) {
        $('.ca-checkcheckbox-help').hide();
        e.preventDefault();
        $('.loader-wrap').show();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/PaymentVDS",
            data: $(this).serialize(),
            method: 'post',
            dataType: 'JSON'
        }).done(function (resp) {
            $('.loader-wrap').hide();
        });
    }
    else {
        $('.ca-checkcheckbox-help').show();
        return false;
    }
});

$(".PayVDSBill").click(function () {

    var AmountPayable = parseFloat($('#PaymentAmount').val());
    var pANNo = $('#PANNo').val();
    var mobile = $('#MobileNumber').val();
    var email = $('#EmailAddress').val();
    var VDSAmount = parseFloat($('#spanAverageVDSAmount').html());
    var maxVDSAmount = parseFloat($('#spanMaxVDSAmount').html());
    var Pancheck = new RegExp("^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$");
    var mobilenocheck = new RegExp("^[0-9]{10,10}$");
    var Emailregex = new RegExp("^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$");
    var checkvalidation = true;
    if (AmountPayable != 0) {
        if (AmountPayable <= 0) {
            $('#errorPaymentAmount').html("Please enter a valid VDS Amount Value, it should be multiple of 500!");
            $('#AmountPayable').addClass('validate invalid');
            $('#errorPaymentAmount').show();
            checkvalidation = false;
            $('#AmountPayable')[0].scrollIntoView(true);
        }
        else if ((AmountPayable) % 500 != 0) {
            $('#errorPaymentAmount').html("Please enter a valid VDS Amount Value, it should be multiple of 500!");
            $('#AmountPayable').addClass('validate invalid');
            $('#errorPaymentAmount').show();
            checkvalidation = false;
            $('#AmountPayable')[0].scrollIntoView(true);
        }
        else if (AmountPayable < VDSAmount || AmountPayable > maxVDSAmount) {
            $('#errorPaymentAmount').html("Please enter a valid VDS Amount Value, it should be in between min and max values.");
            $('#AmountPayable').addClass('validate invalid');
            $('#errorPaymentAmount').show();
            checkvalidation = false;
            $('#AmountPayable')[0].scrollIntoView(true);
        }
        else {
            $('#AmountPayable').removeClass('validate invalid');
            $('#errorPaymentAmount').html("");
            $('#errorPaymentAmount').hide();
        }
    }
    else {
        $('#errorPaymentAmount').html("Please enter a valid VDS Amount Value, it should be multiple of 500!");
        $('#AmountPayable').addClass('validate invalid');
        $('#errorPaymentAmount').show();
        checkvalidation = false;
    }
    if (pANNo != "") {
        if (!Pancheck.test(pANNo)) {
            $('#errorPANNo').html("Please enter a valid PAN No.");
            $('#PANNo').addClass('validate invalid');
            $('#errorPANNo').show();
            checkvalidation = false;
            $('#PANNo')[0].scrollIntoView(true);
        }
        else {
            $('#errorPANNo').html("");
            $('#PANNo').removeClass('validate invalid');
            $('#errorPANNo').hide();
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