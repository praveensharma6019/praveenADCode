$(document).ready(function () {
    var message = $("#submit_message").val();
    if (message !== undefined && message !== null && message !== "") {
        $('#submit_message_modal').modal('show');
        $("#submit_message").val("");
    }
    $('#AjaxLoader').hide();
    //$("#NewConCity").select2();
    //$("#NewConArea").select2();
    //$("#NewConApartmentComplex").select2();
    //$("#NewConHouseNumber").select2();

    if (localStorage['page'] == document.URL && localStorage['scrollTop'] != null && localStorage['scrollTop'] != "") {
        $(document).scrollTop(localStorage['scrollTop']);
    }
    else {
        $(document).scrollTop(0);
        localStorage['page'] = "";
        localStorage['scrollTop'] = "";
    }
});

function showinformationPopup(id) {
    var message = "";
    if (id == "Builder_Case")
        message = "When a customer purchases property directly from the builder, i.e., 1st sale of property and gas connection is already installed within the premises.";
    else if (id == "Property_ReSale")
        message = "Re-sale of property other than direct purchase from a builder.";
    else if (id == "Demise")
        message = "In case of the demise of the property owner, the legal heir can apply for a name change with the required documents.";
    $(".message_modal_message").html(message);
    $("#message_modal").modal("show");
}

//$("#proceedforpayment").click(function () {
//    var name = $("#FirstName").val().toUpperCase() + " " + $("#MiddleName").val().toUpperCase() + " " + $("#LastName").val().toUpperCase();
//    $('.confirmation_modal_message').html("Your name will appear on your bill as below: <br><br><strong>" + name + "</strong><br><br> Are you sure you want to submit the application?");
//    $('#confirmation_modal').modal("show");
//    event.preventDefault();
//    return false;
//});


$('.confirmation_modalbtnYesNO').click(function (e) {
    if (this.value === '1') {
        YesNO = '1';
        $('#confirmation_modal').modal("hide");
        //$("#proceedforpayment").submit();
        //$('#name-transfer-step1').trigger('submit', ['proceedforpayment', 'true']);
        $("#name-transfer-step1").submit(function (eventObj) {
            $(this).append('<input type="hidden" name="proceedforpayment" value="value" /> ');
            return true;
        });
        $("#name-transfer-step1").submit();
    }
    else {
        $('#confirmation_modal').modal("hide");
    }
    e.preventDefault();
});

$('.confirmation_Additional_modalbtnYesNO').click(function (e) {
    if (this.value === '1') {
        YesNO = '1';
        $('#confirmation_Additional_modal').modal("hide");
        $("#proceedforAdditionalpayment123").click();
    }
    else {
        $('#confirmation_Additional_modal').modal("hide");
    }
    e.preventDefault();
});



function searchAreaOfCity(selectedval, targetDrp) {
    $("#SocietyAddress1").html("");
    $("#SocietyAddress2").html("");
    $('#NewConApartmentComplex option').remove();
    $('#NewConApartmentComplex').append('<option value="">--Please Select--</option>');
    $('#NewConHouseNumber option').remove();
    $('#NewConHouseNumber').append('<option value="">--Please Select--</option>');
    if (selectedval > 0) {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
        $.getJSON('/api/AdaniGas/GetAreaOfCity', { PartnerTypeCode: "9001", cityCode: selectedval }, function (response) {
            $.each(response, function (index, item) {
                $('' + targetDrp + '').append('<option value="' + item.Value + '">' + item.Text + '</option>');
            });
        });

    }
    else {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
    }
    //$(".select2.select2-container.select2-container--default.select2-container--below").show();
}


function searchSocietyByArea(selectedval, targetDrp) {
    $('#NewConHouseNumber option').remove();
    $('#NewConHouseNumber').append('<option value="">--Please Select--</option>');
    $("#SocietyAddress1").html("");
    $("#SocietyAddress2").html("");
    if (selectedval > 0) {
        var cityCode = $("#NewConCity").val();
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
        $.getJSON('/api/AdaniGas/GetSocietyByArea', { AreaCode: selectedval, cityCode: cityCode }, function (response) {
            $.each(response, function (index, item) {
                if (item.Msg_Flag !== "F") {
                    $('' + targetDrp + '').append('<option value="' + item.SocietyCode + '">' + item.SocietyName + '</option>');
                }
            });

        });

    }
    else {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
    }
    //$(".select2.select2-container.select2-container--default.select2-container--below").show();
}

function searchHouseBySociety(selectedval, targetDrp) {
    if (selectedval > 0) {
        var cityCode = $("#NewConCity").val();
        var areaCode = $("#NewConArea").val();
        $("#SocietyAddress1").html("");
        $("#SocietyAddress2").html("");
        $.getJSON('/api/AdaniGas/GetSocietyByArea', { AreaCode: areaCode, cityCode: cityCode }, function (response) {
            $.each(response, function (index, item) {
                if (item.SocietyCode === selectedval) {
                    $("#SocietyAddress1").html(item.AddressLine1);
                    $("#SocietyAddress2").html(item.AddressLine2);
                }
            });
        });
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
        $.getJSON('/api/AdaniGas/GetHouseNumberBySociety', { SocietyCode: selectedval }, function (response) {
            $.each(response, function (index, item) {
                if (item.Msg_Flag !== "F") {
                    $('' + targetDrp + '').append('<option value="' + item.HouseNumber + '">' + item.HouseNumber + '</option>');
                }
            });

        });
    }
    else {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">--Please Select--</option>');
    }
    //$(".select2.select2-container.select2-container--default.select2-container--below").show();
}

function searchHouseByNumber() {

    $(".select2.select2-container.select2-container--default.select2-container--below").show();
}


$('#name-transfer-step1').submit(function () {
    $('#AjaxLoader').show();
});

//$('#name-transfer-download').submit(function () {
//    $('#AjaxLoader').show();
//});

$('.rbProceedWith').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});

$('.rbProceedWiths').change(function (event) {
    GetScrollPosition();
    var form = $(event.target).parents('form');
    form.submit();
});


$(document).ready(function () {

    $("#Continuestep2").click(function (e) {
        //$('.completed nav-link active').show();
        $("#Details-tab").click();
        e.preventDefault();
    });

    $("#backid1").click(function (e) {
        $("#Connection-tab").click();
        e.preventDefault();
    });

    $("#Details-tab").click(function (e) {
       
        var Property_ReSale = $("#Property_ReSale").is(":checked");
        var Demise = $("#Demise").is(":checked");
        var gas = $('input[name="GasSupply"]').is(":checked");


        var Registered_Housing_Society = $("#Registered_Housing_Society").is(":checked");
        var Unregistered_Housing_Society = $("#Unregistered_Housing_Society").is(":checked");

        if ((Property_ReSale === true && (Registered_Housing_Society === false || Unregistered_Housing_Society === false)) ) {
            $("#SocietyTypeCheckError").show();
        }
        else {
            $("#SocietyTypeCheckError").hide();
        }

        if (Demise === true && (Registered_Housing_Society === false && Unregistered_Housing_Society === false)) {
            $("#SocietyTypeCheckError").show();
        }
        else {
            $("#SocietyTypeCheckError").hide();
        }

        if ($("#Builder_Case").is(":checked") == true) {
            if (gas == false) {
                $("#GasSupplyCheckError").show();
            }
            else {
                $("#GasSupplyCheckError").hide();
            }
        }
    });

    $("#Documents-tab").click(function (e) {
        if ($("#FirstName").val() != '' && $("#LastName").val() != '' && ($("#Builder_Case").is(":checked") == true || $("#Property_ReSale").is(":checked") == true || $("#Demise").is(":checked") == true)) {

            var val = $("input[name=ApplicationType]:checked").val();

            //$("#Documents-tab").click();

            if ($("#Builder_Case").is(":checked") == true) {
                $(".buildercase").show();
                $(".demise").hide();
                $(".propertyresale").hide();
                $(".UnregisteredHousing").hide();
                $(".RegisteredHousing").hide();
                $(".demiseUnregisteredHousing").hide();
            }

            if ($("#Property_ReSale").is(":checked") == true) {
                if ($("#Registered_Housing_Society").is(":checked") == true || $("#Unregistered_Housing_Society").is(":checked") == true) {

                    if ($("#Registered_Housing_Society").is(":checked") == true) {
                        $(".RegisteredHousing").show();
                        $(".UnregisteredHousing").hide();
                        $(".demise").hide();
                        $(".buildercase").hide();
                        $(".demiseUnregisteredHousing").hide();
                    }
                    if ($("#Unregistered_Housing_Society").is(":checked") == true) {
                        $(".UnregisteredHousing").show();
                        $(".demise").hide();
                        $(".buildercase").hide();
                        $(".RegisteredHousing").hide();
                        $(".demiseUnregisteredHousing").hide();
                    }
                }
            }

            if ($("#Demise").is(":checked") == true) {
                if ($("#Registered_Housing_Society").is(":checked") == true) {
                    $(".demise").show();
                    $(".buildercase").hide();
                    $(".propertyresale").hide();
                    $(".UnregisteredHousing").hide();
                    $(".RegisteredHousing").hide();
                    $(".demiseUnregisteredHousing").hide();
                }
                if ($("#Unregistered_Housing_Society").is(":checked") == true) {
                    $(".demiseUnregisteredHousing").show();
                    $(".demise").hide();
                    $(".buildercase").hide();
                    $(".propertyresale").hide();
                    $(".UnregisteredHousing").hide();
                    $(".RegisteredHousing").hide();
                }
            }
            e.preventDefault();
        } else { e.preventDefault(); return false; }
    });

    $("#continuestep3").click(function (e) {
        var AlphaPattern = /^[a-z\d\-_\s]+$/i;
        var Builder_Case = $("#Builder_Case").is(":checked");
        var Property_ReSale = $("#Property_ReSale").is(":checked");
        var Demise = $("#Demise").is(":checked");
        var gas = $('input[name="GasSupply"]').is(":checked");

        if (Builder_Case === false && Property_ReSale === false && Demise === false) {
            $("#ApplicationTypeCheckError").show();
        }
        else {
            $("#ApplicationTypeCheckError").hide();
        }

        var Registered_Housing_Society = $("#Registered_Housing_Society").is(":checked");
        var Unregistered_Housing_Society = $("#Unregistered_Housing_Society").is(":checked");

        if ((Property_ReSale === true && (Registered_Housing_Society === false || Unregistered_Housing_Society === false)) || (Demise === true && (Registered_Housing_Society === false || Unregistered_Housing_Society === false))) {
            $("#SocietyTypeCheckError").show();
        }
        else {
            $("#SocietyTypeCheckError").hide();
        }

        //if (Demise === true && (Registered_Housing_Society === false || Unregistered_Housing_Society === false)) {
        //    $("#SocietyTypeCheckError").show();
        //}
        //else {
        //    $("#SocietyTypeCheckError").hide();
        //}

        if ($("#Builder_Case").is(":checked") == true) {
            if (gas == false) {
                $("#GasSupplyCheckError").show();
            }
            else {
                $("#GasSupplyCheckError").hide();
            }
        }

        var FirstName = $("#FirstName").val();
        var isValidF = AlphaPattern.test(FirstName);
        var FirstNameError = $("#FirstNameCheckError").html();
        if (FirstName === '') {
            $("#FirstNameCheckError").show();
        }
        else {
            if (!isValidF) {
                $("#FirstNameCheckError").html("only alphanumeric, dash , underscore and space are allowed.");
                $("#FirstNameCheckError").show();
                return false;
            } else {
                $("#FirstNameCheckError").html(FirstNameError);
                $("#FirstNameCheckError").hide();
            }
            //$("#FirstNameCheckError").hide();
        }

        var MiddleName = $("#MiddleName").val();
        var isValidM = AlphaPattern.test(MiddleName);

        if (MiddleName.length > 0) {
            if (!isValidM) {
                $("#MiddleNameCheckError").html("only alphanumeric, dash , underscore and space are allowed.");
                $("#MiddleNameCheckError").show();
                return false;
            } else {

                $("#MiddleNameCheckError").hide();
            }
        } else {
            $("#MiddleNameCheckError").hide();
        }

        var Comment = $("#Comment").val();
        var isValidC = AlphaPattern.test(Comment);

        if (Comment.length > 0) {
            if (!isValidC) {
                $("#CommentCheckError").html("only alphanumeric, dash , underscore and space are allowed.");
                $("#CommentCheckError").show();
                return false;
            } else {

                $("#CommentCheckError").hide();
            }
        } else {
            $("#CommentCheckError").hide();
        }
        //$("#LastNameCheckError").hide();



        var LastName = $("#LastName").val();
        var isValidL = AlphaPattern.test(LastName);
        var LastNameError = $("#LastNameCheckError").html();
        if (LastName === '') {
            $("#LastNameCheckError").show();
        }
        else {
            if (!isValidL) {
                $("#LastNameCheckError").html("only alphanumeric, dash , underscore and space are allowed.");
                $("#LastNameCheckError").show();
                return false;
            } else {
                $("#LastNameCheckError").html(LastNameError);
                $("#LastNameCheckError").hide();
            }
            //$("#LastNameCheckError").hide();
        }

        //if ($("#emailValidator").text() != "" || $("#emailValidator").text() != null) {
        //    return false;
        //}



        var emailaddressVal = $("#CustomerEmailId").val();

        if (emailaddressVal !== null || emailaddressVal != undefined) {
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;

            if (!emailReg.test(emailaddressVal)) {
                $("#CustomerEmailId").removeAttr("style='display:none'");
                hasError = true;
                return false;
            }

        }


        if ($("#FirstName").val() != '' && $("#LastName").val() != '' && (($("#Builder_Case").is(":checked") == true && $('input[name="GasSupply"]').is(":checked") == true) || ($("#Demise").is(":checked") == true && ($("#Registered_Housing_Society").is(":checked") == true || $("#Unregistered_Housing_Society").is(":checked") == true))|| ($("#Property_ReSale").is(":checked") == true && ($("#Registered_Housing_Society").is(":checked") == true || $("#Unregistered_Housing_Society").is(":checked") == true)))) {

            var val = $("input[name=ApplicationType]:checked").val();

            $("#Documents-tab").click();

            if ($("#Builder_Case").is(":checked") == true) {
                $(".buildercase").show();
                $(".demise").hide();
                $(".propertyresale").hide();
                $(".UnregisteredHousing").hide();
                $(".RegisteredHousing").hide();
                $(".demiseUnregisteredHousing").hide();
            }

            if ($("#Property_ReSale").is(":checked") == true) {
                if ($("#Registered_Housing_Society").is(":checked") == true || $("#Unregistered_Housing_Society").is(":checked") == true) {

                    if ($("#Registered_Housing_Society").is(":checked") == true) {
                        $(".RegisteredHousing").show();
                        $(".UnregisteredHousing").hide();
                        $(".demise").hide();
                        $(".buildercase").hide();
                        $(".demiseUnregisteredHousing").hide();
                    }
                    if ($("#Unregistered_Housing_Society").is(":checked") == true) {
                        $(".UnregisteredHousing").show();
                        $(".demise").hide();
                        $(".buildercase").hide();
                        $(".RegisteredHousing").hide();
                        $(".demiseUnregisteredHousing").hide();
                    }
                }
            }

            if ($("#Demise").is(":checked") == true) {
                if ($("#Registered_Housing_Society").is(":checked") == true) {
                    $(".demise").show();
                    $(".buildercase").hide();
                    $(".propertyresale").hide();
                    $(".UnregisteredHousing").hide();
                    $(".RegisteredHousing").hide();
                    $(".demiseUnregisteredHousing").hide();
                }
                if ($("#Unregistered_Housing_Society").is(":checked") == true) {
                    $(".demiseUnregisteredHousing").show();
                    $(".demise").hide();
                    $(".buildercase").hide();
                    $(".propertyresale").hide();
                    $(".UnregisteredHousing").hide();
                    $(".RegisteredHousing").hide();
                }
            }
        }
        e.preventDefault();

    });

    $('input[type=radio][name=ApplicationType]').change(function () {
        if ($("#Property_ReSale").is(":checked") == true) {
            $("#housing-society").show();
            $("#GasSupply").hide();
        }
        if ($("#Demise").is(":checked") == true) {
            $("#housing-society").show();
            $("#GasSupply").hide();
        }
        if ($("#Builder_Case").is(":checked") == true) {
            $("#housing-society").hide();
            $("#GasSupply").show();
        }
    });

    $("#backid2").click(function (e) {

        $("#Details-tab").click();
        e.preventDefault();
    });

    $("input:file").change(function () {
        //console.log(this.files.length);
        $(this).addClass("custom-file-uploaded");
        var nextbutton = $(this).parent().find("input[type='button']");
        if (nextbutton.length > 0) {
            $(this).parent().find("input[type='button']").remove();
        }
        if (this.files.length > 0) {
            $(this).after('<input class="previewdoc" type=button value=Preview onclick=onfileselection("' + $(this).attr("id") + '"); />');
        }
    });

    $("#name-transfer-step1 select").on("change", function () {
        var check = $(this).val();
        if (check != null) {
            $(this).next().hide();
        }

    });

});


$('.nametransfer-applicationstatus input[type="file"]').on("change", function (e) {

    var fileDetail2 = document.getElementById($(this).attr("id"));
    if (fileDetail2.files.length === 0) {
        $("#Document_Required_message_modal").show();
        fileDetail2.focus();
        if (isvalid === true)
            isvalid = false;

        return false;
    }
    else {
        var MAX_FILE_SIZE = 5 * 1024 * 1024;
        var _validFileExtensions = ["jpg", "pdf", "jpeg"];
        var filename = fileDetail2.files["0"].name;
        var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
        if (fileDetail2.files["0"].size > MAX_FILE_SIZE) {

            $("#Document_Size_message_modal").show();
            fileDetail2.focus();
            fileDetail2.value = "";
            this.reportValidity();
            if (isvalid === true)
                isvalid = false;

            return false;
        }
        else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
            fileDetail2.value = "";
            $("#Document_Validation_message_modal").show();
            fileDetail2.focus();
            return false;
        }
    }

});

$('.name-transfer-step1:visible input[type="file"]').on("change", function (e) {

    var fileDetail2 = document.getElementById($(this).attr("id"));
    if (fileDetail2.files.length === 0) {
        $("#Document_Required_message_modal").show();
        fileDetail2.focus();
        if (isvalid === true)
            isvalid = false;

        return false;
    }
    else {
        var MAX_FILE_SIZE = 5 * 1024 * 1024;
        var _validFileExtensions = ["jpg", "pdf", "jpeg"];
        var filename = fileDetail2.files["0"].name;
        var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
        if (fileDetail2.files["0"].size > MAX_FILE_SIZE) {

            $("#Document_Size_message_modal").show();
            fileDetail2.focus();
            fileDetail2.value = "";
            this.reportValidity();
            if (isvalid === true)
                isvalid = false;

            return false;
        }
        else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
            fileDetail2.value = "";
            $("#Document_Validation_message_modal").show();
            fileDetail2.focus();
            return false;
        }
    }

});


additionaldetailsubmit = function () {
    var buttonName = $(document.activeElement).attr('name');
    if (buttonName === "UpdateChanges") {

        var isvalid = true;
        $(".buildercase:visible .buildercaseSelect").each(function () {
            var DocDetails = document.getElementById($(this).attr("id"));
            if (DocDetails.value == "") {
                $("#" + DocDetails.id).parent().children('label').removeAttr('style');
                DocDetails.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails.id).parent().children('label').removeAttr('style').hide();
            }
        });
        $(".buildercase:visible input:file").each(function () {
            var fileDetail1 = document.getElementById($(this).attr("id"));

            if (fileDetail1.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail1.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg"];
                var filename = fileDetail1.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail1.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Required_message_modal").show();
                    fileDetail1.focus();
                    fileDetail1.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail1.value = "";
                    $("#Document_Required_message_modal").show();
                    fileDetail1.focus();
                    return false;
                }
            }
        });

        $(".demise:visible .demisecaseSelect").each(function () {
            var DocDetails1 = document.getElementById($(this).attr("id"));
            if (DocDetails1.value == "") {
                $("#" + DocDetails1.id).parent().children('label').removeAttr('style');
                DocDetails1.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails1.id).parent().children('label').removeAttr('style').hide();
            }

        });
        $(".demise:visible input:file").each(function () {
            var fileDetail2 = document.getElementById($(this).attr("id"));
            if (fileDetail2.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail2.focus();
                if (isvalid === true)
                    isvalid = false;

                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg"];
                var filename = fileDetail2.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail2.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Size_message_modal").show();
                    fileDetail2.focus();
                    fileDetail2.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail2.value = "";
                    $("#Document_Validation_message_modal").show();
                    fileDetail2.focus();
                    return false;
                }
            }
        });

        $(".demiseUnregisteredHousing:visible .demiseUnregisteredHousingSelect").each(function () {
            var DocDetails4 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "demiseUnregisteredHousing6" || $(this).attr("id") == "demiseUnregisteredHousing7")
                return true;
            if (DocDetails4.value == "") {
                $("#" + DocDetails4.id).parent().children('label').removeAttr('style');
                DocDetails4.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails4.id).parent().children('label').removeAttr('style').hide();
            }

        });
        $(".demiseUnregisteredHousing:visible input:file").each(function () {
            var demiseUnregisteredHousing = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "FiledemiseUnregisteredHousing7" || $(this).attr("id") == "FiledemiseUnregisteredHousing6")
                return true;
            if (demiseUnregisteredHousing.files.length === 0) {
                $("#Document_Required_message_modal").show();
                demiseUnregisteredHousing.focus();
                if (isvalid === true)
                    isvalid = false;

                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg", "JPG", "PDF", "JPEG"];
                var filename = demiseUnregisteredHousing.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (demiseUnregisteredHousing.files["0"].size > MAX_FILE_SIZE) {
                    $("#Document_Size_message_modal").show();
                    demiseUnregisteredHousing.focus();
                    demiseUnregisteredHousing.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    demiseUnregisteredHousing.value = "";
                    $("#Document_Validation_message_modal").show();
                    demiseUnregisteredHousing.focus();
                    return false;
                }
            }
        });

        $(".RegisteredHousing:visible .RegisteredHousingSelect").each(function () {
            var DocDetails2 = document.getElementById($(this).attr("id"));
            if (DocDetails2.value == "") {
                $("#" + DocDetails2.id).parent().children('label').removeAttr('style');
                DocDetails2.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails2.id).parent().children('label').removeAttr('style').hide();
            }

        });
        $(".RegisteredHousing:visible input:file").each(function () {
            var fileDetail3 = document.getElementById($(this).attr("id"));
            if (fileDetail3.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail3.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg"];
                var filename = fileDetail3.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail3.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Size_message_modal").show();
                    fileDetail3.focus();
                    fileDetail3.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail3.value = "";
                    $("#Document_Validation_message_modal").show();
                    fileDetail3.focus();
                    return false;
                }
            }
        });

        $(".UnregisteredHousing:visible .UnregisteredHousingSelect").each(function () {
            var DocDetails3 = document.getElementById($(this).attr("id"));
            if (DocDetails3.value == "") {
                $("#" + DocDetails3.id).parent().children('label').removeAttr('style');
                DocDetails3.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails3.id).parent().children('label').removeAttr('style').hide();
            }
        });
        $(".UnregisteredHousing:visible input:file").each(function () {
            var fileDetail4 = document.getElementById($(this).attr("id"));
            if (fileDetail4.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail4.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg"];
                var filename = fileDetail4.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail4.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Size_message_modal").show();
                    fileDetail4.focus();
                    fileDetail4.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail4.value = "";
                    $("#Document_Validation_message_modal").show();
                    fileDetail4.focus();
                    return false;
                }
            }
        });

        $(".isAdditionalDocuments:visible input:file").each(function () {
            var fileDetail5 = document.getElementById($(this).attr("id"));;
            if (fileDetail5.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail5.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg"];
                var filename = fileDetail5.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail5.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Size_message_modal").show();
                    fileDetail5.focus();
                    fileDetail5.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail5.value = "";
                    $("#Document_Validation_message_modal").show();
                    fileDetail4.focus();
                    return false;
                }
            }
        });



        if (!isvalid)
            return false;
        else {
            //var name = $("#FirstName").val().toUpperCase() + " " + $("#MiddleName").val().toUpperCase() + " " + $("#LastName").val().toUpperCase();
            $('.confirmation_modal_Additional_message').html(/*"Your name will appear on your bill as below: <br><br><strong>" + name +*/ "</strong><br><br> Are you sure you want to submit the application?");
            $('#confirmation_Additional_modal').modal("show");
            event.preventDefault();
            return false;
        }
    }
};

onfileselection = function (This) {
    var fileinput = document.getElementById(This);
    if (fileinput.files.length > 0) {
        var pdffile = fileinput.files[0];
        var pdffile_url = URL.createObjectURL(pdffile);
        //  $('#viewer').attr('src', pdffile_url);
        window.open(pdffile_url, "_blank", "status=true,toolbar=false,menubar=false,location=false,width=1018,height=792")
    }
};

test = function (Guid) {

    $.ajax({
        url: "/api/AdaniGas/ShowNameTransferAttachmentFileview",
        type: "GET",
        data: { "id": Guid },
        cache: false,
        success: function (response) {

            window.open("/api/AdaniGas/ShowNameTransferAttachmentFileview?id=" + Guid, "_blank", "location=no,addressbar=no,status=true,directories=no,titlebar=no,toolbar=no,width=1018,height=792")
            //window.open('', '_blank').location.href = "/api/sitecore/AdaniGas/ShowNameTransferAttachmentFileview?id=" + Guid;
        },
        error: function (response) {
            console.log("Error in delete methode");
        }
    })


};

nametransfersubmit = function () {
    var buttonName = $(document.activeElement).attr('name');

    if (buttonName === "proceedforpayment") {
        var isvalid = true;

        $(".buildercase:visible .buildercaseSelect").each(function () {
            var DocDetails = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "buildercaseSelect4" || $(this).attr("id") == "buildercaseSelect5")
                return true;
            if (DocDetails.value == "") {
                $("#" + DocDetails.id).parent().children('label').removeAttr('style');
                DocDetails.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails.id).parent().children('label').removeAttr('style').hide();
            }
        });
        $(".buildercase:visible input:file").each(function () {
            var fileDetail1 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "Builder4" || $(this).attr("id") == "Builder5")
                return true;
            if (fileDetail1.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail1.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg"];
                var filename = fileDetail1.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail1.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Required_message_modal").show();
                    fileDetail1.focus();
                    fileDetail1.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail1.value = "";
                    $("#Document_Required_message_modal").show();
                    fileDetail1.focus();
                    return false;
                }
            }
        });

        $(".demise:visible .demisecaseSelect").each(function () {
            var DocDetails1 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "demisecaseSelect5" || $(this).attr("id") == "demisecaseSelect4")
                return true;
            if (DocDetails1.value == "") {
                $("#" + DocDetails1.id).parent().children('label').removeAttr('style');
                DocDetails1.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails1.id).parent().children('label').removeAttr('style').hide();
            }

        });
        $(".demise:visible input:file").each(function () {
            var fileDetail2 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "Demise4" || $(this).attr("id") == "Demise5")
                return true;
            if (fileDetail2.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail2.focus();
                if (isvalid === true)
                    isvalid = false;

                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg", "JPG", "PDF", "JPEG"];
                var filename = fileDetail2.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail2.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Size_message_modal").show();
                    fileDetail2.focus();
                    fileDetail2.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail2.value = "";
                    $("#Document_Validation_message_modal").show();
                    fileDetail2.focus();
                    return false;
                }
            }
        });

        $(".demiseUnregisteredHousing:visible .demiseUnregisteredHousingSelect").each(function () {
            var DocDetails4 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "demiseUnregisteredHousing6" || $(this).attr("id") == "demiseUnregisteredHousing7")
                return true;
            if (DocDetails4.value == "") {
                $("#" + DocDetails4.id).parent().children('label').removeAttr('style');
                DocDetails4.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails4.id).parent().children('label').removeAttr('style').hide();
            }

        });
        $(".demiseUnregisteredHousing:visible input:file").each(function () {
            var demiseUnregisteredHousing = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "FiledemiseUnregisteredHousing7" || $(this).attr("id") == "FiledemiseUnregisteredHousing6")
                return true;
            if (demiseUnregisteredHousing.files.length === 0) {
                $("#Document_Required_message_modal").show();
                demiseUnregisteredHousing.focus();
                if (isvalid === true)
                    isvalid = false;

                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg", "JPG", "PDF", "JPEG"];
                var filename = demiseUnregisteredHousing.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (demiseUnregisteredHousing.files["0"].size > MAX_FILE_SIZE) {
                    $("#Document_Size_message_modal").show();
                    demiseUnregisteredHousing.focus();
                    demiseUnregisteredHousing.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    demiseUnregisteredHousing.value = "";
                    $("#Document_Validation_message_modal").show();
                    demiseUnregisteredHousing.focus();
                    return false;
                }
            }
        });

        $(".RegisteredHousing:visible .RegisteredHousingSelect").each(function () {
            var DocDetails2 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "RegisteredHousingSelect3" || $(this).attr("id") == "RegisteredHousingSelect4")
                return true;
            if (DocDetails2.value == "") {
                $("#" + DocDetails2.id).parent().children('label').removeAttr('style');
                DocDetails2.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails2.id).parent().children('label').removeAttr('style').hide();
            }

        });
        $(".RegisteredHousing:visible input:file").each(function () {
            var fileDetail3 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "Registered_Housing3" || $(this).attr("id") == "Registered_Housing4")
                return true;
            if (fileDetail3.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail3.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg", "JPG", "PDF", "JPEG"];
                var filename = fileDetail3.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail3.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Size_message_modal").show();
                    fileDetail3.focus();
                    fileDetail3.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail3.value = "";
                    $("#Document_Validation_message_modal").show();
                    fileDetail3.focus();
                    return false;
                }
            }
        });

        $(".UnregisteredHousing:visible .UnregisteredHousingSelect").each(function () {
            var DocDetails3 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "UnregisteredHousingSelect4" || $(this).attr("id") == "UnregisteredHousingSelect5")
                return true;
            if (DocDetails3.value == "") {
                $("#" + DocDetails3.id).parent().children('label').removeAttr('style');
                DocDetails3.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                $("#" + DocDetails3.id).parent().children('label').removeAttr('style').hide();
            }
        });
        $(".UnregisteredHousing:visible input:file").each(function () {
            var fileDetail4 = document.getElementById($(this).attr("id"));
            if ($(this).attr("id") == "Unregistered_Housing4" || $(this).attr("id") == "Unregistered_Housing5")
                return true;
            if (fileDetail4.files.length === 0) {
                $("#Document_Required_message_modal").show();
                fileDetail4.focus();
                if (isvalid === true)
                    isvalid = false;
                return false;
            }
            else {
                var MAX_FILE_SIZE = 5 * 1024 * 1024;
                var _validFileExtensions = ["jpg", "pdf", "jpeg", "JPG", "PDF", "JPEG"];
                var filename = fileDetail4.files["0"].name;
                var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
                if (fileDetail4.files["0"].size > MAX_FILE_SIZE) {

                    $("#Document_Size_message_modal").show();
                    fileDetail4.focus();
                    fileDetail4.value = "";
                    this.reportValidity();
                    if (isvalid === true)
                        isvalid = false;

                    return false;
                }
                else if ($.inArray(fileExtension.toLowerCase(), _validFileExtensions) == -1) {
                    fileDetail4.value = "";
                    $("#Document_Validation_message_modal").show();
                    fileDetail4.focus();
                    return false;
                }
            }
        });

        if (!isvalid)
            return false;
        else {
            if (!$("#checkbox").prop("checked")) {
                $("#agree_chk_error").show();
                if (isvalid === true)
                    isvalid = false;
            }
            else {
                $("#agree_chk_error").hide();
            }
        }

        if (!isvalid)
            return false;
        else {
            var name = $("#FirstName").val().toUpperCase() + " " + $("#MiddleName").val().toUpperCase() + " " + $("#LastName").val().toUpperCase();
            $('.confirmation_modal_message').html("Please verify your Name mentioned below & proceed to submit the application:<br><strong>" + name + "</strong>");
            $('#confirmation_modal').modal("show");
            event.preventDefault();
            return false;
        }
    }
    else {
        $('#AjaxLoader').show();
        return true;
    }

};

$("#Close_Document_Required_message_modal").click(function () {
    $("#Document_Required_message_modal").hide();
});
$("#Close_Document_Required_message_modals").click(function () {
    $("#Document_Required_message_modal").hide();
});

$("#Close_Document_Size_message_modal").click(function () {
    $("#Document_Size_message_modal").hide();
});
$("#Close_Document_Size_message_modals").click(function () {
    $("#Document_Size_message_modal").hide();
});

$("#Close_Document_Validation_message_modal").click(function () {
    $("#Document_Validation_message_modal").hide();
});
$("#Close_Document_Validation_message_modals").click(function () {
    $("#Document_Validation_message_modal").hide();
});

function GetScrollPosition() {
    localStorage['page'] = document.URL;
    localStorage['scrollTop'] = $(document).scrollTop();
}

$("#btn_cancle").click(function () {
    window.location.href = window.location.href.split('?')[0];
});

$("#btn_Popupok").click(function () {
    window.location.href = window.location.href.split('?')[0];
});

$("#BLOCK_name_transfer_admin_yes").click(function () {
    if (TempdeleteId != undefined) {
        $.ajax({
            url: "/api/AdaniGas/BlockUnBlockNameTransferAdmin",
            type: "POST",
            data: { "createdadminid": TempdeleteId },
            cache: false,
            success: function (response) {
                window.location.href = "/NameTransferHome/NameTransferAdminList"

            },
            error: function (response) {
                console.log("Error in delete methode");
            }
        })
    }
});

var TempdeleteId = "";
$(".BLOCK-admin #btn_AdminBLOCK").click(function () {

    TempdeleteId = $(this).attr('data-id');
    $("#name_transfer_BLOCK_admin").show();

});
$("#BLOCK_name_transfer_admin_no").click(function () {
    $("#name_transfer_BLOCK_admin").hide();
});

$(".Unblock-Admin #btn_AdminUnblock").click(function () {
    TempdeleteId = $(this).attr('data-id');
    $("#name_transfer_Unblock_admin").show();
});
$("#Unblock_name_transfer_admin_no").click(function () {
    $("#name_transfer_Unblock_admin").hide();

});
$("#Unblock_name_transfer_admin_yes").click(function () {
    if (TempdeleteId != undefined) {
        $.ajax({
            url: "/api/AdaniGas/BlockUnBlockNameTransferAdmin",
            type: "POST",
            data: { "createdadminid": TempdeleteId },
            cache: false,
            success: function (response) {
                window.location.href = "/NameTransferHome/NameTransferAdminList"
            },
            error: function (response) {
                console.log("Error in delete methode");
            }
        })
    }
});

//$("#Unlock_admin_success_OK").on("click", function () {

//});

$("#Clear_Search_Result").click(function () {
    window.location.href = window.location.href.split('?')[0];
});

$("#Re-open_By_Admin").click(function () {
    $("#submit_Reopen_modal").show();
});
$("#CloseReopen").click(function () {
    $("#submit_Reopen_modal").hide();
});

$("#Approve_By_Admin").click(function () {
    $("#submit_Approved_modal").show();
});
$("#CloseApproved").click(function () {
    $("#submit_Approved_modal").hide();
});

$("#Reject_By_Admin").click(function () {
    $("#submit_Rejected_modal").show();
});
$("#CloseRejected").click(function () {
    $("#submit_Rejected_modal").hide();
});

$("#Additional_Details_By_Admin").click(function () {
    $("#submit_AdditionalDetails_modal").show();
});
$("#CloseAdditionalDetails").click(function () {
    $("#submit_AdditionalDetails_modal").hide();
});

$("#Save_Approved_Details").click(function () {
    $("#submited_Approved_Message").show();
});

$("#Save_Rejected_Details").click(function () {
    $("#submited_Rejected_Message").show();
});

$("#Save_Additional_Details").click(function () {
    $("#submited_AdditionalDetails_Message").show();
});

$("#submited_Approved").click(function () {
    var removedata = removeParams("ApprovedMessage");
    window.location.href = removedata;
    $("#submited_Approved_Message").hide();

});
$("#submited_Rejected").click(function () {
    var removemessage = removeParams("RejectedMessage");
    window.location.href = removemessage;
    $("#submited_Rejected_Message").hide();

});
$("#submited_AdditionalDetails").click(function () {
    var removeAdditionalDetails = removeParams("AdditionalDetailsMessage");
    window.location.href = removeAdditionalDetails;
    $("#submited_AdditionalDetails_Message").hide();

});

$("#submited_Approved_Application").click(function () {
    var removedata = removeParams("ApprovedMessage");
    window.location.href = removedata;

    $("#submited_Approved_Message").hide();

});
function removeParams(sParam) {
    var url = window.location.href.split('?')[0] + '?';
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] != sParam) {
            url = url + sParameterName[0] + '=' + sParameterName[1] + '&'
        }
    }
    return url.substring(0, url.length - 1);
}
$("#submited_Rejected__Application").click(function () {
    var removemessage = removeParams("RejectedMessage");
    window.location.href = removemessage;
    $("#submited_Rejected_Message").hide();

});

$("#submited_AdditionalDetails_Application").click(function () {
    var removeAdditionalDetails = removeParams("AdditionalDetailsMessage");
    window.location.href = removeAdditionalDetails;
    $("#submited_AdditionalDetails_Message").hide();

});

$("#AdditionalPaymentRequired").click(function () {
    if ($(this).is(":checked")) {
        $("#AddAdditionalPayment").show();

    } else {
        $("#AddAdditionalPayment").hide();

    }
});

$("#submited_Closed_Application").click(function () {
    var removeAdditionalDetails = removeParams("ClosedApplicationMessage");
    window.location.href = removeAdditionalDetails;
    $("#submited_closedApplication_Message").hide();
});
$("#submited_closedApplications").click(function () {
    var removeAdditionalDetails = removeParams("ClosedApplicationMessage");
    window.location.href = removeAdditionalDetails;
    $("#submited_closedApplication_Message").hide();
});


function showOtherPG() {
    $(".opg").show();
    $("#opg").attr('disabled', true);
}
//var comments = "";
function rejectdocument(guid, comments) {

    if ((guid != undefined || guid != "") && comments != undefined) {
        $.ajax({
            url: "/api/AdaniGas/RejectNameTransferAttachmentFile",
            type: "GET",
            data: { "id": guid, "AdminComment": comments },
            cache: false,
            success: function (response) {
                window.location.href = window.location.href;
            },
            error: function (response) {
                console.log("Error in delete methode");
            }
        })
    }
}

//Name Transfer Application Status flow JS
$("#CustomerInput1").on('focusout', function (e) {
    var CustomerInput = $("#CustomerInput1").val();
    if (CustomerInput == "" || CustomerInput == undefined) {
        alert("Input cannot be blank. Please enter Customer ID/ Meter Number / Request Number to track status");
    }
    else {
        $.ajax({
            url: "/api/AdaniGas/NameTransferApplicationStatus",
            type: "POST",
            data: {
                CustomerID: CustomerInput

            },
            cache: false,
            async: true,
            success: function (data) {
                //window.location.href = "/CarbonCalculatorHistory";
            }

        });
    }
});

function checkquerystring() {

    var uri = window.location.href;

    var startdate = $("#Filter_Application_StartDate").val();
    var enddate = $("#Filter_Application_EndDate").val();
    var filterstatus = $("#FilterNameTransferRequestStatus").val();
    var applicationtype = $("#NTApplicationType").val();
    var postdata = {};
    if (startdate != '')
        postdata.StartDate = startdate;
    if (enddate != '')
        postdata.EndDate = enddate;
    if (filterstatus != '')
        postdata.check = filterstatus;
    if (applicationtype != '')
        postdata.ApplicationTypes = applicationtype;



    if (((startdate != undefined && startdate != "") && (enddate != undefined && enddate != "")) || (filterstatus != undefined && filterstatus != "") || (applicationtype != undefined && applicationtype != "")) {
        $('#AjaxLoader').show();
        $.ajax({

            url: "/api/AdaniGas/NameTransferHome",
            type: "GET",
            data: postdata,
            cache: false,
            success: function (response) {

                if (uri.toString().indexOf("?") > 0) {

                    uri = uri.split('?')[0];

                    window.location.href = uri + "?StartDate=" + startdate + "&EndDate=" + enddate + "&check=" + filterstatus + "&ApplicationTypes=" + applicationtype + "&SearchDataFromDate=Search";

                }
                else {
                    window.location.href = uri + "?StartDate=" + startdate + "&EndDate=" + enddate + "&check=" + filterstatus + "&ApplicationTypes=" + applicationtype + "&SearchDataFromDate=Search";

                }

            },
            error: function (response) {
                console.log("Error in search data");
            }
        })
    }

}

$("#IndexBuilderId_1").click(function () {
    $("#Rejected_Document1").show();
});
$("#PossessionletterBuilderId_2").click(function () {
    $("#Rejected_Document2").show();
});
$("#PhotoIDBuilderId_3").click(function () {
    $("#Rejected_Document3").show();
});
$("#CoownerBuilderId_4").click(function () {
    $("#Rejected_Document4").show();
});
$("#SignedIDproofId_5").click(function () {
    $("#Rejected_Document5").show();
});

$("#DeathCertificateDemiseId_6").click(function () {
    $("#Rejected_Document6").show();
});
$("#DocumentaryDemiseId_7").click(function () {
    $("#Rejected_Document7").show();
});
$("#IDProofDemiseId_8").click(function () {
    $("#Rejected_Document8").show();
});
$("#NOCDemiseId_9").click(function () {
    $("#Rejected_Document9").show();
});
$("#SignedIDproofDemiseId_10").click(function () {
    $("#Rejected_Document10").show();
});

$("#DeathCertificateDemiseUnregisteredHousingId_23").click(function () {
    $("#Rejected_Document23").show();
});
$("#MunicipalCorporationTaxBillDemiseUnregisteredHousingId_20").click(function () {
    $("#Rejected_Document20").show();
});
$("#ElectricityBillDemiseUnregisteredHousingId_21").click(function () {
    $("#Rejected_Document21").show();
});
$("#OtherDemiseUnregisteredHousingId_22").click(function () {
    $("#Rejected_Document22").show();
});
$("#IDProofDemiseUnregisteredHousingId_24").click(function () {
    $("#Rejected_Document24").show();
});
$("#NOCDemiseUnregisteredHousingId_25").click(function () {
    $("#Rejected_Document25").show();
});
$("#SignedIDprooDemiseUnregisteredHousingID_26").click(function () {
    $("#Rejected_Document26").show();
});

$("#FirstRegisteredHousingSocietypropertyresaleId_11").click(function () {
    $("#Rejected_Document11").show();
});
$("#SecondRegisteredHousingSocietypropertyresaleId_12").click(function () {
    $("#Rejected_Document12").show();
});
$("#NOCRegisteredHousingSocietypropertyresaleId_13").click(function () {
    $("#Rejected_Document13").show();
});
$("#SignedIDproofRegisteredHousingSocietypropertyresaleId_14").click(function () {
    $("#Rejected_Document14").show();
});

$("#FirstUnregisteredHousingSocietypropertyresaleId_15").click(function () {
    $("#Rejected_Document15").show();
});
$("#SecondUnregisteredHousingSocietypropertyresaleId_16").click(function () {
    $("#Rejected_Document16").show();
});
$("#IDProofUnregisteredHousingSocietypropertyresaleId_17").click(function () {
    $("#Rejected_Document17").show();
});
$("#NOCUnregisteredHousingSocietypropertyresaleId_18").click(function () {
    $("#Rejected_Document18").show();
});
$("#SignedIDproofUnregisteredHousingSocietypropertyresaleId_19").click(function () {
    $("#Rejected_Document19").show();
});


$("#isAdditionalDocumentID_0").click(function () {
    $("#Rejected_Document0").show();
});

$(document).ready(function () {
    var message = $("#submit_message_Name_Transfer").val();
    if (message !== undefined && message !== null && message !== "") {
        $("#submit_message_modal_Name_Transfer").modal('show');
        $("#submit_message_Name_Transfer").val("");
    }
});

function fixOnScroll() {

    var fixEle = $('#myTabContent');
    $(window).scroll(function () {
        if (fixEle.length > 0) {
            if ($(window).scrollTop() > fixEle.offset().top + $('.main-header').outerHeight()) {
                $('body').addClass('custom-sticky');
            } else {
                $('body').removeClass('custom-sticky');
            }
        }
    });
}

//$("#AdminCityList").select2();

$("#submited_updatepassword").click(function () {
    window.location.href = window.location.href.split('?')[0];
    $("#submited_Change_Name_Transfer_Message").hide();

});

$("#submited_NameTransfer_updatepassword").click(function () {
    window.location.href = window.location.href.split('?')[0];
    $("#submited_Change_Name_Transfer_Message").hide();

});


var timelefts = 30;
var ResendOtpTimer = setInterval(function () {

    timelefts = timelefts - 1;
    if (timelefts <= 0) {
        clearInterval(ResendOtpTimer);
        $("#NameTransferResendOtp").show();
        var CustomerID = $("#CustomerID").val();
        var MobileNumber = $("#MobileNumber").val();
        if (CustomerID != null && CustomerID != "" && MobileNumber != null && MobileNumber != "") {
            $("#NameTransferResendOtp").click(function () {
                $.ajax({
                    url: "/api/AdaniGas/NameTransfer",
                    type: "POST",
                    data: { "ResendOtps": true, "resendotpid": CustomerID, "resendotpNo": MobileNumber },
                    cache: false,
                    success: function (response) {
                    },
                    error: function (response) {
                        console.log("Error in resend otp");
                    }
                })
            });
        }
        $("#countdown").hide();

    } else {
        $("#countdown").text(timelefts + " seconds remaining for Re-Send OTP");
    }

}, 1000);

var timeleft = 30;
var StatusResendOtpTimer = setInterval(function () {

    if (timeleft <= 0) {
        clearInterval(StatusResendOtpTimer);
        $("#NameTransferStatusResendOtp").show();
        var CustomerID = $("#CustomerID").val();
        var MobileNumber = $("#MobileNumber").val();
        if (CustomerID != null && CustomerID != "" && MobileNumber != null && MobileNumber != "") {
            $("#NameTransferStatusResendOtp").click(function () {
                $.ajax({
                    url: "/api/AdaniGas/NameTransferApplicationStatus",
                    type: "POST",
                    data: { "StatusResendOtps": true, "resendotpid": CustomerID, "resendotpNo": MobileNumber },
                    cache: false,
                    success: function (response) {
                    },
                    error: function (response) {
                        console.log("Error in resend otp");
                    }
                })
            });
        }
        $("#Statuscountdown").hide();
    } else {
        $("#Statuscountdown").text(timeleft + " seconds remaining for Re-Send OTP");
    }
    timeleft = timeleft - 1;
}, 1000);


$("#Close_Update_NameBy_Admins_message_modals").click(function () {
    window.location.href = window.location.href.split('&&')[0];
    $("#Update_NameBy_Admins_message_modal").hide();
});

$("#Close_Change_Password_message_modal").click(function () {
    window.location.href = window.location.href.split('?')[0];
    $("#Change_Password_message_modal").hide();
});
$("#Close_Change_Password_message_modals").click(function () {
    window.location.href = window.location.href.split('?')[0];
    $("#Change_Password_message_modal").hide();
});

$("#Rejected0").click(function () {
    $("#Rejected_Document0").hide();
});
$("#Rejected1").click(function () {
    $("#Rejected_Document1").hide();
});
$("#Rejected2").click(function () {
    $("#Rejected_Document2").hide();
});
$("#Rejected3").click(function () {
    $("#Rejected_Document3").hide();
});
$("#Rejected4").click(function () {
    $("#Rejected_Document4").hide();
});
$("#Rejected5").click(function () {
    $("#Rejected_Document5").hide();
});
$("#Rejected6").click(function () {
    $("#Rejected_Document6").hide();
});
$("#Rejected7").click(function () {
    $("#Rejected_Document7").hide();
});
$("#Rejected8").click(function () {
    $("#Rejected_Document8").hide();
});
$("#Rejected9").click(function () {
    $("#Rejected_Document9").hide();
});
$("#Rejected10").click(function () {
    $("#Rejected_Document10").hide();
});
$("#Rejected11").click(function () {
    $("#Rejected_Document11").hide();
});
$("#Rejected12").click(function () {
    $("#Rejected_Document12").hide();
});
$("#Rejected13").click(function () {
    $("#Rejected_Document13").hide();
});
$("#Rejected14").click(function () {
    $("#Rejected_Document14").hide();
});
$("#Rejected15").click(function () {
    $("#Rejected_Document15").hide();
});
$("#Rejected16").click(function () {
    $("#Rejected_Document16").hide();
});
$("#Rejected17").click(function () {
    $("#Rejected_Document17").hide();
});
$("#Rejected18").click(function () {
    $("#Rejected_Document18").hide();
});
$("#Rejected19").click(function () {
    $("#Rejected_Document19").hide();
});


$("#CustomerEmailId").on("change", function () {

    $(".error").hide();
    var hasError = false;
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;

    var emailaddressVal = $("#CustomerEmailId").val();
    if (!emailReg.test(emailaddressVal)) {
        $("#CustomerEmailId").after('<span class="error" style="color: red;">Enter a valid email address.</span>');
        hasError = true;
    }
    else {
        $(".error").hide();
    }

    if (hasError == true) { return false; }

});



var didScroll;
var lastScrollTop = 0;
var delta = 0;
var navbarHeight = 0;
$(window).scroll(function (event) {
    didScroll = true;
});
setInterval(function () {
    if (didScroll) {
        hasScrolled();
        didScroll = false;
    }
}, 250);
function hasScrolled() {
    var st = $(this).scrollTop();
    // Make sure they scroll more than delta
    if (Math.abs(lastScrollTop - st) <= delta)
        return;
    // If they scrolled down and are past the navbar, add class .nav-up.
    // This is necessary so you never see what is "behind" the navbar.
    $('#back-to-top').fadeIn();
    $('header').addClass('fixed-header');
    if (st > lastScrollTop && st > navbarHeight) {
        // Scroll Down
        //$('.btm-floating').addClass('active');
    } else {
        // Scroll Up
        if (st + $(window).height() < $(document).height()) {
        }
    }
    if (st < 150) {
        $('#back-to-top').hide();
        $('header').removeClass('fixed-header');
        //$('.btm-floating').removeClass('active');
    }
    lastScrollTop = st;
}

var s2 = $("#FilterNameTransferRequestStatus").select2();
var s1 = $("#NTApplicationType").select2();
$(document).ready(function () {
    var url = window.location.href;
    var hash = url.split("?")[1].split('&');

    if (hash !== null && hash !== undefined) {
        if (hash[2] != null && hash[2] != undefined) {
            var Statusvalue = hash[2].replace("check=", "").split(',');
            s2.val(Statusvalue).trigger("change");
        }
        if (hash[3] != null && hash[3] != undefined) {
            var ApplicationTypevalue = hash[3].replace("ApplicationTypes=", "").split(',');
            s1.val(ApplicationTypevalue).trigger("change");
        }

    }
});

$(document).ready(function () {
    $("#ReviewNameTransferApplication").click(function () {
        $('#AjaxLoader').show();
    });
    $("#Save_Approved_Details").click(function () {
        $('#AjaxLoader').show();
    });
    $("#Save_Rejected_Details").click(function () {
        $('#AjaxLoader').show();
    });
    $("#Save_Additional_Details").click(function () {
        $('#AjaxLoader').show();
    });
    $("#btn_NameTransferAdminList").click(function () {
        $('#AjaxLoader').show();
    });
    $("#btn_NameTransferCreateAdmin").click(function () {
        $('#AjaxLoader').show();
    });
    $("#btn_NameTransferAdminLogin").click(function () {
        $('#AjaxLoader').show();
    });
    $("#btn_NameTransferForgotPassword").click(function () {
        $('#AjaxLoader').show();
    });
    $("#btn_Submit_ClosedApplication").click(function () {
        $('#AjaxLoader').show();
    });
   
});


$("#btn_NameTransferClosedApplication").click(function () {
    $("#Closed_NTApplication").show();
});
$("#btn_Closed_NTApplication").click(function () {
    $("#Closed_NTApplication").hide();
});

$("#Rejected20").click(function () {
    $("#Rejected_Document20").hide();
});
$("#Rejected21").click(function () {
    $("#Rejected_Document21").hide();
});
$("#Rejected22").click(function () {
    $("#Rejected_Document22").hide();
});
$("#Rejected23").click(function () {
    $("#Rejected_Document23").hide();
});
$("#Rejected24").click(function () {
    $("#Rejected_Document24").hide();
});
$("#Rejected25").click(function () {
    $("#Rejected_Document25").hide();
});
$("#Rejected26").click(function () {
    $("#Rejected_Document26").hide();
});
