$("#btnsubmit").on('click', function () {

    var message = '';
    var error = false;
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        message = message + "Captcha required";
        error = true;
    }
    $("#reResponse").val(response);
    var PanNo = $("#PanNo").val().toUpperCase();
    if (PanNo != "") {
        var IsValidPAN = validatePAN(PanNo);
        if (!IsValidPAN) {
            message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
            error = true;
        }
    }
    var EmailId = $("#EmailId").val();
    if (EmailId != "") {
        var IsValidMail = validateEmail(EmailId);
        if (!IsValidMail) {
            message = message + "Invalid Email Address </br>";
            error = true;
        }
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
    function validatePAN(number) {
        var regex = /^[A-Z]{5}[0-9]{4}[A-Z]{1}/;
        return regex.test(number);
    };

});
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});
$(document).ready(function () {
    $('.multiselectallow').multiselect({

        includeSelectAllOption: true

    });
    // document is loaded and DOM is ready
    $('.de_sector_serve_type').on('change', function () {
        $("#de_sector_serve_other_container").css('display', 'none');
        var defence_platforms = $(this).attr('data-text');
        var checked = $(this).is(":checked");
        if (defence_platforms.toLowerCase() === 'other') {
            if (checked === true) {
                $("#de_sector_serve_other_container").css('display', 'block');
            }
            else {
                $("#de_sector_serve_other_container :input").val('');
            }
        }
    });

    $('.defence_segemt_served').on('change', function () {
        $("#defence_segemt_served_other_container").css('display', 'none');
        var defence_platforms = $(this).attr('data-text');
        var checked = $(this).is(":checked");
        if (defence_platforms.toLowerCase() === 'other') {
            if (checked === true) {
                $("#defence_segemt_served_other_container").css('display', 'block');
            }
            else {
                $("#defence_segemt_served_other_container :input").val('');
            }
        }
    });


    $('.defence_platforms_other').on('change', function () {
        $("#defence_platforms_to_other_container").css('display', 'none');
        var defence_platforms = $(this).attr('data-text');
        var checked = $(this).is(":checked");
        if (defence_platforms.toLowerCase() === 'other') {
            if (checked === true) {
                $("#defence_platforms_to_other_container").show();
            }
            else {
                $("#defence_platforms_to_other_container :input").val('');
            }
        }
    });


    $('.defence_supplier_to').on('change', function () {
        $("#defence_supplier_to_other_container").css('display', 'none');
        var defence_supplier_to = $(this).attr('data-text');
        var checked = $(this).is(":checked");
        if (defence_supplier_to.toLowerCase() === 'other') {
            if (checked === true) {
                $("#defence_supplier_to_other_container").show();
            }
            else {
                $("#defence_supplier_to_other_container :input").val('');
            }
        }
    });

    $('.business_type').on('change', function () {
        var currentElement = $(this).is(":checked");
        var business_type = $(this).attr('data-ref');
        if (currentElement) {
            $("#" + business_type).show();
        }
        else {
            $($('#' + business_type).find('.multiselectallow')).multiselect("deselectAll", false).multiselect("refresh");
            $("#" + business_type).hide();
        }
    });
});

$(document).ready(function (initialize) {

    $("#de_sector_serve_other_container").css('display', 'none');
    var defence_platforms1 = $('#SectorServedList_Other').attr('data-text');
    var checked1 = $('#SectorServedList_Other').is(":checked");
    if (defence_platforms1.toLowerCase() === 'other') {
        if (checked1 === true) {
            $("#de_sector_serve_other_container").css('display', 'block');
        }
    }
    $("#defence_segemt_served_other_container").css('display', 'none');
    var defence_platforms2 = $('#SegmentServedTypeList_Other').attr('data-text');
    var checked2 = $('#SegmentServedTypeList_Other').is(":checked");
    if (defence_platforms2.toLowerCase() === 'other') {
        if (checked2 === true) {
            $("#defence_segemt_served_other_container").css('display', 'block');
        }
    }


    $("#defence_platforms_to_other_container").css('display', 'none');
    var defence_platforms3 = $('#DA_PlatformsServedList_Other').attr('data-text');
    var checked3 = $('#DA_PlatformsServedList_Other').is(":checked");
    if (defence_platforms3.toLowerCase() === 'other') {
        if (checked3 === true) {
            $("#defence_platforms_to_other_container").show();
        }
    }


    $("#defence_supplier_to_other_container").css('display', 'none');
    var defence_supplier_to = $('#SupplierToList_Other').attr('data-text');
    var checked4 = $('#SupplierToList_Other').is(":checked");
    if (defence_supplier_to.toLowerCase() === 'other') {
        if (checked4 === true) {
            $("#defence_supplier_to_other_container").show();
        }
    }
    $(".business_type").each(function () {
        var currentElement = $(this).is(":checked");
        var business_type = $(this).attr('data-ref');
        if (currentElement) {
            $("#" + business_type).show();
        }
        else {
            $("#" + business_type).hide();
        }
    });
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
            fileSize = $(fileid)[0].files[0].size
            fileSize = fileSize / 1048576;
        }

        return fileSize;
    }
    catch (e) {
        alert("Error is :" + e);
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
            case 'pdf':
            case 'doc':
            case 'docx':
                flag = true;
                break;
            default:
                flag = false;
        }
    }

    if (flag == false) {
        $(value).val('');
        alert("Only '.pdf',.doc','.docx' formats are allowed.");
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        if (size > 8) {
            $(value).val('');
            alert("Maximum file size allowed is 8MB.");
            return false;
        }
    }
}

