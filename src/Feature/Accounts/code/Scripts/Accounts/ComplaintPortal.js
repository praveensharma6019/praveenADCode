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

    $(function () {
        $('#datetimepickerComplaintStartDate').datetimepicker(
            { format: 'DD/MM/YYYY' });
    });
    $(function () {
        $('#datetimepickerComplaintEndDate').datetimepicker({ format: 'DD/MM/YYYY' });
    });


    $('#datetimepickerComplaintFromPreviousLevelAppliedDate').datetimepicker(
        {
            format: 'DD/MM/YYYY hh:mm:ss'
            //format: 'DD/MM/YYYY'
        });

    $('#datetimepickerComplaintHearingDate').datetimepicker(
        {
            format: 'DD/MM/YYYY hh:mm:ss'
            //format: 'DD/MM/YYYY'
        });

    $("#searchCONStatusApp").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#CONApps tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});

$('.cexport').on("click", function () {
    var sdate = $("#datetimepickerComplaintStartDate").val();
    var edate = $("#datetimepickerComplaintEndDate").val();
    var status = $("#SelectedComplaintStatus").val();
    var zone = $("#SelectedConsumerZone").val();
    var division = $("#SelectedConsumerDivision").val();
    var category = $("#SelectedComplaintCategory").val();
    var url = $(this).attr('href') + '?startDate=' + sdate + '&endDate=' + edate + '&consumerZone=' + zone + '&consumerDivision=' + division + '&complaintstatus=' + status + '&complaintCategory=' + category;
    location.href = url;
    return false;
});

$('#ddlComplaintCategory').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});


$('#ddlComplaintSubCategory').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$("#fileComplaintSupportingDocs").on("change", function () {
    if ($("#fileComplaintSupportingDocs")[0].files.length > 5) {
        alert("Max 5 files are allowed");
        $("#fileComplaintSupportingDocs").val('');
    } else {
        return true;
    }
});

$("#frmComplaintRegistration").submit(function (event) {
    var buttonName = $(document.activeElement).attr('name');

    if (buttonName === "SubmitApplication") {
        if (!$('input[name="termsCb"]').is(':checked')) {
            $("#docErrorMessage").html("Please confirm terms and conditions by checking the check boxes.");
            return false;
        }
    }

    //$("#loader-wrapper").show();
    return true;
});

$("#frmComplaintRegistrationAdmin").submit(function (event) {
    var buttonName = $(document.activeElement).attr('name');

    if (buttonName == "CloseComplaint") {
        $('.confirmation_modal_message').html("Are you sure you want to Close the complaint?");
        $('#confirmation_modal').modal("show");
        event.preventDefault();
        return false;
    }

    //$("#loader-wrapper").show();
    return true;
});

$('.confirmation_modalbtnYesNO').click(function (e) {
    if (this.value === '1') {
        YesNO = '1';
        $('#confirmation_modal').modal("hide");
        $("#CloseComplaint").click();
    }
    else {
        $('#confirmation_modal').modal("hide");
    }
    e.preventDefault();
});

$('#ddlReasonToApply').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});


$('#ddlReasonToApplySubType').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});


