//Contact Us Form
$("#ContactUsButton").click(function (e) {
    $('#ContactUsButton').attr('disabled', 'disabled');
	if(!$("#invalidCheck2").prop('checked'))
	{
		$(".TACError").css('display','block')
		$('#ContactUsButton').removeAttr('disabled');
        return false;
	}
    if ($("#Fullname").val() == '' || $("#Email").val().trim() == '' || $("#ContactNo").val() == '' || $('#Purpose').val() == '') {
        $("#Fullname").blur()
        $("#Email").blur()
        $("#ContactNo").blur()
        $("#Purpose").blur()
        $('#ContactUsButton').removeAttr('disabled');
        return false;
    }
    getCaptchaResponseForm();
    e.preventDefault();

});
$("#invalidCheck2").click(function () {
    if($("#invalidCheck2").prop('checked'))
	{
		$(".TACError").css('display','none')
	}
});
$("#Fullname").blur(function () {
    var FullNameRegex = /^[A-Za-z ]{3,100}$/g;
    if (!FullNameRegex.test($("#Fullname").val())) {
        $("#Fullname").next().next().css('display','block')
        return false;
    } else {
        $("#Fullname").next().next().css('display','none')
    }
});
$("#Email").blur(function () {
    var EmailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/g;
    if (!EmailRegex.test($("#Email").val())) {
        $("#Email").next().next().css('display','block')
        return false;
    } else {
        $("#Email").next().next().css('display','none')
    }
});
$("#ContactNo").blur(function () {
    var ContactRegex = /^\d{10}$/g;
    if (!ContactRegex.test($("#ContactNo").val())) {
        $("#ContactNo").next().next().css('display','block')
        return false;
    } else {
        $("#ContactNo").next().next().css('display','none')
    }
});
$("#Purpose").blur(function () {
    var MessageRegex = /^[A-Za-z0-9;,.?_\-!@&:""'\/\\ ]{0,1000}$/g;
    if (!MessageRegex.test($("#Purpose").val())) {
        $("#Purpose").next().next().css('display','block')
        return false;
    } else {
        $("#Purpose").next().next().css('display','none')
    }
});

function getCaptchaResponseForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lffy6MmAAAAAJWCrNIqaaSIz3mHsEy_xpKrmiaY', { action: 'ContactUsForm' }).then(function (token) {
            $('.g-recaptcha').val(token);



            var savecontactdata = {
                Fullname: $("#Fullname").val(),
                Email: $("#Email").val().trim(),
                ContactNo: $("#ContactNo").val(),
                Purpose: $("#Purpose").val(),
                reResponse: token
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(savecontactdata),
                url: "/api/AdaniWind/ContactUsForm",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == "1") {
                        window.location.href = "/thank-you";
                    }
                    else {
                        alert("System operation failed. Please try again later!!"); $('#ContactUsButton').removeAttr('disabled');

                    }
                }
            });
        });
    });
}





