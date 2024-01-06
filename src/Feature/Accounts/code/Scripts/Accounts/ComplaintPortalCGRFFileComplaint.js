$(document).ready(function () {
    if ($("#ComplaintFromPreviousLevel").val() == null || $("#ComplaintFromPreviousLevel").val() == "") {
        $("#datetimepickerComplaintFromPreviousLevelAppliedDate").val(null)
        $('#onload_confirmation_modal').modal("show");
    }

    var reason = $("#ddlReasonToApply").val();
    if (reason == "Unredressed within resolution period") {
        $("#divReasonToApplySubType").show();
    }

    reason = $("#ddlReasonToApplySubType").val();
    if (reason == "Others type complaint : 15 Days") {
        $("#divReasonToApplySubTypeOtherText").show();
    }


});

$('.onload_confirmation_modalbtnYesNO').click(function (e) {
    if (this.value === '1') {
        YesNO = '1';
        $('#onload_confirmation_modal').modal("hide");
    }
    if (this.value === '2') {
        YesNO = '2';
        $('#onload_confirmation_modal').modal("hide");
        window.location.href = "https://www.adanielectricity.com/complaint-registration-home/complaint-registration-cgrf-home";
    }
    else {
        //$('.onload_confirmation_modal_message').html("Kindly contact <a href='#'>helpdesk.mumbaielectricity@adani.com</a> for complaint registration. or register the complaint to Helpdesk i.e. Level 1 from <a href='/complaint-registration-home/complaint-registration-helpdesk-home/complaint-registration-file-complaint'>here</a>");
        $('.onload_confirmation_modal_message').html("Kindly contact <a href='#'>helpdesk.mumbaielectricity@adani.com</a> for complaint registration or dial 19122 from mobile.");
        $(".onload_confirmation_modal_message_ok").show();
        $(".onload_confirmation_modalbtnYesNO").hide();
    }
    e.preventDefault();
});

$('.onload_confirmation_modal_message_ok').click(function (e) {
    if (this.value === '2') {
        YesNO = '2';
        $('#onload_confirmation_modal').modal("hide");
        window.location.href = "/complaint-registration-home/complaint-registration-cgrf-home";
    }
    e.preventDefault();
});


