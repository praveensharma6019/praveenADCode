



//File name and extension validation
function Validate(obj) {
    var format = /[!@&#$%^*()+\=\[\]{};':"\\|,<>\/?]+/;
    if (format.test($(obj).val().replace(/^.*[\\\/]/, ''))) {
        alert('File name should not contain any special characters!');
        $(obj).val("");
        return false;
    }
    var ext = $(obj).val().split('.').pop().toLowerCase();

    if ($.inArray(ext, ['jpg', 'jpeg', 'dwg', 'pdf', 'doc', 'docx', 'xls', 'xlsx']) == -1) {
        alert('File extension not supported!');
        $(obj).val("");
        return false;
    }
}









$("#frmNewConnectionApplication").submit(function (event) {
    var buttonName = $(document.activeElement).attr('name');

    if (buttonName === "SaveDocumentDetail") {
        

        //Check for Mandatory documents
        //var MandFlg = $("[id*='mand_']");
        try {
            var FileUpload1 = $("[id*='file_']");
            var docname = $("[id*='docnumber_']");
            var madfile = $("[id*='manddocname_']");
            var error = false;


            var message = "* Following files are required:</br>";
            for (var i = 0; i < FileUpload1.length; i++) {
                if ($("." + FileUpload1[i].id).val() == undefined) {
                    if ($("#" + FileUpload1[i].id).val() == '') {
                        error = true;
                        message = message + " " + $("#" + madfile[i].id).val() + "</br>";
                    }
                }
            }
            if (error) {
                $("#docErrorMessage").html(message);
                return false;
            }

            for (var j = 0; j < docname.length; j++) {
                if ($("#" + docname[j].id).val() == null || $("#" + docname[j].id).val() == "") {
                    error = true;
                }
            }
            if (error) {
                message = "* Please provide valid ID Number</br>";
                $("#docErrorMessage").html(message);
                return false;
            }

            if ($("#IDDOC_1").val() != undefined && $("#IDDOC_1").val() == "") {
                message = message + "Please select any Identity Document</br>";
                error = true;
            }
            if ($("#IDDOC_1").val() != undefined && $("#IDDOC_1").val() != "") {
                if ($("#" + docname[0].id).val() != null || $("#" + docname[0].id).val() != "") {
                    if ($("#IDDOC_1").val() == "990") {
                        var IsValid = validateAadhar($("#" + docname[0].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid 12 digit Aadhar number eg: xxxxxxxxxxxx </br>";
                            error = true;
                        }
                    }
                    if ($("#IDDOC_1").val() == "920") {
                        var IsValid = validatePAN($("#" + docname[0].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
                            error = true;
                        }
                    }
                    if ($("#IDDOC_1").val() == "940") {
                        var IsValid = validatePassport($("#" + docname[0].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid Passport number </br>";
                            error = true;
                        }
                    }

                }
            }
            if ($("#IDDOC_2").val() != undefined && $("#IDDOC_2").val() == "") {
                message = message + "Please select any Identity Document for Applicant 2</br>";
                error = true;
            }
            if ($("#IDDOC_2").val() != undefined && $("#IDDOC_2").val() != "") {
                if ($("#" + docname[1].id).val() != null || $("#" + docname[1].id).val() != "") {
                    if ($("#IDDOC_2").val() == "990") {
                        var IsValid = validateAadhar($("#" + docname[1].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid 12 digit Aadhar number eg: xxxxxxxxxxxx </br>";
                            error = true;
                        }
                    }
                    if ($("#IDDOC_2").val() == "920") {
                        var IsValid = validatePAN($("#" + docname[1].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
                            error = true;
                        }
                    }
                    if ($("#IDDOC_2").val() == "940") {
                        var IsValid = validatePassport($("#" + docname[1].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid Passport number </br>";
                            error = true;
                        }
                    }

                }
            }
            if ($("#PHOTOFILE_1").val() != undefined && $("#PHOTOFILE_1").val() == "") {
                message = message + "Please select any Photo Identity Document</br>";
                error = true;
            }
            
            if ($("#OWNERDOC_1").val() != undefined && $("#OWNERDOC_1").val() == "") {
                message = message + "Please select any Ownership Document</br>";
                error = true;
            }
            if ($("#OWNERDOC_2").val() != undefined && $("#OWNERDOC_2").val() == "") {
                message = message + "Please select any Ownership Document 2</br>";
                error = true;
            }
            if (error) {
                $("#docErrorMessage").html(message);
                return false;
            }
            else $("#docErrorMessage").html("");
        }
        catch (tes) {
            $("#docErrorMessage").html("Please upload all files");
            return false;
        }

        

        //check if tearms and cinditions are checked and confrim
        if (!$('input[name="termsCb"]').is(':checked') || !$('input[name="confirmCb"]').is(':checked')) {
            $("#docErrorMessage").html("Please confirm terms and conditions by checking the check boxes.");
            return false;
        }

  
        event.preventDefault();
        return false;
    }
    else if (buttonName == undefined || buttonName == "SaveAsDraft") {
        var filesUploaded = $("[id*='file_']");
        var docNumber = $("[id*='docnumber_']");
        var mandatoryFile = $("[id*='manddocNumber_']");
        var isError = false;
        var message = "";
        try {
            var applicant_Type = $("input[name='ApplicantType']:checked").val();
            if (applicant_Type == "1" || applicant_Type == "3") {
                if ($("#" + filesUploaded[0].id).val() != '' && $("#" + filesUploaded[0].id).val() != null) {
                    if ($("#" + docNumber[0].id).val() == null || $("#" + docNumber[0].id).val() == "") {
                        isError = true;
                    }
                }
            }
            else {
                if ($("#" + filesUploaded[0].id).val() != '' && $("#" + filesUploaded[0].id).val() != null) {
                    if ($("#" + docNumber[0].id).val() == null || $("#" + docNumber[0].id).val() == "") {
                        isError = true;
                    }
                }
                if ($("#" + filesUploaded[1].id).val() != '' && $("#" + filesUploaded[1].id).val() != null) {
                    if ($("#" + docNumber[1].id).val() == null || $("#" + docNumber[1].id).val() == "") {
                        isError = true;
                    }
                }
            }
        }
        catch (e) {
            //skip
        }
        if (isError) {
            message = "Please provide document number for the Identity documents.";
            $("#docErrorMessage").html(message);
            return false;
        }
        if ($("#IDDOC_1").val() != undefined && $("#IDDOC_1").val() != "") {
            if ($("#" + docNumber[0].id).val() != null || $("#" + docNumber[0].id).val() != "") {
                if ($("#IDDOC_1").val() == "990") {
                    var IsValid = validateAadhar($("#" + docNumber[0].id).val());
                    if (!IsValid) {
                        message = message + "Please enter valid 12 digit Aadhar number eg: xxxxxxxxxxxx </br>";
                        error = true;
                    }
                }
                if ($("#IDDOC_1").val() == "920") {
                    var IsValid = validatePAN($("#" + docNumber[0].id).val());
                    if (!IsValid) {
                        message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
                        error = true;
                    }
                }
                if ($("#IDDOC_1").val() == "940") {
                    var IsValid = validatePassport($("#" + docNumber[0].id).val());
                    if (!IsValid) {
                        message = message + "Please enter valid Passport number </br>";
                        error = true;
                    }
                }

            }
        }
    }
    if ($("#IDDOC_2").val() != undefined && $("#IDDOC_2").val() != "") {
        //if ($("#" + docNumber[1].id).val() != null || $("#" + docNumber[1].id).val() != "") {
        //    if ($("#ddlID2").val() == "990") {
        //        var IsValid = validateAadhar($("#" + docNumber[1].id).val());
        //        if (!IsValid) {
        //            message = message + "Please enter valid 12 digit Aadhar number eg: xxxxxxxxxxxx </br>";
        //            error = true;
        //        }
        //    }
        //    if ($("#ddlID2").val() == "920") {
        //        var IsValid = validatePAN($("#" + docNumber[1].id).val());
        //        if (!IsValid) {
        //            message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
        //            error = true;
        //        }
        //    }
        //    if ($("#ddlID2").val() == "940") {
        //        var IsValid = validatePassport($("#" + docNumber[1].id).val());
        //        if (!IsValid) {
        //            message = message + "Please enter valid Passport number </br>";
        //            error = true;
        //        }
        //    }

        //}
    }
    if (error) {
        $("#docErrorMessage").html(message);
        return false;
    }


    $("#loader-wrapper").show();

    return true;
});





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

function ValidateFile(value) {

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

        // var str = value.name;
        // var res = str.split("_");
        // var data = "_val" + res[1];
        $("#message_modal .modal-body").text("Only jpg, jpeg, png and pdf files are allowed.");
        $("#message_modal").modal("show");
        $(value).val('');
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];
        if (size > 5) {
            $("#message_modal .modal-body").text("Max file Size is 5 MB.");
            $("#message_modal").modal("show");
            $(value).val('');
            return false;
        }
    }
    var FileUpload1 = $("[id*='file_']");
    var docname = $("[id*='docnumber_']");
    if (file != null && FileUpload1.length > 0) {
        var applicantType = $("input[name='ApplicantType']:checked").val();
        if (applicantType == "1" || applicantType == "3") {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", false);
            }
        }
        else {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", false);
            }
            if (value.id == FileUpload1[1].id) {
                $("#docnumber_ID2").prop("disabled", false);
            }

        }
    }
    else if (file == null && FileUpload1.length > 0) {
        var applicantType2 = $("input[name='ApplicantType']:checked").val();
        if (applicantType2 == "1" || applicantType2 == "3") {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", true);
            }
        }
        else {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", true);
            }
            if (value.id == FileUpload1[1].id) {
                $("#docnumber_ID2").prop("disabled", true);
            }
        }
    }
};

function ValidatePHFile(value) {

    var file = getNameFromPath($(value).val());
    if (file != null) {
        var extension = file.substr((file.lastIndexOf('.') + 1));
        switch (extension.toLowerCase()) {
            case 'jpg':
            case 'jpeg':
            case 'png':
                flag = true;
                break;
            default:
                flag = false;
        }
    }

    if (flag == false) {

        // var str = value.name;
        // var res = str.split("_");
        // var data = "_val" + res[1];
        $("#message_modal .modal-body").text("Only jpg, jpeg files are allowed.");
        $("#message_modal").modal("show");
        $(value).val('');
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];
        if (size > 5) {
            $("#message_modal .modal-body").text("Max file Size is 5 MB.");
            $("#message_modal").modal("show");
            $(value).val('');
            return false;
        }
    }
    var FileUpload1 = $("[id*='file_']");
    var docname = $("[id*='docnumber_']");
    if (file != null && FileUpload1.length > 0) {
        var applicantType = $("input[name='ApplicantType']:checked").val();
        if (applicantType == "1" || applicantType == "3") {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", false);
            }
        }
        else {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", false);
            }
            if (value.id == FileUpload1[1].id) {
                $("#docnumber_ID2").prop("disabled", false);
            }

        }
    }
    else if (file == null && FileUpload1.length > 0) {
        var applicantType2 = $("input[name='ApplicantType']:checked").val();
        if (applicantType2 == "1" || applicantType2 == "3") {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", true);
            }
        }
        else {
            if (value.id == FileUpload1[0].id) {
                $("#docnumber_ID").prop("disabled", true);
            }
            if (value.id == FileUpload1[1].id) {
                $("#docnumber_ID2").prop("disabled", true);
            }
        }
    }
};

function validateAadhar(number) {
    var regex = /^\d{12}$/;
    return regex.test(number);
};
function validatePAN(number) {
    var regex = /^[A-Z]{5}[0-9]{4}[A-Z]{1}/;
    return regex.test(number);
};

function validatePassport(number) {
    var regex = /^(?!^0+$)[a-zA-Z0-9]{3,20}$/;
    return regex.test(number);
};

