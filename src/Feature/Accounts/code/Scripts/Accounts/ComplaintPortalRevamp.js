$(document).ready(function () {
    $("#loader-wrapper").hide();
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {
        $('#message_modal').modal('show');
        $("#message").val("");
    }

    if (localStorage['page'] == document.URL && localStorage['scrollTop'] != null && localStorage['scrollTop'] != "") {
        $(document).scrollTop(localStorage['scrollTop']);
    }
    else {
        $(document).scrollTop(0);
        localStorage['page'] = "";
        localStorage['scrollTop'] = "";
    }

    //$(function () {
    //    $('#datetimepickerComplaintStartDate').datetimepicker(
    //        { format: 'DD/MM/YYYY' });
    //});
    //$(function () {
    //    $('#datetimepickerComplaintEndDate').datetimepicker({ format: 'DD/MM/YYYY' });
    //});


    //$('#datetimepickerComplaintFromPreviousLevelAppliedDate').datetimepicker(
    //    {
    //        format: 'DD/MM/YYYY'
    //        //format: 'DD/MM/YYYY'
    //    });

    //$('#datetimepickerComplaintHearingDate').datetimepicker(
    //    {
    //        format: 'DD/MM/YYYY hh:mm:ss'
    //        //format: 'DD/MM/YYYY'
    //    });

    $("#searchCONStatusApp").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#CONApps tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});

$('.cexport').on("click", function () {
    var sdate = $("#datetimepickerComplaintStartDate").val();
    var edate = $("#datetimepickerComplaintEndDate").val();
    var status = $("#SelectedComplaintStatus").val();
    var zone = $("#SelectedConsumerZone").val();
    var division = $("#SelectedConsumerDivision").val();
    var category = $("#SelectedComplaintCategory").val();
    var url = $(this).attr('href') + '?startDate=' + sdate + '&endDate=' + edate + '&consumerZone=' + zone + '&consumerDivision=' + division + '&complaintstatus=' + status + '&complaintCategory=' + category;
    location.href = url;
    return false;
});

function GetScrollPosition() {
    localStorage['page'] = document.URL;
    localStorage['scrollTop'] = $(document).scrollTop();
}

$('#ddlComplaintCategory').on('change', function (event) {
    $('.loader-wrap').show();
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});


$('#ddlComplaintSubCategory').on('change', function (event) {
    $('.loader-wrap').show();
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$("#fileComplaintSupportingDocs").on("change", function () {
    if ($("#fileComplaintSupportingDocs")[0].files.length > 5) {
        alert("Max 5 files are allowed");
        $("#fileComplaintSupportingDocs").val(''); 
    } else {
        return true;
    }
});

$("#frmComplaintRegistration").submit(function (event) {
    var buttonName = $(document.activeElement).attr('name');
    var isvalid = true;
    if (buttonName === "SubmitApplication") {
        if ((!$('#ComplaintDescription').val().trim().length > 0)) {
            $("#descriptionErrorMessage").addClass('field-validation-error').html("Please enter the description.");
            isvalid=  false;
        }else{
            $("#descriptionErrorMessage").removeClass('field-validation-error').html("");
        }
        if (!$('input[name="termsCb"]').is(':checked')) {
            $("#docErrorMessage").addClass('field-validation-error').html("Please confirm terms and conditions by checking the check boxes.");
            isvalid=  false;
        }else{
            $("#docErrorMessage").removeClass('field-validation-error').html("");
        }
        if ($('select#ddlComplaintCategory option:selected').val() == '') {
            $("#catError").addClass('field-validation-error').html("Please select Category");
            isvalid = false;
        } else {
            $("#catError").removeClass('field-validation-error').html("");
        }
        if ($('select#ddlComplaintSubCategory option:selected').val() == '') {
            $("#subCatError").addClass('field-validation-error').html("Please Select Sub Category");
            isvalid = false;
        } else {
            $("#subCatError").removeClass('field-validation-error').html("");
        }
        var captcharesponse = grecaptcha.getResponse(FileComplaintCaptcha);
        if (!captcharesponse) {
            $('#Captchaerror').show().html('Please validate captcha to continue');
            isvalid = false;
        }
        else {
            $('#Captchaerror').hide().html('');
        }

    }
    if (isvalid) {
        $('.loader-wrap').show();
        return isvalid;
    } else {
        return isvalid;
    }
   
});

$("#frmComplaintRegistrationAdmin").submit(function (event) {
    var buttonName = $(document.activeElement).attr('name');

    if (buttonName == "CloseComplaint") {
        $('.confirmation_modal_message').html("Are you sure you want to Close the complaint?");
        $('#confirmation_modal').modal("show");
        event.preventDefault();
        return false;
    }

    //$("#loader-wrapper").show();
    return true;
});

$('.confirmation_modalbtnYesNO').click(function (e) {
    if (this.value === '1') {
        YesNO = '1';
        $('#confirmation_modal').modal("hide");
        $("#CloseComplaint").click();
    }
    else {
        $('#confirmation_modal').modal("hide");
    }
    e.preventDefault();
});

$('#ddlReasonToApply').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});


$('#ddlReasonToApplySubType').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$(document).on("click", ".escalateToICRS", function () {
    var complaint_id = $(this).attr('data-id');
    var remarks = $(this).attr('data-content');
    $("#ExcalationComplaintId").val(complaint_id);
    $("#ExcalationRemarks").val(remarks);
});


function ValidateFile(value) {
    var flag = false;
    var file = getNameFromPath($(value).val());
    if (file != null) {
        var extension = file.substr((file.lastIndexOf('.') + 1));
        switch (extension.toLowerCase()) {
            case 'jpg':
            case 'jpeg':
            case 'png':
            case 'pdf':
                flag = true;
                break;
            default:
                flag = false;
        }
    }

   

    if (flag == false) {
        $("#FileUpload p.modal_message").text("Only jpg, jpeg, png files are allowed.");
        var elem = document.getElementById('FileUpload');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();
        $(value).closest('.file-field').find('.remove-upload').click();
        $(value).val('');
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];
        if (size > 5) {
            $("#FileUpload p.modal_message").text("Max file Size is 5 MB.");
            var elem = document.getElementById('FileUpload');
            var instance = M.Modal.init(elem, { dismissible: false });
            instance.open();
            $(value).closest('.file-field').find('.remove-upload').click();
            $(value).val('');
            return false;
        }
    }
}

window.ValidateFile = ValidateFile

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