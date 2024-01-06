$(document).ready(function () {
    $("#loader-wrapper").hide();
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {

        var elem = document.getElementById('message_modal');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();

        //$('#message_modal').modal('show');
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
    $(function () {
        $('#datetimepickerLECStartDate').datetimepicker(
            { format: 'DD/MM/YYYY' });
    });
    $(function () {
        $('#datetimepickerLECEndDate').datetimepicker({ format: 'DD/MM/YYYY' });
    });
    $("#searchCONStatusApp").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#CONApps tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});

$('.cexport').on("click", function () {
    var sdate = $("#datetimepickerLECStartDate").val();
    var edate = $("#datetimepickerLECEndDate").val();
    var status = $('#Status').val();
    var url = $(this).attr('href') + '?sdate=' + sdate + '&edate=' + edate + '&status=' + status;
    location.href = url;
    return false;
});

$('#frmCONApplication').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});

$("#viewapplicationform").click(function () {
    $("#divviewapplicationform").show();
});

function GetScrollPosition() {
    localStorage['page'] = document.URL;
    localStorage['scrollTop'] = $(document).scrollTop();
}

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

//Email Validation
function validateEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

//Mobile number number validation
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
        $("#mobileerror").html("");
    }
    else {
        $("#mobileerror").html("Please enter a 10 digit valid mobile number");
    }
}

function validateEmailId(event, t) {
    var emailAddress = $("#emailAddress").val();
    if (validateEmail(emailAddress)) {
        $("#emailerror").html("");
    }
    else {
        $("#emailerror").html("Please enter a valid Email Address");
    }
}

function validateName(name) {
    var regex = /^[a-zA-Z ]+$/;
    return regex.test(name);

}


function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) {
            $("#nameerror").html("");
            return true;
        }
        else {
            $("#nameerror").html("Please enter a valid name containing alphabets only");
            return false;
        }
    }
    catch (err) {
        alert(err.Description);
    }
}

//$("#CheckApplication").click(function (event) {
//    $("#loader-wrapper").show();
//    $("#errorCaReg").html("");
//    var accountNumber = $("#AccountNoForCheckApplication").val();
//    var registrationNumber = $("#RegistrationNoForCheckApplication").val();
//    jQuery.ajax(
//        {
//            url: "/api/AEMLChangeOfName/ChangeOfNameCheckApplication",
//            method: "POST",
//            data: { accountNumber: accountNumber, registrationNumber: registrationNumber },
//            success: function (data) {
//                if (data.Issuccess == true) {
//                    location.href = data.Message;
//                }
//                else {
//                    $("#loader-wrapper").hide();
//                    $("#errorCaReg").html(data.Message);
//                    //alert(data.Message);
//                }
//            },
//            error: function (textStatus, errorThrown) {
//                $("#loader-wrapper").hide();
//                $("#errorCaReg").html("An error ocurred, please try again!");
//            }
//        });
//});

$("#CancelApp").click(function () {
    window.location.reload();
});

$("#CheckApplication").click(function (event) {
    $("#loader-wrapper").show();
    $("#errorCaReg").html("");
    var accountNumber = $("#AccountNoForCheckApplication").val();
    var registrationNumber = $("#RegistrationNoForCheckApplication").val();
    var OTPNumber = $("#OTPNumberForCheckApplication").val();
    jQuery.ajax(
        {
            url: "/api/AEMLChangeOfName/ChangeOfNameCheckApplication",
            method: "POST",
            data: { accountNumber: accountNumber, registrationNumber: registrationNumber, OTPNumber: OTPNumber },
            success: function (data) {
                if (data.Issuccess == true && data.IsVerified == true) {
                    location.href = data.Message;
                }
                else if (data.Issuccess == true && data.IsVerified == false) {
                    $("#CheckApplication").val("Proceed");
                    if (accountNumber != null && accountNumber != "") {
                        $("#divAccountNum").show();
                        $("#AccountNoForCheckApplication").attr('readonly', true);
                        $("#divRegNum").hide();
                        $("#divor").hide();
                        $("#CancelApp").show();
                    }
                    else {
                        $("#divAccountNum").hide();
                        $("#divRegNum").show();
                        $("#RegistrationNoForCheckApplication").attr('readonly', true);
                        $("#divor").hide();
                        $("#CancelApp").show();
                    }
                    $("#loader-wrapper").hide();
                    $("#OTPForCheckApplication").show();
                    $("#OTPNumberForCheckApplication").focus();
                    $("#errorCaReg").html(data.Message);
                }
                else {
                    $("#loader-wrapper").hide();
                    $("#errorCaReg").html(data.Message);
                }
            },
            error: function (textStatus, errorThrown) {
                $("#loader-wrapper").hide();
                $("#errorCaReg").html("An error ocurred, please try again!");
            }
        });
});

$("#CheckApplicationLEC").click(function (event) {
    $("#loader-wrapper").show();
    $("#errorCaReg").html("");
    var accountNumber = $("#AccountNoForCheckApplication").val();
    var registrationNumber = $("#RegistrationNoForCheckApplication").val();
    var OTPNumber = $("#OTPNumberForCheckApplication").val();
    jQuery.ajax(
        {
            url: "/api/AEMLChangeOfName/ChangeOfNameCheckApplicationLEC",
            method: "POST",
            data: { accountNumber: accountNumber, registrationNumber: registrationNumber, OTPNumber: OTPNumber },
            success: function (data) {
                if (data.Issuccess == true && data.IsVerified == true) {
                    location.href = data.Message;
                }
                else if (data.Issuccess == true && data.IsVerified == false) {
                    $("#CheckApplication").val("Proceed");
                    if (accountNumber != null && accountNumber != "") {
                        $("#divAccountNum").show();
                        $("#AccountNoForCheckApplication").attr('readonly', true);
                        $("#divRegNum").hide();
                        $("#divor").hide();
                        $("#CancelApp").show();
                    }
                    else {
                        $("#divAccountNum").hide();
                        $("#divRegNum").show();
                        $("#RegistrationNoForCheckApplication").attr('readonly', true);
                        $("#divor").hide();
                        $("#CancelApp").show();
                    }
                    $("#loader-wrapper").hide();
                    $("#OTPForCheckApplication").show();
                    $("#OTPNumberForCheckApplication").focus();
                    $("#errorCaReg").html(data.Message);
                }
                else {
                    $("#loader-wrapper").hide();
                    $("#errorCaReg").html(data.Message);
                }
            },
            error: function (textStatus, errorThrown) {
                $("#loader-wrapper").hide();
                $("#errorCaReg").html("An error ocurred, please try again!");
            }
        });
});

$("#UseRegistrationBtn").click(function (event) {
    var mobileNo = $("#mobileNumber").val();
    var emailAddress = $("#emailAddress").val();
    var company = $("#company").val();
    var fax = $("#fax").val();
    var name = $("#name").val();
    if (!validateName(name)) {
        event.preventDefault();
        $("#nameerror").html("Please enter a valid name containing alphabets only");
        $("#name").focus();
        return false;
    }
    else {
        $("#nameerror").html("");
    }
    if (company == null || company.trim() == "") {
        event.preventDefault();
        $("#companyerror").html("Please enter a valid company name");
        $("#company").focus();
        return false;
    }
    else {
        $("#companyerror").html("");
    }
    if (!validateMobileNo(mobileNo)) {
        event.preventDefault();
        $("#mobileerror").html("Please enter a 9 digit valid mobile number");
        $("#mobileNumber").focus();
        return false;
    }
    else {
        $("#mobileerror").html("");
    }
    if (fax != null && fax.trim() != "") {
        if (!validateFax(fax)) {
            event.preventDefault();
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            $("#fax").focus();
            return false;
        }
        else {
            $("#faxerror").html("Please enter valid Fax number");
        }
    }
    else {
        $("#faxerror").html("");
    }
    if (!validateEmail(emailAddress)) {
        event.preventDefault();
        $("#emailerror").html("Please enter a valid Email Address");
        $("#emailAddress").focus();
        return false;
    }
    else {
        $("#emailerror").html("");
    }
    // $('#UseRegistrationForm').submit();
    // return true;
});


//CON module


$('#ddlTitle').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('#ddlPremiseType').on('change', function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('#BillingSelectedSuburb').on('change', function (event) {
    var form = $(event.target).parents('form');
    form.submit();
});

$('#BillingSelectedCity').on('change', function (event) {
    var form = $(event.target).parents('form');
    form.submit();
});

$('#SelectedSuburb').on('change', function (event) {
    var form = $(event.target).parents('form');
    form.submit();
});

$('#SelectedCity').on('change', function (event) {
    var form = $(event.target).parents('form');
    form.submit();
});


$('.rbIsRentalProperty').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('.rbIsContinueWithExistingSD').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('.rbIsStillLiving').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('.rbApplicantType').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('.rbIsAddressCorrectionRequired').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('.rbIsBillingAddressDifferent').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$("#frmregistrationValidate").submit(function (event) {
    $("#loader-wrapper").show();
});

$("#frmCONApplication").submit(function (event) {
    var buttonName = $(document.activeElement).attr('name');

    if (buttonName === "SubmitApplication") {
        //check validation
        if ($('#ddlPremiseType').val() == null || $('#ddlPremiseType').val() == "") {
            $("#docErrorMessage").html("Please select Premise Type");
            return false;
        }

        var applicantType = $("input[name='ApplicantType']:checked").val();
        //if joint then both names
        if (applicantType == "2") {
            if ($('#Name1Joint').val() == null || $('#Name1Joint').val() == "") {
                $("#docErrorMessage").html("Please enter Name of Applicant 1");
                return false;
            }
            if ($('#Name2Joint').val() == null || $('#Name2Joint').val() == "") {
                $("#docErrorMessage").html("Please enter Name of Applicant 2");
                return false;
            }
        }
        //if application type is 1 or 3 then title is mandatory
        if (applicantType == "1" || applicantType == "3") {
            if ($('#ddlTitle').val() == null || $('#ddlTitle').val() == "") {
                $("#docErrorMessage").html("Please select Title");
                return false;
            }
            if ($("#ddlTitle").val() == "0006") {
                if ($('#OrganizationName').val() == null || $('#OrganizationName').val() == "") {
                    $("#docErrorMessage").html("Please enter Organization Name");
                    return false;
                }
            }
            else {
                if ($('#FirstName').val() == null || $('#FirstName').val() == "") {
                    $("#docErrorMessage").html("Please enter FirstName");
                    return false;
                }
                if ($('#LastName').val() == null || $('#LastName').val() == "") {
                    $("#docErrorMessage").html("Please enter LastName");
                    return false;
                }
            }
        }

        //if premise is redidential then address check required

        if ($('#ddlPremiseType').val() == "026" && $("input[name='IsAddressCorrectionRequired']:checked").val() == "Yes") {
            if ($('#HouseNumber').val() == null || $('#HouseNumber').val() == "") {
                $("#docErrorMessage").html("Please enter House Number in Address Correction");
                return false;
            }
            if ($('#Street').val() == null || $('#Street').val() == "") {
                $("#docErrorMessage").html("Please enter Street in Address Correction");
                return false;
            }
            if ($('#Landmark').val() == null || $('#Landmark').val() == "") {
                $("#docErrorMessage").html("Please enter Landmark in Address Correction");
                return false;
            }
            if ($('#Area').val() == null || $('#Area').val() == "") {
                $("#docErrorMessage").html("Please enter Area in Address Correction");
                return false;
            }
            if ($('#SelectedSuburb').val() == null || $('#SelectedSuburb').val() == "") {
                $("#docErrorMessage").html("Please select Suburb in Address Correction");
                return false;
            }
            if ($('#SelectedCity').val() == null || $('#SelectedCity').val() == "") {
                $("#docErrorMessage").html("Please select City in Address Correction");
                return false;
            }
            if ($('#SelectedPincode').val() == null || $('#SelectedPincode').val() == "") {
                $("#docErrorMessage").html("Please select Pincode in Address Correction");
                return false;
            }
        }

        if ($("input[name='IsBillingAddressDifferent']:checked").val() == "Yes") {
            if ($('#BillingHouseNumber').val() == null || $('#BillingHouseNumber').val() == "") {
                $("#docErrorMessage").html("Please enter Area in Billing Address");
                return false;
            }
            if ($('#BillingBuildingName').val() == null || $('#BillingBuildingName').val() == "") {
                $("#docErrorMessage").html("Please enter Building Name in Billing Address");
                return false;
            }
            if ($('#BillingStreet').val() == null || $('#BillingStreet').val() == "") {
                $("#docErrorMessage").html("Please enter Street in Billing Address");
                return false;
            }
            if ($('#BillingLandmark').val() == null || $('#BillingLandmark').val() == "") {
                $("#docErrorMessage").html("Please enter Landmark in Billing Address");
                return false;
            }
            if ($('#BillingSelectedSuburb').val() == null || $('#BillingSelectedSuburb').val() == "") {
                $("#docErrorMessage").html("Please select Suburb in Billing Address");
                return false;
            }
            if ($('#BillingSelectedCity').val() == null || $('#BillingSelectedCity').val() == "") {
                $("#docErrorMessage").html("Please select City in Billing Address");
                return false;
            }
            if ($('#BillingSelectedPincode').val() == null || $('#BillingSelectedPincode').val() == "") {
                $("#docErrorMessage").html("Please select Pincode in Billing Address");
                return false;
            }
        }

        if ($("#IsLEC").val() == null || $("#IsLEC").val() != "True") {
            if ($('#EmailId').val() == null || $('#EmailId').val() == "") {
                $("#docErrorMessage").html("Please enter EmailId");
                return false;
            }
        }
        if ($('#MobileNo').val() == null || $('#MobileNo').val() == "") {
            $("#docErrorMessage").html("Please enter MobileNo");
            return false;
        }

        if ($('#SelectedBillLanguage').val() == null || $('#SelectedBillLanguage').val() == "") {
            $("#docErrorMessage").html("Please select Bill Language");
            return false;
        }

        //if rentad premise then address check required
        if ($('#ddlPremiseType').val() == "034" || $("input[name='IsRentalProperty']:checked").val() == "Yes") {
            if ($('#LandlordName').val() == null || $('#LandlordName').val() == "") {
                $("#docErrorMessage").html("Please enter Landlord Name");
                return false;
            }
            if ($('#LandlordMobile').val() == null || $('#LandlordMobile').val() == "") {
                $("#docErrorMessage").html("Please enter Landlord Mobile");
                return false;
            }
        }

        var isContinueWithExistingSD = $("input[name='IsContinueWithExistingSD']:checked").val();
        if (isContinueWithExistingSD == "Yes") {
            if (Number($("#SecurityDepositeAmount").val()) < 0 || Number($("#SecurityDepositeAmount").val()) > Number($("#hdnExistingSecurityDepositeAmount").val())) {
                $("#docErrorMessage").html("Security Deposite Amount should be between 0 and Existing Security Deposite Amount");
                return false;
            }
        }

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

            if ($("#ddlID").val() != undefined && $("#ddlID").val() == "") {
                message = message + "Please select any Identity Document</br>";
                error = true;
            }
            if ($("#ddlID").val() != undefined && $("#ddlID").val() != "") {
                if ($("#" + docname[0].id).val() != null || $("#" + docname[0].id).val() != "") {
                    if ($("#ddlID").val() == "990") {
                        var IsValid = validateAadhar($("#" + docname[0].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid 12 digit Aadhar number eg: xxxxxxxxxxxx </br>";
                            error = true;
                        }
                    }
                    if ($("#ddlID").val() == "920") {
                        var IsValid = validatePAN($("#" + docname[0].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
                            error = true;
                        }
                    }
                    if ($("#ddlID").val() == "940") {
                        var IsValid = validatePassport($("#" + docname[0].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid Passport number </br>";
                            error = true;
                        }
                    }

                }
            }
            if ($("#ddlID2").val() != undefined && $("#ddlID2").val() == "") {
                message = message + "Please select any Identity Document for Applicant 2</br>";
                error = true;
            }
            if ($("#ddlID2").val() != undefined && $("#ddlID2").val() != "") {
                if ($("#" + docname[1].id).val() != null || $("#" + docname[1].id).val() != "") {
                    if ($("#ddlID2").val() == "990") {
                        var IsValid = validateAadhar($("#" + docname[1].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid 12 digit Aadhar number eg: xxxxxxxxxxxx </br>";
                            error = true;
                        }
                    }
                    if ($("#ddlID2").val() == "920") {
                        var IsValid = validatePAN($("#" + docname[1].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
                            error = true;
                        }
                    }
                    if ($("#ddlID2").val() == "940") {
                        var IsValid = validatePassport($("#" + docname[1].id).val());
                        if (!IsValid) {
                            message = message + "Please enter valid Passport number </br>";
                            error = true;
                        }
                    }

                }
            }
            if ($("#ddlPH").val() != undefined && $("#ddlPH").val() == "") {
                message = message + "Please select any Photo Identity Document</br>";
                error = true;
            }
            if ($("#ddlSD").val() != undefined && $("#ddlSD").val() == "") {
                message = message + "Please select any one Supporting Document</br>";
                error = true;
            }
            if ($("#ddlOD").val() != undefined && $("#ddlOD").val() == "") {
                message = message + "Please select any Ownership Document</br>";
                error = true;
            }
            if ($("#ddlOT").val() != undefined && $("#ddlOT").val() == "") {
                message = message + "Please select any one Statutory / Legal Requirement Document</br>";
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

        ////Check for Mandatory documents
        //var MandFlg = $("[id*='mand_']");
        //var FileUpload1 = $("[id*='file_']");
        //var madfile = $("[id*='manddocname_']");
        //var error = true;
        //var message = "* Following files are required:</br>";
        //for (var i = 0; i < MandFlg.length; i++) {
        //    if ($("#" + MandFlg[i].id).val() == 'True' && $("#" + FileUpload1[i].id).val() == '') {
        //        error = false;
        //        message = message + " " + $("#" + madfile[i].id).val() + "</br>";

        //    }
        //}
        //if (!error) {
        //    $("#docErrorMessage").html(message);
        //    return false;
        //}
        //else $("#docErrorMessage").html("");

        //check if tearms and cinditions are checked and confrim
        if (!$('input[name="termsCb"]').is(':checked') || !$('input[name="confirmCb"]').is(':checked')) {
            $("#docErrorMessage").html("Please confirm terms and conditions by checking the check boxes.");
            return false;
        }

        //confirmation pop up message
        var name = $("#FirstName").val();
        //var applicantType = $("input[name='ApplicantType']:checked").val();
        if (applicantType == "1" || applicantType == "3") {
            if ($("#ddlTitle").val() == "0006") {
                name = $("#ddlTitle option:selected").text() + " " + $("#OrganizationName").val().toUpperCase();
            }
            else {
                name = $("#ddlTitle option:selected").text() + " " + $("#FirstName").val().toUpperCase() + " " + $("#MiddleName").val().toUpperCase() + " " + $("#LastName").val().toUpperCase();
            }
        }
        else {
            name = $("#Name1Joint").val().toUpperCase() + " & " + $("#Name2Joint").val().toUpperCase();
        }

        //confirmation pop up
        $('.confirmation_modal_message').html("Your name will appear on your energy bill as below: <br><br><strong>" + name + "</strong><br><br> Are you sure you want to submit the application?");
        //$('#confirmation_modal').modal("show");
        //debugger;
        console.log("LECCNB");
        var elem = document.getElementById('confirmation_modal');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();
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
        if ($("#ddlID").val() != undefined && $("#ddlID").val() != "") {
            if ($("#" + docNumber[0].id).val() != null || $("#" + docNumber[0].id).val() != "") {
                if ($("#ddlID").val() == "990") {
                    var IsValid = validateAadhar($("#" + docNumber[0].id).val());
                    if (!IsValid) {
                        message = message + "Please enter valid 12 digit Aadhar number eg: xxxxxxxxxxxx </br>";
                        error = true;
                    }
                }
                if ($("#ddlID").val() == "920") {
                    var IsValid = validatePAN($("#" + docNumber[0].id).val());
                    if (!IsValid) {
                        message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
                        error = true;
                    }
                }
                if ($("#ddlID").val() == "940") {
                    var IsValid = validatePassport($("#" + docNumber[0].id).val());
                    if (!IsValid) {
                        message = message + "Please enter valid Passport number </br>";
                        error = true;
                    }
                }

            }
        }
    }
    if ($("#ddlID2").val() != undefined && $("#ddlID2").val() != "") {
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



$('.confirmation_modalbtnYesNO').click(function (e) {
    //alert("btnYesNO");

    if (this.value === '1') {
        YesNO = '1';
        //$('#confirmation_modal').modal("hide");
        var elem = document.getElementById('confirmation_modal');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.close();
        $("#SubmitApplication").click();
    }
    else {
        $('#confirmation_modal').modal("hide");
    }
    e.preventDefault();
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
        $("#message_modal .modal-content").text("Only jpg, jpeg, png and pdf files are allowed.");
        //$("#message_modal").modal("show");

        var elem = document.getElementById('message_modal');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();

        $(value).val('');
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];
        if (size > 5) {
            $("#message_modal .modal-content").text("Max file Size is 5 MB.");

            //$("#message_modal").modal("show");

            var elem = document.getElementById('message_modal');
            var instance = M.Modal.init(elem, { dismissible: false });
            instance.open();

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
        $("#message_modal .modal-content").text("Only jpg, jpeg files are allowed.");

        var elem = document.getElementById('message_modal');
        var instance = M.Modal.init(elem, { dismissible: false });
        instance.open();
        //$("#message_modal").modal("show");

        $(value).val('');
        return false;
    }
    else {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];
        if (size > 5) {
            $("#message_modal .modal-content").text("Max file Size is 5 MB.");

            var elem = document.getElementById('message_modal');
            var instance = M.Modal.init(elem, { dismissible: false });
            instance.open();

            //$("#message_modal").modal("show");
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

