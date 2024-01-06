$(document).ready(function () {
    $("#emiOption").change(function () {
        if (this.checked) {
            $("#divEmiInstallment").show();
            $("#divAdvanceAmount").hide();
            $("#amountPayable").attr('readonly', 'readonly');
        }
        else {
            $("#divEmiInstallment").hide();
            $("#divAdvanceAmount").show();
            if ($("#amountPayable").val() > 0) {
                $("#amountPayable").removeAttr('readonly');
            }
        }
    });

    var YesNO = "";
    $("#frmRegister").submit(function (event) {

        var buttonName = $(document.activeElement).attr('name');
        if (buttonName === "Pay_PaymentGateway") {
            var actualAmount = $("#amount_payable_actual").val();
            var newAmount = $("#amountPayable").val();

            if ($("#emiOption").is(':checked')) {
                if (YesNO !== "1") {
                    var EMIOutstandingAmount = $("#EMIOutstandingAmount").val();
                    var EMIInstallmentAmount = $("#EMIamount").val();
                    $("#modelEMIOutstandingAmount").html($("#EMIOutstandingAmount").val());
                    $("#modelEMIamount").html($("#EMIamount").val());
                    $('#emi_model').modal("show");
                    return false;
                }
                else {
                    return true;
                }
            }

            if (actualAmount > 0 && actualAmount >= 100 && newAmount < 100) {
                $('#partpaymentmessage').html("Minimum Amount payable Value is 100. Please enter valid amount.");
                $('#default_modal').modal("show");
                event.preventDefault();
                return false;
            }

            if (actualAmount > 0 && newAmount !== actualAmount) {
                if (YesNO !== "1") {
                    $('#partpaymentmessage1').html("You are paying an amount which is different from the amount payable, please confirm before proceeding.");
                    $('#default_modal2').modal("show");
                    return false;
                }
                else {
                    return true;
                }

            }
            else {
                return true;
            }
        }
        else {
            return true;
        }

        //event.preventDefault();
        //return false;
    });


    $('.btnYesNO').click(function (e) {
        //alert("btnYesNO");

        if (this.value === '1') {
            YesNO = '1';
            //$("#frmRegister").submit();
            $("#Pay_PaymentGateway").click();
        }
        else {
            $('#default_modal2').modal("hide");
            $('#amountPayable').focus();
        }
        e.preventDefault();
    });

});


