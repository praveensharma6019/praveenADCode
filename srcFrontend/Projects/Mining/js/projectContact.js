var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}

$("#project-btn").on('click', function (e) {
    getCaptchaResponseForm();
    e.preventDefault();

});

function getCaptchaResponseForm(e) {

    // debugger;

    if (!$("#name-id").val()) {
        $("#name-id").next().html('Please Enter Your Name!');
        return false;
    } else {
        $("#name-id").next().html("");
    }

    if (!/^[a-zA-Z ]+$/.test($("#name-id").val())) {
        $("#name-id").next().html('Name should contains only alphabets');
        return false;
    } else {
        $("#name-id").next().html("");
    }


    if (!$("#email-id").val()) {
        $("#email-id").next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#email-id").next().html("");
    }


    if (validateEmail($("#email-id").val()) == false) {
        $("#email-id").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#email-id").next().html("");
    }


    if (!/^\d*(?:\.\d{1,2})?$/.test($("#mobile-No").val())) {
        $("#mobile-No").next().html("Mobile number should be numeric.");
        return false;
    } else {
        $("#mobile-No").next().html("");
    }

    if (!$("#mobile-No").val()) {
        $("#mobile-No").next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#mobile-No").next().html("");
    }


    if ($("#mobile-No").val().length !== 10) {
        $("#mobile-No").next().html("Please enter 10 digit Mobile Number!");
        return false;
    } else {
        $("#mobile-No").next().html("");
    }


    if (!document.getElementById("ddlSubject").value) {
        $("#ddlSubject").next().html('Please Select Solution Type');
        return false;
    } else {
        $("#ddlSubject").next().html("");
    }


    if (!$("#customerQuery").val()) {
        $("#customerQuery").next().html('Please Enter Your Message!');
        return false;
    } else {
        $("#customerQuery").next().html("");
    }

    if (!/^[a-zA-Z ]+$/.test($("#customerQuery").val())) {
        $("#customerQuery").next().html('Message should contains only alphabets');
        return false;
    } else {
        $("#customerQuery").next().html("");
    }





    var savecontactdata = {
        Fullname: $("#name-id").val(),
        Email: $("#email-id").val().trim(),
        ContactNo: $("#mobile-No").val(),
        SelectType: $("#ddlSubject").val(),
        Message: $("#customerQuery").val(),
        reResponse: token
    };


    $.ajax({
        type: "POST",
        data: JSON.stringify(savecontactdata),
        url: "formsapi/SitecoreForms/submitform",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                window.location.href = "https://www.google.com";
            }
            else if (data.status == "2") {
                alert("Invalid Captcha!!");
                $('#project-btn').removeAttr('disabled');
            }
            else if (data.status == "3") {
                alert("Please fill all required fields with valid information");
                $('#project-btn').removeAttr('disabled');
            }
            else if (data.status == "4") {
                alert("Please enter a valid Subjecct");
                $('#project-btn').removeAttr('disabled');
            }
            else {
                alert("System operation failed. Please try again later!!"); $('#project-btn').removeAttr('disabled');

            }
        }
    });


}