$(document).ready(function () {

    if ($('#message').val() != "" && $('#message').val() != undefined) {
        TenderDetailsAlertMessage();
        $('#message').val('');

    }
    ShowHideDoc();

    $(function () {
        $('#datetimepickerTenderAdvDate').datepicker(
            { format: 'DD/MM/YYYY' });
    });
    $(function () {
        $('#datetimepickerTenderClosingDate').datepicker(
            { format: 'DD/MM/YYYY' });
    });

    $("#reset").click(function () {
        window.location.reload();
    });



    $(function () {
        $('.TenderFeeDateOfPayment').datepicker(
            { format: 'dd/mm/yyyy', minDate: new Date() })
    });

    $(function () {
        $('.EMDDateOfPayment').datepicker(
            { format: 'dd/mm/yyyy', minDate: new Date() })
    });
});

function Validate(obj) {
    var format = /[!@&#$%^*()+\=\[\]{};':"\\|,<>\/?]+/;

    if (format.test($(obj).val().replace(/^.*[\\\/]/, ''))) {
        alert('File name should not contain any special characters!');
        $(obj).val("");
        return false;
    }
    var ext = $(obj).val().split('.').pop().toLowerCase();
    // if($.inArray(ext, ['jpg','jpeg','png','pdf','doc','docx','xls','xlsx','.zip']) == -1) {
    // alert('invalid extension!');
    // }
    if ($.inArray(ext, ['jpg', 'jpeg', 'dwg', 'doc', 'docx', 'xls', 'xlsx', 'pdf', 'mp4']) == -1) {
        alert('File extension not supported!');
        $(obj).val("");
        $(obj).closest('.file-upload-wrp').removeClass('show');
        return false;
    }
}

function validateEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}


function validateMobileNo(mobile) {
    if (mobile.match(/^[6789]\d{9}$/)) {
        return true;
    }
    else {
        return false;
    }
}

function validateMobile(event, t) {
    var mobile = $("#mobileNumber").val();
    if (validateMobileNo(mobile)) {
        $("#mobileerror").html("").hide();
    }
    else {
        $("#mobileerror").html("Please enter a 10 digit valid mobile number").show();
    }
}

function validateEmailId(event, t) {
    var emailAddress = $("#emailAddress").val();
    if (validateEmail(emailAddress)) {
        $("#emailerror").html("").hide();
    }
    else {
        $("#emailerror").html("Please enter a valid Email Address").show();
    }
}



function validateName(name) {
    var regex = /^[a-zA-Z ]+$/;
    return regex.test(name);

}

function onchangeValidateFax(event, t) {
    var fax = $("#fax").val();
    if (fax != null && fax.trim() != "") {
        if (!validateFax(fax)) {
            $("#faxerror").html("Please enter valid Fax number containing 12 digits").show();
            //$("#fax").focus();
        }
        else {
            $("#faxerror").html("").hide();
        }
    }
    else {
        $("#faxerror").html("").hide();
    }
}

function validateFax(fax) {
    var regex = /^[0-9]{12,12}$/;
    return regex.test(fax.replace('-',''));
}

$("#resetclick").click(function () {
    $(".reset").click();
});

$(".reset").click(function () {
    $("#nameerror").html("").hide();
    $("#companyerror").html("").hide();
    $("#mobileerror").html("").hide();
    $("#faxerror").html("").hide();
    $("#emailerror").html("").hide();
    $("#name").val("");
    $("#company").val("");
    $("#mobileNumber").val("");
    $("#fax").val("");
    $("#emailAddress").val("");
    $('#company').closest('.input-field').find('label').removeClass('active')
    $('#name').closest('.input-field').find('label').removeClass('active')
    $('#fax').closest('.input-field').find('label').removeClass('active')
    $('#emailAddress').closest('.input-field').find('label').removeClass('active')
    $('#mobileNumber').closest('.input-field').find('label').removeClass('active')
});

function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32)) {
            // $("#nameerror").html("").hide();
            return true;
        }
        else {
            // $("#nameerror").html("Please enter a valid name containing alphabets only");
            return false;
        }
    }
    catch (err) {
        alert(err.Description);
    }
}



$("#UseRegistrationBtn").click(function (event) {
    var mobileNo = $("#mobileNumber").val();
    var emailAddress = $("#emailAddress").val();
    var company = $("#company").val();
    var fax = $("#fax").val();
    var name = $("#name").val();
    var checkvalidation = true;
    if (!validateName(name)) {
        event.preventDefault();
        $("#nameerror").html("Please enter a valid name containing alphabets only").show();
        $("#name").focus();
        checkvalidation = false;
    }
    else {
        $("#nameerror").html("").hide();
    }
    if (company.trim() == "") {
        event.preventDefault();
        $("#companyerror").html("Please enter a valid company name").show();
        $("#company").focus();
        checkvalidation = false;
    }
    else {
        $("#companyerror").html("").hide();
    }
    if (!validateMobileNo(mobileNo)) {
        event.preventDefault();
        $("#mobileerror").html("Please enter a 10 digit valid mobile number").show();
        $("#mobileNumber").focus();
        checkvalidation = false;
    }
    else {
        $("#mobileerror").html("").hide();
    }
    if (fax.trim() != "") {
        if (!validateFax(fax)) {
            event.preventDefault();
            $("#faxerror").html("Please enter valid Fax number containing 12 digits").show();
            $("#fax").focus();
            checkvalidation = false;
        }
        else {
            $("#faxerror").html("").hide();

        }
    }
    else {
        $("#faxerror").html("").hide();
    }
    if (!validateEmail(emailAddress)) {
        event.preventDefault();
        $("#emailerror").html("Please enter a valid Email Address").show();
        $("#emailAddress").focus();
        checkvalidation = false;
    }
    else {
        $("#emailerror").html("").hide();
    }
    if (checkvalidation) {
        $('.loader-wrap').show();
        $('.sidenav').sidenav('close');
        return true;
    }
    else {
        return false;
    }

});

//$("#btnsubmitchangepwd").click(function (event) {
//    var OldPassword = $("#OldPassword").val();
//    var ConfirmPassword = $("#ConfirmPassword").val();
//    var Password = $("#Password").val();
//    if (OldPassword=='') {
//        event.preventDefault();
//        $("#OldPassworderror").html("Please enter old password").show();
//        $("#OldPassworderror").focus();
//        return false;
//    }
//    else {
//        $("#OldPassworderror").html("").hide();
//    }
//    if (Password == '') {
//        event.preventDefault();
//        $("#Passworderror").html("Please enter New password").show();
//        $("#Passworderror").focus();
//        return false;
//    }
//    else {
//        $("#Passworderror").html("").hide();
//    }
//    if (ConfirmPassword == '') {
//        event.preventDefault();
//        $("#ConfirmPassworderror").html("Please enter Confirm password").show();
//        $("#ConfirmPassworderror").focus();
//        return false;
//    }
//    else {
//        $("#ConfirmPassworderror").html("").hide();
//    }

//    if (ConfirmPassword != Password) {
//        event.preventDefault();
//        $("#ConfirmPassworderror").html("Password and Confirm password not same").show();
//        $("#ConfirmPassworderror").focus();
//        return false;
//    }
//    else {
//        $("#ConfirmPassworderror").html("").hide();
//    }
//});
function TenderDetailsAlertMessage() {
    window.setTimeout(function () {
        var elem = document.getElementById('tenderDetailstabone');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();
    }, 1500);
}

function onEnv3Business(val) {
    ShowHideDoc();
}

function onEnv3Category(val) {
    ShowHideDoc();
}

function ShowHideDoc() {
    var businessValue = $("#Business").val();
    var categoryValue = $("#Category").val();

    $(".env3docs").hide();
    $(".env2docsMan").hide();

    if (businessValue == "1") {
        if (categoryValue == "1") {

            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManTypeTestReports").hide();
            $("#ManOtherDocuments").hide();
            $("#ManDrawingsandGeneralArrangement").hide();

        }
        else if (categoryValue == "2") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "3") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManTypeTestReports").hide();
            $("#ManOtherDocuments").hide();
            $("#ManDrawingsandGeneralArrangement").hide();

        }
    }
    else if (businessValue == "2") {
        if (categoryValue == "1") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "2") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "3") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
    }
    else if (businessValue == "3") {
        if (categoryValue == "1") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "2") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "3") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
    }
}

