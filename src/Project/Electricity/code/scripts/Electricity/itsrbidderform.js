$(document).ready(function () {
    $("#loader-wrapper").hide();

    $(function () {
        $('#datetimepickerBidderFormStartDate').datetimepicker(
            { format: 'DD/MM/YYYY' });
    });
    $(function () {
        $('#datetimepickerBidderFormEndDate').datetimepicker({ format: 'DD/MM/YYYY' });
    });

    var value = $('input[name=Deviation]:checked').val();
    if (value == "Yes") {
        $("#Q10deviation1").show();
        $("#IsConsolidatedDeviationSheetMandatory").show();
    }
    else {
        $("#Q10deviation1").hide();
    }

    var value1 = $('input[name=AcceptanceOfBillOfQuantity]:checked').val();
    if (value1 == "Deviation") {
        $("#Q10deviation2").show();
        $("#IsConsolidatedDeviationSheetMandatory").show();
    }
    else {
        $("#Q10deviation2").hide();
    }
});

function DeleteBidderUploadedFile(id) {
    jQuery.ajax(
        {
            url: "/api/ITSRForm/DeleteBidderFile",
            data: { id: id },
            method: "POST",
            success: function (data) {
                if (data == "True") {
                    $("#divDeleteFile" + id).hide();
                }
                else {
                    alert("Error in deleting file!");
                }
            }
        });
}


$("#formSubmitAll").click(function (event) {
    var Deviationvalue = $('input[name=Deviation]:checked').val();
    var AcceptanceOfBillOfQuantityvalue = $('input[name=AcceptanceOfBillOfQuantity]:checked').val();


    if (Deviationvalue == "Yes" || AcceptanceOfBillOfQuantityvalue == "Deviation") {
        var obj = document.getElementById("ConsolidatedDeviationSheet");
        var filesInCS = obj.files;
        if (filesInCS != null && filesInCS.length > 0) {
            $("#ErrorMsg").text('');
            return true;
        }
        else {
            $("#ErrorMsg").text("Please select file in ConsolidatedDeviationSheet");
            event.preventDefault();
        }
    }
});

$('.fromBidderProcess').submit(function () {

    $("#loader-wrapper").show();
});

$('input[name=Deviation]').change(function () {
    var value = $('input[name=Deviation]:checked').val();

    var AcceptanceOfBillOfQuantityvalue = $('input[name=AcceptanceOfBillOfQuantity]:checked').val();

    if (value == "Yes") {
        $("#Q10deviation1").show();
        $("#IsConsolidatedDeviationSheetMandatory").show();
    }
    else {
        $("#Q10deviation1").hide();
        $("#IsConsolidatedDeviationSheetMandatory").hide();
        if (AcceptanceOfBillOfQuantityvalue == "Deviation") {
            $("#IsConsolidatedDeviationSheetMandatory").show();
        }
    }
});

$('input[name=AcceptanceOfBillOfQuantity]').change(function () {
    var value = $('input[name=AcceptanceOfBillOfQuantity]:checked').val();
    var DeviationValue = $('input[name=Deviation]:checked').val();
    if (value == "Deviation") {
        $("#Q10deviation2").show();
        $("#IsConsolidatedDeviationSheetMandatory").show();
    }
    else {
        $("#Q10deviation2").hide();
        $("#IsConsolidatedDeviationSheetMandatory").hide();
        if (DeviationValue == "Yes" ) {
            $("#IsConsolidatedDeviationSheetMandatory").show();
        }
    }
});


const input = document.querySelector('#PastExperienceList');
const inputPerformanceCertificate = document.querySelector('#PerformanceCertificate');
const inputPastOrders = document.querySelector('#PastOrders');
const inputGuaranteeTechnicalParticulars = document.querySelector('#GuaranteeTechnicalParticulars');
const inputGeneralArrangement = document.querySelector('#GeneralArrangement');
const inputTestCertificate = document.querySelector('#TestCertificate');
const inputOtherdocumentsSpecified = document.querySelector('#OtherdocumentsSpecified');
const inputConsolidatedDeviationSheet = document.querySelector('#ConsolidatedDeviationSheet');

inputConsolidatedDeviationSheet.addEventListener('change', (e) => {
    // Retrieve all files
    const files = inputConsolidatedDeviationSheet.files;
    var isSizelarge = false;
    $("#ErrorMsg").text('');
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 5) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB.");
        $("#ConsolidatedDeviationSheet").val('');
        return;
    }
    // Check files count
    if (files.length > 10) {
        alert("Max 10 files are allowed to upload.");
        $("#ConsolidatedDeviationSheet").val('');
        return;
    }
});
inputOtherdocumentsSpecified.addEventListener('change', (e) => {
    // Retrieve all files
    const files = inputOtherdocumentsSpecified.files;
    var isSizelarge = false;
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 10) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB."); 
        $("#OtherdocumentsSpecified").val('');
        return;
    }
    // Check files count
    if (files.length > 10) {
        alert("Max 10 files are allowed to upload.");
        $("#OtherdocumentsSpecified").val('');
        return;
    }
});
inputTestCertificate.addEventListener('change', (e) => {
    // Retrieve all files
    const files = inputTestCertificate.files;
    var isSizelarge = false;
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 5) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB.");
        $("#TestCertificate").val('');
        return;
    }
    // Check files count
    if (files.length > 10) {
        alert("Max 10 files are allowed to upload.");
        $("#TestCertificate").val('');
        return;
    }
});
inputGeneralArrangement.addEventListener('change', (e) => {
    // Retrieve all files
    const files = inputGeneralArrangement.files;
    var isSizelarge = false;
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 5) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB.");
        $("#GeneralArrangement").val('');
        return;
    }
    // Check files count
    if (files.length > 10) {
        alert("Max 10 files are allowed to upload.");
        $("#GeneralArrangement").val('');
        return;
    }
});
inputGuaranteeTechnicalParticulars.addEventListener('change', (e) => {
    // Retrieve all files
    const files = inputGuaranteeTechnicalParticulars.files;
    var isSizelarge = false;
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 5) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB.");
        $("#GuaranteeTechnicalParticulars").val('');
        return;
    }
    // Check files count
    if (files.length > 10) {
        alert("Max 10 files are allowed to upload.");
        $("#GuaranteeTechnicalParticulars").val('');
        return;
    }
});

inputPastOrders.addEventListener('change', (e) => {
    // Retrieve all files
    const files = inputPastOrders.files;
    var isSizelarge = false;
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 5) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB.");
        $("#PastOrders").val('');
        return;
    }
    // Check files count
    if (files.length > 5) {
        alert("Max 5 files are allowed to upload.");
        $("#PastOrders").val('');
        return;
    }
});
// Listen for files selection
input.addEventListener('change', (e) => {
    // Retrieve all files
    const files = input.files;

    var isSizelarge = false;
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 5) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB.");
        $("#PastExperienceList").val('');
        return;
    }
    // Check files count
    if (files.length > 5) {
        alert("Max 5 files are allowed to upload.");
        $("#PastExperienceList").val('');
        return;
    }
});

// Listen for files selection
inputPerformanceCertificate.addEventListener('change', (e) => {
    // Retrieve all files
    const files = inputPerformanceCertificate.files;

    var isSizelarge = false;
    for (var i = 0; i < files.length; i++) {
        var filesize = files[i].size / 1048576;
        if (filesize > 5) {
            isSizelarge = true;
        }
    }
    if (isSizelarge == true) {
        alert("Max file size is 5 MB.");
        $("#PerformanceCertificate").val('');
        return;
    }
    // Check files count
    if (files.length > 5) {
        alert("Max 5 files are allowed to upload.");
        $("#PerformanceCertificate").val('');
        return;
    }
});


function ValidateFile(obj) {
    //var format = /[!@&#$%^*()+\=\[\]{};':"\\|,<>\/?]+/;

    //if (format.test($(obj).val().replace(/^.*[\\\/]/, ''))) {
    //    alert('File name should not contain any special characters!');
    //    $(obj).val("");
    //    return false;
    //}
    var ext = $(obj).val().split('.').pop().toLowerCase();

    if ($.inArray(ext, ['jpg', 'jpeg', 'png', 'pdf', 'ppt', 'pptx', 'doc', 'docx', 'xls', 'xlsx', 'txt', 'zip', 'rar']) == -1) {
        alert('File extension not supported!');
        $(obj).val("");
        return false;
    }
}

function ClearSelected(obj) {
    $(obj).val("");
}