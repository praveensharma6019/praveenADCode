$(function () {
    $('#port-select').change(function () {
        $('.bank-child').hide();
        $('.' + $(this).val()).show();
    });
});
$(function () {
    $('#port-select').change(function () {
        $('#customers-option').hide();
        $('.' + $(this).val()).show();
    });
});
$(function () {
    $('#customer-type').change(function () {
        $('.customers-option').hide();
        $('.' + $(this).val()).show();
    });
});



var max_fields = 3; //maximum input boxes allowed
var otherwrapper = $(".other_certification"); //Fields otherwrapper
var otheradd_button = $(".other_certification-add_field_button"); //Add button ID

var x = 1; //initlal text box count
$(otheradd_button).click(function (e) { //on add input button click
    e.preventDefault();
    if (x < max_fields) { //max input box allowed
        x++; //text box increment
        var ftmName = 'CommunicationEmail' + x
        $(otherwrapper).append('<div class="col-lg-4 mb-3"><div class="form-group"><input type="Email" name=' + ftmName + ' class="form-control"/><a href="#" class="other_certification-remove_field"></a></div></div>'); //add input box
    }
});
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});
$("#customer-type").on("change", function () {
    var marker = $("#customer-type").val();
    $('#upload1').removeClass("is-required");
    $('#upload2').removeClass("is-required");
    $('#upload3').removeClass("is-required");
    $('#upload4').removeClass("is-required");
    $('#upload5').removeClass("is-required");
    $('#upload6').removeClass("is-required");
    $('#upload7').removeClass("is-required");
    $('#upload8').removeClass("is-required");
    $('#upload9').removeClass("is-required");
    $('#upload10').removeClass("is-required");
    $('#upload13').removeClass("is-required");
    if (marker == "Shipping-Line" || marker == "NVOCC") {
        $('#upload1').addClass("is-required");
        $('#upload2').addClass("is-required");
        $('#upload3').addClass("is-required");
        $('#upload5').addClass("is-required");
        $('#upload6').addClass("is-required");
    }
    else if (marker == "Vessel-Agent") {
        $('#upload2').addClass("is-required");
        $('#upload3').addClass("is-required");
        $('#upload5').addClass("is-required");
        $('#upload6').addClass("is-required");
        $('#upload11').addClass("is-required");
    }
    else {
        $('#upload2').addClass("is-required");
        $('#upload5').addClass("is-required");
        $('#upload6').addClass("is-required");
        $('#upload10').addClass("is-required");
    }
    if (marker != "CHA") {
        $('#upload13').addClass("is-required");
    }
});
function ValidateEmail(email) {
    var expr = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;
    return expr.test(email);
};
function checkForm(formdate) {
    // regular expression to match required date format
    re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
    if (formdate != '' && !formdate.match(re)) {
        alert("Invalid date format");
        return false;
    }
    return true;
}

$(otherwrapper).on("click", ".other_certification-remove_field", function (e) { //user click on remove text
    e.preventDefault(); $(this).parent('div').parent('.col-lg-4').remove(); x--;
})
//Other Certification
$('#defaultCheck1').change(function () {
    if ($(this).is(":checked")) {
        $("#LocalOfficeAddress").val($('#RegisteredOfficeAddress').val()).prop("disabled", true);
        $("#LocalOfficeState").val($('#RegisteredOfficeState').val()).prop("disabled", true);
        $("#LocalOfficeCountry").val($('#RegisteredOfficeCountry').val()).prop("disabled", true);
        $("#LocalOfficeContactNumber").val($('#RegisteredOfficeContactNumber').val()).prop("disabled", true);
        $("#LocalOfficeEmail").val($('#RegisteredOfficeEmail').val()).prop("disabled", true);
        return;
    };
    if ($(this).is(":not(:checked)")) {
        $("#LocalOfficeAddress").val("").prop("disabled", false);
        $("#LocalOfficeState").val("").prop("disabled", false);
        $("#LocalOfficeCountry").val("").prop("disabled", false);
        $("#LocalOfficeContactNumber").val("").prop("disabled", false);
        $("#LocalOfficeEmail").val("").prop("disabled", false);
        return;
    }
});
// $("input:radio[name=exampleRadios]").change(function() {
// if(this.val()=="1")
// {
// $("#MICT").val(""). prop("disabled", false);
// }
// if(this.val()=="1")
// {
// $("#MICT").val(""). prop("disabled", true);
// }
// })
$('#AgreementWithPrincipal').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#AgreementWithPrincipal')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#CertificationOfIncorporation').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#CertificationOfIncorporation')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#MemorandumArticleAssociation').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#MemorandumArticleAssociation')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#DrivingLicense').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#DrivingLicense')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#PanCard').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#PanCard')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#AcknowledgementGSTRegistration').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#AcknowledgementGSTRegistration')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#MunicipalLicence').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#MunicipalLicence')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#AEOLicence').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#AEOLicence')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#CustomDPDPermission').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#CustomDPDPermission')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#CancelledCheque').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#CancelledCheque')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#IECodeCertificate').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#IECodeCertificate')[0].files[0].name;
    $(this).prev('label').text(file);
});
$('#SCMTRProof').change(function () {
    var i = $(this).prev('label').clone();
    var file = $('#SCMTRProof')[0].files[0].name;
    $(this).prev('label').text(file);
});
$(document).ready(function () {
    var today = new Date();

    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    // today = yyyy + '/' + mm + '/' + dd;
    today = dd + '/' + mm + '/' + yyyy;
    $("#tempSubmitOnDate").val(today);
});
$("#portsRegisterbtn").click(function (e) {
    getCaptchaResponseForm();
    e.preventDefault();
});

function getCaptchaResponseForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwlk8mAAAAADm4UhICQR0KxNZKWC_3TwNpDlEm', { action: 'PortAcountRegisteration' }).then(function (token) {
            $('.g-recaptcha').val(token);
            var TypeofCustomers = $("#customer-type").val();
            if (TypeofCustomers == "") { alert("Please Select Type of Customer"); $("#customer-type").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            else {
                if (TypeofCustomers == "Shipping-Line" || TypeofCustomers == "NVOCC") {
                    var ShippingAgentDetails = $("#ShippingAgentDetails").val();
                    if (ShippingAgentDetails == "") { alert("Please enter your Shipping Agent Details"); $("#ShippingAgentDetails").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    var NameofPrincipal = $("#NameofPrincipal").val();
                    if (NameofPrincipal == "") { alert("Please enter Name of Principal"); $("#NameofPrincipal").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }


                    var AgreementWithPrincipal = $("#AgreementWithPrincipal").val();
                    if (AgreementWithPrincipal == "") { alert("Please upload Agreement with the principal"); $("#AgreementWithPrincipal").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    var ext = $('#AgreementWithPrincipal').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#AgreementWithPrincipal").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var CertificationOfIncorporation = $("#CertificationOfIncorporation").val();
                    if (CertificationOfIncorporation == "") { alert("Please upload Certification of Incorporation"); $("#CertificationOfIncorporation").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#CertificationOfIncorporation').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#CertificationOfIncorporation").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var MemorandumArticleAssociation = $("#MemorandumArticleAssociation").val();
                    if (MemorandumArticleAssociation == "") { alert("Please upload Memorandum of Article of Association"); $("#MemorandumArticleAssociation").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#MemorandumArticleAssociation').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#MemorandumArticleAssociation").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var PanCard = $("#PanCard").val();
                    if (PanCard == "") { alert("Please upload Pan Card "); $("#PanCard").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#PanCard').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#PanCard").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var AcknowledgementGSTRegistration = $("#AcknowledgementGSTRegistration").val();
                    if (AcknowledgementGSTRegistration == "") { alert("Please upload Acknowledgement of GST Registration"); $("#AcknowledgementGSTRegistration").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#AcknowledgementGSTRegistration').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#AcknowledgementGSTRegistration").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }

                }
                else if (TypeofCustomers == "Vessel-Agent") {

                    var ShippingAgentDetails1 = $("#ShippingAgentDetails").val();
                    if (ShippingAgentDetails1 == "") { alert("Please enter your Shipping Agent Details"); $("#ShippingAgentDetails").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    var NameofPrincipal1 = $("#NameofPrincipal").val();
                    if (NameofPrincipal1 == "") { alert("Please enter Name of Principal"); $("#NameofPrincipal").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

                    var CertificationOfIncorporation = $("#CertificationOfIncorporation").val();
                    if (CertificationOfIncorporation == "") { alert("Please upload Certification of Incorporation"); $("#CertificationOfIncorporation").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#CertificationOfIncorporation').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#CertificationOfIncorporation").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var MemorandumArticleAssociation = $("#MemorandumArticleAssociation").val();
                    if (MemorandumArticleAssociation == "") { alert("Please upload Memorandum of Article of Association"); $("#MemorandumArticleAssociation").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#MemorandumArticleAssociation').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#MemorandumArticleAssociation").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var PanCard = $("#PanCard").val();
                    if (PanCard == "") { alert("Please upload Pan Card "); $("#PanCard").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#PanCard').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#PanCard").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var AcknowledgementGSTRegistration = $("#AcknowledgementGSTRegistration").val();
                    if (AcknowledgementGSTRegistration == "") { alert("Please upload Acknowledgement of GST Registration"); $("#AcknowledgementGSTRegistration").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#AcknowledgementGSTRegistration').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#AcknowledgementGSTRegistration").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var PowerofAttorney = $("#PowerofAttorney").val();
                    if (PowerofAttorney == "") { alert("Please upload filled Power of Attorney document"); $("#PowerofAttorney").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#PowerofAttorney').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#PowerofAttorney").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                }
                else {

                    var CertificationOfIncorporation = $("#CertificationOfIncorporation").val();
                    if (CertificationOfIncorporation == "") { alert("Please upload Certification of Incorporation Details"); $("#CertificationOfIncorporation").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#CertificationOfIncorporation').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#CertificationOfIncorporation").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var PanCard = $("#PanCard").val();
                    if (PanCard == "") { alert("Please upload Pan Card "); $("#PanCard").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#PanCard').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#PanCard").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var AcknowledgementGSTRegistration = $("#AcknowledgementGSTRegistration").val();
                    if (AcknowledgementGSTRegistration == "") { alert("Please upload Acknowledgement of GST Registration"); $("#AcknowledgementGSTRegistration").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#AcknowledgementGSTRegistration').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#AcknowledgementGSTRegistration").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }

                    var CancelledCheque = $("#CancelledCheque").val();
                    if (CancelledCheque == "") { alert("Please upload Cancelled Cheque"); $("#CancelledCheque").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    vext = $('#CancelledCheque').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#CancelledCheque").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }
                    var IECodeCertificate = $("#IECodeCertificate").val();
                    if (IECodeCertificate == "") { alert("Please upload IE Code Certificate"); $("#IECodeCertificate").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                    ext = $('#IECodeCertificate').val().split('.').pop().toLowerCase();
                    if ($.inArray(ext, ['pdf', 'jpg', 'jpeg']) == -1) {
                        alert('invalid file! Only upload pdf, jpg, jpeg files');
                        $("#IECodeCertificate").focus();
                        $('#portsRegisterbtn').removeAttr("disabled");
                        return false;
                    }

                }
            }
            var PortType = $("#port-select").val();
            if (PortType == "") { alert("Please Select any Port Name"); $("#port-select").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var NameofCompany = $("#NameofCompanyT").val();
            if (NameofCompany == "") { alert("Please enter your Company Name"); $("#NameofCompanyT").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var CompanyRegistrationDate = $("#tempCompanyRegistrationDate").val();
            if (CompanyRegistrationDate == "") { alert("Please enter Company Registration Date"); $("#tempCompanyRegistrationDate").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            var validateDate = checkForm(CompanyRegistrationDate);
            if (validateDate == false) { $("#tempCompanyRegistrationDate").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            if (TypeofCustomers != "CHA") {
                var SCMTRCode = $("#SCMTR-select").val();
                if (SCMTRCode == "") { alert(" Please select SCMTR Code"); $("#SCMTR-select").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
                var SCMTRID = $("#SCMTRID").val();
                if (SCMTRID == "") { alert(" Please enter SCMTR ID"); $("#SCMTRID").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            }
            var PANNumber = $("#PANNumber").val();
            if (PANNumber == "") { alert("Please enter your PAN Card Number"); $("#PANNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            var GSTNumber = $("#GSTNumber").val();
            if (GSTNumber == "") { alert("Please enter your GST Number"); $("#GSTNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            var TANNumber = $("#TANNumber").val();
            if (TANNumber == "") { alert("Please enter your TAN Number"); $("#TANNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var RegisteredOfficeAddress = $("#RegisteredOfficeAddress").val();
            if (RegisteredOfficeAddress == "") { alert("Please enter your Registered Office Details"); $("#RegisteredOfficeAddress").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var RegisteredOfficeState = $("#RegisteredOfficeState").val();
            if (RegisteredOfficeState == "") { alert("Please enter your Registered Office Details"); $("#RegisteredOfficeState").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var RegisteredOfficeCountry = $("#RegisteredOfficeCountry").val();
            if (RegisteredOfficeCountry == "") { alert("Please enter your Registered Office Details"); $("#RegisteredOfficeCountry").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var RegisteredOfficePhoneNumber = $("#RegisteredOfficePhoneNumber").val();
            if (RegisteredOfficePhoneNumber == "") { alert("Please enter your Registered Office Details"); $("#RegisteredOfficePhoneNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var RegisteredOfficeContactNumber = $("#RegisteredOfficeContactNumber").val();
            if (RegisteredOfficeContactNumber == "") { alert("Please enter your Registered Office Details"); $("#RegisteredOfficeContactNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var RegisteredOfficeEmail = $("#RegisteredOfficeEmail").val();
            if (RegisteredOfficeEmail == "") { alert("Please enter your Registered Office Details"); $("#RegisteredOfficeEmail").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            if (!ValidateEmail(RegisteredOfficeEmail)) { alert("Invalid email address."); $("#RegisteredOfficeEmail").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }


            var LocalOfficeAddress = $("#LocalOfficeAddress").val();
            if (LocalOfficeAddress == "") { alert("Please enter your Local Office Details"); $("#LocalOfficeAddress").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var LocalOfficeState = $("#LocalOfficeState").val();
            if (LocalOfficeState == "") { alert("Please enter your Local Office Details"); $("#LocalOfficeState").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var LocalOfficeCountry = $("#LocalOfficeCountry").val();
            if (LocalOfficeCountry == "") { alert("Please enter your Local Office Details"); $("#LocalOfficeCountry").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var LocalOfficePhoneNumber = $("#LocalOfficePhoneNumber").val();
            if (LocalOfficePhoneNumber == "") { alert("Please enter your Local Office Details"); $("#LocalOfficePhoneNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var LocalOfficeContactNumber = $("#LocalOfficeContactNumber").val();
            if (LocalOfficeContactNumber == "") { alert("Please enter your Local Office Details"); $("#LocalOfficeContactNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var LocalOfficeEmail = $("#LocalOfficeEmail").val();
            if (LocalOfficeEmail == "") { alert("Please enter your Local Office Details"); $("#LocalOfficeEmail").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            if (!ValidateEmail(LocalOfficeEmail)) { alert("Invalid email address."); $("#LocalOfficeEmail").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }


            var BillingAddress = $("#BillingAddress").val();
            if (BillingAddress == "") { alert("Please enter your Billing Address Details"); $("#BillingAddress").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var BillingState = $("#BillingState").val();
            if (BillingState == "") { alert("Please enter your Billing Address Details"); $("#BillingState").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var BillingCountry = $("#BillingCountry").val();
            if (BillingCountry == "") { alert("Please enter your Billing Address Details"); $("#BillingCountry").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var BillingPhoneNumber = $("#BillingPhoneNumber").val();
            if (BillingPhoneNumber == "") { alert("Please enter your Billing Address Details"); $("#BillingPhoneNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var BillingContactNumber = $("#BillingContactNumber").val();
            if (BillingContactNumber == "") { alert("Please enter your Billing Address Details"); $("#BillingContactNumber").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var BillingEmail = $("#BillingEmail").val();
            if (BillingEmail == "") { alert("Please enter your Billing Address Details"); $("#BillingEmail").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            if (!ValidateEmail(BillingEmail)) { alert("Invalid email address."); $("#BillingEmail").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }


            var OperationsName = $("#OperationsName").val();
            var OperationsDesignation = $("#OperationsDesignation").val();
            var OperationDirectNo = $("#OperationDirectNo").val();
            var OperationMobileNumber = $("#OperationMobileNumber").val();
            var OperationEmailID = $("#OperationEmailID").val();
            if (OperationsName == "" || OperationsDesignation == "" || OperationDirectNo == "" || OperationMobileNumber == "" || OperationEmailID == "") {
                alert("Please enter your Operations' Representative Details");
                $("#OperationsName").focus();
                $('#portsRegisterbtn').removeAttr("disabled"); return false;
            }
            if (!ValidateEmail(OperationEmailID)) { alert("Invalid email address."); $("#OperationEmailID").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var OperationEmailID2 = $("#OperationEmailID2").val();
            if (OperationEmailID2 != "") {
                if (!ValidateEmail(OperationEmailID2)) { alert("Invalid email address."); $("#OperationEmailID2").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            }

            var OperationEmailID3 = $("#OperationEmailID3").val();
            if (OperationEmailID3 != "") {
                if (!ValidateEmail(OperationEmailID3)) { alert("Invalid email address."); $("#OperationEmailID3").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            }
            var FinanceName = $("#FinanceName").val();
            var FinanceDesignation = $("#FinanceDesignation").val();
            var FinanceDirectNo = $("#FinanceDirectNo").val();
            var FinanceMobileNumber = $("#FinanceMobileNumber").val();
            var FinanceEmailID = $("#FinanceEmailID").val();
            if (FinanceName == "" || FinanceDesignation == "" || FinanceDirectNo == "" || FinanceMobileNumber == "" || FinanceEmailID == "") {
                alert("Please enter your Finance's Representative Details");
                $("#FinanceName").focus();
                $('#portsRegisterbtn').removeAttr("disabled"); return false;
            }
            if (!ValidateEmail(FinanceEmailID)) { alert("Invalid email address."); $("#FinanceEmailID").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var FinanceEmailID2 = $("#FinanceEmailID2").val();
            if (FinanceEmailID2 != "") {
                if (!ValidateEmail(FinanceEmailID2)) { alert("Invalid email address."); $("#FinanceEmailID3").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            }

            var FinanceEmailID3 = $("#FinanceEmailID3").val();
            if (FinanceEmailID3 != "") {
                if (!ValidateEmail(FinanceEmailID3)) { alert("Invalid email address."); $("#FinanceEmailID3").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            }

            var Name = $("#Name").val();
            if (Name == "") { alert("Please enter your Name"); $("#Name").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var Designation = $("#Designation").val();
            if (Designation == "") { alert("Please enter your Designation"); $("#Designation").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var CommunicationEmail1 = $("#CommunicationEmail1").val();
            if (CommunicationEmail1 == "") { alert("Please enter your Communication Email"); $("#CommunicationEmail1").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            if (!ValidateEmail(CommunicationEmail1)) { alert("Invalid email address."); $("#CommunicationEmail1").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }

            var CommunicationEmail2 = $("#CommunicationEmail2").val();
            if (CommunicationEmail2 == "") { alert("Please enter your Communication Email"); $("#CommunicationEmail2").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            if (CommunicationEmail2 != "" && CommunicationEmail2 != undefined) {
                if (!ValidateEmail(CommunicationEmail2)) { alert("Invalid email address."); $("#CommunicationEmail2").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            }

            var CommunicationEmail3 = $("#CommunicationEmail3").val();
            if (CommunicationEmail3 == "") { alert("Please enter your Communication Email"); $("#CommunicationEmail3").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            if (CommunicationEmail3 != "" && CommunicationEmail3 != undefined) {
                if (!ValidateEmail(CommunicationEmail3)) { alert("Invalid email address."); $("#CommunicationEmail3").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false; }
            }

            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            today = mm + '/' + dd + '/' + yyyy;
            $("#SubmitOnDate").val(today);

            $("#RegisteredOfficeAddress").val($("#RegisteredOfficeAddress").val().replace(',', '-'));
            $("#LocalOfficeAddress").val($("#LocalOfficeAddress").val().replace(',', '-'));
            $("#BillingAddress").val($("#BillingAddress").val().replace(',', '-'));
            var nc = $("#NameofCompanyT").val();
            nc = nc + '(' + $("#tempCompanyRegistrationDate").val() + ')';
            $("#NameofCompany").val(nc);

            if ($('#defaultCheck2').is(":not(:checked)")) {
                alert("Please accept the confirmation of details"); $("#defaultCheck2").focus(); $('#portsRegisterbtn').removeAttr("disabled"); return false;
            }


            var exampleRadioss = $("input[name='exampleRadios']:checked").val();


            //$("#reResponse").val(response);
            //for file upload
            var filedata = new FormData();

            $('#PortsfilesRecordSubmitted input[type="file"]').each(function () {
                var inputIds = $(this).attr("id");
                var nameIds = $(this).attr("name");
                var fileuploaddata = $("#" + inputIds).get(0);
                var files = fileuploaddata.files;
                for (var i = 0; i < files.length; i++) {
                    filedata.append(nameIds, files[i]);
                }
            });

            filedata.append("BillingAddress", $("#BillingAddress").val());
            filedata.append("BillingContactNumber", $('#BillingContactNumber').val());
            filedata.append("BillingCountry", $("#BillingCountry").val());
            filedata.append("BillingEmail", $("#BillingEmail").val());
            filedata.append("BillingState", $("#BillingState").val());
            filedata.append("CommunicationEmail1", $("#CommunicationEmail1").val());
            filedata.append("CommunicationEmail2", $("#CommunicationEmail2").val());
            filedata.append("CommunicationEmail3", $("#CommunicationEmail3").val());
            filedata.append("MICTRegisterCode", $("#MICT").val());
            filedata.append("CT2RegisterCode", $("#CT2RegisterCode").val());
            filedata.append("T2RegisterCode", $("#T2RegisterCode").val());
            filedata.append("CT3RegisterCode", $("#CT3RegisterCode").val());
            filedata.append("CT4RegisterCode", $("#CT4RegisterCode").val());
            filedata.append("Designation", $("#Designation").val());
            filedata.append("FinanceDesignation", $("#FinanceDesignation").val());
            filedata.append("FinanceDesignation2", $("#FinanceDesignation2").val());
            filedata.append("FinanceDesignation3", $("#FinanceDesignation3").val());
            filedata.append("FinanceDirectNo", $("#FinanceDirectNo").val());
            filedata.append("FinanceDirectNo2", $("#FinanceDirectNo2").val());
            filedata.append("FinanceDirectNo3", $("#FinanceDirectNo3").val());
            filedata.append("FinanceEmailID", $("#FinanceEmailID").val());
            filedata.append("FinanceEmailID2", $("#FinanceEmailID2").val());
            filedata.append("FinanceEmailID3", $("#FinanceEmailID3").val());
            filedata.append("FinanceMobileNumber", $("#FinanceMobileNumber").val());
            filedata.append("FinanceMobileNumber2", $("#FinanceMobileNumber2").val());
            filedata.append("FinanceMobileNumber3", $("#FinanceMobileNumber3").val());
            filedata.append("FinanceName", $("#FinanceName").val());
            filedata.append("FinanceName2", $("#FinanceName2").val());
            filedata.append("FinanceName3", $("#FinanceName3").val());
            filedata.append("GSTNumber", $("#GSTNumber").val());
            filedata.append("LocalOfficeAddress", $("#LocalOfficeAddress").val());
            filedata.append("LocalOfficeContactNumber", $("#LocalOfficeContactNumber").val());
            filedata.append("LocalOfficeCountry", $("#LocalOfficeCountry").val());
            filedata.append("LocalOfficeEmail", $("#LocalOfficeEmail").val());
            filedata.append("LocalOfficeState", $("#LocalOfficeState").val());
            filedata.append("Name", $("#Name").val());
            filedata.append("NameofCompany", $("#NameofCompanyT").val());
            filedata.append("NameofPrincipal", $("#NameofPrincipal").val());
            filedata.append("OperationDirectNo", $("#OperationDirectNo").val());
            filedata.append("OperationDirectNo2", $("#OperationDirectNo2").val());
            filedata.append("OperationDirectNo3", $("#OperationDirectNo3").val());
            filedata.append("OperationEmailID", $("#OperationEmailID").val());
            filedata.append("OperationEmailID2", $("#OperationEmailID2").val());
            filedata.append("OperationEmailID3", $("#OperationEmailID3").val());
            filedata.append("OperationMobileNumber", $("#OperationMobileNumber").val());
            filedata.append("OperationMobileNumber2", $("#OperationMobileNumber2").val());
            filedata.append("OperationMobileNumber3", $("#OperationMobileNumber3").val());
            filedata.append("OperationsDesignation", $("#OperationsDesignation").val());
            filedata.append("OperationsDesignation2", $("#OperationsDesignation2").val());
            filedata.append("OperationsDesignation3", $("#OperationsDesignation3").val());
            filedata.append("OperationsName", $("#OperationsName").val());
            filedata.append("OperationsName2", $("#OperationsName2").val());
            filedata.append("OperationsName3", $("#OperationsName3").val());
            filedata.append("PANNumber", $("#PANNumber").val());
            filedata.append("PortType", $("#port-select").val());
            filedata.append("RegisteredOfficeAddress", $("#RegisteredOfficeAddress").val());
            filedata.append("RegisteredOfficeContactNumber", $("#RegisteredOfficeContactNumber").val());
            filedata.append("RegisteredOfficeCountry", $("#RegisteredOfficeCountry").val());
            filedata.append("RegisteredOfficeEmail", $("#RegisteredOfficeEmail").val());
            filedata.append("RegisteredOfficeState", $("#RegisteredOfficeState").val());
            filedata.append("Rupees", $("#Rupees").val());
            filedata.append("RupeesInWords", $("#RupeesInWords").val());
            filedata.append("ShippingAgentDetails", $("#ShippingAgentDetails").val());
            filedata.append("CompanyRegistrationDate", $("#tempCompanyRegistrationDate").val());
            filedata.append("T2RegisterCode", $("#T2RegisterCode").val());
            filedata.append("TANNumber", $("#TANNumber").val());
            filedata.append("TypeofCustomers", $("#customer-type").val());
            filedata.append("SCMTRCode", $("#SCMTR-select").val());
            filedata.append("SCMTRID", $("#SCMTRID").val());
            filedata.append("reResponse", token);




            //create json object

            //ajax calling to insert  custom data function
            $.ajax({
                type: "POST",
                data: filedata,
                url: "/api/ports/PortAcountRegisteration",
                contentType: false,
                processData: false,
                headers: { "__RequestVerificationToken": $('[name="__RequestVerificationToken"]').val() },
                success: function (data) {
                    if (data.status == "1") {
                        alert(data.ErrorMessage);
                    }
                    else if (data.IsErrorResonse == false) {
                        $("#" + data.FieldName).html(data.ErrorMessage);
                        $("#" + data.FieldName).css('color', 'red');
                        $("#" + data.FieldName).show();
                    }
                    else if (data.IsInfoSave != null && data.IsInfoSave == true) {
                        window.location.href = "/thankyou";
                    }
                }
            });
            return false;
            return;
        });
    });
}

$("form#contact_form :input").on("change", function () {
    var inputIds = $(this).attr("id");
    if ($("#" + inputIds).val() != null || $("#" + inputIds).val() != undefined || $("#" + inputIds).val() != empty) {
        $("#" + inputIds).next(".field-validation-error").html("");
        $("#" + inputIds).next(".field-validation-error").hide();
    }
});

$("form#contact_form :input").each(function () {
    var inputIds = $(this).attr("id");
    var input = "#" + inputIds;
    $(input).attr("autocomplete", "off");
});