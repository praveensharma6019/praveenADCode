


$("#loginOtpbtn").click(function (e) {
    document.getElementById("loginOtpbtn").style.fontSize = "14px";

    var OTPFor = $('#mobile-login').val();
    var OTPType = "login";
    var Status = 0;
    if (OTPFor.length === 10 && validateMobileNo(OTPFor)) {
        $.ajax({
            type: 'POST',
            data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: true },
            url: "/api/Transmission/CarbonCalculatorGenerateOTP",
            success: function (data) {
                if (data.status === "1") {
                    $('#mo-login-msg').html("Please enter the 5-digit verification code, sent on your registered mobile number");
                }
                else if (data.status === "2") {
                    alert('Entered Mobile no. or OTP is invalid.');

                }
                else if (data.status === "3") {
                    alert('OTP Generate more than 4 time, Try after 1 Hours');
                }
                else if (data.status === "4") {
                    alert('Entered Email ID or OTP is invalid.');

                }

            },

            error: function (data) {
                alert("error!");
            }
        });
    }

    else {

        if (validateEmail(OTPFor)) {

            $.ajax({
                type: 'POST',
                data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: false },
                url: "/api/Transmission/CarbonCalculatorGenerateOTP",
                success: function (data) {
                    if (data.status === "1") {
                        $('#mo-login-msg').html("Please enter the 5-digit verification code, sent on your registered email address");
                    }
                    else if (data.status === "2") {
                        alert('Entered Mobile no. or OTP is invalid.');

                    }
                    else if (data.status === "3") {
                        alert('OTP Generate more than 4 time, Try after 1 Hours');
                    }
                    else if (data.status === "4") {
                        alert('Entered Email ID or OTP is invalid.');

                    }
                },
                error: function (data) {
                    alert("error!");
                }
            });

        }
    }

});


$("#mobileOtpbtn").click(function (e) {
    if ($("#MobileNumber").val().length == 0 || $("#MobileNumber").val() == "") {
        alert("Please Enter 10 digit Mobile Number");
    }

    else if (!/^\d*(?:\.\d{1,2})?$/.test($("#MobileNumber").val())) {
        alert("Invalid Mobile Number");
    }
    else if ($("#MobileNumber").val().length != 10) {
        alert("Mobile Number cannot be less than 10 digit");
    }

    else {
        var OTPFor = $('#MobileNumber').val();
        var OTPType = "login";
        var Status = 0;
        if (OTPFor != "") {
            if (OTPFor.length === 10 && validateMobileNo(OTPFor)) {
                $.ajax({
                    type: 'POST',
                    data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: true },
                    url: "/api/Transmission/CarbonCalculatorRegistrationGenerateOTP",
                    success: function (data) {
                        if (data.status === "1") {
                            $('#mo-login-msg').html("Please enter the 5-digit verification code, sent on your registered mobile number");
                        }

                    },

                    error: function (data) {
                        alert("error!");
                    }
                });
            }
        }
    }

});

$("#registration-otpMobile").focusout(function (e) {

    if (!/^\d*(?:\.\d{1,2})?$/.test($("#registration-otpMobile").val())) {
        alert("OTP can contains only numeric value");
    }
    if ($("#registration-otpMobile").val().length < 5) {
        alert("OTP length should be 5 digit only");
    }
    if ($("#registration-otpMobile").val().length > 0 && $("#MobileNumber").val().length == 0) {
        alert("Please first enter valid mobile number");
        document.getElementById("registration-otpMobile").value = '';
    }
});

$("#registration-otpEmail").focusout(function (e) {
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#registration-otpEmail").val())) {
        alert("OTP can contains only numeric value");
    }
    if ($("#registration-otpEmail").val().length < 5) {
        alert("OTP length should be 5 digit only");
    }
    if ($("#registration-otpEmail").val().length > 0 && $("#EmailId").val().length == 0) {
        alert("Please first enter valid Email-ID");
        document.getElementById("registration-otpEmail").value = '';
    }
});



$("#emailOtpbtn").click(function (e) {
    if ($("#EmailId").val().length == 0 || $("#EmailId").val() == "") {
        alert("Please Enter Email-Id");
    }
    else if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#EmailId").val().trim())) {
        alert("Invalid Email ID");
    }

    else {
        var OTPFor = $('#EmailId').val();
        var OTPType = "login";
        var Status = 0;
        if (OTPFor != "") {
            if (validateEmail(OTPFor)) {
                $.ajax({
                    type: 'POST',
                    data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: false },
                    url: "/api/Transmission/CarbonCalculatorRegistrationGenerateOTP",
                    success: function (data) {
                        if (data.status === "1") {
                            $('#mo-login-msg').html("Please enter the 5-digit verification code, sent on your registered email address");
                        }

                    },

                    error: function (data) {
                        alert("error!");
                    }
                });

            }
        }
    }

});


$(document).ready(function () {
    $("#ReviewMessage").hide();
    var ReviewMessage = $("#ReviewMessage").val();
    if (ReviewMessage !== undefined && ReviewMessage !== null && ReviewMessage !== "") {
        $('#message_modal').modal('show');
        $("#message").val("");
    }
});


$(document).ready(function () {
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {
        $('#message_modal').modal('show');
        $("#message").val("");
    }
});
function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}

function validateEmailId(event, t) {
    var emailAddress = $(t).val();
    if (validateEmail(emailAddress)) {
        $("#emailerror").html("");
    }
    else {
        $("#emailerror").html("Please enter a valid Email Address");
    }
}


function validateMobile(event, t) {

    var mobile = $(t).val();

    if (validateMobileNo(mobile)) {
        $("#mobileerror").html("");
    }
    else {
        $("#mobileerror").html("Entered the invalid mobile number");
    }
}
function validateNumber(inputtxts) {
    var numbers = /^[0-9]+$/;
    if (numbers.test(inputtxts)) { return true; }
    else { return false; }
};

function onchangeValidateFax(event, t) {
    var fax = $("#fax").val();
    if (fax !== null && fax.trim() !== "") {
        if (!validateFax(fax)) {
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            //$("#fax").focus();
        }
        else {
            $("#faxerror").html("");
        }
    }
    else {
        $("#faxerror").html("");
    }
}

function validateFax(fax) {
    var regex = /^[0-9]{12,12}$/;
    return regex.test(fax);
}

function validateMobileNo(mobile) {
    if (mobile.match(/^[6789]\d{9}$/)) {
        return true;
    }
    else {
        return false;
    }
}


function validateName(sname) {
    var regex = /^[a-zA-Z ]+$/;

    if (regex.test(sname)) { return true; }
    else { return false; }
};

function isValidDate(s) {
    var bits = s.split('/');
    var d = new Date(bits[2] + '/' + bits[1] + '/' + bits[0]);
    return !!(d && (d.getMonth() + 1) === bits[1] && d.getDate() === Number(bits[0]));
}


$(document).ready(function () {


});


var body = $('body');

function goToNextInput(e) {
    var key = e.which,
        t = $(e.target),
        sib = t.next('input');

    if (key !== 9 && (key < 48 || key > 57)) {
        e.preventDefault();
        return false;
    }

    if (key === 9) {
        return true;
    }

    if (!sib || !sib.length) {
        sib = body.find('input').eq(0);
    }
    sib.select().focus();
}

function onKeyDown(e) {
    var key = e.which;

    if (key === 9 || (key >= 48 && key <= 57)) {
        return true;
    }

    e.preventDefault();
    return false;
}

function onFocus(e) {
    $(e.target).select();
}


$(".mobile").bind('keyup', function (e) {
    if ($(this).val().length === 10 && $('#Mobile_Verified').val() === "") {
        var OTPFor = $(this).val();
        var OTPType = "registration";
        var Status = 0;
        $.ajax({
            type: 'POST',
            data: { OTPFor: OTPFor, OTPType: OTPType, Status: Status, IsMobile: true },
            url: "/api/Transmission/CarbonCalculatorGenerateOTP",
            success: function (data) {
                if (data.status === "1") {
                    $("#mobile-otp-ver").modal('show');
                }
                else if (data.status === "0") {
                    alert("Sorry Operation Failed!!! Please try again later");
                }

            },

            error: function (data) {
                alert("error!");  // 
            }
        });
    }


});




$(".mo-otp").bind('keyup', function (e) {
    var txtData = [];
    txtData.push($('#mobile-otp').val());
    txtData.push($(this).val());

    $("#mobile-otp").val(txtData.join(""));
    e.preventDefault();
});
$("#submit-mo-otp").click(function (e) {
    var txtData = [];
    $(".mo-otp").each(function () {
        txtData.push($(this).val());
    });
    var otp = txtData.join("");
    var OTPType = "registration";
    $.ajax({
        type: 'POST',
        data: { OTP: otp, OTPFor: $('#Mobile').val(), OTPType: OTPType },
        url: "/api/Ports/PortsGMSVerifyOTP",
        success: function (data) {
            if (data.status === "1") {
                $("#mobile-otp-ver").modal('hide');
                $(".mo-otp").val("");
                $('#Mobile_Verified').val(otp);
            } else {
                $("#msg-mo-otp").html("Please enter correct OTP or refresh page to change mobile number");
            }

        },

        error: function (data) {
            alert("error!");  // 
        }
    });
    e.preventDefault();
});


$("#logoutbtn").click(function (e) {


    $.ajax(
        {
            type: 'GET',

            url: "/api/Transmission/Logout",
            success: function (data) {
                window.location.href = "/Carbon-Footprint-Calculator/User-Login";
            }


        });
    e.preventDefault();
});


$("#submit-email-otp").click(function (e) {
    var txtData = [];
    $(".mail-otp").each(function () {
        txtData.push($(this).val());
    });
    var otp = txtData.join("");
    var OTPType = "registration";
    $.ajax({
        type: 'POST',
        data: { OTP: otp, OTPFor: $('#Email').val(), OTPType: OTPType },
        url: "/api/Ports/PortsGMSVerifyOTP",
        success: function (data) {
            if (data.status === "1") {
                $("#email-otp-ver").modal('hide');
                $(".mail-otp").val("");
                $('#Email_Verified').val(otp);
            } else {
                $("#msg-mail-otp").html("Please enter correct OTP or refresh page to change email");
            }

        },

        error: function (data) {
            alert("error!");  // 
        }
    });
    e.preventDefault();
});

$(".reset").click(function (e) {
    window.location.reload();
});

$("#btnSaveTarget").click(function (e) {
    var CarbonEmissionReducePercentage = document.getElementById('CarbonEmissionReducePercentage').innerText.replace('%', '');
    var CEP = Number(CarbonEmissionReducePercentage);
    //$("#CarbonEmissionReducePercentage").val();
    var CRY = $("#CarbonEmissionReviewYear").val();
    var CED = $("#CarbonEmissionReviewDate").val();
    var numberofTrees = $("#numberofTress").val();
    var Projectname = $("#ProjectName").val();
    var Monthofreplacingwithcng = $("#replacewithCNG").val();
    var Monthofreplacingwithelectric = $("#replacewithElectric").val();
    var regno = document.getElementById("reg-number-hidden") ? Number(document.getElementById("reg-number-hidden").value) : "";

    if (CEP == "" || CEP == null) {
        alert("please fill carbon emission percent by this year");
    }
    if ((CRY == "" || CRY == null) && (CED == "" || CED == null || CED == undefined)) {
        alert("please fill review date or select valid period");
    }
    else {

        $.ajax({
            url: "/api/Transmission/InsertCostCalculatorDate",
            type: "POST",
            data: {
                CarbonEmissionReducePercentage: CEP,
                CarbonEmissionReviewYear: CRY,
                CarbonEmissionReviewDate: CED,
                NumberofTress: numberofTrees,
                ProjectName: Projectname,
                MonthofreplacingwithCNG: Monthofreplacingwithcng,
                MonthofreplacingwithElectric: Monthofreplacingwithelectric,
                RegNo: regno

            },
            cache: false,
            async: true,
            success: function (data) {
                window.location.href = "/Carbon-Footprint-Calculator/History";
                $(".btn-primary").css('background', 'green');
                $("#emoji").show();
            }
        });
    }



});

$(document).ready(function () {
    var radioButton2 = document.getElementById("ElectricityConsumptionRadioBtn");
    radioButton2.checked = true;
    var radioButton1 = document.getElementById("ApplianceListRadioBtn");
    radioButton1.checked = false;
    document.getElementById("ConsumedElectricity_1").disabled = true;
    document.getElementById("applianceAddbtn_1").disabled = true;
    document.getElementById("consume_elec_1").disabled = true;
    if ($("#ElectricityConsumptionRadioBtn").attr("checked") === "checked") {
        radioButton2.checked = true;
        radioButton1.checked = false;
        $("#ElectricityConsumptionRadioBtn").click();
        if ($("#cookievalues").attr("value") !== "1") {
            document.getElementById("txt_11").disabled = true;
        }
    }
    if ($("#ApplianceListRadioBtn").attr("checked") === "checked") {
        radioButton1.checked = true;
        radioButton2.checked = false;
        $("#ApplianceListRadioBtn").click();
    }

    var chkBox1 = document.getElementById("PersonalTransportChkBox");
    chkBox1.checked = false;
    var chkBox2 = document.getElementById("PublicTransportChkBox");
    chkBox2.checked = false;
    var chkBox3 = document.getElementById("AirTripsChkBox");
    chkBox3.checked = false;
    var chkBox4 = document.getElementById("FiveStarAppliancesChkBox");
    chkBox4.checked = false;
    var chkBox5 = document.getElementById("NoOfTreesChkBox");
    chkBox5.checked = false;
    var chkBox6 = document.getElementById("TreeFundChkBox");
    chkBox6.checked = false;
    var chkBox7 = document.getElementById("NoOfPlantationProjectTreesChkBox");
    chkBox7.checked = false;
    document.getElementById("DiselPetrolCons").disabled = true;
    document.getElementById("CNGBusCons").disabled = true;
    document.getElementById("txt_888").disabled = true;
    document.getElementById("txt_8888").disabled = true;
    document.getElementById("txt_999").disabled = true;
});

$('#ApplianceListRadioBtn').click('change', function () {
    if (document.getElementById('ApplianceListRadioBtn').checked) {
        var radioButton1 = document.getElementById("ApplianceListRadioBtn");
        radioButton1.checked = true;
        var radioButton2 = document.getElementById("ElectricityConsumptionRadioBtn");
        radioButton2.checked = false;
        for (var i = 1; i <= 20; i++) {
            var drpdwnField = "ConsumedElectricity_" + i;
            var AddBtn = "applianceAddbtn_" + i;
            var textField = "consume_elec_" + i;
            if ($("#cookievalues").attr("value") !== "1") {
                document.getElementById(drpdwnField).disabled = true;
                document.getElementById(AddBtn).disabled = true;
                document.getElementById(textField).disabled = true;
            } else {
                document.getElementById(drpdwnField).disabled = false;
                document.getElementById(AddBtn).disabled = false;
                document.getElementById(textField).disabled = false;
            }

        }
        document.getElementById("txt_11").disabled = true;
        document.getElementById("txt_11").value = '';
        document.getElementById("d_txt_11").value = '';
    }
});

$('#ElectricityConsumptionRadioBtn').click('change', function () {
    if (document.getElementById('ElectricityConsumptionRadioBtn').checked) {
        var radioButton1 = document.getElementById("ApplianceListRadioBtn");
        radioButton1.checked = false;
        var radioButton2 = document.getElementById("ElectricityConsumptionRadioBtn");
        radioButton2.checked = true;
        for (var i = 1; i <= 20; i++) {
            var drpdwnField = "ConsumedElectricity_" + i;
            var AddBtn = "applianceAddbtn_" + i;
            var textField = "consume_elec_" + i;
            var CaltextField = "d_consume_elec_" + i;

            document.getElementById(drpdwnField).disabled = true;
            document.getElementById(AddBtn).disabled = true;
            document.getElementById(textField).disabled = true;
            document.getElementById(CaltextField).value = '';
            document.getElementById(textField).value = '';
            var dropDown = document.getElementById(drpdwnField);
            dropDown.selectedIndex = 0;
        }
        document.getElementById("txt_11").disabled = false;

    }
});


$('#PersonalTransportChkBox').click('change', function () {
    for (var i = 1; i <= 5; i++) {
        var drpdwnField = "DiselPetrolCons";
        var textField = "diselpetrol_" + i;
        var CaltextField = "d_diselpetrol_" + i;
        if (document.getElementById('PersonalTransportChkBox').checked) {
            var chkBox1 = document.getElementById("PersonalTransportChkBox");
            chkBox1.checked = true;
            document.getElementById(drpdwnField).disabled = false;
        }
        else {
            document.getElementById(drpdwnField).disabled = true;
            document.getElementById(CaltextField).value = '';
            document.getElementById(textField).value = '';
            var dropDown = document.getElementById(drpdwnField);
            dropDown.selectedIndex = 0;
        }
    }
});


$('#PublicTransportChkBox').click('change', function () {
    var textField = "CNGBus";
    var CaltextField = "d_CNGBus";
    if (document.getElementById('PublicTransportChkBox').checked) {
        var chkBox1 = document.getElementById("PublicTransportChkBox");
        chkBox1.checked = true;
        var totalConsumptionsss = 0;
        var totalConsumptions = 0;
        var get_textbox_values = Number(document.getElementById("txt_6").value);
        document.getElementById('CNGBus').value = get_textbox_values;
        var cons = "0.0805";
        if ($.isNumeric(get_textbox_values)) {
            totalConsumptions = (parseFloat(cons) * parseFloat(get_textbox_values));
            totalConsumptionsss = totalConsumptions.toFixed(2);
            document.getElementById('d_CNGBus').value = totalConsumptionsss;
        }
    }
    else {
        document.getElementById(CaltextField).value = '';
        document.getElementById(textField).value = '';
    }
});
$('#AirTripsChkBox').click('change', function () {
    var textField = "OnlineMeeting_Airtrips";
    var CaltextField = "d_OnlineMeeting_Airtrips";
    if (document.getElementById('AirTripsChkBox').checked) {
        var chkBox1 = document.getElementById("AirTripsChkBox");
        chkBox1.checked = true;
        var cal_factor = new Array();
        cal_factor[0] = "";
        cal_factor[111] = "0.227011";//short haul
        cal_factor[222] = "0.137156";//medium haul
        cal_factor[333] = "0.167421";//long haul
        var shorthaul_dist = Number(document.getElementById('numbertrips_1').value);
        var mediumhaul_dist = document.getElementById('numbertrips_2') !== null ? Number(document.getElementById('numbertrips_2').value) : 0;
        var longhaul_dist = document.getElementById('numbertrips_3') !== null ? Number(document.getElementById('numbertrips_3').value) : 0;
        document.getElementById('OnlineMeeting_Airtrips').value = Number(shorthaul_dist + mediumhaul_dist + longhaul_dist).toFixed(2);
        //document.getElementById('d_OnlineMeeting_Airtrips').value = (((shorthaul_dist * cal_factor[111] * 700) + (mediumhaul_dist * cal_factor[222] * 2350) + (longhaul_dist * cal_factor[333] * 5000))/1000).toFixed(2);
        //var totalTrips = (Number(document.getElementById("TotalAir").value)) / 1000;
        var totalTrips = Number(document.getElementById("TotalAir").value);
        document.getElementById('d_OnlineMeeting_Airtrips').value = totalTrips;
        if (window.location.search.includes("RegistrationNo")) {
            document.getElementById('d_OnlineMeeting_Airtrips').value = $("#TotalAirInViewEdit").attr("value");
        }
    }
    else {
        document.getElementById(CaltextField).value = '';
        document.getElementById(textField).value = '';
    }
});

$('#FiveStarAppliancesChkBox').click('change', function () {
    var textField = "FiveStar_Appliances";
    var CaltextField = "d_FiveStar_Appliances";
    if (document.getElementById('FiveStarAppliancesChkBox').checked) {
        var chkBox1 = document.getElementById("FiveStarAppliancesChkBox");
        chkBox1.checked = true;
        var ConsumptionsUnit = $('#txt_11').val();
        document.getElementById('FiveStar_Appliances').value = ConsumptionsUnit;
        document.getElementById('d_FiveStar_Appliances').value = (ConsumptionsUnit * 0.220416).toFixed(2); // common paramter of the LED bulb
        // document.getElementById('d_FiveStar_Appliances').value = "";
    }
    else {
        document.getElementById(CaltextField).value = '';
        document.getElementById(textField).value = '';
    }
});

$('#NoOfTreesChkBox').click('change', function () {
    var textField = "txt_888";
    var CaltextField = "d_txt_888";
    if (document.getElementById('NoOfTreesChkBox').checked) {
        var chkBox1 = document.getElementById("NoOfTreesChkBox");
        chkBox1.checked = true;
        document.getElementById(textField).disabled = false;
    }
    else {
        document.getElementById(textField).disabled = true;
        document.getElementById(CaltextField).value = '';
        document.getElementById(textField).value = '';
    }
});

$('#NoOfPlantationProjectTreesChkBox').click('change', function () {
    var textField = "txt_8888";
    var CaltextField = "d_txt_8888";
    if (document.getElementById('NoOfPlantationProjectTreesChkBox').checked) {
        var chkBox1 = document.getElementById("NoOfPlantationProjectTreesChkBox");
        chkBox1.checked = true;
        document.getElementById(textField).disabled = false;
    }
    else {
        document.getElementById(textField).disabled = true;
        document.getElementById(CaltextField).value = '';
        document.getElementById(textField).value = '';
    }
});


$('#TreeFundChkBox').click('change', function () {
    var textField = "txt_999";
    var CaltextField = "d_txt_999";
    if (document.getElementById('TreeFundChkBox').checked) {
        var chkBox1 = document.getElementById("TreeFundChkBox");
        chkBox1.checked = true;
        document.getElementById(textField).disabled = false;
        document.getElementById('d_txt_999').value = (Number(document.getElementById(textField).value) / 1000) * 1.81;
    }
    else {
        document.getElementById(textField).disabled = true;
        document.getElementById(CaltextField).value = '';
        document.getElementById(textField).value = '';
    }
});

$(document).ready(function () {
    if ($("#cookievalues").attr("value") !== "1") {
        let array = ["PersonalTransportChkBox", "AirTripsChkBox", "FiveStarAppliancesChkBox", "NoOfTreesChkBox", "NoOfPlantationProjectTreesChkBox", "TreeFundChkBox", "PublicTransportChkBox"];
        for (let i = 0; i < array.length; i++) {
            if ($("#" + array[i]).attr("checked") === "checked") {
                $("#" + array[i]).click();
            }
        }


    }

});
if ($("#ApplianceListRadioBtn").attr("checked") === "checked") {
    $("#ApplianceListRadioBtn").click();
    if ($("#cookievalues").attr("value") !== "1") {
        $("#ApplianceListRadioBtn").attr("disabled", true);
    } else {
        $("#ApplianceListRadioBtn").attr("disabled", false);
    }


}