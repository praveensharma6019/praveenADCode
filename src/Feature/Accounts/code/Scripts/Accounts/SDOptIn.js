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


    $("#frmSDOptRegistration").submit(function (event) {
        var buttonName = $(document.activeElement).attr('name');

        if (buttonName === "Submit") {
            if (!$('input[name="termsCb"]').is(':checked')) {
                $("#docErrorMessage").html("Please confirm terms and conditions by checking the check box.");
                return false;
            }
        }

        //$("#loader-wrapper").show();
        return true;
    });

});