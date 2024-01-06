$("#cmessageType").on("change", function () {
    var fileattachment = $(this).val();
    if (fileattachment == "Careers") {
        $("#ResumeAttached").show();
    } else {
        $("#ResumeAttached").hide();
    }
});
$("#btnsubmit").on('click', function () {

    var message = '';
    var error = false;
   
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
    var MessageType = $("#cmessageType").val();
    var message = $("#cmessage").val();
    if (message != "") {
        var IsValidMessage = validateMessage(message);
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





