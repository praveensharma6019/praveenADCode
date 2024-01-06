

$(document).ready(function () {
    $("input[name=IsApplicantType]").change(function () {
        var inputValue = $(this).attr("value");
        if (inputValue == '0') {
            $('#governmentlist').show();
        }
        else {
            $('#governmentlist').hide();
        }
    })
    var total = $('#ddlsubtype').val();
    if ((total == "1") || (total == "2")) {
        if ($('.totalFund').text() >= 99999 || $('.totalFund').text() == 0) {
            $('.totalFund').hide();
            $("#ddlConnectedLoadKW").val("");
            $("#ddlConnectedLoadHP").val("");
        }
    }
    else {
        if ($('.totalFund').text() <= 100 || $('.totalFund').text() >= 160) {
            $('.totalFund').hide();
            $("#ddlConnectedLoadKW").val("");
            $("#ddlConnectedLoadHP").val("");
        }

    }
    var dtToday = new Date();

    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate();
    var year = dtToday.getFullYear();

    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var maxDate = year + '-' + month + '-' + day;
    $('#DateofBirth').attr('max', maxDate);
    $('#Name1JointDateofBirth').attr('max', maxDate);
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {
        $('#message_modal').modal('show');
        $("#message").val("");
    }

})

$('.rbbillingdifferentthanAddress').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('.rbtype').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('.rbIsExistingCustomer').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('.rbExistingConnection').change(function (event) {
    GetScrollPosition();

    var form = $(event.target).parents('form');
    form.submit();
});
$('.rbAddressInCaseOfRental').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('.rbApplicantType').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('.rbbissez').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#LECNumber').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#ddlSelectedPincode').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#NearestConsumerMeterNumber').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('#ddlApplicationType').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

//Shekhar
$('#ddlsubtype').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#CANumber').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#TempStartDate').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#TempEndDate').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#ddlTitle').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#Name1Joint').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#Name2Joint').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#DateofBirth').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#Name1JointDateofBirth').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#Name2JointDateofBirth').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#BankAccountNumber').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#BankAccountNumberConfirm').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#ExistingConsumerAccountNumber').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});

$('#ddlPurposeOfSupply').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$("#LPDOrders").on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$("#ConsumerNo").on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#DdlVoltageLevel').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$("#chkPassport").click(function () {
    if ($(this).is(":checked")) {
        $(this).attr('data-target', '#greentariffmodal');
        $("#greentariffmodal").show();
    } else {
        $("#greentariffmodal").hide();
        $(this).removeAttr('data-target', '#');
        $("#greenlist").hide();
    }
});
$("#Ok").click(function () {
    if ($("#chkPassport").is(":checked")) {

        $("#greenlist").show();
    } else {
        $("#greenlist").hide();



    }
});
$("#Cancel").click(function () {
    $("#chkPassport").prop("checked", false);
    $("#greentariffmodal").hide();
    $(this).removeAttr('data-target', '#');
    $("#greenlist").hide();
});

$('input[name="metersupplier"]').change(function () {
    if ($(this).is(':checked') && $(this).val() == 'SELF') {

        $('#myModal').show();

        $('#Meterspecifications').show();
        $('.ownmeters').hide();
        $('#Metersupplierss').attr('data-target', '#myModal');
    }
    else if ($(this).is(':checked') && $(this).val() == 'AEML') {
        $('#myModal').hide();
        $('.ownmeters').show();
        $('#Meterspecifications').hide();
        $(this).removeAttr('data-target', '#');

    }
});


$("#Yess").click(function () {

    $('#Meterspecifications').hide();
    $('.ownmeters').show();
    $("#Metersuppliers").prop("checked", true);


});



$('.IsMeterConnection').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$("#bankconfirm").change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});



$('#ddlConnectedLoadKW').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#ddlConnectedLoadHP').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#DdlConnectionType').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#firstname').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#MICR').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#lastname').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});

$('#ddlGovernmentType').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#ddlsubtype').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});
$('#CANumber').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});

$('.ddf').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('#TRNumber').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();

});
$('#frmNewConnectionApplication').submit(function () {
    $("#loader-wrapper").show();
});
$('#frmregistrationValidate').submit(function () {
    $("#loader-wrapper").show();
});




$(document).ready(function () {
    // Single select example if using params obj or configuration seen above
    var configParamsObj = {
        placeholder: 'Select an option...', // Place holder text to place in the select
        minimumResultsForSearch: 3, // Overrides default of 15 set above
        matcher: function (params, data) {
            // If there are no search terms, return all of the data
            if ($.trim(params.term) === '') {
                return data;
            }

            // `params.term` should be the term that is used for searching
            // `data.text` is the text that is displayed for the data object
            if (data.text.toLowerCase().startsWith(params.term.toLowerCase())) {
                var modifiedData = $.extend({}, data, true);
                modifiedData.text += ' (matched)';

                // You can return modified objects from here
                // This includes matching the `children` how you want in nested data sets
                return modifiedData;
            }

            // Return `null` if the term should not be displayed
            return null;
        }
    };
    $("#ddlPurposeOfSupply").select2(configParamsObj);
    $("#ddlPremiseType").select2(configParamsObj);
});

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
    var message = '';

    if (buttonName === "SaveDocumentDetail" || buttonName === "SubmitApplication") {
        message = validateDocumentAfterSubmit("IDDOC_1","DOCUMENTID_1");
        message += validateDocumentAfterSubmit("IDDOC_2","DOCUMENTID_2");
        message += validateDocumentAfterSubmit("IDJOINTDOC_1","JOINTDOCUMENTID_1", true);
        message += validateDocumentAfterSubmit("IDJOINTDOC_2","JOINTDOCUMENTID_2", true);
    }
    if (message !== "") {
        $(".txt-error-document").html(message);
        $("#loader-wrapper").hide();
        return false;
    }
});


function validateDocumentAfterSubmit(ctrlid,docctrlid,isjoint = false) {
    var message = "";
    if ($("#" + ctrlid) !== undefined && $("#" + ctrlid).val() !== "") {
        var val = $("#" + docctrlid).val();
        if (val !== null || val !== "") {
            if ($("#" + ctrlid).val() === "990") {
                var IsValid = validateAadhar(val);
                if (!IsValid) {
                    message = message + "Please enter valid 12 digit Aadhar number " + (isjoint == true ? "With join account" : "") + " eg: xxxxxxxxxxxx </br>";
                }
            }
            if ($("#" + ctrlid).val() === "920") {
                var IsValid1 = validatePAN(val);
                if (!IsValid1) {
                    message = message + "Please enter valid PAN Card number " + (isjoint == true ? "With join account" : "") + " eg: AAAAA1234A </br>";
                }
            }
            if ($("#" + ctrlid).val() === "940") {
                var IsValid2 = validatePassport(val);
                if (!IsValid2) {
                    message = message + "Please enter valid Passport number " + (isjoint == true ? "With join account" : "") + " </br>";
                }
            }
        }
    }
    return message;
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


$(function () {
    $("select[id*=IDDOC]").change(function () {
        var currentval = $("#" + this.id).val();
        var currentid = this.id;
        $("select[id*=IDDOC]").each(function () {
            if (currentid != this.id) {
                var compareval = $("#" + this.id).val();
                if (currentval == compareval) {
                    $("#DIV_" + currentid).show();
                    $("#" + currentid).val("")
                }
                else {
                    $("#DIV_" + currentid).hide();
                }
            }
        })
    });

    $("select[id*=OWNERDOC]").change(function () {
        var flag = false;
        var currentval = $("#" + this.id).val();
        var currentid = this.id;
        $("select[id*=OWNERDOC]").each(function () {
            if (currentid != this.id && flag == false) {
                var compareval = $("#" + this.id).val();
                if (compareval != "") {
                    if (currentval == compareval) {
                        $("#DIV_" + currentid).show();
                        $("#" + currentid).val("")
                        flag = true;
                    }
                    else {

                        $("#DIV_" + currentid).hide();
                    }
                }
            }
        })
    });

    var x = 3;
    $(".add_ownership_document").click(function (e) {
        if (x == 5) {
            $(".add_ownership_document").hide();
        }
        else {
            $(".add_ownership_document").show();
        }
        $("#TR_OWNERDOC_" + x).show();
        x++;
    });

    $(".mar").hide();

    if ($("#AppLanguage").val() === "EN") {
        $(".eng").show();
        $(".mar").hide();
    }
    else {
        $(".eng").hide();
        $(".mar").show();
    }
    $('#AppLanguage').change(function (event) {
        var lang = $(this).val();
        if (lang === "EN") {
            $(".eng").show();
            $(".mar").hide();
        }
        else {
            $(".eng").hide();
            $(".mar").show();
        }
    });
});
