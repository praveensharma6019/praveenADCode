$(document).ready(function () {
    if ($("#updateMobilehdn").val() == "1") {
        $("#MobileNumber").val("");
    }

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


    $("#frmGreenPowerOptInRegistration").submit(function (event) {
        var buttonName = $(document.activeElement).attr('name');

        if (buttonName === "Accept") {
            if (!$('input[name="termsCb"]').is(':checked')) {
                $("#docErrorMessage").html("Please accept switching to green energy sources.");
                return false;
            }
        }

        //$("#loader-wrapper").show();
        return true;
    });

    $("#frmGreenPowerOptInRegistration1").submit(function (event) {
        var buttonName = $(document.activeElement).attr('name');
        if (buttonName === "SubmitCapture") {
            if (!$('input[name="termsCbPledge"]').is(':checked')) {
                $("#docErrorMessage").html("Please accept switching to green energy sources.");
                return false;
            }
        }
        return true;
    });

});