$(document).ready(function () {

    $("#frmRegister").submit(function (event) {

        var selectedPG = $("input[name='PaymentGateway']:checked").val();
        if (selectedPG == "7") {
            event.preventDefault();
            window.open('https://www.bharatbillpay.com/Billpay.php', '_blank');
        }
        else {
            return true;
        }
    });


    $("#frmPayOnline").submit(function (event) {

        var selectedPG = $("input[name='PaymentGateway']:checked").val();
        if (selectedPG == "7") {
            event.preventDefault();
            window.open('https://www.bharatbillpay.com/Billpay.php', '_blank');
        }
        else {
            return true;
        }
    });

});

function EditEmail(emailToEdit) {
    $("#oldEmailId").val(emailToEdit);
    $('#EditEmailModal').modal('show');
}

function validateEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function UpdateEmail() {
    var emailToDelete = $("#oldEmailId").val();
    var newEmailID = $("#newEmailId").val();
    if (newEmailID.trim() == "") {
        $("#newEmailIdError").text("Please enter new email ID");
        return false;
    }
    if (validateEmail(newEmailID) == false) {
        $("#newEmailIdError").text("Please enter valid email ID");
        return false;
    }
    var param = {
        emailToUpdate: emailToDelete, newEmailID: newEmailID
    };
    jQuery.ajax(
        {
            dataType: "json",
            url: "/api/AdaniGas/ModifyEmailAdaniGas",
            type: "POST",

            data: param,
            success: function (data) {
                window.location.reload();
            },
            error: function (error, ex, er) {

            }
        });
}

function DeleteEmail(emailToDelete) {
    if (confirm("Are you sure you want to delete this?")) {
        jQuery.ajax(
            {
                dataType: 'JSON',
                url: "/api/AdaniGas/DeleteEmailAdaniGas",
                type: "POST",

                data: {
                    'emailToDelete': emailToDelete
                },
                success: function (data) {
                    window.location.reload();
                },
                error: function (error, ex, er) {

                }
            });
    }
    else {
        return false;
    }
}

function EditMobile(mobileNumberToEdit) {
    $("#oldMobileNumber").val(mobileNumberToEdit);
    $('#EditMobileModal').modal('show');
}

function validateMobile(Mobile) {
    if (!Mobile.match('[0-9]{10}')) {
        return false;
    }
    return true;
}

function UpdateMobile() {
    var mobileToDelete = $("#oldMobileNumber").val();
    var newMobileNumber = $("#newMobileNumber").val();
    if (newMobileNumber.trim() == "") {
        $("#newMobileNumberError").text("Please enter new Mobile Number");
        return false;
    }
    if (validateMobile(newMobileNumber) == false) {
        $("#newMobileNumberError").text("Please enter 10 digit valid Mobile Number");
        return false;
    }
    var param = {
        mobileNumberToUpdate: mobileToDelete, newMobileNumber: newMobileNumber
    };
    jQuery.ajax(
        {
            dataType: 'JSON',
            url: "/api/AdaniGas/ModifyMobileAdaniGas",
            type: "POST",

            data: param,
            success: function (data) {
                window.location.reload();
            },
            error: function (error, ex, er) {

            }
        });
}

function DeleteMobile(MobileToDelete, obj) {
    if (confirm("Are you sure you want to delete this?")) {
        jQuery.ajax(
            {
                dataType: "json",
                url: "/api/AdaniGas/DeleteMobileAdaniGas",
                type: "POST",

                data: {
                    'mobileNumberToDelete': MobileToDelete
                },
                success: function (data) {
                    window.location.reload();
                },
                error: function (error, ex, er) {

                }
            });
    }
    else {
        return false;
    }
}

$(':radio[name=PaymentGateway]').change(function () {
    if ($('input[name=PaymentGateway]:checked').val() == 3) {
        $("#paytmTable").show();
        $("#payUTable").hide();
        $("#bdeskTable").hide();
        $("#hdfcTable").hide();
    } else if ($('input[name=PaymentGateway]:checked').val() == 1) {
        $("#paytmTable").hide();
        $("#payUTable").show();
        $("#bdeskTable").hide();
        $("#hdfcTable").hide();
    }
    else if ($('input[name=PaymentGateway]:checked').val() == 7) {
        $("#paytmTable").hide();
        $("#payUTable").hide();
        $("#bdeskTable").hide();
        $("#hdfcTable").hide();
    }
    else if ($('input[name=PaymentGateway]:checked').val() == 8) {
        $("#paytmTable").hide();
        $("#payUTable").hide();
        $("#bdeskTable").hide();
        $("#hdfcTable").show();
    }
    else {
        $("#paytmTable").hide();
        $("#payUTable").hide();
        $("#bdeskTable").show();
        $("#hdfcTable").hide();
    }
});

function showOtherPG() {
    $(".opg").show();
    $("#opg").attr('disabled', true);
}

