$(document).ready(function () {
    $("#Amount").attr("disabled", "disabled");
    $("#selectedAfterSalesServiceRequest").change(function () {
        $('#AdaniGasaftersalesModal').modal('hide');
        $("#spnpaymentinfo").text('');
        $("#paymentmethod").show();
        $("#paytmTable").show();
        $(".alert-info").hide();
        $("#btnsubmit").removeAttr("disabled");
		$("#quantityMSG").text('');
		$("#Quantity").val("01");
        $.ajax(
            {
                type: "get",
                dataType: 'JSON',
                url: '/api/AdaniGas/AfterSalesServiceChangeDDR',
                data: {
                    serviceValue: $("#selectedAfterSalesServiceRequest option:selected").val(),
                    Quantity: $("#Quantity").val(),
                },
                success:
                    function (data) {
                        if (data != null && data != undefined) {
                            if (data.Quantity_Min != "00" && data.Quantity_Max != "00") {
                                if (data.Msgflag === "F") {
                                    $("#txtmessage").text(data.Message);
                                    $("#Quantity").attr("readonly", "readonly");
                                    $("#minQuantity").text(data.Quantity_Min);
                                    $("#maxQuantity").text(data.Quantity_Max);
                                    $("#TempQuantity_Min").val(data.Quantity_Min);
                                    $("#TempQuantity_Max").val(data.Quantity_Max);
                                    $("#Amount").val(data.AmountToBePaid);
                                    $("#TempAmount").val(data.AmountToBePaid);
                                    $("#btnsubmit").attr("disabled", "disabled");
                                }
                                else if (data.Quantity_Min == "01" && data.Quantity_Max == "01") {
                                    
                                    if (data.extraamount != undefined && data.extraamount != "") {
                                        var txt1 = "₹ " + data.AmountToBePaid + " (inclusive of Tax ₹ " + data.Tax + "). \n Extra Pipe charges of ₹ " + data.extraamount + " per meter(Incl.tax) will be charged beyond " + data.MaxLengthgas + " meter length. " + "In case of refund, visit charges of Rs 250 + tax will be deducted from amount paid.";
                                        $("#spnpaymentinfo").text(txt1);
                                        $('#AdaniGasaftersalesModal').modal('show');
                                    }
                                    $("#txtmessage").text('');
                                    $("#Quantity").attr("readonly", "readonly");
                                    $("#Quantity").val("01");
                                    $("#minQuantity").text(data.Quantity_Min);
                                    $("#maxQuantity").text(data.Quantity_Max);
                                    $("#TempQuantity_Min").val(data.Quantity_Min);
                                    $("#TempQuantity_Max").val(data.Quantity_Max);
                                    $("#Amount").val(data.AmountToBePaid);
                                    $("#TempAmount").val(data.AmountToBePaid);


                                } else {
									if (data.Msgflag == "S" && data.extraamount != undefined && data.extraamount != "") {
                                        var txt1 = "₹ " + data.AmountToBePaid + " (inclusive of Tax ₹ " + data.Tax + "). \n Extra Pipe charges of ₹ " + data.extraamount + " per meter(Incl.tax) will be charged beyond " + data.MaxLengthgas + " meter length. " + "In case of refund, visit charges of Rs 250 + tax will be deducted from amount paid.";
                                        $("#spnpaymentinfo").text(txt1);
                                        $('#AdaniGasaftersalesModal').modal('show');
                                    }
                                    $("#txtmessage").text('');
                                    $("#Quantity").val("01");
                                    $("#Quantity").removeAttr("readonly");
                                    $("#minQuantity").text(data.Quantity_Min);
                                    $("#maxQuantity").text(data.Quantity_Max);
                                    $("#TempQuantity_Min").val(data.Quantity_Min);
                                    $("#TempQuantity_Max").val(data.Quantity_Max);
                                    $("#Amount").val(data.AmountToBePaid);
                                    $("#TempAmount").val(data.AmountToBePaid);
                                }
                            }
                            else {
                                if (data.Msgflag === "F") {
                                    $("#txtmessage").text(data.Message);
                                    $("#Quantity").attr("readonly", "readonly");
                                    $("#minQuantity").text(data.Quantity_Min);
                                    $("#maxQuantity").text(data.Quantity_Max);
                                    $("#TempQuantity_Min").val(data.Quantity_Min);
                                    $("#TempQuantity_Max").val(data.Quantity_Max);
                                    $("#Amount").val(data.AmountToBePaid);
                                    $("#TempAmount").val(data.AmountToBePaid);
                                    $("#paymentmethod").hide();
                                    $("#paytmTable").hide();
                                }
                                else {
                                    $("#txtmessage").text('');
                                    $("#Quantity").val("00");
                                    $("#Quantity").attr("readonly", "readonly");
                                    $("#minQuantity").text(data.Quantity_Min);
                                    $("#maxQuantity").text(data.Quantity_Max);
                                    $("#TempQuantity_Min").val(data.Quantity_Min);
                                    $("#TempQuantity_Max").val(data.Quantity_Max);
                                    $("#Amount").val(data.AmountToBePaid);
                                    $("#TempAmount").val(data.AmountToBePaid);
                                    
                                    $("#paymentmethod").hide();
                                    $("#paytmTable").hide();
                                }
                            }

                        }
                    }
            });
    });

    $("#Quantity").change(function () {
        var qnt = $("#Quantity").val();
        var min = $("#minQuantity").text();
        var max = $("#maxQuantity").text();
        $('#AdaniGasaftersalesModal').modal('hide');
        var intQnt = parseInt(qnt);
        var intMin = parseInt(min);
        var intMax = parseInt(max);

		if($("#selectedAfterSalesServiceRequest option:selected").val() == undefined || $("#selectedAfterSalesServiceRequest option:selected").val() == "")
		{
			$("#quantityMSG").text("Please select request.");
			return false;
		}

        if (intQnt >= intMin && intQnt <= intMax) {
            $("#quantityMSG").text('');
			$("#btnsubmit").removeAttr("disabled");
        }
        else {
            $("#Quantity").focus();
            $("#quantityMSG").text("*Quantity should be in between min and max");
			$("#btnsubmit").attr("disabled", "disabled");
			return false;
        }

        $.ajax(
            {
                type: "get",
                dataType: 'JSON',
                url: '/api/AdaniGas/AfterSalesServiceOnTextBoxChange',
                data: {
                    serviceValue: $("#selectedAfterSalesServiceRequest option:selected").val(),
                    Quantity: $("#Quantity").val(),
                },
                success:
                    function (data) {
                        if (data != null && data != undefined) {
                            if (data.Msgflag == "S" && data.extraamount != undefined && data.extraamount != "") {
                                var txt1 = "₹ " + data.AmountToBePaid + " (inclusive of Tax ₹ " + data.Tax + "). \n Extra Pipe charges of ₹ " + data.extraamount + " per meter(Incl.tax) will be charged beyond " + data.MaxLengthgas + " meter length. " + "In case of refund, visit charges of Rs 250 + tax will be deducted from amount paid.";
                                $("#spnpaymentinfo").text(txt1);
                                $('#AdaniGasaftersalesModal').modal('show');
                            }
                            $("#Amount").val(data.AmountToBePaid);
                            $("#TempAmount").val(data.AmountToBePaid);
                        }
                    }
            });
    });

});