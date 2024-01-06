$(document).ready(function () {
	if($("#CustomerName").val() == "cng")
	{
		$("#dvNewInformation").show();
        ResetNewDetail();
        SetDummyBaseDetail();
        $("#dvExistingInformation").hide();
	}
	else
	{
    ResetBaseDetail();
    $("#dvNewInformation").hide();
    SetDummyNewDetail();
	}
    $("#CustomerID").blur(function () {
        jQuery.ajax(
            {
                url: "/api/AdaniGas/GetCustomerInfo",
                method: "POST",
                async: false,
                data: {
                    customerId: $(this).val()
                },
                success: function (data) {
                    if (data != null && data != undefined) {
                        $("#CustomerName").val(data.customername);
						$("#CustomerName").attr("readonly","readonly");
                    } else {
                        $("#CustomerName").val('')
                    }
                }
            });
    });
    $(".forgotUser").click(function () {
        $("#dvNewInformation").show();
        ResetNewDetail();
        SetDummyBaseDetail();
        $("#dvExistingInformation").hide();
    });
$("#a_reload").on('click', function () {
        window.location.href = window.location.href;
    })
    function ResetBaseDetail() {
        $("#CustomerName").val('');
        $("#CustomerID").val('');
    }
    function ResetNewDetail() {
        $("#NewCustomerName").val('');
        $("#Address").val('');
        $("#city").val('');
        $("#pincode").val('');
        $("#contact").val('');
    }

    function SetDummyBaseDetail() {
        $("#CustomerName").val('CustomerName');
        $("#CustomerID").val('0000000000');
    }

    function SetDummyNewDetail() {
        $("#NewCustomerName").val('NewCustomerName');
        $("#Address").val('address');
        $("#city").val('city');
        $("#pincode").val('pincode');
        $("#contact").val('9999999999');
    }

});