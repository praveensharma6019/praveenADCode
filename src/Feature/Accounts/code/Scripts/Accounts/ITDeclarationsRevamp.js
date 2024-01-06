$(document).ready(function () {
    $("#loader-wrapper").hide();
   
    if (localStorage['page'] == document.URL && localStorage['scrollTop'] != null && localStorage['scrollTop'] != "") {
        $(document).scrollTop(localStorage['scrollTop']);
    }
    else {
        $(document).scrollTop(0);
        localStorage['page'] = "";
        localStorage['scrollTop'] = "";
    }
    

    $("#frmITDeclarations").submit(function (event) {
        var buttonName = $(document.activeElement).attr('id');
        var captcharesponse = grecaptcha.getResponse(ITDeclerationcaptcha);
        if (buttonName === "btnValidateCA") {            
            var valid = true;
            if ($('#AccountNo').val() == "") {
                $("#lblCANumbererror").show().html('Please enter valid Account Number.');
               // $('#CAmsg').hide();
                valid = false;
            }
            else {
                $("#lblCANumbererror").hide().html('');
                $("#CANumbererror").hide().html('');
                $('#AccountNo').removeClass("invalid");
            }
           
            if (!captcharesponse) {
                $('#Captchaerror').html('Please validate captcha to continue');
                valid = false;
                if ($('#AccountNo').val() != "") {
                   // $('#CAmsg').show();
                    $("#lblCANumbererror").hide().html('');
                }
            }
            else {
                $('#Captchaerror').html('');
                return true;
            }
            if (!valid)
                return false;
            else
                return true;

        }


        if (buttonName === "btnfour") {
            var valid = true;
            if ($("#Amount_considered_for_TDS").val() == null || $("#Amount_considered_for_TDS").val() == "") {
                $("#Amount_considered_for_TDSerror").text("Please enter a valid Amount.");
                $("#TDS_Deducted").text("");
                $("#Net_amount_post_TDS_deduction").text("");
                valid = false;
            }
            else {
                $("#Amount_considered_for_TDSerror").hide().html('');
                $("#valAmount_considered_for_TDSerror").hide().html('');
                $('#Amount_considered_for_TDS').removeClass("invalid");
            }
            var Amount_considered_for_TDS = parseFloat($("#Amount_considered_for_TDS").val());
            var Bill_amount = parseFloat($("#Bill_Amount").val());
            if (Amount_considered_for_TDS > Bill_amount) {
                $("#error_Amount_considered_for_TDS").text("This amount should be less than or equal to 'Latest Bill Amount'. ");
                $("#TDS_Deducted").text("");
                $("#Net_amount_post_TDS_deduction").text("");
                valid= false;
            }
            //if (!$('input[name="termsCb"]').is(':checked')) {
            //    $("#docErrorMessage").html("Please accept.");
            //    valid= false;
            //}           
          
            if ($("#datetimepickerPOSTING_DATE").val() == "") {
                $("#POSTING_DATESerror").show().html('Please enter a valid Date of Bill payment.');
                valid = false;
            }
            else {
                $("#POSTING_DATESerror").hide().html('');
                $("#valPOSTING_DATESerror").hide().html('');
                $('#datetimepickerPOSTING_DATE').removeClass("invalid");
            }

            if (!captcharesponse) {
                $('#Captchaerror').html('Please validate captcha to continue.');
                valid = false;
            }
            else {
                $('#Captchaerror').html('');
            }
            if (!valid)
                return false;
            else
                return true;

        }
        else if (buttonName === "ValidateOTP") {
            var valid = true;
            if ($("#OTPNumber").val() == "") {
                $("#sendOtpMsg").show();
                valid = false;
            }
            else {
                $("#sendOtpMsg").hide();
                $("#ErrorOTPNumber").hide().html('');
                $('#OTPNumber').removeClass("invalid");
            }
            if (!captcharesponse) {
                $('#Captchaerror').html('Please validate captcha to continue');
                valid = false;

            }
            else {
                $('#Captchaerror').html('');
            }
            if (!valid)
                return false;
            else
                return true;
        }
        //$("#loader-wrapper").show();

        else if (buttonName === "btnfirst") {
            var valid = true;
            if ($("#AadharNumber").val() == "") {
                $("#AadharNumbererror").show().html('Please enter a valid 12 digit Aadhar Number.');
                valid = false;
            }
            else {
                $("#AadharNumbererror").hide().html('');
                $("#valAadharNumber").hide().html('');
                $('#AadharNumber').removeClass("invalid");
            }
            if (!captcharesponse) {
                $('#Captchaerror').html('Please validate captcha to continue.');
                valid = false;
            }
            else {
                $('#Captchaerror').html('');
            }
            if (!valid)
                return false;
            else
                return true;
        }
        else if (buttonName === "btnthird") {
            var valid = true;
            if ($("#FY_3AcknowledgementNumber").val() == "") {
                $("#FY_3AcknowledgementNumbererror").show().html('Please enter a valid Acknowledgement Number.');
                valid = false;
            }
            else {
                $("#FY_3AcknowledgementNumbererror").hide().html('');
                $("#valFY_3AcknowledgementNumber").hide().html('');
                $('#FY_3AcknowledgementNumber').removeClass("invalid");
            }
            if ($("#datetimepickerFY_3AcknowledgementNumber").val() == "") {
                $("#FY_3DateOfFilingReturnerror").show().html('Please enter a valid Filing Return Date.');
                valid = false;
            }
            else {
                $("#FY_3DateOfFilingReturnerror").hide().html('');
                $("#valFY_3DateOfFilingReturn").hide().html('');
                $('#datetimepickerFY_3AcknowledgementNumber').removeClass("invalid");
            }
            if ($("#FY_2AcknowledgementNumber").val() == "") {
                $("#FY_2AcknowledgementNumbererror").show().html('Please enter a valid Acknowledgement Number.');
                valid = false;
            }
            else {
                $("#FY_2AcknowledgementNumbererror").hide().html('');
                $("#valFY_2AcknowledgementNumber").hide().html('');
                $('#FY_2AcknowledgementNumber').removeClass("invalid");
            }
            if ($("#datetimepickerFY_2AcknowledgementNumber").val() == "") {
                $("#FY_2DateOfFilingReturn").show().html('Please enter a valid Filing Return Date.');
                valid = false;
            }
            else {
                $("#FY_2DateOfFilingReturn").hide().html('');
                $("#valFY_2DateOfFilingReturn").hide().html('');
                $('#datetimepickerFY_2AcknowledgementNumber').removeClass("invalid");
            }

            if (!captcharesponse) {
                $('#Captchaerror').html('Please validate captcha to continue.');
                valid = false;
            }
            else {
                $('#Captchaerror').html('');
            }
            if (!valid)
                return false;
            else
                return true;
        }

       
        return true;
    });

    $(function () {
        $('#datetimepickerFY_3AcknowledgementNumber').datepicker(
            { format: 'dd/mm/yyyy', maxDate: new Date, minDate: new Date(2018, 3, 1) });
        $('#datetimepickerFY_3AcknowledgementNumber').val('');
    });

    $(function () {
        $('#datetimepickerFY_2AcknowledgementNumber').datepicker(
            { format: 'dd/mm/yyyy', maxDate: new Date, minDate: new Date(2019, 3, 1) });
        $('#datetimepickerFY_2AcknowledgementNumber').val('');
    });

    $(function () {       
        var date = new Date();
        date.setDate(date.getDate() - 4);
        $('#datetimepickerPOSTING_DATE').datepicker(
            { format: 'dd/mm/yyyy', maxDate: new Date, minDate: date });
        $('#datetimepickerPOSTING_DATE').val('');
    });

    $('#datetimepickerPOSTING_DATE, #datetimepickerFY_2AcknowledgementNumber, #datetimepickerFY_3AcknowledgementNumber').keypress(function (event) {
        event.preventDefault();
        return false;
    });
   
    
});


function onchangeAmount_considered_for_TDS() {
    $("#TDS_Deducted").text("");
    $("#Net_amount_post_TDS_deduction").text("");
    if ($("#Amount_considered_for_TDS").val() == null || $("#Amount_considered_for_TDS").val() == "") {
        //$("#error_Amount_considered_for_TDS").text("Please enter a value.");
        return false;
    }
    var Amount_considered_for_TDS = parseFloat($("#Amount_considered_for_TDS").val());
    var Bill_amount = parseFloat($("#Bill_Amount").val());
    if (Amount_considered_for_TDS > Bill_amount) {
        $("#error_Amount_considered_for_TDS").text("This amount should be less than or equal to 'Latest Bill Amount'. ");
        return false;
    }
    else {
        $("#error_Amount_considered_for_TDS").text("");
        var TDS_Deducted = Math.ceil(Amount_considered_for_TDS * 0.1 / 100);
        var Net_amount_post_TDS_deduction = Math.ceil(Amount_considered_for_TDS - TDS_Deducted);
        $("#TDS_Deducted").text(TDS_Deducted.toFixed(2));
        $("#Net_amount_post_TDS_deduction").text(Net_amount_post_TDS_deduction.toFixed(2));
    }
}