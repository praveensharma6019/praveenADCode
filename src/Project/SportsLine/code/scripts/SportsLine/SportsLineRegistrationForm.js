$("#registration_form #Gender").change(function () {
    if ($("#registration_form #Gender").val() == 'male') {
        $('#selectsport').show();
        $('#cricket').show();
        $('#football').show();
        $('#tugofwar').hide();

    }
    else if ($("#registration_form #Gender").val() == 'Female') {
        $('#selectsport').show();
        $('#cricket').show();
        $('#football').show();
        $('#tugofwar').show();
    }
});

   



$("#btnsubmits").on('click', function () {

    var message = '';
    var error = false;
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        message = message + "Captcha required";
        error = true;
    }
    $("#reResponse").val(response);
    var EmailId = $("#cmailid").val();
    if (EmailId != "") {
        var IsValidMail = validateEmail(EmailId);
        if (!IsValidMail) {
            message = message + "Invalid Email Address </br>";
            error = true;
        }
    }
    var name = $("#cname").val();
    if (name != "") {
        var IsValidName = validateName(name);
        if (!IsValidName) {
            message = message + "Invalid Name </br>";
            error = true;
        }
    }
    if ($('#defaultCheck2').is(":not(:checked)")) {
        alert("Please accept the confirmation of details"); $("#defaultCheck2").focus(); $('#btnsubmits').removeAttr("disabled"); return false;
    }

    var Lastname = $("#clastname").val();
    if (Lastname != "") {
        var IsValidName = validateName(Lastname);
        if (!IsValidName) {
            message = message + "Invalid LastName </br>";
            error = true;
        }
    }
    var Gender = $("#Gender").val();
    var address = $("#cAddress").val();
    if (address != "") {
        var IsValidMessage = validateMessage(address);
        if (!IsValidMessage) {
            message = message + "Invalid Message </br>";
            error = true;
        }
    }
    function validateMessage(smessage) {
        var letterNumber = /[a-zA-Z0-9,. ]/;
        if (letterNumber.test(smessage)) { return true; }
        else { return false; }
    }



    if (error) {
        $("#docErrorMessage").html(message);
        $("#docErrorMessage").focus();
        return false;
    }
    else $("#docErrorMessage").html("");
    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    };
    function validateName(sname) {
        var regex = /^[a-zA-Z ]+$/;

        if (regex.test(sname)) { return true; }
        else { return false; }
    };


});





