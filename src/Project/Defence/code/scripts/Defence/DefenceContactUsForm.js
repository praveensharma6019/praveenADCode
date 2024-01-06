$("#cmessageType").on("change", function () {
    var fileattachment = $(this).val();
    if (fileattachment == "Careers") {
        $("#ResumeAttached").show();
    } else {
        $("#ResumeAttached").hide().prop('required',false);
    }
});
$("#btnsubmit").on('click', function () {

	var fileInput =document.getElementById('ResumeAttachment');
	var filePath = fileInput.value;
        var allowedExtensions = /(\.pdf|\.doc|\.docx)$/i;
        if (!allowedExtensions.exec(filePath)) 
	{
        alert('Invalid file type');
        fileInput.value = '';
        return false;
	}

    var message = '';
	var namemessage = '';
	var emailmessage = '';
	var contactmessage = '';
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
            emailmessage = "Invalid Email Address </br>";
			$("#cmailid").next(".required").html(emailmessage);
            error = true;
        }
    }
    var name = $("#cname").val();
    if (name != "") {
        var IsValidName = validateName(name);
        if (!IsValidName) {
            namemessage = "Invalid Name </br>";
			$("#cname").next(".required").html(namemessage);
            error = true;
        }
    }
	var number = $("#ccontactno").val();
    if (number != "") {
        var IsValidNumber = validatePhone(number);
        if (!IsValidNumber) {
            contactmessage = "Invalid Number </br>";
			$("#ccontactno").next(".required").html(contactmessage);
            error = true;
        }
    }
    var MessageType = $("#cmessageType").val();
    var Message = $("#cmessage").val();
    if (Message != "") {
        var IsValidMessage = validateMessage(Message);
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
	function validatePhone(txtPhone) {
    
    var filterr = /^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/;
    if (filterr.test(txtPhone)) {
        return true;
    }
    else {
        return false;
    }
	};


});
function validateSize(input) {
  const fileSize = input.files[0].size / 1024 / 1024; // in MiB
  if (fileSize > 3) {
    alert('File size exceeds 3 MB!!');
     $(file).val(''); //for clearing with Jquery
  } else {
    // Proceed further
  }
}





