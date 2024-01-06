$(document).ready(function () {
    if ($("#ComplaintFromPreviousLevel").val() == null || $("#ComplaintFromPreviousLevel").val() == "") {
        $("#datetimepickerComplaintFromPreviousLevelAppliedDate").val(null);
        window.setTimeout(function () {
            var elem = document.getElementById('onload_confirmation_modal');
            var instance = M.Modal.init(elem, { dismissible: false });
            instance.open();
        }, 1500);
        //$('#onload_confirmation_modal').modal();
    }
    else if (!($("#ComplaintFromPreviousLevel").val() == null || $("#ComplaintFromPreviousLevel").val() == "") && $("#ddlReasonToApply").val() == "") {
        window.setTimeout(function () {
            var elem = document.getElementById('onload_confirmation_modal_Process');
            var instance = M.Modal.init(elem, { dismissible: false });
            instance.open();
        }, 1500);
        //$('#onload_confirmation_modal_Process').modal();
    }

    var reason = $("#ddlReasonToApply").val();
    if (reason == "Unredressed within resolution period") {
        $("#divReasonToApplySubType").show();
    }

    reason = $("#ddlReasonToApplySubType").val();
    if (reason == "Others type complaint : 15 Days") {
        $("#divReasonToApplySubTypeOtherText").show();
    }

});

$('#ValidateComplaintNumber').click(function () {
    var isValid = true;
    if (($("#ComplaintFromPreviousLevel").val() == null || $("#ComplaintFromPreviousLevel").val() == "")) {
        $('#ComplaintFromPreviousLevelerror').show().html('Please enter Complaint Number');
        isValid = false;
    } else {
        $('#ComplaintFromPreviousLevelerror').hide().html('');
    }
    if (($("#ddlReasonToApply").val() == "")) {
        $('#ddlReasonToApplyerror').show().html('Please Select Reason for approaching CGRF');
        isValid = false;
    } else {
        $('#ddlReasonToApplyerror').hide().html('');
    }
    if (($("#ComplaintFromPreviousLevelAppliedDate").val() == null || $("#ComplaintFromPreviousLevelAppliedDate").val() == "")) {
        $('#datetimepickerComplaintFromPreviousLevelAppliedDateerror').show().html('Please Select Date of First Complaint');
        isValid = false;
    } else {
        $('#datetimepickerComplaintFromPreviousLevelAppliedDateerror').hide().html('');
    }
    var captcharesponse = grecaptcha.getResponse(CGRFFileComplaintCaptcha);
    if (!captcharesponse) {
        $('#CGRFFileComplaintCaptchaerror').show().html('Please validate captcha to continue');
        isValid = false;
    }
    else {
        $('#CGRFFileComplaintCaptchaerror').hide().html('');
    }

    if (isValid) {
        $('.loader-wrap').show();
        return isValid;
    } else {
        return isValid;
    }
});

$('#SubmitApplication, #SaveAsDraft').click(function () {
    $('.loader-wrap').show();
})


$('.onload_confirmation_Process_modalbtnYesNO').click(function (e) {
    if (this.value === '0') {
        YesNO = '1';
    }
    else {
        window.location.href = "/complaint-registration";
    }
    e.preventDefault();
});

$('.onload_confirmation_modalbtnYesNO').click(function (e) {
    if (this.value === '1') {
        YesNO = '1';
    }
    if (this.value === '2') {
        YesNO = '2';
        window.location.href = "/complaint-registration";
    }
    else {
        //$('.onload_confirmation_modal_message').html("Kindly contact <a href='#'>helpdesk.mumbaielectricity@adani.com</a> for complaint registration. or register the complaint to Helpdesk i.e. Level 1 from <a href='/complaint-registration-home/complaint-registration-helpdesk-home/complaint-registration-file-complaint'>here</a>");
        $('.onload_confirmation_modal_message').html("Kindly contact <a href='mailto:helpdesk.mumbaielectricity@adani.com'>helpdesk.mumbaielectricity@adani.com</a> for complaint registration or dial 19122 from mobile. or click <a href='/complaint-registration'>here</a> to register a complaint to Helpdesk.");
        $(".onload_confirmation_modal_message_ok").show();
        $(".onload_confirmation_modalbtnYesNO").hide();
    }
    e.preventDefault();
});



$('.onload_confirmation_modal_message_ok').click(function (e) {
    if (this.value === '2') {
        YesNO = '2';
        window.location.href = "/complaint-registration";
    }
    e.preventDefault();
});

$('#datetimepickerComplaintFromPreviousLevelAppliedDate').datepicker({ format: 'dd/mm/yyyy' });



window.ValidateComplaintDocsFile = ValidateComplaintDocsFile
function ValidateComplaintDocsFile(value) {
    var flag = false;
    var file = getNameFromPath($(value).val());
    if (file != null) {
        var extension = file.substr((file.lastIndexOf('.') + 1));
        switch (extension.toLowerCase()) {
            case 'doc':
            case 'docx':
            case 'pdf':
                flag = true;
                break;
            default:
                flag = false;
        }
    }



    if (flag == false) {
        $("#FileUpload p.modal_message").text("Only doc, docx, pdf files are allowed.");
        $(value).val('');
        setTimeout(
            function () {
                if ($(value).closest('.file-upload-wrp').length > 0) {
                    $(value).closest('.file-upload-wrp').removeClass('show');
                }
            }, 200);
        var elem = document.getElementById('FileUpload');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];
        if (size > 5) {
            $("#FileUpload p.modal_message").text("Max file Size is 5 MB.");
            $(value).val('');
            setTimeout(
                function () {
                    if ($(value).closest('.file-upload-wrp').length > 0) {
                        $(value).closest('.file-upload-wrp').removeClass('show');
                    }
                }, 200);
            var elem = document.getElementById('FileUpload');
            var instance = M.Modal.init(elem, { dismissible: false });
            instance.open();
            return false;
        }
    }
}

window.ValidateComplaintSupportingDocsFile = ValidateComplaintSupportingDocsFile

function ValidateComplaintSupportingDocsFile(value) {
    var flag = false;
    var file = getNameFromPath($(value).val());
    if (file != null) {
        var extension = file.substr((file.lastIndexOf('.') + 1));
        switch (extension.toLowerCase()) {
            case 'jpg':
            case 'jpeg':
            case 'pdf':
                flag = true;
                break;
            default:
                flag = false;
        }
    }



    if (flag == false) {
        $("#FileUpload p.modal_message").text("Only jpg, jpeg, pdf files are allowed.");
        $(value).val('');
        setTimeout(
            function () {
                if ($(value).closest('.file-upload-wrp').length > 0) {
                    $(value).closest('.file-upload-wrp').removeClass('show');
                }
            }, 200);
        var elem = document.getElementById('FileUpload');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];
        if (size > 5) {
            $("#FileUpload p.modal_message").text("Max file Size is 5 MB.");
            $(value).val('');
            setTimeout(
                function () {
                    if ($(value).closest('.file-upload-wrp').length > 0) {
                        $(value).closest('.file-upload-wrp').removeClass('show');
                    }
                }, 200);
            var elem = document.getElementById('FileUpload');
            var instance = M.Modal.init(elem, { dismissible: false });
            instance.open();
            return false;
        }
    }
}



function ValidateFileSize(fileid) {
    try {
        var fileSize = 0;
        if (navigator.userAgent.match(/msie/i)) {
            var obaxo = new ActiveXObject("Scripting.FileSystemObject");
            var filePath = $("#" + fileid)[0].value;
            var objFile = obaxo.getFile(filePath);
            fileSize = objFile.size;
            fileSize = fileSize / 1048576;
        }
        else {
            fileSize = $(fileid)[0].files[0].size;
            fileSize = fileSize / 1048576;
        }

        return fileSize;
    }
    catch (e) {
        //alert("Error is :" + e);
    }
}

function getNameFromPath(strFilepath) {
    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}