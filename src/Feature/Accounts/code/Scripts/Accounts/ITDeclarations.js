$(document).ready(function () {
    $("#loader-wrapper").hide();
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {
        $('#message_modal').modal('show');
        $("#message").val("");
    }

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

        if (buttonName === "submittds") {
            if ($("#Amount_considered_for_TDS").val() == null || $("#Amount_considered_for_TDS").val() == "") {
                //$("#error_Amount_considered_for_TDS").text("Please enter a value.");
                $("#TDS_Deducted").text("");
                $("#Net_amount_post_TDS_deduction").text("");
                return false;
            }
            var Amount_considered_for_TDS = parseFloat($("#Amount_considered_for_TDS").val());
            var Bill_amount = parseFloat($("#Bill_Amount").val());
            if (Amount_considered_for_TDS > Bill_amount) {
                $("#error_Amount_considered_for_TDS").text("This amount should be less than or equal to 'Latest Bill Amount'. ");
                $("#TDS_Deducted").text("");
                $("#Net_amount_post_TDS_deduction").text("");
                return false;
            }
            if (!$('input[name="termsCb"]').is(':checked')) {
                $("#docErrorMessage").html("Please accept.");
                return false;
            }
        }

        //$("#loader-wrapper").show();
        return true;
    });

    $(function () {
        $('#datetimepickerFY_3AcknowledgementNumber').datetimepicker(
            { format: 'DD/MM/YYYY', maxDate: new Date, minDate: new Date(2015, 1, 1) });
        $('#datetimepickerFY_3AcknowledgementNumber').val('');
    });

    $(function () {
        $('#datetimepickerFY_2AcknowledgementNumber').datetimepicker(
            { format: 'DD/MM/YYYY', maxDate: new Date, minDate: new Date(2015, 1, 1) });
        $('#datetimepickerFY_2AcknowledgementNumber').val('');
    });

    $(function () {
        var date = new Date();
        date.setDate(date.getDate() - 4);
        $('#datetimepickerPOSTING_DATE').datetimepicker(
            { format: 'DD/MM/YYYY', maxDate: new Date, minDate: date });
        $('#datetimepickerPOSTING_DATE').val('');
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