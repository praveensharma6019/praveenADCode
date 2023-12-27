var namecheck = new RegExp(/^[a-zA-Z. ]{0,30}$/);
var mobilenocheck = new RegExp(/^[6-9]\d{9}$/);
var emaillcheck = new RegExp(/^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$/);

$('#fullName').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var disallowedRegex = namecheck;
    if (!disallowedRegex.test(inputValue)) {
        $(this).val(inputValue.replace(/[^a-zA-Z. ]/g, '').substring(0, 30));
    }
});

$('#contactnumber').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var allowedRegex = /^[6-9]\d{9}$/;
    if (!allowedRegex.test(inputValue)) {
        $(this).val(inputValue.replace(/\D/g, '').substring(0, 10));
    }
});

$('#email').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var disallowedRegex = emaillcheck;
    if (!disallowedRegex.test(inputValue)) {

        $(this).val(inputValue.replace(disallowedRegex, ''));
    }
});

$('#organization').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var disallowedRegex = namecheck;
    if (!disallowedRegex.test(inputValue)) {
        $(this).val(inputValue.replace(/[^a-zA-Z. ]/g, '').substring(0, 30));
    }
});

$('#callBackSubmit').click(function () {
    //var nameRegex = /^[a-zA-Z ]+$/;
    var isvalid = true;
    if ($('#fullName').val() == "") {
        $('#fullName').closest('.form-floating').find('p.field-validation-error').html("First Name is required").show();
        isvalid = false;
    } else {
        if (!namecheck.test($('#fullName').val())) {
            $('#fullName').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid First Name.").show();
            isvalid = false;
        } else {
            $('#fullName').closest('.form-floating').find('p.field-validation-error').html("");
        }
    }
    if (!mobilenocheck.test($('#contactnumber').val())) {
        $('#contactnumber').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid Mobile number").show();
        isvalid = false;
    } else {
        $('#contactnumber').closest('.form-floating').find('p.field-validation-error').hide();
    }
    if (!emaillcheck.test($('#email').val())) {
        $('#email').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid Email").show();
        isvalid = false;
    } else {
        $('#email').closest('.form-floating').find('p.field-validation-error').hide();
    }
    if ($('#organization').val() == "") {
        $('#organization').closest('.form-floating').find('p.field-validation-error').html("Organization is required").show();
        isvalid = false;
    } else {
        if (!namecheck.test($('#organization').val())) {
            $('#organization').closest('.form-floating').find('p.field-validation-error').html("organization is required").show();
            isvalid = false;
        } else {
            $('#organization').closest('.form-floating').find('p.field-validation-error').html("");
        }
    }
    if (!isvalid)
        return false;
    else
        return true;
});
