var namecheck = new RegExp(/^[a-zA-Z. ]{0,30}$/);
var mobilenocheck = new RegExp(/^[6-9]\d{9}$/);
var emaillcheck = new RegExp(/^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$/);
var pnrcheck = new RegExp(/^[a-zA-Z0-9\b]{6}$/);
var datecheck = new RegExp(/^\d{4}-\d{2}-\d{2}$/);

$('#firstName').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var disallowedRegex = namecheck;
    if (!disallowedRegex.test(inputValue)) {
        $(this).val(inputValue.replace(/[^a-zA-Z. ]/g, '').substring(0, 30));
    }
});

$('#lastName').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var disallowedRegex = namecheck;
    if (!disallowedRegex.test(inputValue)) {

        $(this).val(inputValue.replace(/[^a-zA-Z. ]/g, '').substring(0, 30));
    }
});

$('#pnr').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var disallowedRegex = pnrcheck;
    if (!disallowedRegex.test(inputValue)) {

        var sanitizedValue = inputValue.match(/[a-zA-Z0-9\b]/g);
        sanitizedValue = sanitizedValue ? sanitizedValue.join('') : ''; $(this).val(sanitizedValue.substring(0, 6));
    }
});

$('#mobile').on('input', function () {
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

$('#pnrDate').on('input', function () {
    var inputValue = $(this).val();
    // Specify the disallowed characters in the regular expression pattern
    var disallowedRegex = datecheck;
    if (!disallowedRegex.test(inputValue)) {

        $(this).val(inputValue.replace(disallowedRegex, ''));
    }
});

$('#pnrSubmit').click(function () {
    //var nameRegex = /^[a-zA-Z ]+$/;
    var isvalid = true;
    if ($('#firstName').val() == "") {
        $('#firstName').closest('.form-floating').find('p.field-validation-error').html("First Name is required").show();
        isvalid = false;
    } else {
        if (!namecheck.test($('#firstName').val())) {
            $('#firstName').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid First Name.").show();
            isvalid = false;
        } else {
            $('#firstName').closest('.form-floating').find('p.field-validation-error').html("");
        }
    }
    if ($('#lastName').val() == "") {
        $('#lastName').closest('.form-floating').find('p.field-validation-error').html("Last Name is required").show();
        isvalid = false;
    } else {
        if (!namecheck.test($('#lastName').val())) {
            setflag = false;
            $('#lastName').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid Last Name.").show();
            isvalid = false;
        } else {
            $('#lastName').closest('.form-floating').find('p.field-validation-error').html("");
        }
    }
    if ($('#pnr').val() == "") {
        $('#pnr').closest('.form-floating').find('p.field-validation-error').html("PNR is required").show();
        isvalid = false;
    }
    else {
        if (!pnrcheck.test($('#pnr').val())) {
            setflag = false;
            $('#pnr').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid PNR").show();
            isvalid = false;
        } else {
            $('#pnr').closest('.form-floating').find('p.field-validation-error').html("");
        }
    }
    if (!mobilenocheck.test($('#mobile').val())) {
        $('#mobile').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid Mobile number").show();
        isvalid = false;
    } else {
        $('#mobile').closest('.form-floating').find('p.field-validation-error').hide();
    }
    if (!emaillcheck.test($('#email').val())) {
        $('#email').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid Email").show();
        isvalid = false;
    } else {
        $('#email').closest('.form-floating').find('p.field-validation-error').hide();
    }

    if ($('#pnrDate').val() == "") {
        $('#pnrDate').closest('.form-floating').find('p.field-validation-error').html("Date is Required").show();
        isvalid = false;
    } else {
        if (!datecheck.test($('#pnrDate').val())) {
            $('#pnrDate').closest('.form-floating').find('p.field-validation-error').html("Please enter a valid Date.").show();
            isvalid = false;
        } else {
            $('#pnrDate').closest('.form-floating').find('p.field-validation-error').html("");
        }
    }

    if (!isvalid)
        return false;
    else
        return true;

});

// Get the current date var
today = new Date(); // Format the date as YYYY-MM-DD 
var yyyy = today.getFullYear(); var mm = String(today.getMonth() + 1).padStart(2, '0'); var dd = String(today.getDate()).padStart(2, '0'); var formattedDate = yyyy + '-' + mm + '-' + dd;
// Set the minimum value of the date field to the current date
document.getElementById('pnrDate').min = formattedDate;

function isValidDate(date) {
    var bits = date.split('-');
    var d = new Date(bits[2] + '-' + bits[1] + '-' + bits[0]);
    return !!(d && (d.getMonth() + 1) == bits[1] && d.getDate() == Number(bits[0]));
}

if ($('#pnrModel')) { $('#pnrModel').modal('show'); } 