$(document).ready(function () {
    $("#PartnerType").change(function () {
        var selectedVal = $("#PartnerType option:selected").val();
        if (selectedVal == "9004") {
            $("#otherField").hide();
            $("#domesticField").show();
            $("#FirstName").val('');
            $("#LastName").val('');
            $("#OrganizationName").val('OrganizationName');
        }
        else {
            $("#domesticField").hide();
            $("#otherField").show();
            $("#OrganizationName").val('');
            $("#FirstName").val('FirstName');
            $("#LastName").val('LastName');
        }
    });
});

$(function () {
    $("#PartnerType").change();
});