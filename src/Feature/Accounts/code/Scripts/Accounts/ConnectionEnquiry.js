$(document).ready(function () {
    $("#a_reload").on('click', function () {
        window.location.href = window.location.href;
    });
	fnBindAreaOfCity($("#city").val(), '#Area');
});
function fnBindAreaOfCity(selectedval, targetDrp) {
    if (selectedval > 0) {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">select</option>');
        $.getJSON('/api/AdaniGas/GetAreaByCity', { cityCode: selectedval }, function (response) {
            $.each(response, function (index, item) {
                $('' + targetDrp + '').append('<option value="' + item.Value + '">' + item.Text + '</option>');
            });
        });
    }
    else {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">select</option>');
    }
};
//NewConnection Module by Ketan Start
function searchAreaOfCity(selectedval, targetDrp) {
    if (selectedval > 0) {        
        $("#NewConApartmentComplex").prop('required', true);
        $("#NewConApartmentComplex").prop('disabled', false);
        $("#NewConHouseType").val("");
        $(".noBunglow").removeClass("d-none");
        $('#NewConApartmentComplex option').remove();
        $('#NewConApartmentComplex').append('<option value="">--Please Select--</option>');
        $(".OtherSociety").addClass("d-none");
        $("#OtherApartmentComplex").val("").prop('required', false);
        $("#NewConAddress1").val("").prop('readonly', false);
        $("#NewConAddress2").val("").prop('readonly', false);
        $("#NewConPincode").val("").prop('readonly', false);
        var exAreas = ["Bavla", "Bhadaj", "Adalaj", "Changodar", "Kerala", "Khodiyar", "Moraiya", "Nana Chiloda", "Sanand", "Adalaj", "Bavla", "Bhadaj (Partly)", "Changodar", "Chharodi (Sanand)", "Kerala", "Kotarpur (Partly)", "Moraiya", "Nana Chilora", "Sanand", "Adalaj", "Khodiyar", "Kali"];
        var partnerTypeCode = $("#NewConPartnertype").val();
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
        $.getJSON('/api/AdaniGas/GetAreaOfCity', { PartnerTypeCode: partnerTypeCode, cityCode: selectedval }, function (response) {
            $.each(response, function (index, item) {
                if (!exAreas.includes(item.Text)) {
                    $('' + targetDrp + '').append('<option value="' + item.Value + '">' + item.Text + '</option>');
                }
            });
        });
    }
    else {
        $("#NewConApartmentComplex").prop('required', true);
        $("#NewConApartmentComplex").prop('disabled', false);
        $("#NewConHouseType").val("");
        $(".noBunglow").removeClass("d-none");
        $('#NewConApartmentComplex option').remove();
        $('#NewConApartmentComplex').append('<option value="">--Please Select--</option>');
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
        $(".OtherSociety").addClass("d-none");
        $("#OtherApartmentComplex").val("").prop('required', false);
        $("#NewConAddress1").val("").prop('readonly', false);
        $("#NewConAddress2").val("").prop('readonly', false);
        $("#NewConPincode").val("").prop('readonly', false);
    }
};

$("#NewConReferenceSource").select2();
$("#NewConCity").select2();
$("#NewConArea").select2();
$("#NewConApartmentComplex").select2();
$("#NewConCurrentFuelUse").select2();
$("#NewConTypeOfIndustry").select2();
$("#NewConCurrentFuelUse").select2();
$("#NewConApplication").select2();
$("#NewConTypeOfCustomer").select2();

function searchSocietyByArea(selectedval, targetDrp) {
    if (selectedval > 0) {        
        $(".OtherSociety").addClass("d-none");
        $("#OtherApartmentComplex").prop('required', false);
        $("#NewConAddress1").val("").prop('readonly', false);
        $("#NewConAddress2").val("").prop('readonly', false);
        $("#NewConPincode").val("").prop('readonly', false);
        var cityCode = $("#NewConCity").val();
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
        $.getJSON('/api/AdaniGas/GetSocietyByArea', { AreaCode: selectedval, cityCode: cityCode }, function (response) {

            $.each(response, function (index, item) {
                if (item.Msg_Flag !== "F") {
                    $('' + targetDrp + '').append('<option value="' + item.SocietyName + '">' + item.SocietyName + '</option>');
                }
            });

        });
    }
    else {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
        $(".OtherSociety").addClass("d-none");
        $("#OtherApartmentComplex").prop('required', false);
        $("#NewConAddress1").val("").prop('readonly', false);
        $("#NewConAddress2").val("").prop('readonly', false);
        $("#NewConPincode").val("").prop('readonly', false);
    }
};

function updateSocietyDetails(selectedval, targetDrp) {
    if (selectedval !== "" && selectedval !== "Other") {
        $(".OtherSociety").addClass("d-none");
        $("#OtherApartmentComplex").val("").prop('required', false);
        var cityCode = $("#NewConCity").val();
        var areaCode = $("#NewConArea").val();
        $("#NewConAddress1").val("");
        $("#NewConAddress2").val("");
        $("#NewConPincode").val("");
        $.getJSON('/api/AdaniGas/GetSocietyByArea', { AreaCode: areaCode, cityCode: cityCode }, function (response) {
            $.each(response, function (index, item) {
                if (item.SocietyName === selectedval) {
                    $("#NewConAddress1").val(item.AddressLine1).prop('readonly', true);
                    $("#NewConAddress2").val(item.AddressLine2).prop('readonly', true);
                    $("#NewConPincode").val(item.PostalCode).prop('readonly', true);
                }
            });
        });
    }
    else if (selectedval === "Other") {
        $(".OtherSociety").removeClass("d-none");
        $("#OtherApartmentComplex").val("").prop('required', true);
        $("#OtherApartmentComplex").focus();
        $("#NewConAddress1").val("").prop('readonly', false);
        $("#NewConAddress2").val("").prop('readonly', false);
        $("#NewConPincode").val("").prop('readonly', false);
    }
    else {
        $(".OtherSociety").addClass("d-none");
        $("#OtherApartmentComplex").val("").prop('required', false);
        $("#NewConAddress1").val("").prop('readonly', false);
        $("#NewConAddress2").val("").prop('readonly', false);
        $("#NewConPincode").val("").prop('readonly', false);
    }
};

function CustTypeChange(selectedval) {
    if (selectedval.toLowerCase() === "other") {
        $(".OtherTOC").removeClass("d-none");
        $("#OtherTypeOfCustomer").val("").prop('required', true);
        $("#OtherTypeOfCustomer").focus();
    }
    else {
        $(".OtherTOC").addClass("d-none");
        $("#OtherTypeOfCustomer").val("").prop('required', false);
    }
};
function AppChange(selectedval) {
    if (selectedval.toLowerCase() === "other") {
        $(".OtherApp").removeClass("d-none");
        $("#OtherApplication").val("").prop('required', true);
        $("#OtherApplication").focus();
    }
    else {
        $(".OtherApp").addClass("d-none");
        $("#OtherApplication").val("").prop('required', false);
    }
};
function FuelTypeChange(selectedval) {
    if (selectedval.toLowerCase() === "other") {
        $(".OtherFuel").removeClass("d-none");
        $("#OtherCurrentFuelUse").val("").prop('required', true);
        $("#OtherCurrentFuelUse").focus();
    }
    else {
        $(".OtherFuel").addClass("d-none");
        $("#OtherCurrentFuelUse").val("").prop('required', false);
    }
};
function TOIChange(selectedval) {
    if (selectedval.toLowerCase() === "other") {
        $(".OtherTOI").removeClass("d-none");
        $("#OtherTypeOfIndustry").val("").prop('required', true);
        $("#OtherTypeOfIndustry").focus();
    }
    else {
        $(".OtherTOI").addClass("d-none");
        $("#OtherTypeOfIndustry").val("").prop('required', false);
    }
};

$(document).ready(function () {
    if ($("#OtherApartmentComplex").val() !== "") {
        $(".OtherSociety").removeClass("d-none");
        $("#OtherApartmentComplex").prop('required', true);
        $("#NewConAddress1").prop('readonly', false);
        $("#NewConAddress2").prop('readonly', false);
        $("#NewConPincode").prop('readonly', false);
    }
    if ($("#ApartmentComplex").val() !== "" && $("#ApartmentComplex").val() !== "Other") {
        $(".OtherSociety").addClass("d-none");
        $("#OtherApartmentComplex").prop('required', false);
        $("#NewConAddress1").prop('readonly', true);
        $("#NewConAddress2").prop('readonly', true);
        $("#NewConPincode").prop('readonly', true);
    }
    if ($("#NewConTypeOfCustomer").val() !== "" && $("#NewConTypeOfCustomer").val() === "Other") {
        $(".OtherTOC").removeClass("d-none");
        $("#OtherTypeOfCustomer").prop('required', true);
    }
    if ($("#NewConApplication").val() !== "" && $("#NewConApplication").val() === "Other") {
        $(".OtherApp").removeClass("d-none");
        $("#OtherApplication").val("").prop('required', true);
    }
    if ($("#NewConCurrentFuelUse").val() !== "" && $("#NewConCurrentFuelUse").val() === "Other") {
        $(".OtherFuel").removeClass("d-none");
        $("#OtherCurrentFuelUse").val("").prop('required', true);
    }
    if ($("#NewConTypeOfIndustry").val() !== "" && $("#NewConTypeOfIndustry").val() === "Other") {
        $(".OtherTOI").removeClass("d-none");
        $("#OtherTypeOfIndustry").val("").prop('required', true);
    }
});
//NewConnection Module by Ketan End
function fnBindNetworkList(selectedval, patnertype) {

    $.getJSON('/api/AdaniGas/pngNetworklist', { group: selectedval, patnertype: patnertype }, function (response) {
        var tempdata = "";
		var cnt = 0;
		$.each(response, function (index, item) {
			
			if(item.length > 0)
			{
				tempdata = item
			}
			else
			{
				tempdata = "No Record Found."
			}
            cnt = cnt + 1;
        });
		$('#divnetworkdetails').html(tempdata);
		$("#divnetworkdetails div").removeClass("d-block");
    });

};

$("#myInput").on("keyup", function() {
    
    var value = $(this).val().toLowerCase();
    $("#divnetworkdetails *").filter(function() {
      $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
  });
  