
$(document).ready(function () {
    $(".chk-connection").change(function () {
        var currentselect = $(this).val();
        $('.activeType').removeClass('activeType');

        $(".chk-connection").each(function () {
            if (currentselect != $(this).val()) {
                $(this).attr('checked', false);
                $("#" + currentselect).addClass('activeType');
            }
        });

        $(this).attr('checked', true);
    });
});

$("#Regioarea").change(function () {
    $("#RegioGroup").empty().append();
    var cityid = $(this).val();
    $.ajax({
        type: "GET",
        url: "/api/AdaniGas/GetRegion",
        //url: '@Url.Action("GetRegion", "Enquiry")',
        data: { "CityId": cityid },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            $.each(data, function () {
                $("#RegioGroup").append($("<option/>").val(this.RegionId).text(this.RegionName));
            });

            var areaId = $("option:selected", $("#RegioGroup")).val();
            LoadSociety(areaId);
        },
        failure: function () {
            alert("Failed!");
        }
    });
});

$("#RegioGroup").change(function () {
    var areaId = $(this).val();
    LoadSociety(areaId);
});

function LoadSociety(areaId) {
    $("#Society").empty().append();

    $.ajax({
        type: "GET",
        url: "/api/AdaniGas/GetSociety",
        //url: '@Url.Action("GetSociety", "Enquiry")',
        data: { "AreaId": areaId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            $.each(data, function () {
                $("#Society").append($("<option/>").val(this.SocietyId).text(this.SocietyName));
            });
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

$(document).ready(function () {
    $("#btnSendOtp").click(function () {
        if ($("#Mob_number").val() != '') {
            $.ajax({
                type: "GET",
                url: "/api/AdaniGas/GetOTP",
                //url: '@Url.Action("GetOTP", "Enquiry")',
                data: { "MobileNo": $("#Mob_number").val() },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#RealOTP").val(data);
                    var message = "The OTP has been sent to " + $("#Mob_number").val() + " It is valid for 10 minutes.";
                    //alert(message);
                    jQuery.noConflict();
                    $('#msgarea').html(message);
                    $('#Messagepopup1').modal('show');
                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }
    })
});

function AssignHiddenValues() {
    $("#Street").val($("option:selected", $("#Society")).text());
    $("#City1").val($("option:selected", $("#Regioarea")).text());
    $("#GroupDesc").val($("option:selected", $("#RegioGroup")).text());
    //$("#HouseTypeValue").val($("option:selected", $("#HouseTypeId")).text());

    if ($("option:selected", $("#Title")).text() == "Select Value") {
        $("#TitleMessage").show();
        $("#Title").css("border-color", "red");
        return false;
    }
    else {
        $("#Title").css("border-color", "#ccc");
        $("#TitleMessage").hide();
        return true;
    }
}
