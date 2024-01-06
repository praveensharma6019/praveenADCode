function PasswordCheck(value) {
    var regEx = /^(?=.*[a-z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/;
    if (value.match(regEx)) {
        return true;
    }
    else {
        //alert('Password should contain at least 1 char, 1 Special char, 1 number and the min length should be 8 char.');
        $(this).val('');
        return false;
    }
}


function AdharCardNoCheck(value) {
    var regEx = /^\d{4}\d{4}\d{4}$/;
    if (value.match(regEx)) {
        return true;
    }
    else {
        $(this).val('');
        return false;
    }
}

//function upLoadvalidatedata() {
//    var docA = $('#DocUploadSectionA tr').length;
//    var docB = $('#DocUploadSectionB tr').length;
//    var docC = $('#DocUploadSectionC tr').length;
//    var count = (docA + docB + docC) - 3;
//    if (count < 3) {
//        var message = "Please upload at least one document in section A, B & C.";
//        jQuery.noConflict();
//        $('#msgarea').html(message);
//        $('#Messagepopup1').modal('show');
//        return false;
//    }
//}


//$(document).ready(function () {

//    function isEmptyOrSpaces(str) {
//        return str === null || str.match(/^ *$/) !== null;
//    }
//    if ($('#msgpop').text() == 'CNG') {
//        // FOR GETTING TRACK INQUIRY STATUS.

//        $.ajax({
//            type: "GET",
//            url: '/api/AdaniGas/GetCNGInquiryStatus',
//            data: { "InquiryNo": '@ViewBag.InquiryNo' },
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            async: false,
//            success: function (data) {
//                $('#AssignedToDealerName').html(data.OEMName);
//                $('#AssignedToDealerDate').html(data.OEMDate);
//                $('#FittingName').html(data.OEMName);
//                $('#FittingDate').html(data.ConvDate);
//                $('#DocUploadName').html(data.OEMName);
//                $('#DocUploadDate').html(data.DocUpload);
//                $('#CardGivenName').html('@ViewBag.CustomerName');
//                $('#CardGivenDate').html(data.CardGiven);

//               // jQuery.noConflict();
//                $('#myModalCNGStatus').modal('show');
//            },
//            failure: function () {
//                alert("Failed!");
//            }
//        });


//    }
//    else if (!isEmptyOrSpaces($('#msgpop').text())) {
//        $('#Messagepopup').modal('show');
//    }


//});



//var  = $.noConflict(true);
function SaveFile1() {
    var documentNo = $("#DocNo2").val();
    var filepath = $("#File2").val();
    var docid = $("#DocumentId").val();
    var selectedsection = $("#SelectedSection").val();

    var isValid = true;
    if (selectedsection == 'A' || selectedsection == 'B') {
        if (docid == "Select Value" || filepath == "") {
            alert("Please Select Document Type or Select a File.");
            isValid = false;
        }
    }
    else {
        if (docid == "Select Value" || documentNo == "") {
            alert("Please Select Document Type or enter Document no.");
            isValid = false;
        }
    }

    var SelectedDocumentText = $("#DocumentId option:selected").text();
    var SelectedDocumentValue = $("#DocumentId option:selected").val();

    if (selectedsection == 'A') {
        $('#DocUploadSectionA > tbody  > tr').each(function () {
            var cellvalue = $(this).find("#DocText").val()

            if (cellvalue == SelectedDocumentText) {
                alert('This Document Type already uploaded');
                isValid = false;
                return false;
            }
        });
    }
    else if (selectedsection == 'B') {

        $('#DocUploadSectionB > tbody  > tr').each(function () {
            var cellvalue = $(this).find("#DocText").val()

            if (cellvalue == SelectedDocumentText) {
                alert('This Document Type already uploaded');
                isValid = false;
                return false;
            }
        });
    }
    else {
        $('#DocUploadSectionC > tbody  > tr').each(function () {
            var cellvalue = $(this).find("#DocText").val()

            if (cellvalue == SelectedDocumentText) {
                alert('This Document Type already uploaded.');
                isValid = false;
                return false;
            }
        });
    }


    if (isValid == true) {
        var filepath = "#";
        if ($("#File2").get(0).files[0] != '') {
            $.ajax({
                url: "/api/AdaniGas/GetFormData",
                type: "POST",
                data: function () {
                    var data = new FormData();
                    data.append("File1", $("#File2").get(0).files[0]);
                    return data;
                }(),
                async: false,
                contentType: false,
                processData: false,
                success: function (response) {
                    filepath = response;
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    console.log(errorMessage);
                }
            });
        }
        var d = new Date();
        var strDate = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();

        var rowcount = 0;
        if (selectedsection == 'A')
            rowcount = $('#DocUploadSectionA tr').length + 1;
        else if (selectedsection == 'B')
            rowcount = $('#DocUploadSectionB tr').length + 1;
        else
            rowcount = $('#DocUploadSectionC tr').length + 1;



        var innerhtml = "<tr class='bgtable' id='Tr" + rowcount + "'>";
        innerhtml += "<input type='hidden' id='DocId' name='DocId' value='" + rowcount + "'>";
        innerhtml += "<input type='hidden' id='DocText' name='DocText' value='" + SelectedDocumentText + "'>";

        innerhtml += "<input type='hidden' id='" + SelectedDocumentValue + "_DocType' name='" + SelectedDocumentValue + "_DocType' value='" + SelectedDocumentValue + "'>";
        innerhtml += "<input type='hidden' id='" + SelectedDocumentValue + "_FilePath' name='" + SelectedDocumentValue + "_FilePath' value='" + filepath + "'>";
        innerhtml += "<input type='hidden' id='" + SelectedDocumentValue + "_DocNo' name='" + SelectedDocumentValue + "_DocNo' value='" + documentNo + "'>";
        innerhtml += "<td class='text-center'><i class='fa fa-file-o' aria-hidden='true'></i></td>";
        innerhtml += "<td class='text-center'>" + SelectedDocumentText + "</td>";
        innerhtml += "<td class='text-center'>" + documentNo + "</td>";
        innerhtml += "<td class='text-center'>" + strDate + "</td>";
        if (filepath != "#")
            innerhtml += "<td class='text-center iconbox'><a class='fa fa-eye' aria-hidden='true' href='ViewDocument?FilePath=" + filepath + "&FileName=" + SelectedDocumentText + "' target='_blank'></a> <a class='fa fa-download' aria-hidden='true' href='DownloadFile?FilePath=" + filepath + "&FileName=" + SelectedDocumentText + "'></a> ";
        else
            innerhtml += "<td class='text-center iconbox'>";

        if (selectedsection == 'A')
            innerhtml += "<a class='fa fa-trash' aria-hidden='true' onclick='deletefunA(" + rowcount + ")'></a></td>";
        else if (selectedsection == 'B')
            innerhtml += "<a class='fa fa-trash' aria-hidden='true' onclick='deletefunB(" + rowcount + ")'></a></td>";
        else
            innerhtml += "<a class='fa fa-trash' aria-hidden='true' onclick='deletefunC(" + rowcount + ")'></a></td>";
        innerhtml += "</tr>";

        if (selectedsection == 'A')
            $("#DocUploadSectionA tbody").append(innerhtml);
        else if (selectedsection == 'B')
            $("#DocUploadSectionB tbody").append(innerhtml);
        else
            $("#DocUploadSectionC tbody").append(innerhtml);

        $("#myModalNew").modal("hide");
        $("#DocNo2").val("").append();
        $("#File2").val("").append();
        $("#SelectedSection").val("").append();

    }
}

function deletefunA(TrId) {
    var id = "Tr" + TrId;
    $("#DocUploadSectionA").find('#' + id).remove();
}
function deletefunB(TrId) {
    var id = "Tr" + TrId;
    $("#DocUploadSectionB").find('#' + id).remove();
}

function deletefunC(TrId) {
    var id = "Tr" + TrId;
    $("#DocUploadSectionC").find('#' + id).remove();
}


function upLoadvalidatedata() {
    var docA = $('#DocUploadSectionA tr').length;
    var docB = $('#DocUploadSectionB tr').length;
    var docC = $('#DocUploadSectionC tr').length;
    var count = (docA + docB + docC) - 3;
    if (count < 3) {
        var message = "Please upload at least one document in section A, B & C.";
        //$.noConflict();
        $('#msgarea').html(message);
        $('#Messagepopup1').modal('show');
        return false;
    }
    
}

function upLoadrejecteddata() {
    var docA = $('#DocUploadSectionA tr').length;
    var docB = $('#DocUploadSectionB tr').length;
    var docC = $('#DocUploadSectionC tr').length;
    var count = (docA + docB + docC) - 3;
    if (count < 3) {
        var message = "Please upload at least one document in section A, B & C.";
        //$.noConflict();
        $('#msgarea').html(message);
        $('#Messagepopup1').modal('show');
        return false;
    }

    var rejectedcount = $('.Rejected').length;
    if (rejectedcount > 0) {
        alert('Please upload rejected document first');
        return false;
    }
}

function SelectSection(SectionName) {
    $("#SelectedSection").val(SectionName);
    $('#DocumentId').empty();
    if (SectionName == 'A') {
        $('#DocumentId').append($('<option>', { value: "Select Value", text: "Select Value" }));
        $('#DocumentId').append($('<option>', { value: "Driving License", text: "Driving License" }));
        $('#DocumentId').append($('<option>', { value: "PAN No", text: "PAN No" }));
        $('#DocumentId').append($('<option>', { value: "Passport", text: "Passport" }));
        $('#DocumentId').append($('<option>', { value: "Ration Card", text: "Ration Card" }));
        $('#DocumentId').append($('<option>', { value: "Recent BSNL Telephone Bill", text: "Recent BSNL Telephone Bill" }));
        $('#DocumentId').append($('<option>', { value: "Recent Electricity Bill", text: "Recent Electricity Bill" }));
        $('#DocumentId').append($('<option>', { value: "Society Certificate", text: "Society Certificate" }));
        $('#DocumentId').append($('<option>', { value: "Voter ID/Ration card No", text: "Voter ID/Ration card No" }));

    }
    else if (SectionName == 'B') {
        $('#DocumentId').append($('<option>', { value: "Select Value", text: "Select Value" }));
        $('#DocumentId').append($('<option>', { value: "A.M.C Tax Bill", text: "A.M.C Tax Bill" }));
        $('#DocumentId').append($('<option>', { value: "Builder Allotment Letter", text: "Builder Allotment Letter" }));
        $('#DocumentId').append($('<option>', { value: "Index", text: "Index" }));
        $('#DocumentId').append($('<option>', { value: "Regd. Sale Deed", text: "Regd. Sale Deed" }));
        $('#DocumentId').append($('<option>', { value: "Soc. Share Deed", text: "Soc. Share Deed" }));
        $('#DocumentId').append($('<option>', { value: "Other (If Any)", text: "Other (If Any)" }));
    }
    else {
        $('#DocumentId').append($('<option>', { value: "Select Value", text: "Select Value" }));
        $('#DocumentId').append($('<option>', { value: "Aadhar No.", text: "Aadhar No." }));
    }
}


$("#DocNo2").change(function () {
    if ($("#SelectedSection").val() == "C") {
        if (!AdharCardNoCheck($(this).val())) {
            $("#AdharCardNoValidationMessage").show();
            $("#DocNo2").css("border-color", "red");
            $(this).val('');
        }
        else {
            $("#DocNo2").css("border-color", "#ccc");
            $("#AdharCardNoValidationMessage").hide();
        }
    }
});
